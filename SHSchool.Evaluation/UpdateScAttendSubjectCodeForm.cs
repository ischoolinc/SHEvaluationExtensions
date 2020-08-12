using FISCA.Presentation.Controls;
using SHSchool.Evaluation.Model;
using SHSchool.Evaluation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SHSchool.Evaluation
{
    public partial class UpdateScAttendSubjectCodeForm : BaseForm
    {
        Dictionary<string, GraduationPlanInfo> GraduationPlanInfos;
        DataService DataService = new DataService();
        public UpdateScAttendSubjectCodeForm(Dictionary<string, GraduationPlanInfo> GraduationPlanInfos)
        {
            InitializeComponent();
            this.GraduationPlanInfos = GraduationPlanInfos;
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UpdateScAttendSubjectCodeForm_Load(object sender, EventArgs e)
        {

            this.SettingCombo();
            //整理有哪些新code 需要update And 刪除;
          //  FillDataGridView(this.cboShowWay.Text, this.GraduationPlanInfos);
        }


        public void SettingCombo()
        {
            this.cboShowWay.Items.Clear();
            this.cboShowWay.Items.Add("顯示總覽");
            this.cboShowWay.Items.Add("選示詳細資料");
            this.cboShowWay.SelectedIndex = 0;

        }

        public void FillDataGridView(string selectText, List<string> subjectCode)
        {
            Dictionary<string, ScAttandCourseInfo> dicScAttandCourseInfo = new Dictionary<string, ScAttandCourseInfo>();
            //將資料取回來
            DataTable dt = DataService.GetScAttendUpdateTarget(subjectCode);
            //轉成物件
            foreach (DataRow dr in dt.Rows)
            {
                ScAttendInfo scAttendInfo = new ScAttendInfo("" + dr["sc_attend_id"]);

                scAttendInfo.RefCourseID = "" + dr["ref_course_id"];
                scAttendInfo.CourseName = "" + dr["course_name"];
                scAttendInfo.Subject = "" + dr["subject"];
                scAttendInfo.SchoolYear = "" + dr["school_year"];
                scAttendInfo.Semester = "" + dr["semester"];
                scAttendInfo.RefStudentID = "" + dr["student_number"];
                scAttendInfo.StudentNumber = "" + dr["ref_student_id"];
                scAttendInfo.StudentClass = "" + dr["student_class"];
                scAttendInfo.SeatNo = "" + dr["seat_no"];
                scAttendInfo.StudentName = "" + dr["student.name "];
                scAttendInfo.OldSubjectCode = "" + dr[".subject_code"];

                if (!dicScAttandCourseInfo.ContainsKey(scAttendInfo.RefCourseID))  //如果不存在此課程
                {
                    dicScAttandCourseInfo.Add(scAttendInfo.RefCourseID, new ScAttandCourseInfo(scAttendInfo.RefCourseID));
                }
                dicScAttandCourseInfo[scAttendInfo.RefCourseID].AddScAttandCourseInfo(scAttendInfo);
            }
            // 將資料物件化
            if (selectText == "顯示總覽")
            {
                SettingView(selectText);
                foreach (string coursrID in dicScAttandCourseInfo.Keys)
                {
                    ScAttandCourseInfo scAttandCourseInfo = dicScAttandCourseInfo[coursrID];
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridViewX1);
                    row.Cells[學年度.Index].Value = scAttandCourseInfo.SchoolYear; // 入學年度
                    row.Cells[學期.Index].Value = scAttandCourseInfo.Semester; // 課程類型 [　H普通高中/ V技術型  /M 綜合高中 ] 等等
                    row.Cells[課程名稱.Index].Value = scAttandCourseInfo.CourseName;  // 課程規劃表名稱
                    row.Cells[科目.Index].Value = scAttandCourseInfo.Subject;  //
                    row.Cells[學生人數.Index].Value = scAttandCourseInfo.ListScAttandCourseInfo.Count;

                    this.dataGridViewX1.Rows.Add(row);
                }
            }
            else if (selectText == "選示詳細資料")
            {
                SettingView(selectText);

                foreach (string coursrID in dicScAttandCourseInfo.Keys)
                {
                    ScAttandCourseInfo scAttandCourseInfo = dicScAttandCourseInfo[coursrID];
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridViewX1);
                    row.Cells[學年度.Index].Value = scAttandCourseInfo.SchoolYear; // 入學年度
                    row.Cells[學期.Index].Value = scAttandCourseInfo.Semester; // 課程類型 [　H普通高中/ V技術型  /M 綜合高中 ] 等等
                    row.Cells[課程名稱.Index].Value = scAttandCourseInfo.CourseName;  // 課程規劃表名稱
                    row.Cells[科目.Index].Value = scAttandCourseInfo.Subject;  //
                    row.Cells[學生人數.Index].Value = scAttandCourseInfo.ListScAttandCourseInfo.Count;
                    this.dataGridViewX1.Rows.Add(row);
                }
            }
        }


        public void SettingView(string showWay)
        {
            if (showWay == "顯示總覽")
            {
                this.班級.Visible = false;
                this.座號.Visible = false;
                this.學號.Visible = false;
                this.姓名.Visible = false;
            }
            else
            {
                this.學生人數.Visible = false;
            }
        }
    }
}
