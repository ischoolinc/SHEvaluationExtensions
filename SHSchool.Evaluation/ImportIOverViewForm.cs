using Aspose.Cells;
using FISCA.Data;
using FISCA.Presentation.Controls;
using SHSchool.Evaluation.Model;
using SHSchool.Evaluation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SHSchool.Evaluation
{
    public partial class ImportGraduationPlanInfoForm : BaseForm
    {
        /// <summary>
        /// CSV 讀取進來之課程規劃表
        /// </summary>
        Dictionary<string, GraduationPlanInfo> NewGraduationInfos;
        QueryHelper QHelper = new QueryHelper();
        DataService DataService = new DataService(); 
        List<string> EntryYears;
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="dicGraduationPlan"></param>
        /// <param name="entryYear"></param>
        public ImportGraduationPlanInfoForm(Dictionary<string, GraduationPlanInfo> dicGraduationPlan, List<string> entryYear)
        {
            InitializeComponent();
            this.NewGraduationInfos = dicGraduationPlan;
            this.EntryYears = entryYear;

        }

        private void ImportInfoForm_Load(object sender, EventArgs e)
        {
            this.LoadExistGraduationPlan(this.EntryYears); // 載入現有資料庫同入學年度資料
            this.LoadingDataGrivew(); // 載入DataGrivew  
        }



        private void LoadingDataGrivew()
        {
            foreach (string courseCode in NewGraduationInfos.Keys) //　以匯入之課程規劃表為基準
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);
                dgvrow.Cells[EntrySchoolYear.Index].Value = NewGraduationInfos[courseCode].EntryYear; // 入學年度
                dgvrow.Cells[CourseType.Index].Value = NewGraduationInfos[courseCode].CourseType; // 課程類型 [　H普通高中/ V技術型  /M 綜合高中 ] 等等
                dgvrow.Cells[GraduationPlanName.Index].Value = NewGraduationInfos[courseCode].GraduationName;  // 課程規劃表名稱
                dgvrow.Cells[GraduatePlanCode.Index].Value = NewGraduationInfos[courseCode].GraduationPlanKey;  //
                dgvrow.Cells[已存在課程規劃表數量.Index].Value = NewGraduationInfos[courseCode].DicOldGraduationPlanInfos.Count;

                if (NewGraduationInfos[courseCode].GetAllOldGraduationPlan().Count != 0)
                {
                    List<string> GPlanName = (NewGraduationInfos[courseCode].GetAllOldGraduationPlan().Values).Select(x => x.GraduationName).ToList();
                    dgvrow.Cells[OldGraduPlanName.Index].Value = string.Join(" , ", GPlanName);
                    dgvrow.Cells[動作.Index].Value = "差異更新";
                }
                else
                {
                    dgvrow.Cells[動作.Index].Value = "新增";
                    (dgvrow.Cells[CheakDetail.Index] as DataGridViewButtonCell).ReadOnly = true;
                }
                dgvrow.Tag = NewGraduationInfos[courseCode]; //把課程規劃表資料放入Tag
                this.dataGridViewX1.Rows.Add(dgvrow);
            }
        }





        /// <summary>
        /// 依據學年度學期 取得系統內是否有 相同code 課程代碼
        /// </summary>
        /// <param name="schoolYear"></param>
        private void LoadExistGraduationPlan(List<string> schoolYear) 
        {

            string sql = @" 
  WITH   all_subject    AS( 
 SELECT
		id
		,entry_year
		, name
        , content
		, array_to_string(xpath('//Subject/@SubjectName', subject_ele), '')::text AS 科目
		, array_to_string(xpath('//Subject/@課程代碼', subject_ele), '')::text AS  課程代碼
		, substring(array_to_string(xpath('//Subject/@課程代碼', subject_ele), '')::text   from 1 for 16)  AS graduation_plan_key
FROM
    (
        SELECT 
            id
            , name
            , content
			, unnest(xpath('//GraduationPlan/@SchoolYear', xmlparse(content content)))  ::TEXT as entry_year
            , unnest(xpath('//GraduationPlan/Subject', xmlparse(content content))) as subject_ele
        FROM 
            graduation_plan
	
    ) AS graduation_plan"
+ $" WHERE entry_year IN( '{string.Join("','", schoolYear)}') " +
@"ORDER BY 
    id
) SELECT 
        id
		, entry_year 
        , content 
		, name
	    , graduation_plan_key
	FROM   
		all_subject
  GROUP BY 
        id
  		, entry_year 
        , content
		, name
	    , graduation_plan_key
HAVING 
	char_length( graduation_plan_key) = 16
";
            DataTable dt = QHelper.Select(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string graduationplanID = "" + dr["id"];
                string graduationPlanName = "" + dr["name"];
                string graduationPlanCode = "" + dr["graduation_plan_key"];
                string content = "" + dr["content"];
                string deptCode = graduationPlanCode.Substring(12, 3);
                string CourseType = graduationPlanCode.Substring(9, 1);

                if (this.NewGraduationInfos.ContainsKey(graduationPlanCode)) //如果已經有存在之課程規劃表
                {
                    if (deptCode != "196") // 如果是一年級不分群 ==> 排除(不加到主key)
                    {
                        if (!this.NewGraduationInfos[graduationPlanCode].DicOldGPlansContain(graduationplanID)) // 如果裝舊課程規劃表沒有這個id
                        {
                            this.NewGraduationInfos[graduationPlanCode].AddOldGraduationPlan(new OldGraduationPlanInfo(graduationplanID, graduationPlanName, content));
                        }
                        else
                        {
                            // 同一課程規劃表中有兩種以上 課程規劃表Key
                        }
                    }
                    else // 196 為不分班群 
                    {

                    }
                }
            }
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dataGridViewX1.Columns[e.ColumnIndex].HeaderText != "檢視")
            {
                return;
            }
            //跳出新視窗
            //取得動作 
            string action = dataGridViewX1.Rows[e.RowIndex].Cells[動作.Index].Value.ToString();
            var selectedItem = this.dataGridViewX1.Rows[e.RowIndex].Tag as GraduationPlanInfo;
            GraduationPlanUpdateDetailForm graduationPlanUpdateDetailForm = new GraduationPlanUpdateDetailForm(selectedItem, action);
            graduationPlanUpdateDetailForm.ShowDialog();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            // 組sql 跟 新增
           List<string > finishGplan= InsertAndUpdate();
           MsgBox.Show($"{string.Join(" ,\n",finishGplan)}\n更新完成!");
           this.Close();
        }

        /// <summary>
        /// 開始新增及更新
        /// </summary>
        public List<string> InsertAndUpdate()
        {
            List<string> FinishUpdateList = new List<string>(); //儲存更新增用
            foreach (string gPlanCode in NewGraduationInfos.Keys) // 每個資料讀進來整理過後的檔案
            {
                if (NewGraduationInfos[gPlanCode].DicOldGraduationPlanInfos.Count != 0) // 如果系統已經有相同課程規劃表
                {
                    foreach (string oldGPlanCode in NewGraduationInfos[gPlanCode].DicOldGraduationPlanInfos.Keys) // 每個Old gPlan
                    {
                        OldGraduationPlanInfo OldGraduationPlanInfo = NewGraduationInfos[gPlanCode].DicOldGraduationPlanInfos[oldGPlanCode];
                        OldGraduationPlanInfo.MakeUpdateXml();  //  產生update XML 
                        try
                        {
                            DataService.UpdateGraduationPlan(OldGraduationPlanInfo.SysID, OldGraduationPlanInfo.GraduationName, OldGraduationPlanInfo.UpdateContentXml.OuterXml);
                            FinishUpdateList.Add(OldGraduationPlanInfo.GraduationName);
                        }
                        catch (Exception ex)
                        {
                            MsgBox.Show($"更新 課程規劃表 : 【{ OldGraduationPlanInfo.GraduationName}】 時 發生錯誤。 \n{ex.Message}");
                        }
                    }
                }
                else  // 系統內部沒有 ==> 新增
                {
                    try
                    {
                        string NewXmlContent = DataService.GetNewGraduationContent(NewGraduationInfos[gPlanCode]);
                        DataService.InsertGraduationPlan(NewGraduationInfos[gPlanCode].GraduationName, NewXmlContent);
                        FinishUpdateList.Add(NewGraduationInfos[gPlanCode].GraduationName);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show($"新增 課程規劃表 :{NewGraduationInfos[gPlanCode].GraduationName} 發生錯誤。 \n{ex.Message}");
                    }
                }
            }
            return FinishUpdateList;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<CourseInfoExport>> courseInfoModels = new Dictionary<string, List<CourseInfoExport>>();
            List<string> selectedGraduationID = new List<string>();
            List<string> noDatas = new List<string>();
            #region 取得ID
            if (this.dataGridViewX1.SelectedRows.Count > 0) //檢查筆數
            {
                foreach (DataGridViewRow row in this.dataGridViewX1.SelectedRows)
                {
                    GraduationPlanInfo graduationPlanInfo = row.Tag as GraduationPlanInfo;
                    if (graduationPlanInfo.DicOldGraduationPlanInfos.Count > 0)
                    {
                        foreach (string oldSysID in graduationPlanInfo.DicOldGraduationPlanInfos.Keys)
                        {
                            selectedGraduationID.Add(oldSysID); //把舊系統ID 給他
                        }
                    }
                    else
                    {
                        noDatas.Add(graduationPlanInfo.GraduationName);
                    }
                }
            }
            else // 選擇筆數
            {
                foreach (DataGridViewCell cell in this.dataGridViewX1.SelectedCells)
                {
                    DataGridViewRow row = dataGridViewX1.Rows[cell.RowIndex];
                    GraduationPlanInfo graduationPlanInfo = row.Tag as GraduationPlanInfo;
                    if (graduationPlanInfo.DicOldGraduationPlanInfos.Count > 0)
                    {
                        foreach (string graduationName in graduationPlanInfo.DicOldGraduationPlanInfos.Keys)
                        {
                            selectedGraduationID.Add(graduationName);
                        }
                    }
                    else
                    {
                        noDatas.Add(graduationPlanInfo.Name);
                    }
                }
            }

            if (noDatas.Count > 0)
            {
                MsgBox.Show($"{string.Join("\n", noDatas)} \n沒有對應舊課程規表");
                return;
            }
            #endregion

            #region 取得資料
            DataTable dt = DataService.GetOldGraduationInfosByID(selectedGraduationID); // 資料庫撈資料
            foreach (DataRow dr in dt.Rows)
            {
                CourseInfoExport courseInfoExport = new CourseInfoExport();
                courseInfoExport.ID = "" + dr["ID"];
                courseInfoExport.課程規劃表名稱 = "" + dr["name"];
                courseInfoExport.領域名稱 = "" + dr["領域"];
                courseInfoExport.學期 = "" + dr["學期"];
                courseInfoExport.分項名稱 = "" + dr["分項"];
                courseInfoExport.年級 = "" + dr["年級"];
                courseInfoExport.科目名稱 = "" + dr["科目"];
                courseInfoExport.科目級別 = "" + dr["科目級別"];
                courseInfoExport.校訂部訂 = "" + dr["校部訂"];
                courseInfoExport.必選修 = "" + dr["必選修"];
                courseInfoExport.學分數 = "" + dr["學分數"];
                courseInfoExport.不計學分 = "" + dr["不計學分"] == "True" ? "是" : "";
                courseInfoExport.不需評分 = "" + dr["不需評分"] == "True" ? "是" : "";
                courseInfoExport.科目代碼 = "" + dr["課程代碼"];

                if (!courseInfoModels.ContainsKey(courseInfoExport.課程規劃表名稱))
                {
                    courseInfoModels.Add(courseInfoExport.課程規劃表名稱, new List<CourseInfoExport>());
                }
                courseInfoModels[courseInfoExport.課程規劃表名稱].Add(courseInfoExport);
            }
            #endregion

            // 裝進Excel
            foreach (string graduationName in courseInfoModels.Keys)
            {
                int rowNum = 1;
                Workbook template = new Workbook(new MemoryStream(Properties.Resources.匯出課程規劃表樣版));
                foreach (CourseInfoExport courseInfo in courseInfoModels[graduationName])
                {
                    int cols = 0;
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.課程規劃表名稱);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.領域名稱);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.分項名稱);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.年級);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.學期);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.科目名稱);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.科目級別);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.校訂部訂);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.必選修);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.學分數);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.不計學分);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.不需評分);
                    template.Worksheets[0].Cells[rowNum, cols++].PutValue(courseInfo.科目代碼);
                    rowNum++;
                }
                Report(template, graduationName); //產出 excel
            }
        }


        /// <summary>
        /// 產出Excel檔
        /// </summary>
        /// <param name="template"></param>
        /// <param name="Name"></param>
        private void Report(Workbook template, string graduationPName)
        {
            string path = Path.Combine(Application.StartupPath, "Reports");
            //如果目錄不存在則建立。
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, $"匯出課程規劃表_{graduationPName}_{DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx");
            try
            {
                template.Save(path);
            }
            catch (IOException)
            {
                try
                {
                    FileInfo file = new FileInfo(path);
                    string nameTempalte = file.FullName.Replace(file.Extension, "") + "{0}.xlsx";
                    int count = 1;
                    string fileName = string.Format(nameTempalte, count);
                    while (File.Exists(fileName))
                        fileName = string.Format(nameTempalte, count++);

                    template.Save(fileName);
                    path = fileName;
                }
                catch (Exception ex)
                {
                    MsgBox.Show("檔案儲存失敗:" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show("檔案儲存失敗:" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MsgBox.Show("檔案開啟失敗:" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}




