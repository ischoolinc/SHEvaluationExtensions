using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SmartSchool.API.PlugIn;
using SHSchool.Data;
using K12.Data;
using FISCA.LogAgent;
using SHEvaluationExtensions.DAO;

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

            // 匯入檔按學生資訊
            Dictionary<string, StudentInfo> StudentInfoDict = new Dictionary<string, StudentInfo>();

            // 所有課程資訊
            Dictionary<string, CourseInfo> AllCourseInfoDict = new Dictionary<string, CourseInfo>();

            // 學生修課紀錄
            Dictionary<string, Dictionary<string, SCAttendInfo>> StudSCAttendDict = new Dictionary<string, Dictionary<string, SCAttendInfo>>();

            Dictionary<string, List<string>> semesterCourseName = new Dictionary<string, List<string>>();
            List<SCAttendInfo> InsertSCAttendList = new List<SCAttendInfo>();
            List<SCAttendInfo> UpdateSCAttendList = new List<SCAttendInfo>();

            wizard.PackageLimit = 3000;

            //wizard.ImportableFields.Add("必選修");
            //wizard.ImportableFields.Add("校部訂");
            wizard.ImportableFields.Add("及格標準");
            wizard.ImportableFields.Add("補考標準");
            wizard.ImportableFields.Add("直接指定總成績");
            wizard.ImportableFields.Add("備註");
            wizard.ImportableFields.Add("科目代碼");

            wizard.RequiredFields.AddRange("學年度", "學期");
            wizard.RequiredFields.Add("課程名稱");



            wizard.ValidateStart += delegate (object sender, SmartSchool.API.PlugIn.Import.ValidateStartEventArgs e)
            {
                // 開始驗證
                #region 取得學生資訊
                StudentInfoDict = DataAccess.GetStudentInfoDictByStudentIDList(e.List.ToList());
                #endregion

                // 取得所有課程
                AllCourseInfoDict = DataAccess.GetAllCourseInfoDict();

                // 取得學生修課資料
                StudSCAttendDict = DataAccess.GetSCAttendDictByStudentIDList(e.List.ToList());

            };

            wizard.ValidateRow += delegate (object sender, SmartSchool.API.PlugIn.Import.ValidateRowEventArgs e)
            {
                int i; decimal d;

                #region 檢查學生是否存在
                //  SHStudentRecord student = null;
                if (StudentInfoDict.ContainsKey(e.Data.ID))
                {
                    // student = students[e.Data.ID];
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
                    string value = e.Data[field].Trim();
                    switch (field)
                    {
                        default:
                            break;
                        case "學年度":
                        case "學期":
                            if (value == "" || !int.TryParse(value, out i))
                            {
                                inputFormatPass = false;
                                e.ErrorFields.Add(field, "必須填入整數");
                            }
                            break;
                        case "課程名稱":
                            if (value == "")
                            {
                                inputFormatPass = false;
                                e.ErrorFields.Add(field, "必須填入課程名稱");
                            }
                            break;

                        //case "必選修":
                        //    if (value == "" || value == "必修" || value == "選修")
                        //    {
                        //    }
                        //    else
                        //    {
                        //        inputFormatPass = false;
                        //        e.ErrorFields.Add(field, "必須填入必修、選修或空白");
                        //    }
                        //    break;
                        //case "校部訂":
                        //    if (value == "" || value == "校訂" || value == "部訂")
                        //    {
                        //    }
                        //    else
                        //    {
                        //        inputFormatPass = false;
                        //        e.ErrorFields.Add(field, "必須填入校訂、部訂或空白");
                        //    }

                        //    break;
                        case "及格標準":
                        case "補考標準":
                        case "直接指定總成績":
                            if (value != "" && !decimal.TryParse(value, out d))
                            {
                                inputFormatPass = false;
                                e.ErrorFields.Add(field, "必須填入數值");
                            }
                            break;
                        case "科目代碼": break;
                    }
                }
                #endregion

                //輸入格式正確才會針對情節做檢驗
                #region 驗證各種情節
                if (inputFormatPass)
                {
                    semesterCourseName.Clear();
                    string errorMessage = "";

                    string sy = e.Data["學年度"];
                    string se = e.Data["學期"];
                    string courseName = e.Data["課程名稱"];
                    string key = e.Data.ID + "_" + sy + "_" + se;
                    string semsKey = sy + "_" + se;
                    string tmpCourseKey = sy + "_" + se + "_" + courseName;
                    string tmpCourseID = "";



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
                    if (!AllCourseInfoDict.ContainsKey(tmpCourseKey))
                    {
                        noCourse = true;
                        errorMessage += (errorMessage == "" ? "" : "\n") + " 系統中找不到該課程";
                    }
                    else
                    {
                        tmpCourseID = AllCourseInfoDict[tmpCourseKey].CourseID;
                    }

                    #endregion

                    #region 檢查學生是否有修此課程
                    bool attended = false;

                    if (StudSCAttendDict.ContainsKey(e.Data.ID))
                    {
                        if (StudSCAttendDict[e.Data.ID].ContainsKey(tmpCourseID))
                        {
                            attended = true;
                        }
                    }
                    else //學生沒修半堂課
                    {
                    }


                    #endregion
                    e.ErrorMessage = errorMessage;
                }
                #endregion
            };

            wizard.ImportPackage += delegate (object sender, SmartSchool.API.PlugIn.Import.ImportPackageEventArgs e)
            {

                InsertSCAttendList.Clear();
                UpdateSCAttendList.Clear();

                // 匯入資料
                #region 分包裝
                foreach (RowData data in e.Items)
                {
                    string sy = data["學年度"];
                    string se = data["學期"];
                    string courseName = data["課程名稱"];
                    string tmpCourseKey = sy + "_" + se + "_" + courseName;
                    string tmpCourseID = "";
                    string StudentID = data.ID;
                    SCAttendInfo si = new SCAttendInfo();
                    si.StudentID = StudentID;
                    si.IsPassingStandardCheck = si.IsMakeupStandardCheck = si.IsSubjectCodeCheck = false;
                    // 有課程才處理
                    if (AllCourseInfoDict.ContainsKey(tmpCourseKey))
                    {
                        tmpCourseID = AllCourseInfoDict[tmpCourseKey].CourseID;

                        bool hasAttend = false;

                        if (StudSCAttendDict.ContainsKey(StudentID))
                        {
                            if (StudSCAttendDict[StudentID].ContainsKey(tmpCourseID))
                            {
                                hasAttend = true;
                                si = StudSCAttendDict[StudentID][tmpCourseID];
                            }
                        }

                        if (data.ContainsKey("及格標準"))
                        {
                            if (data["及格標準"] != null)
                            {
                                if (data["及格標準"].ToString() == "")
                                {
                                    si.PassingStandard = null;
                                }
                                else
                                {
                                    decimal pp;
                                    if (decimal.TryParse(data["及格標準"].ToString(), out pp))
                                    {
                                        si.PassingStandard = pp;
                                    }
                                }
                            }
                            si.IsPassingStandardCheck = true;
                        }
                       
                        if (data.ContainsKey("補考標準"))
                        {
                            if (data["補考標準"] != null)
                            {
                                if (data["補考標準"].ToString() == "")
                                {
                                    si.MakeupStandard = null;
                                }
                                else
                                {
                                    decimal mm;
                                    if (decimal.TryParse(data["補考標準"].ToString(), out mm))
                                    {
                                        si.MakeupStandard = mm;
                                    }
                                }
                            }
                            si.IsMakeupStandardCheck = true;
                        }

                        if (data.ContainsKey("直接指定總成績"))
                        {
                            if (data["直接指定總成績"] != null)
                            {
                                if (data["直接指定總成績"].ToString() == "")
                                {
                                    si.DesignateFinalScore = null;
                                }
                                else
                                {
                                    decimal mm;
                                    if (decimal.TryParse(data["直接指定總成績"].ToString(), out mm))
                                    {
                                        si.DesignateFinalScore = mm;
                                    }
                                }
                            }
                            si.IsDesignateFinalScoreCheck = true;
                        }

                        if (data.ContainsKey("科目代碼"))
                        {
                            if (data["科目代碼"] != null)
                            {
                                si.SubjectCode = data["科目代碼"].ToString();
                            }
                            si.IsSubjectCodeCheck = true;
                        }

                        if (data.ContainsKey("備註"))
                        {
                            if (data["備註"] != null)
                            {
                                si.Remark = data["備註"].ToString();
                            }
                            si.IsRemarkCheck = true;
                        }


                        si.CourseID = tmpCourseID;

                        // 已經修課需要更新
                        if (hasAttend)
                        {
                            UpdateSCAttendList.Add(si);
                        }
                        else
                        {                            
                            InsertSCAttendList.Add(si);
                        }

                    }

                }
                #endregion


                if (InsertSCAttendList.Count > 0)
                {
                    DataAccess.InsertSCAttendList(InsertSCAttendList);
                    ApplicationLog.Log("成績系統.匯入匯出", "匯入課程修課學生", "總共匯入 新增" + InsertSCAttendList.Count + "筆課程修課學生。");
                }


                if (UpdateSCAttendList.Count > 0)
                {
                    DataAccess.UpdateSCAttendList(UpdateSCAttendList);
                    ApplicationLog.Log("成績系統.匯入匯出", "匯入課程修課學生", "總共匯入 更新" + UpdateSCAttendList.Count + "筆課程修課學生。");
                }

            };
        }

    }
}