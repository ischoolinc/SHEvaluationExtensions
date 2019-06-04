using Aspose.Cells;
using FISCA.Data;
using FISCA.Presentation;
using FISCA.Presentation.Controls;
using SHSchool.Evaluation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SHSchool.Evaluation.InportForm
{
    public partial class ImportCurriculumMappingForm : BaseForm
    {

        private BackgroundWorker _Worker = new BackgroundWorker();
        private QueryHelper _Qh = new QueryHelper();

        /// <summary>
        /// 目前課程規劃表_名稱
        /// </summary>
        private string CurrentCurriculumMappingName;

        private FileInfo _FileInfo;
        /// <summary>
        /// 存放CSVFile 轉成物件 
        /// </summary>
        private Dictionary<string, CourseInfo> _DicCourseInfo;
        /// <summary>
        /// 課程類型 　-對照表 用
        /// </summary>
        private Dictionary<string, MappingInfo> _DicCourseType;
        /// <summary>
        ///  群別     　-對照表 用
        /// </summary>
        private Dictionary<string, MappingInfo> _DicGroupType;
        /// <summary>
        /// 科別 對照表
        /// </summary>
        private Dictionary<string, MappingInfo> _DicDept;
        /// <summary>
        /// 班群 對照表
        /// </summary>
        private Dictionary<string, MappingInfo> _DicClassGroup;

        /// <summary>
        /// 課程類別 對照表
        /// </summary>
        private Dictionary<string, string> _DicCourseClassified;  // 課程類別 
        /// <summary>
        /// 開課方式 對照表
        /// </summary>
        private Dictionary<string, string> _DicOpenWay;           // 開課方式
        /// <summary>
        ///  科目屬性 對照表
        /// </summary>
        private Dictionary<string, string> _DicSubjectAttribute;  // 科目屬性
        /// <summary>
        /// 領域名稱 
        /// </summary>
        private Dictionary<string, MappingInfo> _DicFieldName;    // 領域名稱
        /// <summary>
        /// 科目固定編碼
        /// </summary>
        private Dictionary<string, MappingInfo> _DicFixedCodes;    // 科目固定編碼

        /// <summary>
        /// 裝學年學期對照表 
        /// </summary>
        Dictionary<int, dynamic> _DicMappingSemester = new Dictionary<int, dynamic>();


        private Dictionary<string, int> _DicLevel;


        public ImportCurriculumMappingForm()
        {
            InitializeComponent();
        }

        private void ImportCurriculumMappingForm_Load(object sender, EventArgs e)
        {
            _Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
            _Worker.DoWork += new DoWorkEventHandler(DoWork);


            //先建立空間 存放 對照表
            _DicCourseInfo = new Dictionary<string, CourseInfo>();
            _DicCourseType = new Dictionary<string, MappingInfo>();
            _DicGroupType = new Dictionary<string, MappingInfo>();
            _DicDept = new Dictionary<string, MappingInfo>();
            _DicClassGroup = new Dictionary<string, MappingInfo>();
            _DicCourseClassified = new Dictionary<string, string>();
            _DicOpenWay = new Dictionary<string, string>();
            _DicSubjectAttribute = new Dictionary<string, string>();
            _DicFieldName = new Dictionary<string, MappingInfo>();
            _DicFixedCodes = new Dictionary<string, MappingInfo>();
            _DicLevel = new Dictionary<string, int>();

            // 1. 載入 對照表  與課程規劃表名稱有關之 欄位
            LoadCodeMapping();
            // 2. 載入 對照表 其他 欄位
            LoadCCodeMappingElse();
            // 3.載若學期對照表 
            GetGradeYearAndSenmester();


            //Console.WriteLine("1.存放CSVFile : " + _DicCourseInfo.Count() + "筆");

            //Console.WriteLine("2.課程類型 : " + _DicCourseType.Count() + "筆");

            //Console.WriteLine("3.群別 : " + _DicGroupType.Count() + "筆");

            //Console.WriteLine("4.科別 : " + _DicDept.Count() + "筆");

            //Console.WriteLine("5.班群 : " + _DicClassGroup.Count() + "筆");

            //Console.WriteLine("6.課程類別 : " + _DicCourseClassified.Count() + "筆");

            //Console.WriteLine("7.開課方式 : " + _DicOpenWay.Count() + "筆");

            //Console.WriteLine("8.科目屬性 : " + _DicSubjectAttribute.Count() + "筆");

            //Console.WriteLine("9.領域名稱 : " + _DicFieldName.Count() + "筆");

            //Console.WriteLine("10.領域名稱 : " + _DicFixedCodes.Count() + "筆");


        }

        //選擇檔案 
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "CSV(逗號分隔)(*.csv)|*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileNames.Count() > 1)
                {
                    //   一次只能上傳一分檔案  跳出視窗 

                }
                else if (ofd.FileNames.Count() == 1)
                {
                    //檔案資訊 放入一個 FileInfo 物件

                    _FileInfo = new FileInfo(ofd.FileNames[0]);

                    txtFileName.Text = _FileInfo.Name;
                }
            }
        }

        //
        private void btnImprt_Click(object sender, EventArgs e)
        {
            if (!_Worker.IsBusy)
            {
                MotherForm.SetStatusBarMessage("課程規劃表匯入中....");
                btnImprt.Enabled = false;
                _Worker.RunWorkerAsync();
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            //讀取 CSV 資料
            LoadCSVFile(this._FileInfo);

            // 讀取 CSV檔 並解析
            XmlDocument xmlDoc = new XmlDocument(); 

            XmlElement graduationPlan = xmlDoc.CreateElement("GraduationPlan");

            int rows = 0;
            int count = 0; // 目前第幾筆 主要是要最後一筆時要儲存 AND 第一筆時要 寫入名稱做紀錄 
            foreach (string keyCodeFromMOE/**/ in this._DicCourseInfo.Keys)
            {
                count++;
                rows++;

                #region 轉換中文 

                //取得切割後code
                string courseCodeFromMOE = _DicCourseInfo[keyCodeFromMOE].CourseCodeFromMOE;    // 1 .課程代碼
                string subjectName = _DicCourseInfo[keyCodeFromMOE].SubjectName;                // 2 .課程名稱
                string Credit = _DicCourseInfo[keyCodeFromMOE].Credit;                          // 3 .學分

                string EnterYear = _DicCourseInfo[keyCodeFromMOE].EnterYear;                    // 入學學年度 
                string SchoolCode = _DicCourseInfo[keyCodeFromMOE].SchoolCode;                  // 學校代碼
                string CourseType = _DicCourseInfo[keyCodeFromMOE].CourseType;                  // 課程類型  
                string GroupCode = _DicCourseInfo[keyCodeFromMOE].GroupCode;                    // 群別代碼
                string Dept = _DicCourseInfo[keyCodeFromMOE].DeptCode;                          // 科別代碼
                string ClassGroup = _DicCourseInfo[keyCodeFromMOE].ClassGroup;                  // 班群 


                string ClassClassified = _DicCourseInfo[keyCodeFromMOE].ClassClassified;        // 課程類別 
                string OpenWay = _DicCourseInfo[keyCodeFromMOE].OpenWay;                        // 開課方式
                string SubjectAttribute = _DicCourseInfo[keyCodeFromMOE].SubjectAttribute;      // 科目屬性
                string FieldName = _DicCourseInfo[keyCodeFromMOE].FieldName;                    // 領域名稱
                string SubjectFixedCode = _DicCourseInfo[keyCodeFromMOE].SubjectFixedCode;       //科目固定編碼 

                //裝入 轉換後中文欄位

                _DicCourseInfo[keyCodeFromMOE].CourseTypeDetail = _DicCourseType[CourseType].IsCodeUniqe ? _DicCourseType[CourseType].Name : _DicCourseType[CourseType].DicForDuplicate[CourseType];
                _DicCourseInfo[keyCodeFromMOE].GroupCodeDetail = _DicGroupType[GroupCode].IsCodeUniqe ? _DicGroupType[GroupCode].Name : _DicGroupType[GroupCode].DicForDuplicate[CourseType];
                _DicCourseInfo[keyCodeFromMOE].DeptCodeDetail = _DicDept[Dept].IsCodeUniqe ? _DicDept[Dept].Name : _DicDept[Dept].DicForDuplicate[CourseType];
                _DicCourseInfo[keyCodeFromMOE].ClassGroupDetail = _DicClassGroup[ClassGroup].IsCodeUniqe ? _DicClassGroup[ClassGroup].Name : _DicClassGroup[ClassGroup].DicForDuplicate[CourseType];
                _DicCourseInfo[keyCodeFromMOE].ClassClassifiedDetail = _DicCourseClassified[ClassClassified];
                _DicCourseInfo[keyCodeFromMOE].OpenWayDetail = _DicOpenWay[OpenWay];
                _DicCourseInfo[keyCodeFromMOE].SubjectAttributeDetail = _DicSubjectAttribute[SubjectAttribute];
                _DicCourseInfo[keyCodeFromMOE].FieldNameDetail = _DicFieldName[FieldName].IsCodeUniqe ? _DicFieldName[FieldName].Name : _DicFieldName[FieldName].DicForDuplicate[CourseType];

                //原始欄位  

                _DicCourseInfo[keyCodeFromMOE].Required = _DicCourseInfo[keyCodeFromMOE].ClassClassifiedDetail.Contains("必修") ? "必修" : "選修";

                _DicCourseInfo[keyCodeFromMOE].RequiredBy = _DicCourseInfo[keyCodeFromMOE].ClassClassifiedDetail.Contains("部定") ? "部訂" : "校訂";

                _DicCourseInfo[keyCodeFromMOE].Entry = _DicCourseInfo[keyCodeFromMOE].SubjectAttributeDetail == "一般科目" ? "學業" : _DicCourseInfo[keyCodeFromMOE].SubjectAttributeDetail;

                //trim 掉 "領域兩字" 【領域特殊處理】

                string filedNameOrigin = _DicCourseInfo[keyCodeFromMOE].FieldNameDetail;

                _DicCourseInfo[keyCodeFromMOE].FieldNameDetail = filedNameOrigin.Substring(filedNameOrigin.Length - 2, 2) == "領域" ? filedNameOrigin.Substring(0, filedNameOrigin.Length - 2) : filedNameOrigin;
                #endregion

                //檢視課程規劃表名稱

                //如果是第一筆
                if (count == 1)
                {
                    this.CurrentCurriculumMappingName = $"{_DicCourseInfo[keyCodeFromMOE].EnterYear}{_DicCourseInfo[keyCodeFromMOE].DeptCodeDetail}{_DicCourseInfo[keyCodeFromMOE].ClassGroupDetail}";//裝目前課程規劃表名稱

                    graduationPlan = xmlDoc.CreateElement("GraduationPlan");

                    graduationPlan.SetAttribute("SchoolYear", EnterYear);

                    xmlDoc.AppendChild(graduationPlan);
                }


                if (this.CurrentCurriculumMappingName != $"{_DicCourseInfo[keyCodeFromMOE].EnterYear}{_DicCourseInfo[keyCodeFromMOE].DeptCodeDetail}{_DicCourseInfo[keyCodeFromMOE].ClassGroupDetail}")
                {
                    Console.WriteLine("開始寫入資料庫 ");

                    InsertGraduationPlan(CurrentCurriculumMappingName, xmlDoc.OuterXml);

                    rows = 1;

                    this.CurrentCurriculumMappingName = $"{_DicCourseInfo[keyCodeFromMOE].EnterYear}{_DicCourseInfo[keyCodeFromMOE].DeptCodeDetail}{_DicCourseInfo[keyCodeFromMOE].ClassGroupDetail}";//裝目前課程規劃表名稱


                    this._DicLevel.Clear(); //清除級別字典 要開始裝新的

                    xmlDoc = new XmlDocument();

                    graduationPlan = xmlDoc.CreateElement("GraduationPlan");

                    graduationPlan.SetAttribute("SchoolYear", EnterYear);

                    xmlDoc.AppendChild(graduationPlan);

                }


                #region 填入XML
                //如果 
                int startLevel = this._DicLevel.ContainsKey(_DicCourseInfo[keyCodeFromMOE].SubjectName) ? this._DicLevel[_DicCourseInfo[keyCodeFromMOE].SubjectName] + 1 : 1;

                //放個學期的學分數
                Dictionary<int,string> DicCreditEachSemester =this._DicCourseInfo[keyCodeFromMOE].DicCreditEachSemester;


                int level = startLevel;
                foreach (int semester in DicCreditEachSemester.Keys)
                {
                    XmlElement subject = xmlDoc.CreateElement("Subject");

                    #region 生成XML

                    subject.SetAttribute("Category", "");
                    subject.SetAttribute("Credit", DicCreditEachSemester[semester]);
                    subject.SetAttribute("Domain", (_DicCourseInfo[keyCodeFromMOE].FieldNameDetail));
                    subject.SetAttribute("Entry", _DicCourseInfo[keyCodeFromMOE].Entry);
                    subject.SetAttribute("FullName", "");
                    subject.SetAttribute("GradeYear", this._DicMappingSemester[semester].GredeYear.ToString());

                    //加入


                    if (!this._DicLevel.ContainsKey(_DicCourseInfo[keyCodeFromMOE].SubjectName))
                    {

                        this._DicLevel.Add(_DicCourseInfo[keyCodeFromMOE].SubjectName, level);
                        subject.SetAttribute("Level", level.ToString());
                        level++;

                    }
                    else
                    {
                        this._DicLevel[_DicCourseInfo[keyCodeFromMOE].SubjectName] = level;
                        subject.SetAttribute("Level", level.ToString());
                        level++;
                    }

                    subject.SetAttribute("NotIncludedInCalc", "False");
                    subject.SetAttribute("NotIncludedInCredit", "False");
                    subject.SetAttribute("Required", _DicCourseInfo[keyCodeFromMOE].Required);    //可將  課程類別 寫入 如果不是
                    subject.SetAttribute("RequiredBy", _DicCourseInfo[keyCodeFromMOE].RequiredBy);
                    subject.SetAttribute("Semester", this._DicMappingSemester[semester].semester.ToString());
                    subject.SetAttribute("SubjectName", _DicCourseInfo[keyCodeFromMOE].SubjectName);

                    subject.SetAttribute("課程代碼", _DicCourseInfo[keyCodeFromMOE].CourseCodeFromMOE);
                    subject.SetAttribute("課程類別", _DicCourseInfo[keyCodeFromMOE].ClassClassifiedDetail);
                    subject.SetAttribute("開課方式", _DicCourseInfo[keyCodeFromMOE].OpenWayDetail);
                    subject.SetAttribute("科目屬性", _DicCourseInfo[keyCodeFromMOE].SubjectAttributeDetail);
                    subject.SetAttribute("領域名稱", _DicCourseInfo[keyCodeFromMOE].FieldNameDetail);
                    subject.SetAttribute("課程名稱", _DicCourseInfo[keyCodeFromMOE].SubjectName);
                    subject.SetAttribute("學分", DicCreditEachSemester[semester]);

                    graduationPlan.AppendChild(subject);

                    XmlElement grouping = xmlDoc.CreateElement("Grouping");

                    grouping.SetAttribute("RowIndex", (rows).ToString());
                    Console.WriteLine((rows).ToString());
                    grouping.SetAttribute("startLevel", startLevel.ToString());

                    subject.AppendChild(grouping);
                    #endregion
                }
                #endregion


                //最後一筆 課程資料可以儲存
                if (count == this._DicCourseInfo.Count)
                {
                    InsertGraduationPlan(CurrentCurriculumMappingName, xmlDoc.OuterXml);
                    Console.WriteLine("最後一本");
                }
            }
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("課程規劃表已匯入");
            this.Close();
        }


        /// <summary>
        /// 將匯入檔案讀進來  並解析
        /// </summary>
        private void LoadCSVFile(FileInfo _fileInfo)
        {
            using (StreamReader sr = new StreamReader(_fileInfo.OpenRead()))
            {
                string row;
                while ((row = sr.ReadLine()) != null)
                {
                    //string row = sr.ReadLine();
                    string[] itemInRow = row.Split(',');

                    CourseInfo courseInfo = new CourseInfo(itemInRow[0], itemInRow[1], itemInRow[2]); //初始化 先裝入 課程代碼 / 課程名稱 / 學分

                    _DicCourseInfo.Add(courseInfo.CourseCodeFromMOE, courseInfo);
                }
            }
        }


        /// <summary>
        /// 載入代碼對照表 (與課程規劃表名稱有關之欄位) 來源:群國高級中等學校_課程計畫平台 (http://course.tchcvs.tw/QueryCode.asp?T=SCH)
        /// </summary>
        private void LoadCodeMapping()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList nodeList;

            #region 課程類別
            // 將 【課程類別】 讀入字典
            xmlDoc.LoadXml(Properties.Resources.CourseTypes);

            nodeList = xmlDoc.SelectNodes("CourseTypes/CourseType");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                //  string courseTypeApplicable = ((XmlElement)courseType).GetAttribute("crstype");

                this._DicCourseType.Add(code, new MappingInfo(code, name));
            }
            #endregion


            #region 群別
            //*****code(代碼有重複)

            xmlDoc.LoadXml(Properties.Resources.GroupTypes);

            nodeList = xmlDoc.SelectNodes("GroupTypes/GroupType");


            Console.WriteLine("印出課程群別對照表 數量");
            foreach (XmlNode courseType in nodeList)
            {

                Console.WriteLine("有重複" + courseType);
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                string courseTypeApplicable = ((XmlElement)courseType).GetAttribute("crstype");

                if (!_DicGroupType.ContainsKey(code))
                {
                    this._DicGroupType.Add(code, new MappingInfo(code, name, courseTypeApplicable));
                }
                else //如果有重複key值(代碼)
                {
                    _DicGroupType[code].IsCodeUniqe = false; // 標示此物件 code 不是唯一值

                    _DicGroupType[code].GetCourseTypeApplicable(courseTypeApplicable, name); // 加入對應值 
                }
            }
            #endregion


            #region 科別 

            xmlDoc.LoadXml(Properties.Resources.Depts);

            nodeList = xmlDoc.SelectNodes("Depts/Dept");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                string courseTypeApplicable = ((XmlElement)courseType).GetAttribute("crstype");

                if (!_DicDept.ContainsKey(code))
                {
                    this._DicDept.Add(code, new MappingInfo(code, name, courseTypeApplicable));
                }
                else //如果有重複key值(代碼)
                {
                    _DicDept[code].IsCodeUniqe = false; // 標示此物件 code 不是唯一值

                    _DicDept[code].GetCourseTypeApplicable(courseTypeApplicable, name); // 加入對應值    ex : <|H|,11 >
                }
            }
            #endregion


            #region 班群

            xmlDoc.LoadXml(Properties.Resources.ClassGroup);

            nodeList = xmlDoc.SelectNodes("ClassGroups/ClassGroup");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                //  string courseTypeApplicable = ((XmlElement)courseType).GetAttribute("crstype");
                this._DicClassGroup.Add(code, new MappingInfo(code, name));
            }

            #endregion
        }


        /// <summary>
        /// 載入 代碼對照表 ()
        /// </summary>
        private void LoadCCodeMappingElse()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList nodeList;

            #region 課程類別

            xmlDoc.LoadXml(Properties.Resources.CourseClassifieds);

            nodeList = xmlDoc.SelectNodes("CourseClassifieds/CourseClassified");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                this._DicCourseClassified.Add(code, name);
            }

            #endregion

            #region 開課方式

            xmlDoc.LoadXml(Properties.Resources.OpenWays);

            nodeList = xmlDoc.SelectNodes("OpenWays/OpenWay");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                this._DicOpenWay.Add(code, name);
            }

            xmlDoc.LoadXml(Properties.Resources.OpenWays);

            nodeList = xmlDoc.SelectNodes("SubjectAttributes/SubjectAttribute");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                this._DicOpenWay.Add(code, name);
            }
            #endregion

            #region 科目屬性

            xmlDoc.LoadXml(Properties.Resources.SubjectAttributes);

            nodeList = xmlDoc.SelectNodes("SubjectAttributes/SubjectAttribute");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                this._DicSubjectAttribute.Add(code, name);
            }
            #endregion

            #region 領域名稱
            xmlDoc.LoadXml(Properties.Resources.FieldNames);

            nodeList = xmlDoc.SelectNodes("FieldNames/FieldName");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                string courseTypeApplicable = ((XmlElement)courseType).GetAttribute("crstype");

                if (!_DicFieldName.ContainsKey(code))
                {
                    this._DicFieldName.Add(code, new MappingInfo(code, name, courseTypeApplicable));
                }
                else //如果有重複key值(代碼)
                {
                    _DicFieldName[code].IsCodeUniqe = false; // 標示此物件 code 不是唯一值

                    _DicFieldName[code].GetCourseTypeApplicable(courseTypeApplicable, name); // 加入對應值 
                }


            }
            #endregion
         
            #region  科目固定編碼
            xmlDoc.LoadXml(Properties.Resources.SubjectFixedCodes);

            nodeList = xmlDoc.SelectNodes("SubjectFixedCodes/SubjectFixedCode");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                string courseTypeApplicable = ((XmlElement)courseType).GetAttribute("crstype");

                if (!_DicFixedCodes.ContainsKey(code))
                {
                    this._DicFixedCodes.Add(code, new MappingInfo(code, name, courseTypeApplicable));
                }
                else //如果有重複key值(代碼)
                {
                    _DicFixedCodes[code].IsCodeUniqe = false; // 標示此物件 code 不是唯一值

                    _DicFixedCodes[code].GetCourseTypeApplicable(courseTypeApplicable, name); // 加入對應值 
                }
            }
            #endregion
        }

        /// <summary>
        /// 轉換學年度學期
        /// </summary>
        /// <param name="semester"></param>
        private void GetGradeYearAndSenmester()
        {
            _DicMappingSemester.Add(1, new { GredeYear = 1, semester = 1 });
            _DicMappingSemester.Add(2, new { GredeYear = 1, semester = 2 });
            _DicMappingSemester.Add(3, new { GredeYear = 2, semester = 1 });
            _DicMappingSemester.Add(4, new { GredeYear = 2, semester = 2 });
            _DicMappingSemester.Add(5, new { GredeYear = 3, semester = 1 });
            _DicMappingSemester.Add(6, new { GredeYear = 3, semester = 2 });

        }
        /// <summary>
        ///寫入資料庫 
        /// </summary>
        /// <param name="queryString"></param>
        private void InsertGraduationPlan(string graduationPlanName, string content)
        {
            try
            {
                //先確認是否存在
                string checkQueryString = $"SELECT * FROM graduation_plan WHERE name = '{graduationPlanName}'";

                DataTable existdRows = _Qh.Select(checkQueryString);
              
                if (existdRows.Rows.Count != 0 && existdRows != null)
                {
                    {
                        string UpDateString = $"UPDATE graduation_plan SET  content ='{content}' WHERE  name = '{graduationPlanName}' RETURNING *";

                        _Qh.Select(UpDateString);
                    }
                }
                else //如果資料庫沒有就 新增

                {
                    string insertString = $"INSERT INTO graduation_plan (name , content ) VALUES ('{graduationPlanName}' , '{content}')  RETURNING * ";

                    DataTable insertedRows = _Qh.Select(insertString);
                }

            }
            catch (Exception ex)
            {
                MsgBox.Show("檔案儲存失敗:" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
