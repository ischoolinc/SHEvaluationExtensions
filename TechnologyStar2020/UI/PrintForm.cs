using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using TechnologyStar2020.DAO;
using Aspose.Cells;
using System.IO;
using FISCA.UDT;


namespace TechnologyStar2020.UI
{
    public partial class PrintForm : BaseForm
    {
        BackgroundWorker bgLoadData = new BackgroundWorker();
        BackgroundWorker bgExportData = new BackgroundWorker();
        DataTable dtTable = new DataTable();
        QueryData qd = new QueryData();
        List<StudentInfo> StudentList = new List<StudentInfo>();
        Dictionary<string, Dictionary<string, StudentScoreInfo>> StudentScoreDict = new Dictionary<string, Dictionary<string, StudentScoreInfo>>();
        // 學生群對照表 DeptName key
        Dictionary<string, udtRegistrationDept> RegistrationDeptDict = new Dictionary<string, udtRegistrationDept>();

        public PrintForm()
        {
            InitializeComponent();
            // 載入資料
            bgLoadData.DoWork += BgLoadData_DoWork;
            bgLoadData.RunWorkerCompleted += BgLoadData_RunWorkerCompleted;
            bgLoadData.ProgressChanged += BgLoadData_ProgressChanged;
            bgLoadData.WorkerReportsProgress = true;

            // 產生報表
            bgExportData.DoWork += BgExportData_DoWork;
            bgExportData.RunWorkerCompleted += BgExportData_RunWorkerCompleted;
            bgExportData.ProgressChanged += BgExportData_ProgressChanged;
            bgExportData.WorkerReportsProgress = true;
        }

        private void BgExportData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("產生資料中...", e.ProgressPercentage);
        }

        private void BgExportData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MsgBox.Show("產生資料發生錯誤..," + e.Error.Message);
            }
            else
            {
                Workbook wb = (Workbook)e.Result;

                if (wb != null)
                {
                    #region 儲存檔案
                    string reportName = K12.Data.School.DefaultSchoolYear + "學年度第" + K12.Data.School.DefaultSemester + "學期 技職繁星報名檔";

                    string path = Path.Combine(System.Windows.Forms.Application.StartupPath, "Reports");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    path = Path.Combine(path, reportName + ".xls");

                    try
                    {


                        if (File.Exists(path))
                        {
                            int i = 1;
                            while (true)
                            {
                                string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                                if (!File.Exists(newPath))
                                {
                                    path = newPath;
                                    break;
                                }
                            }
                        }
                        wb.Save(path, SaveFormat.Excel97To2003);
                        System.Diagnostics.Process.Start(path);
                    }
                    catch
                    {
                        System.Windows.Forms.SaveFileDialog sd = new System.Windows.Forms.SaveFileDialog();
                        sd.Title = "另存新檔";
                        sd.FileName = reportName + ".doc";
                        sd.Filter = "Word檔案 (*.doc)|*.doc|所有檔案 (*.*)|*.*";
                        if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            try
                            {
                                wb.Save(sd.FileName, SaveFormat.Excel97To2003);

                            }
                            catch
                            {
                                FISCA.Presentation.Controls.MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    MsgBox.Show("Excel 檔案無法產生。");
                }
            }

            btnExport.Enabled = true;
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");
        }

        private void BgExportData_DoWork(object sender, DoWorkEventArgs e)
        {
            bgExportData.ReportProgress(1);
            // 讀取樣板
            Workbook wb = new Workbook(new MemoryStream(Properties.Resources.Template));
            Worksheet wstR = wb.Worksheets["報名資料"];
            Worksheet wstD = wb.Worksheets["比序資料"];

            // 比序資料:學號 0 ,班級 1 ,座號 2 ,姓名 3 ,學業平均成績 4 ,學業群名次 5 ,學業群百分比 6 ,專業及實習科目平均成績 7 ,專業及實習科目群名次 8 ,專業及實習群科目百分比 9 ,英文平均成績 10 ,英文群名次 11 ,英文群百分比 12 ,國文平均成績 13 ,國文群名次 14 ,國文群百分比 15 ,數學平均成績 16 ,數學群名次 17 ,數學群百分比 18 

            // 報名資料:序號 0 ,學號 1 ,學生姓名 2 ,群別代碼 3 ,學制代碼 4 ,科(組)、學程名稱 5 ,班級名稱 6 ,學業平均成績科(組)、學程名次 7 ,學業平均成績群名次 8 ,專業及實習科目平均成績群名次 9 ,英文平均成績群名次 10 ,國文平均成績群名次 11 ,數學平均成績群名次 12

            bgExportData.ReportProgress(5);
            int wstDrIdx = 2;
            foreach (StudentInfo si in StudentList)
            {

                // 比序資料:學號 0 
                wstD.Cells[wstDrIdx, 0].PutValue(si.StudentNumber);

                // 班級 1 
                wstD.Cells[wstDrIdx, 1].PutValue(si.ClassName);

                // 座號 2 
                wstD.Cells[wstDrIdx, 2].PutValue(si.SeatNo);

                // 姓名 3 
                wstD.Cells[wstDrIdx, 3].PutValue(si.Name);

                if (StudentScoreDict.ContainsKey(si.StudentID))
                {
                    // item_name:國文,rank_type:學群排名,rank_name:外語群
                    // key: item_name_rank_type
                    // item_name:學業、專業及實習、國文、英文、數學
                    // rank_type:學群排名,科排名

                    if (StudentScoreDict[si.StudentID].ContainsKey("學業_學群排名"))
                    {
                        // 學業平均成績 4 
                        if (StudentScoreDict[si.StudentID]["學業_學群排名"].Score.HasValue)
                            wstD.Cells[wstDrIdx, 4].PutValue(StudentScoreDict[si.StudentID]["學業_學群排名"].Score.Value);

                        // 學業群名次 5 
                        if (StudentScoreDict[si.StudentID]["學業_學群排名"].Rank.HasValue)
                            wstD.Cells[wstDrIdx, 5].PutValue(StudentScoreDict[si.StudentID]["學業_學群排名"].Rank.Value);

                        // 學業群百分比 6 
                        if (StudentScoreDict[si.StudentID]["學業_學群排名"].Percentile.HasValue)
                            wstD.Cells[wstDrIdx, 6].PutValue(StudentScoreDict[si.StudentID]["學業_學群排名"].Percentile.Value);
                    }

                    if (StudentScoreDict[si.StudentID].ContainsKey("專業及實習_學群排名"))
                    {
                        // 專業及實習平均成績 7 
                        if (StudentScoreDict[si.StudentID]["專業及實習_學群排名"].Score.HasValue)
                            wstD.Cells[wstDrIdx, 7].PutValue(StudentScoreDict[si.StudentID]["專業及實習_學群排名"].Score.Value);

                        // 專業及實習群名次 8 
                        if (StudentScoreDict[si.StudentID]["專業及實習_學群排名"].Rank.HasValue)
                            wstD.Cells[wstDrIdx, 8].PutValue(StudentScoreDict[si.StudentID]["專業及實習_學群排名"].Rank.Value);

                        // 專業及實習群百分比 9 
                        if (StudentScoreDict[si.StudentID]["專業及實習_學群排名"].Percentile.HasValue)
                            wstD.Cells[wstDrIdx, 9].PutValue(StudentScoreDict[si.StudentID]["專業及實習_學群排名"].Percentile.Value);
                    }

                    if (StudentScoreDict[si.StudentID].ContainsKey("英文_學群排名"))
                    {
                        // 英文平均成績 10 
                        if (StudentScoreDict[si.StudentID]["英文_學群排名"].Score.HasValue)
                            wstD.Cells[wstDrIdx, 10].PutValue(StudentScoreDict[si.StudentID]["英文_學群排名"].Score.Value);

                        // 英文群名次 11 
                        if (StudentScoreDict[si.StudentID]["英文_學群排名"].Rank.HasValue)
                            wstD.Cells[wstDrIdx, 11].PutValue(StudentScoreDict[si.StudentID]["英文_學群排名"].Rank.Value);

                        // 英文群百分比 12 
                        if (StudentScoreDict[si.StudentID]["英文_學群排名"].Percentile.HasValue)
                            wstD.Cells[wstDrIdx, 12].PutValue(StudentScoreDict[si.StudentID]["英文_學群排名"].Percentile.Value);
                    }

                    if (StudentScoreDict[si.StudentID].ContainsKey("國文_學群排名"))
                    {
                        // 國文平均成績 13 
                        if (StudentScoreDict[si.StudentID]["國文_學群排名"].Score.HasValue)
                            wstD.Cells[wstDrIdx, 13].PutValue(StudentScoreDict[si.StudentID]["國文_學群排名"].Score.Value);

                        // 國文群名次 14 
                        if (StudentScoreDict[si.StudentID]["國文_學群排名"].Rank.HasValue)
                            wstD.Cells[wstDrIdx, 14].PutValue(StudentScoreDict[si.StudentID]["國文_學群排名"].Rank.Value);

                        // 國文群百分比 15 
                        if (StudentScoreDict[si.StudentID]["國文_學群排名"].Percentile.HasValue)
                            wstD.Cells[wstDrIdx, 15].PutValue(StudentScoreDict[si.StudentID]["國文_學群排名"].Percentile.Value);
                    }

                    if (StudentScoreDict[si.StudentID].ContainsKey("數學_學群排名"))
                    {
                        // 數學平均成績 16 
                        if (StudentScoreDict[si.StudentID]["數學_學群排名"].Score.HasValue)
                            wstD.Cells[wstDrIdx, 16].PutValue(StudentScoreDict[si.StudentID]["數學_學群排名"].Score.Value);

                        // 數學群名次 17 
                        if (StudentScoreDict[si.StudentID]["數學_學群排名"].Rank.HasValue)
                            wstD.Cells[wstDrIdx, 17].PutValue(StudentScoreDict[si.StudentID]["數學_學群排名"].Rank.Value);

                        // 數學群百分比 18 
                        if (StudentScoreDict[si.StudentID]["數學_學群排名"].Percentile.HasValue)
                            wstD.Cells[wstDrIdx, 18].PutValue(StudentScoreDict[si.StudentID]["數學_學群排名"].Percentile.Value);
                    }
                }

                wstDrIdx++;
            }

            bgExportData.ReportProgress(50);

            int wstRrIdx = 1;
            foreach (StudentInfo si in StudentList)
            {
                // 序號 0 
                wstR.Cells[wstRrIdx, 0].PutValue(wstRrIdx);

                // 學號 1
                wstR.Cells[wstRrIdx, 1].PutValue(si.StudentNumber);

                // 學生姓名 2 
                wstR.Cells[wstRrIdx, 2].PutValue(si.Name);


                if (RegistrationDeptDict.ContainsKey(si.DeptName))
                {
                    // 群別代碼 3 
                    wstR.Cells[wstRrIdx, 3].PutValue(RegistrationDeptDict[si.DeptName].RegGroupCode);

                    // 科(組)、學程名稱 5 
                    wstR.Cells[wstRrIdx, 5].PutValue(RegistrationDeptDict[si.DeptName].RegDeptName);
                }
                // 學制代碼 4



                // 班級名稱 6
                wstR.Cells[wstRrIdx, 6].PutValue(si.ClassName);

                if (StudentScoreDict.ContainsKey(si.StudentID))
                {
                    if (StudentScoreDict[si.StudentID].ContainsKey("學業_科排名"))
                    {
                        // 學業平均成績科(組)、學程名次 7                
                        if (StudentScoreDict[si.StudentID]["學業_科排名"].Rank.HasValue)
                            wstR.Cells[wstRrIdx, 7].PutValue(StudentScoreDict[si.StudentID]["學業_科排名"].Rank.Value);
                    }


                    if (StudentScoreDict[si.StudentID].ContainsKey("學業_學群排名"))
                    {
                        // 學業平均成績群名次 8 
                        if (StudentScoreDict[si.StudentID]["學業_學群排名"].Rank.HasValue)
                            wstR.Cells[wstRrIdx, 8].PutValue(StudentScoreDict[si.StudentID]["學業_學群排名"].Rank.Value);
                    }

                    if (StudentScoreDict[si.StudentID].ContainsKey("專業及實習_學群排名"))
                    {
                        // 專業及實習科目平均成績群名次 9 
                        if (StudentScoreDict[si.StudentID]["專業及實習_學群排名"].Rank.HasValue)
                            wstR.Cells[wstRrIdx, 9].PutValue(StudentScoreDict[si.StudentID]["專業及實習_學群排名"].Rank.Value);
                    }

                    if (StudentScoreDict[si.StudentID].ContainsKey("英文_學群排名"))
                    {
                        // 英文平均成績群名次 10
                        if (StudentScoreDict[si.StudentID]["英文_學群排名"].Rank.HasValue)
                            wstR.Cells[wstRrIdx, 10].PutValue(StudentScoreDict[si.StudentID]["英文_學群排名"].Rank.Value);
                    }

                    if (StudentScoreDict[si.StudentID].ContainsKey("國文_學群排名"))
                    {
                        // 國文平均成績群名次 11
                        if (StudentScoreDict[si.StudentID]["國文_學群排名"].Rank.HasValue)
                            wstR.Cells[wstRrIdx, 11].PutValue(StudentScoreDict[si.StudentID]["國文_學群排名"].Rank.Value);
                    }
                    if (StudentScoreDict[si.StudentID].ContainsKey("數學_學群排名"))
                    {
                        // 數學平均成績群名次 12
                        if (StudentScoreDict[si.StudentID]["數學_學群排名"].Rank.HasValue)
                            wstR.Cells[wstRrIdx, 12].PutValue(StudentScoreDict[si.StudentID]["數學_學群排名"].Rank.Value);
                    }
                }


                wstRrIdx++;
            }

            wb.Worksheets.ActiveSheetIndex = wstR.Index;

            bgExportData.ReportProgress(100);
            e.Result = wb;
        }

        private void BgLoadData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("讀取學生清單與成績...", e.ProgressPercentage);
        }

        private void BgLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnExport.Enabled = true;
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");
        }

        private void BgLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            bgLoadData.ReportProgress(1);
            AccessHelper accessHelper = new AccessHelper();
            // 取得群對照
            RegistrationDeptDict.Clear();
            List<udtRegistrationDept> RegistrationDeptList = accessHelper.Select<udtRegistrationDept>();
            foreach (udtRegistrationDept data in RegistrationDeptList)
            {
                if (!RegistrationDeptDict.ContainsKey(data.DeptName))
                    RegistrationDeptDict.Add(data.DeptName, data);
            }
            // 取得學生資料
            StudentList = qd.GetStudentList();
            bgLoadData.ReportProgress(50);
            // 取得學生固定排名資料
            StudentScoreDict = qd.GetStudentScoreDict();
            bgLoadData.ReportProgress(100);
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
            this.btnExport.Enabled = false;
            bgLoadData.RunWorkerAsync();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = false;
            if (StudentList != null && StudentList.Count > 0)
            {
                bgExportData.RunWorkerAsync();
            }
            else
            {
                MsgBox.Show("沒有資料。");
                btnExport.Enabled = true;
            }
        }



    }
}
