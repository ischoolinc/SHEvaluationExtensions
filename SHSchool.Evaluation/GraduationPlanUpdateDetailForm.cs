using FISCA.Presentation.Controls;
using SHSchool.Evaluation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SHSchool.Evaluation
{
    public partial class GraduationPlanUpdateDetailForm : BaseForm
    {

        internal GraduationPlanInfo GraduationPlanInfo;
        internal OldGraduationPlanInfo CurrentGraduationPlan;


        public GraduationPlanUpdateDetailForm(GraduationPlanInfo graduationPlanInfo, string action)
        {
            InitializeComponent();

            this.GraduationPlanInfo = graduationPlanInfo;

            // 如果 
            if (action == "差異更新")
            {
                this.LoadCombo();
                this.CurrentGraduationPlan = ((OldGraduationPlanInfo)(this.cboGraduationName.SelectedItem));
                checkBoxShowOnly.Checked = true;
                this.FillDataGridView(this.GraduationPlanInfo.DicOldGraduationPlanInfos[CurrentGraduationPlan.SysID].OldContentXml, this.checkBoxShowOnly.Checked);
            }
            else if (action == "新增")
            {
                this.SettView(action);
                this.FillDataGridViewShowNew();
            }
        }

        public void  SettView(string action) 
        {
            if (action == "新增") 
            {
                // 畫面設定前三行不用顯示
                labelX1.Text = "課程規劃表名稱";
                _DomainIndex.Visible = false;
                科目代碼_原.Visible = false;
                科目_原.Visible = false;
                授課學期學分_新.Visible = true;
                修改欄位.Visible = false;
                Action.Visible = false;
                this.checkBoxShowOnly.Visible = false;
                this.cboGraduationName.Enabled = false;
            }
        }




        private void LoadCombo()
        {
            if (GraduationPlanInfo.DicOldGraduationPlanInfos.Count == 0)
            {
                return;
            }
            this.cboGraduationName.Items.AddRange(GraduationPlanInfo.DicOldGraduationPlanInfos.Values.ToArray());
            this.cboGraduationName.SelectedIndex = 0;
        }


        /// <summary>
        /// FillDataGridView
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ShowOnlyUpdate"></param>
        public void FillDataGridView(System.Xml.XmlElement source, bool ShowOnlyUpdate)
        {
            // 抓取 屬性
            // Dictionary<string, string> dicAttrubutes = new Dictionary<string, string>();
            dataGridViewX1.Rows.Clear();

            Dictionary<string, DataGridViewRow> rowDictionary = new Dictionary<string, DataGridViewRow>();
            if (source != null)
            {
                string currentSubjCode = "";
                int numRow = 1;
                foreach (XmlNode node in source.SelectNodes("Subject"))
                {
                    DataGridViewRow row;
                    XmlElement element = (XmlElement)node;
                    XmlNode groupNode = element.SelectSingleNode("Grouping");
                    //檢查是否符合群組設定
                    if (groupNode != null && groupNode.SelectSingleNode("@RowIndex") != null && groupNode.SelectSingleNode("@startLevel") != null)
                    {
                        #region 以第一筆資料為主填入各級別學年度學期學分數
                        XmlElement groupElement = (XmlElement)groupNode;
                        if (!rowDictionary.ContainsKey(groupElement.Attributes["RowIndex"].InnerText))
                        {
                            row = new DataGridViewRow();
                            row.CreateCells(dataGridViewX1);
                            row.Cells[_DomainIndex.Index].Value = element.Attributes["Domain"].InnerText;
                            string OldSubjectCode = element.Attributes["課程代碼"].InnerText;
                            if (currentSubjCode == OldSubjectCode) // 如果上一筆
                            {
                                continue;
                            }
                            currentSubjCode = OldSubjectCode; // 去除相對科目重複 
                            row.Cells[科目代碼_原.Index].Value = OldSubjectCode;
                            row.Cells[科目_原.Index].Value = element.Attributes["SubjectName"].InnerText;
                            row.Cells[授課學期學分.Index].Value = element.HasAttribute("授課學期學分") ? element.Attributes["授課學期學分"].InnerText : "";

                            // 如果有更新
                            if (CurrentGraduationPlan.UpdateCourseInfos.Any(x => x.OldSubjectCode == OldSubjectCode))
                            {
                                row.Cells[科目代碼_新.Index].Value = CurrentGraduationPlan.UpdateCourseInfos.Find(x => x.OldSubjectCode == OldSubjectCode).NewSubjectCode;
                                row.Cells[科目_新.Index].Value = CurrentGraduationPlan.UpdateCourseInfos.Find(x => x.OldSubjectCode == OldSubjectCode).NewSubjectName;
                                row.Cells[Action.Index].Value = CurrentGraduationPlan.UpdateCourseInfos.Find(x => x.OldSubjectCode == OldSubjectCode).NewCourseInfo.Action;
                                // 異動欄位
                                row.Cells[修改欄位.Index].Value = CurrentGraduationPlan.UpdateCourseInfos.Find(x => x.OldSubjectCode == OldSubjectCode).GetUpdateColumns();
                                row.Cells[備註.Index].Value = CurrentGraduationPlan.UpdateCourseInfos.Find(x => x.OldSubjectCode == OldSubjectCode).GetErrorInfo();

                                if (row.Cells[Action.Index].Value.ToString() == (EnumAction.修改授課學期學分_代碼.ToString()))
                                {
                                    row.Cells[備註.Index].Value = "此動作只更新代碼(紀錄用)，實際授課學期/學分數之異動 請人員手動調整。";
                                    row.Cells[備註.Index].Style.ForeColor = Color.Red;
                                }

                            }
                            // 如果有刪除
                            if (CurrentGraduationPlan.DeleteCourseInfos.Any(x => x.OldSubjectCode == OldSubjectCode))
                            {
                                row.Cells[Action.Index].Value = CurrentGraduationPlan.UpdateCourseInfos.Find(x => x.OldSubjectCode == OldSubjectCode).NewCourseInfo.Action;
                            }


                            if (((row.Cells[Action.Index].Value == null) && ShowOnlyUpdate))
                            {
                                continue;
                            }
                            // todo 取得
                            // 就版沒有下面 Attributes
                            #region 配合課程代碼匯入 放入屬性

                            #endregion

                            dataGridViewX1.Rows.Add(row);

                        }
                        #endregion

                    }
                    numRow++;
                }



                // 新增科目部分 
                if (CurrentGraduationPlan.InsertCourseInfos.Count != 0)
                {
                    DataGridViewRow row;


                    foreach (UpdateCourseInfo oldCourseInfo in CurrentGraduationPlan.InsertCourseInfos)
                    {
                        row = new DataGridViewRow();
                        row.CreateCells(dataGridViewX1);
                        row.Cells[科目代碼_新.Index].Value = oldCourseInfo.NewSubjectCode;
                        row.Cells[科目_新.Index].Value = oldCourseInfo.NewSubjectName;
                        row.Cells[Action.Index].Value = oldCourseInfo.NewCourseInfo.Action;
                        dataGridViewX1.Rows.Add(row);
                    }

                }
            }
            else
            {







            }

        }


        /// <summary>
        ///  顯示新增畫面
        /// </summary>
        public void FillDataGridViewShowNew()
        {
            this.cboGraduationName.Text = GraduationPlanInfo.GraduationName;

            foreach (CourseInfo  courseInfo in this.GraduationPlanInfo.ListCourseInfos)
            {
                DataGridViewRow row = new DataGridViewRow();    

                row.CreateCells(dataGridViewX1);
                row.Cells[科目代碼_新.Index].Value = courseInfo.New課程代碼 ;
                row.Cells[科目_新.Index].Value = courseInfo.NewSubjectName ;
                row.Cells[授課學期學分_新.Index].Value = courseInfo.授課學期學分;
                row.Cells[備註.Index].Value = "" ;




                dataGridViewX1.Rows.Add(row);
            }


        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cboGraduationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentGraduationPlan = ((OldGraduationPlanInfo)(this.cboGraduationName.SelectedItem));

            this.FillDataGridView(this.GraduationPlanInfo.DicOldGraduationPlanInfos[CurrentGraduationPlan.SysID].OldContentXml, this.checkBoxShowOnly.Checked);
        }

        private void checkBoxShowOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.CurrentGraduationPlan = ((OldGraduationPlanInfo)(this.cboGraduationName.SelectedItem));

            this.FillDataGridView(this.GraduationPlanInfo.DicOldGraduationPlanInfos[CurrentGraduationPlan.SysID].OldContentXml, this.checkBoxShowOnly.Checked);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
