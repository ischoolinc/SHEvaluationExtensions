using System;
using System.Collections.Generic;
using System.Text;
using SmartSchool.API.PlugIn;
using SHSchool.Data;
using FISCA.LogAgent;

namespace SHEvaluationExtensions.Course
{
    public class ExportCourseStudents : SmartSchool.API.PlugIn.Export.Exporter
    {
        private string Item { get; set; }
        public ExportCourseStudents(string item)
        {
            this.Image = null;
            Item = item;           
                this.Text = "匯出課程修課學生";
        }

        private List<string> InternalExportableFields = new List<string>();

        public override void InitializeExport(SmartSchool.API.PlugIn.Export.ExportWizard wizard)
        {
            wizard.ExportableFields.AddRange("姓名", "學號", "班級", "座號","必選修","校部訂","學生狀態");
            wizard.ExportPackage += delegate(object sender, SmartSchool.API.PlugIn.Export.ExportPackageEventArgs e)
            {
                //課程資訊
                List<SHCourseRecord> courses = SHCourse.SelectByIDs(e.List);
                //學生修課資訊
                Dictionary<string, List<SHSCAttendRecord>> scattends = new Dictionary<string, List<SHSCAttendRecord>>();
                //課程修課學生
                Dictionary<string,SHStudentRecord> students = new Dictionary<string, SHStudentRecord>();

                #region 取得修課記錄
                foreach (SHSCAttendRecord record in SHSCAttend.SelectByStudentIDAndCourseID(new string[] { }, e.List))
                {                    
                    if (!scattends.ContainsKey(record.RefCourseID))
                        scattends.Add(record.RefCourseID, new List<SHSCAttendRecord>());
                    scattends[record.RefCourseID].Add(record);

                    if (!students.ContainsKey(record.RefStudentID))
                        students.Add(record.RefStudentID, null);
                }
                #endregion

                #region 取得學生資訊
                SHStudent.RemoveAll();
                foreach (SHStudentRecord record in SHStudent.SelectByIDs(new List<string>(students.Keys)))
                {
                    if (students.ContainsKey(record.ID))
                        students[record.ID] = record;
                }
                #endregion

                #region 產生 Row Data
                foreach (SHCourseRecord course in courses)
                {
                    //Debug
                    if (!scattends.ContainsKey(course.ID)) continue;

                    foreach (SHSCAttendRecord record in scattends[course.ID])
                    {
                        RowData row = new RowData();
                        row.ID = course.ID;
                        foreach (string field in e.ExportFields)
                        {
                            if (wizard.ExportableFields.Contains(field))
                            {
                                switch (field)
                                {
                                    case "姓名": row.Add(field, students[record.RefStudentID].Name); break;
                                    case "學號": row.Add(field, students[record.RefStudentID].StudentNumber); break;
                                    case "班級": row.Add(field, (students[record.RefStudentID].Class != null ? students[record.RefStudentID].Class.Name : "")); break;
                                    case "座號": row.Add(field, "" + students[record.RefStudentID].SeatNo); break;
                                    case "必選修":
                                            if (record.Required)
                                                row.Add(field, "" + "必修");
                                            else
                                                row.Add(field, "" + "選修");
                                        break;
                                    case "校部訂":                                        
                                            row.Add(field, "" + record.RequiredBy);                                        
                                        break;
                                    case "學生狀態": row.Add(field, "" + students[record.RefStudentID].Status.ToString()); break;
                                }
                            }
                        }
                        e.Items.Add(row);
                    }
                }
                #endregion
                                
                ApplicationLog.Log("成績系統.匯入匯出", "匯出課程修課學生", "總共匯出" + e.Items.Count + "筆課程修課學生。");
            };
        }
    }
}
