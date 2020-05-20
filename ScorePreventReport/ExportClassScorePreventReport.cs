using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation;
using FISCA.Presentation.Controls;
using K12.Data;
using FISCA.Data;
using Aspose.Cells;
using System.IO;
using System.Xml.Linq;

namespace ScorePreventReport
{
    class ExportClassScorePreventReport
    {
        private List<string> listClassID = new List<string>();
        private Workbook wbTemplate;
        private BackgroundWorker bgWorker = new BackgroundWorker();
        private QueryHelper qh = new QueryHelper();
        //private Dictionary<string, CoreSubjectRec> dicCoreSubjectByKey = new Dictionary<string, CoreSubjectRec>();
        private Dictionary<string, StudentRec> dicStudentRecByID = new Dictionary<string, StudentRec>();
        private Dictionary<string, Dictionary<string, Dictionary<string, CreditRec>>> dicCreditRecByClassStuKey = new Dictionary<string, Dictionary<string, Dictionary<string, CreditRec>>>();

        Dictionary<string, List<string>> ruleCoreSubjectNameDict = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> ruleCoreSubjectTableDict = new Dictionary<string, List<string>>();

        // 班級學生成績計算規則ID
        Dictionary<string, string> studentScoreRuleDict = new Dictionary<string, string>();


        public ExportClassScorePreventReport(List<string> _listClassID)
        {
            // 載入樣板
            Stream wbStream = new MemoryStream(Properties.Resources.ClassPreventScore_template);
            wbTemplate = new Workbook(wbStream);

            InitializeBackgroundWorker();

            listClassID = _listClassID;
        }

        private void InitializeBackgroundWorker()
        {
            bgWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
        }

        private void GetClassStudentSemsScore()
        {
            #region SQL
            string sql = string.Format(@"
WITH target_student AS (
	SELECT
		id
        , name
        , seat_no
        , ref_class_id AS class_id
	FROM
		student
	WHERE
		ref_class_id IN( {0} ) 
        AND status IN(1, 2)
) ,target_score AS(
    SELECT
	    target_student.id AS student_id
        , target_student.name AS student_name
        , target_student.seat_no
        , target_student.class_id
	    , sems_subj_score_ext.grade_year
	    , sems_subj_score_ext.semester
	    , sems_subj_score_ext.school_year
	    , array_to_string(xpath('/Subject/@科目', subj_score_ele), '')::text AS 科目
	    , array_to_string(xpath('/Subject/@科目級別', subj_score_ele), '')::text AS 科目級別
	    , array_to_string(xpath('/Subject/@開課學分數', subj_score_ele), '')::text AS 學分數
	    , array_to_string(xpath('/Subject/@是否取得學分', subj_score_ele), '')::text AS 是否取得學分
	    , array_to_string(xpath('/Subject/@修課必選修', subj_score_ele), '')::text AS 必選修
	    , array_to_string(xpath('/Subject/@不計學分', subj_score_ele), '')::text AS 不計學分
    FROM 
        target_student
        LEFT OUTER JOIN (
		    SELECT 
			    sems_subj_score.*
			    , 	unnest(xpath('/SemesterSubjectScoreInfo/Subject', xmlparse(content score_info))) as subj_score_ele
		    FROM 
			    sems_subj_score
	    ) as sems_subj_score_ext
            ON sems_subj_score_ext.ref_student_id = target_student.id
    ORDER BY 
        target_student.class_id
        , target_student.seat_no
)
SELECT 
    * 
FROM 
    target_score
WHERE
    不計學分 = '否'
            ", string.Join(",", listClassID));
            #endregion

            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                string classID = "" + row["class_id"];
                string studentID = "" + row["student_id"];
                string studentName = "" + row["student_name"];
                string seatNo = "" + row["seat_no"];
                string gradeYear = "" + row["grade_year"];
                string semester = "" + row["semester"];
                string key = $"{gradeYear}_{semester}";
                string subjectName = "" + row["科目"];
                string level = "" + row["科目級別"];
                float credit = float.Parse("" + row["學分數"] == "" ? "0" : "" + row["學分數"]);
                string mode = "" + row["必選修"];
                bool isGetCredit = "" + row["是否取得學分"] == "是" ? true : false;

                #region 學生學分數整理
                if (!dicCreditRecByClassStuKey.ContainsKey(classID))
                {
                    dicCreditRecByClassStuKey.Add(classID, new Dictionary<string, Dictionary<string, CreditRec>>());
                }
                Dictionary<string, Dictionary<string, CreditRec>> dicStuCreditRec = dicCreditRecByClassStuKey[classID];

                if (!dicStuCreditRec.ContainsKey(studentID))
                {
                    dicStuCreditRec.Add(studentID, new Dictionary<string, CreditRec>());
                }
                Dictionary<string, CreditRec> dicCreditRecByKey = dicStuCreditRec[studentID];

                if (!dicCreditRecByKey.ContainsKey(key))
                {
                    dicCreditRecByKey.Add(key, new CreditRec() { TotalCredit = 0, CoredCredit = 0, HaveToCredit = 0, SelectedCredit = 0 });
                }
                CreditRec creditRec = dicCreditRecByKey[key];
                creditRec.TotalCredit += credit;
                if (isGetCredit)
                {
                    if (studentScoreRuleDict.ContainsKey(studentID))
                    {
                        if (ruleCoreSubjectNameDict.ContainsKey(studentScoreRuleDict[studentID]))
                        {
                            if (ruleCoreSubjectNameDict[studentScoreRuleDict[studentID]].Contains($"{subjectName}_{level}"))
                            {
                                // 核心
                                creditRec.CoredCredit += credit;
                            }
                        }
                    }


                    //if (dicCoreSubjectByKey.ContainsKey($"{subjectName}_{level}"))
                    //{
                    //    // 核心
                    //    creditRec.CoredCredit += credit;
                    //}
                    switch (mode)
                    {
                        case "必修":
                            creditRec.HaveToCredit += credit;
                            break;
                        case "選修":
                            creditRec.SelectedCredit += credit;
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region 學生基本資料整理
                if (!dicStudentRecByID.ContainsKey(studentID))
                {
                    StudentRec stuRec = new StudentRec();
                    stuRec.ID = studentID;
                    stuRec.Name = studentName;
                    stuRec.SeatNo = seatNo;
                    stuRec.ClassID = classID;

                    dicStudentRecByID.Add(studentID, stuRec);
                }
                #endregion
            }
        }


        private List<string> GetCoreSubjectNameList(List<string> nameList)
        {
            List<string> value = new List<string>();
            if (nameList.Count > 0)
            {
                string sql = string.Format(@"
WITH subject_list AS(
	SELECT
		array_to_string(xpath('/Subject/@Name', subject_xml), '')::TEXT AS subject_name
		,  xpath('/Subject/Level/text()', subject_xml)  AS arr_level
	FROM (
			SELECT 
				unnest(xpath('/root/SubjectTableContent/Subject', xmlparse(content '<root>' || content || '</root>'))) AS subject_xml
			FROM 
				subj_table 
			WHERE 
				name in ('" + string.Join("','", nameList.ToArray()) + @"')
		) AS subject	
)
SELECT
	subject_list.subject_name
	, subject.level
FROM
	subject_list
	LEFT OUTER JOIN (
		SELECT
			subject_name
			, unnest(arr_level) AS level
		FROM
			subject_list
	) subject
		ON subject.subject_name = subject_list.subject_name
            ");


                DataTable dt = qh.Select(sql);

                foreach (DataRow row in dt.Rows)
                {
                    string subjectName = "" + row["subject_name"];
                    string level = "" + row["level"];
                    string key = $"{subjectName}_{level}";

                    if (!value.Contains(key))
                        value.Add(key);
                }
            }

            return value;
        }


        //        private void GetCoreSubjects()
        //        {
        //            dicCoreSubjectByKey = new Dictionary<string, CoreSubjectRec>();

        //            #region SQL
        //            string sql = string.Format(@"
        //WITH subject_list AS(
        //	SELECT
        //		array_to_string(xpath('/Subject/@Name', subject_xml), '')::TEXT AS subject_name
        //		,  xpath('/Subject/Level/text()', subject_xml)  AS arr_level
        //	FROM (
        //			SELECT 
        //				unnest(xpath('/root/SubjectTableContent/Subject', xmlparse(content '<root>' || content || '</root>'))) AS subject_xml
        //			FROM 
        //				subj_table 
        //			WHERE 
        //				name = '後期中等教育核心課程'
        //		) AS subject	
        //)
        //SELECT
        //	subject_list.subject_name
        //	, subject.level
        //FROM
        //	subject_list
        //	LEFT OUTER JOIN (
        //		SELECT
        //			subject_name
        //			, unnest(arr_level) AS level
        //		FROM
        //			subject_list
        //	) subject
        //		ON subject.subject_name = subject_list.subject_name
        //            ");
        //            #endregion

        //            DataTable dt = qh.Select(sql);

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                string subjectName = "" + row["subject_name"];
        //                string level = "" + row["level"];
        //                string key = $"{subjectName}_{level}";

        //                if (!dicCoreSubjectByKey.ContainsKey(key))
        //                {
        //                    CoreSubjectRec sub = new CoreSubjectRec();
        //                    sub.SubjectName = subjectName;
        //                    sub.Level = level;

        //                    dicCoreSubjectByKey.Add(key, sub);
        //                }
        //            }
        //        }

        /// <summary>
        /// 取得班級學生成績計算規則ID
        /// </summary>
        /// <param name="classIDs"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetStudentScoreRuleByClassID(List<string> classIDs)
        {
            Dictionary<string, string> value = new Dictionary<string, string>();
            if (classIDs.Count > 0)
            {
                QueryHelper qh = new QueryHelper();

                string strSQL = @"
SELECT 
student.id AS student_id
,COALESCE(student.ref_score_calc_rule_id,class.ref_score_calc_rule_id) as score_rule_id
,class.id AS class_id
 FROM student INNER JOIN class on student.ref_class_id = class.id 
 WHERE class.id in (" + string.Join(",", classIDs.ToArray()) + @");
";

                DataTable dt = qh.Select(strSQL);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sid = dr["student_id"].ToString();
                        string r_id = "";

                        if (dr["score_rule_id"] != null)
                        {
                            r_id = dr["score_rule_id"].ToString();
                        }

                        if (!value.ContainsKey(sid))
                            value.Add(sid, r_id);
                    }
                }

            }
            return value;
        }


        private Workbook FillWorkBookData()
        {
            Workbook wb = new Workbook();
            wb.Copy(wbTemplate);

            string schoolName = School.ChineseName;
            string schoolYear = School.DefaultSchoolYear;

            int sheetIndex = 0;
            foreach (string classID in listClassID)
            {
                if (sheetIndex > 0)
                {
                    wb.Worksheets.Add();
                }

                int rowIndex = 0;

                ClassRecord classRec = Class.SelectByID(classID);

                Worksheet sheet = wb.Worksheets[sheetIndex];
                sheet.Copy(wbTemplate.Worksheets[0]);

                sheet.Name = classRec.Name;
                sheet.Cells[rowIndex++, 0].PutValue($"{schoolName} {schoolYear}學年度 {classRec.Name} 修讀學分統計表");

                rowIndex = 4;

                Range range = sheet.Cells.CreateRange(1, 1, 1, 30);
                Style style = sheet.Cells.GetCellStyle(4, 0);
                StyleFlag styleFlag = new StyleFlag() { All = true };

                // 有班級學生資料
                if (dicCreditRecByClassStuKey.ContainsKey(classID))
                {
                    foreach (string stuID in dicCreditRecByClassStuKey[classID].Keys)
                    {
                        // 沒有任何核心必修科目
                        bool nototalCoreCredit = true;
                        if (studentScoreRuleDict.ContainsKey(stuID))
                        {
                            if (ruleCoreSubjectNameDict.ContainsKey(studentScoreRuleDict[stuID]))
                            {
                                if (ruleCoreSubjectNameDict[studentScoreRuleDict[stuID]].Count > 0)
                                {

                                    nototalCoreCredit = false;
                                }
                            }
                        }

                        int colIndex = 0;
                        float totalCredit = 0;
                        // 累計實得
                        float totalHaveCredit = 0;
                        float totalCoreCredit = 0;
                        float totalHaveToCredit = 0;
                        float totalSelectedCredit = 0;
                        Dictionary<string, CreditRec> dicCreditRecByKey = dicCreditRecByClassStuKey[classID][stuID];

                        // 座號 姓名
                        if (dicStudentRecByID.ContainsKey(stuID))
                        {
                            StudentRec stuRec = dicStudentRecByID[stuID];
                            sheet.Cells[rowIndex, colIndex++].PutValue(stuRec.SeatNo);
                            sheet.Cells[rowIndex, colIndex++].PutValue(stuRec.Name);
                        }

                        // 成績年級學期學分數
                        for (int gradeYear = 1; gradeYear <= 3; gradeYear++)
                        {
                            for (int semester = 1; semester <= 2; semester++)
                            {
                                string key = $"{gradeYear}_{semester}";

                                if (dicCreditRecByKey.ContainsKey(key))
                                {
                                    CreditRec creditRec = dicCreditRecByKey[key];
                                    sheet.Cells[rowIndex, colIndex++].PutValue(creditRec.TotalCredit);

                                    if (nototalCoreCredit)
                                    {
                                        sheet.Cells[rowIndex, colIndex++].PutValue("");
                                    }
                                    else
                                    {
                                        sheet.Cells[rowIndex, colIndex++].PutValue(creditRec.CoredCredit);
                                    }

                                    sheet.Cells[rowIndex, colIndex++].PutValue(creditRec.HaveToCredit);
                                    sheet.Cells[rowIndex, colIndex++].PutValue(creditRec.SelectedCredit);

                                    totalCredit += creditRec.TotalCredit;
                                    totalCoreCredit += creditRec.CoredCredit;
                                    totalHaveToCredit += creditRec.HaveToCredit;
                                    totalSelectedCredit += creditRec.SelectedCredit;
                                    totalHaveCredit += (creditRec.HaveToCredit + creditRec.SelectedCredit);
                                }
                                else
                                {
                                    colIndex += 4;
                                }
                            }
                        }



                        // 累計實得學分
                        // 核心必修                       

                        if (nototalCoreCredit)
                        {
                            sheet.Cells[rowIndex, colIndex++].PutValue("");
                        }
                        else
                        {
                            sheet.Cells[rowIndex, colIndex++].PutValue(totalCoreCredit);
                        }

                        // 必修
                        sheet.Cells[rowIndex, colIndex++].PutValue(totalHaveToCredit);
                        // 選修
                        sheet.Cells[rowIndex, colIndex++].PutValue(totalSelectedCredit);
                        // 合計
                        sheet.Cells[rowIndex, colIndex++].PutValue(totalHaveCredit);

                        Range currentRow = sheet.Cells.CreateRange(rowIndex, 0, 1, 30);
                        currentRow.ApplyStyle(style, styleFlag);

                        rowIndex++;
                    }
                }
                // 沒有班級學生資料
                else
                {
                    sheet.Cells.Merge(4, 0, 3, 30);
                    sheet.Cells[4, 0].PutValue("無資料");
                }
                sheetIndex++;
            }

            return wb;
        }


        /// <summary>
        ///  取得成績計算規則有勾選核心科目表
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<string>> GetRuleCoreSubjectTableByRuleID(List<string> ruleIDList)
        {
            Dictionary<string, List<string>> value = new Dictionary<string, List<string>>();

            if (ruleIDList.Count > 0)
            {
                QueryHelper qh = new QueryHelper();
                string strSQL = @"
SELECT id,name,content FROM score_calc_rule WHERE id in(" + string.Join(",", ruleIDList.ToArray()) + ");";

                DataTable dt = qh.Select(strSQL);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string id = dr["id"].ToString();

                        if (!value.ContainsKey(id))
                            value.Add(id, new List<string>());
                        try
                        {
                            XElement elmRoot = XElement.Parse(dr["content"].ToString());

                            if (elmRoot.Element("核心科目表") != null && elmRoot.Element("核心科目表").HasElements)
                            {
                                foreach (XElement elm in elmRoot.Element("核心科目表").Elements("科目表"))
                                {
                                    if (!value[id].Contains(elm.Value))
                                        value[id].Add(elm.Value);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return value;
        }




        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 1. 取得班級學生成績計算規則
            studentScoreRuleDict = GetStudentScoreRuleByClassID(listClassID);

            // 整理有使用的成績計算規則
            List<string> ruleList = new List<string>();
            foreach (string str in studentScoreRuleDict.Values)
            {
                if (string.IsNullOrWhiteSpace(str))
                    continue;

                if (!ruleList.Contains(str))
                    ruleList.Add(str);
            }

            // 取得成績計算規則有勾選核心科目表
            ruleCoreSubjectTableDict = GetRuleCoreSubjectTableByRuleID(ruleList);

            // 成績計算規則內核心科目表內的科目名稱
            ruleCoreSubjectNameDict.Clear();

            foreach (string id in ruleCoreSubjectTableDict.Keys)
            {
                List<string> subjNameList = new List<string>();
                if (ruleCoreSubjectTableDict[id].Count > 0)
                {
                    subjNameList = GetCoreSubjectNameList(ruleCoreSubjectTableDict[id]);
                }
                ruleCoreSubjectNameDict.Add(id, subjNameList);
            }




            // 1. 取得後期中等教育核心課程 科目表
            //     GetCoreSubjects();
            bgWorker.ReportProgress(30);

            // 2. 取得班級學生學期科目成績
            GetClassStudentSemsScore();
            bgWorker.ReportProgress(40);

            // 3. 資料填入Excel
            Workbook wb = FillWorkBookData();
            bgWorker.ReportProgress(60);

            e.Result = wb;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Workbook wb = (Workbook)e.Result;

            string path = $"{Application.StartupPath}\\Reports\\班級成績預警通知單.xlsx";
            int i = 1;
            while (File.Exists(path))
            {
                path = $"{Application.StartupPath}\\Reports\\班級成績預警通知單{i++}{Path.GetExtension(path)}";
            }

            wb.Save(path, SaveFormat.Xlsx);
            MotherForm.SetStatusBarMessage("班級成績預警通知單產生完成。");

            DialogResult result = MsgBox.Show($"{path}\n班級成績預警通知單產生完成，是否立刻開啟？", "訊息", MessageBoxButtons.YesNo);

            if (DialogResult.Yes == result)
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MotherForm.SetStatusBarMessage("班級成績預警通知單 產生中:", e.ProgressPercentage);
        }

        public void Export()
        {
            if (!bgWorker.IsBusy)
            {
                bgWorker.RunWorkerAsync();
            }
        }

        private class StudentRec
        {
            public string ID;
            public string Name;
            public string SeatNo;
            public string ClassID;
        }

        private class CreditRec
        {
            public float TotalCredit;
            public float CoredCredit;
            public float HaveToCredit;
            public float SelectedCredit;
        }

        private class CoreSubjectRec
        {
            public string SubjectName;
            public string Level;
        }
    }
}
