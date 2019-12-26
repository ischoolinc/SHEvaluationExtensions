using System;
using System.Collections.Generic;
using System.Text;
using SmartSchool.API.PlugIn;
using SHSchool.Data;
using FISCA.LogAgent;
using FISCA.Data;
using System.Data;

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
            wizard.ExportableFields.AddRange("姓名", "學號", "班級", "座號","必選修","校部訂", "及格標準", "補考標準", "直接指定總成績", "備註", "科目代碼", "學生狀態");
            wizard.ExportPackage += delegate(object sender, SmartSchool.API.PlugIn.Export.ExportPackageEventArgs e)
            {
                // 課程 ID
                List<string> courseIDList = e.List;
                
                if (courseIDList.Count > 0)
                {
                    // 取得學生修課資訊
                    Dictionary<string, Dictionary<string, DataRow>> SCAttendDict = new Dictionary<string, Dictionary<string, DataRow>>();

                    // 取得學生修課資料
                    string qrySCAttend = "SELECT " +
                    "student.name AS student_name" +
                    ",student.student_number" +
                    ",class.class_name" +
                    ",student.seat_no" +
                    ",CASE is_required WHEN '1' THEN '必修' WHEN '0' THEN '選修' ELSE '' END AS is_required" +
                    ",CASE required_by WHEN 1 THEN '部訂' WHEN 2 THEN '校訂' ELSE '' END AS required_by" +
                    ",ref_course_id AS course_id" +
                    ",ref_student_id AS student_id" +
                    ",passing_standard" +
                    ",makeup_standard" +
                    ",remark" +
                    ",designate_final_score" +
                    ",subject_code" +
                    ",CASE student.status WHEN 1 THEN '一般' WHEN 2 THEN '延修' WHEN 4 THEN '休學' WHEN 8 THEN '輟學' WHEN 16 THEN '畢業或離校' WHEN 256 THEN '刪除' END AS status" +
                    " FROM sc_attend INNER JOIN student" +
                    " ON sc_attend.ref_student_id = student.id INNER JOIN class" +
                    " ON student.ref_class_id = class.id  WHERE ref_course_id IN(" + string.Join(",", courseIDList.ToArray()) + ")" +
                    " ORDER BY class_name,seat_no,student_number;";

                    QueryHelper qh = new QueryHelper();
                    
                    DataTable dtSCAttend = qh.Select(qrySCAttend);

                    #region 產生 Row Data
                    foreach (DataRow dr in dtSCAttend.Rows)
                    {
                        RowData row = new RowData();
                        row.ID = GetFieldString(dr, "course_id");
                        foreach (string field in e.ExportFields)
                        {
                            if (wizard.ExportableFields.Contains(field))
                            {
                                switch (field)
                                {
                                    case "姓名": row.Add(field, GetFieldString(dr, "student_name")); break;
                                    case "學號": row.Add(field, GetFieldString(dr, "student_number")); break;
                                    case "班級": row.Add(field, GetFieldString(dr, "class_name")); break;
                                    case "座號":row.Add(field, GetFieldString(dr, "seat_no"));break;
                                    case "必選修":row.Add(field, GetFieldString(dr, "is_required"));break;
                                    case "校部訂":row.Add(field, GetFieldString(dr, "required_by"));break;
                                    case "及格標準": row.Add(field, GetFieldString(dr, "passing_standard")); break;
                                    case "補考標準": row.Add(field, GetFieldString(dr, "makeup_standard")); break;
                                    case "直接指定總成績": row.Add(field, GetFieldString(dr, "designate_final_score")); break;
                                    case "備註": row.Add(field, GetFieldString(dr, "remark")); break;
                                    case "科目代碼": row.Add(field, GetFieldString(dr, "subject_code")); break;
                                    case "學生狀態": row.Add(field, GetFieldString(dr, "status")); break;
                                }
                            }
                        }
                        e.Items.Add(row);

                    }

                    #endregion

                    ApplicationLog.Log("成績系統.匯入匯出", "匯出課程修課學生", "總共匯出" + e.Items.Count + "筆課程修課學生。");
                }                
            };
        }

        private string GetFieldString(DataRow dr,string name)
        {
            string value = "";
            if (dr[name] != null)
                value = dr[name].ToString();
            return value;
        }
        
    }
}
