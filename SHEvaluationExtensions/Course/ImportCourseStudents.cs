using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SmartSchool.API.PlugIn;
using SHSchool.Data;
using K12.Data;
using FISCA.LogAgent;

namespace SHEvaluationExtensions.Course
{
    public class ImportCourseStudents : SmartSchool.API.PlugIn.Import.Importer
    {
        private string Item { get; set; }
        public ImportCourseStudents(string item)
        {
            this.Image = null;
            Item = item;
                this.Text = "匯入課程修課學生";
        }

        public override void InitializeImport(SmartSchool.API.PlugIn.Import.ImportWizard wizard)
        {
            //學生資訊
            Dictionary<string, SHStudentRecord> students = new Dictionary<string, SHStudentRecord>();
            //學生修課資訊 studentID -> List:SCAttendRecord
            Dictionary<string, List<SHSCAttendRecord>> scattends = new Dictionary<string, List<SHSCAttendRecord>>();
            //學生修習的課程 courseID -> CourseRecord
            Dictionary<string,SHCourseRecord> courses = new Dictionary<string, SHCourseRecord>();
            //所有課程(依學年度學期分開) schoolYear_semester -> (courseName -> CourseRecord)
            Dictionary<string, Dictionary<string,SHCourseRecord>> allcourses = new Dictionary<string, Dictionary<string, SHCourseRecord>>();
            //studentID_schoolYear_semester -> List:courseName
            Dictionary<string, List<string>> semesterCourseName = new Dictionary<string, List<string>>();
            //準備加入修課的資料 studentID -> (schoolYear_semester_courseName -> RowData)
            Dictionary<string, Dictionary<string, RowData>> prepareAttends = new Dictionary<string, Dictionary<string, RowData>>();


            wizard.PackageLimit = 3000;
            wizard.ImportableFields.Add("課程系統編號");
            wizard.ImportableFields.AddRange("學年度", "學期");
            wizard.ImportableFields.Add("課程名稱");            
            wizard.ImportableFields.AddRange("班級", "座號");
            wizard.RequiredFields.AddRange("學年度", "學期");
            wizard.RequiredFields.Add("課程名稱");
            

            wizard.ValidateStart += delegate(object sender, SmartSchool.API.PlugIn.Import.ValidateStartEventArgs e)
            {
                #region 取得學生資訊
                foreach (SHStudentRecord stu in SHStudent.SelectByIDs(e.List))
                {
                    if (!students.ContainsKey(stu.ID))
                        students.Add(stu.ID, stu);
                }
                #endregion

                #region 取得修課記錄
                MultiThreadWorker<string> loader1 = new MultiThreadWorker<string>();
                loader1.MaxThreads = 3;
                loader1.PackageSize = 250;
                loader1.PackageWorker += delegate(object sender1, PackageWorkEventArgs<string> e1)
                {
                    foreach (SHSCAttendRecord record in SHSCAttend.SelectByStudentIDAndCourseID(e1.List, new string[] { }))
                    {
                        if (!scattends.ContainsKey(record.RefStudentID))
                            scattends.Add(record.RefStudentID, new List<SHSCAttendRecord>());
                        scattends[record.RefStudentID].Add(record);

                        if (!courses.ContainsKey(record.RefCourseID))
                            courses.Add(record.RefCourseID, null);
                    }
                };
                loader1.Run(e.List);
                #endregion

                #region 取得課程資訊
                MultiThreadWorker<string> loader2 = new MultiThreadWorker<string>();
                loader2.MaxThreads = 3;
                loader2.PackageSize = 250;
                loader2.PackageWorker += delegate(object sender2, PackageWorkEventArgs<string> e2)
                {
                    foreach (SHCourseRecord record in SHCourse.SelectByIDs(new List<string>(e2.List)))
                    {
                        if (courses.ContainsKey(record.ID))
                            courses[record.ID] = record;
                    }
                };
                loader2.Run(courses.Keys);

                foreach (SHCourseRecord course in SHCourse.SelectAll())
                {
                    string key = course.SchoolYear + "_" + course.Semester;
                    if (!allcourses.ContainsKey(key))
                        allcourses.Add(key, new Dictionary<string, SHCourseRecord>());
                    if (!allcourses[key].ContainsKey(course.Name))
                        allcourses[key].Add(course.Name, course);
                }
                #endregion
            };

            wizard.ValidateRow += delegate(object sender, SmartSchool.API.PlugIn.Import.ValidateRowEventArgs e)
            {
                int i;

                #region 檢查學生是否存在
                SHStudentRecord student = null;
                if (students.ContainsKey(e.Data.ID))
                {
                    student = students[e.Data.ID];
                }
                else
                {
                    e.ErrorMessage = "壓根就沒有這個學生" + e.Data.ID;
                    return;
                }
                #endregion

                #region 驗證各個欄位格式
                bool inputFormatPass = true;
                foreach (string field in e.SelectFields)
                {
                    string value = e.Data[field];
                    switch (field)
                    {
                        default:
                            break;
                        case "學年度":
                        case "學期":
                            if (value == "" || !int.TryParse(value, out i))
                            {
                                inputFormatPass &= false;
                                e.ErrorFields.Add(field, "必須填入整數");
                            }
                            break;
                        case "課程名稱":
                            if (value == "")
                            {
                                inputFormatPass &= false;
                                e.ErrorFields.Add(field, "必須填入課程名稱");
                            }
                            break;                       
                        case "班級":
                            if (value == "")
                            {
                            }
                            break;
                        case "座號":
                            if (value != "" && !int.TryParse(value, out i))
                            {
                                inputFormatPass &= false;
                                e.ErrorFields.Add(field, "必須填入空白或整數");
                            }
                            break;
                    }
                }
                #endregion

                //輸入格式正確才會針對情節做檢驗
                #region 驗證各種情節
                if (inputFormatPass)
                {
                    string errorMessage = "";

                    string sy = e.Data["學年度"];
                    string se = e.Data["學期"];
                    string courseName =e.Data["課程名稱"];
                    string key = e.Data.ID + "_" + sy + "_" + se;
                    string semsKey = sy + "_" + se;

                    //int schoolyear = Framework.Int.ParseInt(sy);
                    //int semester = Framework.Int.ParseInt(se);

                    #region 同一個學年度學期不能有重覆的課程名稱
                    if (!semesterCourseName.ContainsKey(key))
                        semesterCourseName.Add(key, new List<string>());
                    if (semesterCourseName[key].Contains(courseName))
                    {
                            errorMessage += (errorMessage == "" ? "" : "\n") + " 同一學年度學期不允許修習多筆相同名稱的課程";
                    }
                    else
                    {
                        semesterCourseName[key].Add(courseName);
                    }
                    #endregion

                    #region 檢查課程是否存在系統中
                    bool noCourse = false;
                    if (!allcourses.ContainsKey(semsKey))
                    {
                        noCourse = true;                        
                            errorMessage += (errorMessage == "" ? "" : "\n") + " 系統中找不到該課程";
                        
                    }
                    else if (!allcourses[semsKey].ContainsKey(courseName))
                    {
                        noCourse = true;
                            errorMessage += (errorMessage == "" ? "" : "\n") + " 系統中找不到該課程";
                    }
                    else
                    {
                    }
                    #endregion

                    #region 檢查學生是否有修此課程
                    bool attended = false;

                    if (scattends.ContainsKey(e.Data.ID))
                    {
                        foreach (SHSCAttendRecord record in scattends[e.Data.ID])
                        {
                            if (courses[record.RefCourseID].Name == courseName &&
                                "" + courses[record.RefCourseID].SchoolYear == sy &&
                                "" + courses[record.RefCourseID].Semester == se)
                                attended = true;
                        }
                    }
                    else //學生沒修半堂課
                    {
                    }

                    if (!attended && !noCourse)
                    {
                            if (!e.WarningFields.ContainsKey("無修課記錄"))
                                e.WarningFields.Add("無修課記錄", "學生在此學期並無修習此課程，將會新增修課記錄");

                        if (!prepareAttends.ContainsKey(e.Data.ID))
                            prepareAttends.Add(e.Data.ID, new Dictionary<string, RowData>());
                        if (!prepareAttends[e.Data.ID].ContainsKey(semsKey + "_" + courseName))
                            prepareAttends[e.Data.ID].Add(semsKey + "_" + courseName, e.Data);
                    }
                    #endregion

                    e.ErrorMessage = errorMessage;
                }
                #endregion
            };

            wizard.ImportPackage += delegate(object sender, SmartSchool.API.PlugIn.Import.ImportPackageEventArgs e)
            {
                Dictionary<string, List<RowData>> id_Rows = new Dictionary<string, List<RowData>>();

                #region 分包裝
                foreach (RowData data in e.Items)
                {
                    if (!id_Rows.ContainsKey(data.ID))
                        id_Rows.Add(data.ID, new List<RowData>());
                    id_Rows[data.ID].Add(data);
                }
                #endregion

                List<SHSCAttendRecord> insertList = new List<SHSCAttendRecord>();
                List<SHSCAttendRecord> updateList = new List<SHSCAttendRecord>();

                //交叉比對各學生資料
                #region 交叉比對各學生資料
                foreach (string id in id_Rows.Keys)
                {
                    SHStudentRecord studentRec = students[id];

                    #region 處理要新增的修課記錄
                    if (prepareAttends.ContainsKey(id))
                    {
                        foreach (RowData data in prepareAttends[id].Values)
                        {
                            string sy = data["學年度"];
                            string se = data["學期"];
                            string semsKey = sy + "_" + se;
                            string courseName = data["課程名稱"];

                            if (allcourses.ContainsKey(semsKey) && allcourses[semsKey].ContainsKey(courseName))
                            {
                                SHSCAttendRecord record = new SHSCAttendRecord();
                                record.RefStudentID = id;
                                record.RefCourseID = allcourses[semsKey][courseName].ID;

                                insertList.Add(record);
                            }
                        }
                    }
                    #endregion
                }

                try
                {
                    if (updateList.Count > 0)
                    {
                        #region 分批次兩路上傳
                        List<List<SHSCAttendRecord>> updatePackages = new List<List<SHSCAttendRecord>>();
                        List<List<SHSCAttendRecord>> updatePackages2 = new List<List<SHSCAttendRecord>>();
                        {
                            List<SHSCAttendRecord> package = null;
                            int count = 0;
                            foreach (SHSCAttendRecord var in updateList)
                            {
                                if (count == 0)
                                {
                                    package = new List<SHSCAttendRecord>(30);
                                    count = 30;
                                    if ((updatePackages.Count & 1) == 0)
                                        updatePackages.Add(package);
                                    else
                                        updatePackages2.Add(package);
                                }
                                package.Add(var);
                                count--;
                            }
                        }
                        Thread threadUpdateSemesterSubjectScore = new Thread(new ParameterizedThreadStart(Update));
                        threadUpdateSemesterSubjectScore.IsBackground = true;
                        threadUpdateSemesterSubjectScore.Start(updatePackages);
                        Thread threadUpdateSemesterSubjectScore2 = new Thread(new ParameterizedThreadStart(Update));
                        threadUpdateSemesterSubjectScore2.IsBackground = true;
                        threadUpdateSemesterSubjectScore2.Start(updatePackages2);

                        threadUpdateSemesterSubjectScore.Join();
                        threadUpdateSemesterSubjectScore2.Join();
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                }

                if (insertList.Count > 0)
                {
                    #region 分批次兩路上傳

                    List<List<SHSCAttendRecord>> insertPackages = new List<List<SHSCAttendRecord>>();
                    List<List<SHSCAttendRecord>> insertPackages2 = new List<List<SHSCAttendRecord>>();
                    {
                        List<SHSCAttendRecord> package = null;
                        int count = 0;
                        foreach (SHSCAttendRecord var in insertList)
                        {
                            if (count == 0)
                            {
                                package = new List<SHSCAttendRecord>(30);
                                count = 30;
                                if ((insertPackages.Count & 1) == 0)
                                    insertPackages.Add(package);
                                else
                                    insertPackages2.Add(package);
                            }
                            package.Add(var);
                            count--;
                        }
                    }
                    Thread threadInsertSemesterSubjectScore = new Thread(new ParameterizedThreadStart(Insert));
                    threadInsertSemesterSubjectScore.IsBackground = true;
                    threadInsertSemesterSubjectScore.Start(insertPackages);
                    Thread threadInsertSemesterSubjectScore2 = new Thread(new ParameterizedThreadStart(Insert));
                    threadInsertSemesterSubjectScore2.IsBackground = true;
                    threadInsertSemesterSubjectScore2.Start(insertPackages2);

                    threadInsertSemesterSubjectScore.Join();
                    threadInsertSemesterSubjectScore2.Join();
                    #endregion
                }
                                
                    ApplicationLog.Log("成績系統.匯入匯出", "匯入課程修課學生", "總共匯入" + (insertList.Count + updateList.Count) + "筆課程修課學生。");
                #endregion

            };
        }

        private void Update(object item)
        {
            List<List<SHSCAttendRecord>> updatePackages = (List<List<SHSCAttendRecord>>)item;
            foreach (List<SHSCAttendRecord> package in updatePackages)
            {
                SHSCAttend.Update(package);
            }
        }

        private void Insert(object item)
        {
            List<List<SHSCAttendRecord>> insertPackages = (List<List<SHSCAttendRecord>>)item;
            foreach (List<SHSCAttendRecord> package in insertPackages)
            {
                SHSCAttend.Insert(package);
            }
        }
    }
}