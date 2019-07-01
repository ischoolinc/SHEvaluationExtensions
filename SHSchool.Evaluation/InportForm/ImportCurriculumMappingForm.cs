﻿using Aspose.Cells;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace SHSchool.Evaluation.InportForm
{
    public partial class ImportCurriculumMappingForm : BaseForm
    {
        private QueryHelper _Qh = new QueryHelper();
        private FileInfo _FileInfoForMD5;
        private FileInfo _FileInfoForcClassGroup;
        private Regex rgx = new Regex(@"^[A-Z]{1,1}"); //班群對照表驗證用


        /// <summary>
        /// 目前課程規劃表_名稱
        /// </summary>
        private string CurrentCurriculumMappingName;

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
        private Dictionary<string, string> _DicClassGroup;

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
        private Dictionary<int, dynamic> _DicMappingSemester = new Dictionary<int, dynamic>();

        /// <summary>
        /// 裝每學期皆0學分之課程 
        /// </summary>
        private Dictionary<string, CourseInfo> _DicZeroCreditEachSems = new Dictionary<string, CourseInfo>();

        /// <summary>
        /// 有錯誤課程
        /// </summary>
        Dictionary<string, CourseInfo> _DicErrorCourse = new Dictionary<string, CourseInfo>();

        /// <summary>
        /// 長度有誤
        /// </summary>
        Dictionary<string, CourseInfo> _DictionaryLengthErr = new Dictionary<string, CourseInfo>();

        private List<string> _ListinSertSuccess = new List<string>();

        private Dictionary<string, int> _DicLevel;




        public ImportCurriculumMappingForm()
        {
            InitializeComponent();
        }

        private void ImportCurriculumMappingForm_Load(object sender, EventArgs e)
        {

            //先建立空間 存放 對照表
            _DicCourseInfo = new Dictionary<string, CourseInfo>();
            _DicCourseType = new Dictionary<string, MappingInfo>();
            _DicGroupType = new Dictionary<string, MappingInfo>();
            _DicDept = new Dictionary<string, MappingInfo>();
            _DicClassGroup = new Dictionary<string, string>();
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
            // 3.載入學期對照表 
            GetGradeYearAndSenmester();

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
                    _FileInfoForMD5 = new FileInfo(ofd.FileNames[0]);
                    txtFileName.Text = _FileInfoForMD5.Name;
                }
            }
        }
        //按下匯入
        private void btnImprt_Click(object sender, EventArgs e)
        {
            // 1. UI 鎖住 
            LuckUI();
            // 2.驗證 檔名
            if (String.IsNullOrEmpty(this.textFileCassGroup.Text))
            {
                DialogResult result = MsgBox.Show("未上傳班群代碼檔，繼續?", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    UnlockUI();
                    return;
                }
            }
            else
            {
                string fileName = this.textFileCassGroup.Text;
                string[] fileNameSplit = fileName.Substring(0, fileName.Length - 3).Split('_');
                if (fileNameSplit[1] != "班群代碼檔")
                {
                    MsgBox.Show("班群代碼檔 檔名有誤 !");
                    UnlockUI();
                    return;
                }
            }
            if (String.IsNullOrWhiteSpace(this.txtFileName.Text))
            {
                MsgBox.Show("未上傳【課程代碼資料檔】");
            }
            else
            {
                string fileName = this.txtFileName.Text;
                string[] fileNameSplit = fileName.Substring(0, fileName.Length - 3).Split('_');
                if (fileNameSplit[1] != "課程代碼資料檔")
                {
                    MsgBox.Show("課程代碼資料檔 檔名有誤 !");
                    UnlockUI();
                    return;
                }
            }
            //清除要用的 Dic
            _DicErrorCourse.Clear();
            _DicZeroCreditEachSems.Clear();
            _ListinSertSuccess.Clear();

            // 載入 【班群對照表】  
            Boolean loadCalssGroupOK = LoadCSVFileForClassGroup(_FileInfoForcClassGroup);
            if (!loadCalssGroupOK)
            {
                MsgBox.Show("班群代碼為 A-Z 請檢查【班群代碼檔】之資料");
                UnlockUI();
                return;
            }
            //載入【課程代碼資料檔】
            LoadCSVFile(_FileInfoForMD5);
            if (this._DictionaryLengthErr.Count > 0)
            {
                string alertString = "部分【課程代碼(23)】或【授課學期學分(6)】長度有誤。";
                DialogResult result =MsgBox.Show(alertString , MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    GetErrorMessageFile();
                }
                this.ClearDic();
                UnlockUI();
                return;
            }
            else { 
            // (驗證) 課程資料檔
            ValidateCourseFileContent();

            if (_DicErrorCourse.Count > 0)
            {
                UnlockUI();
                DialogResult result= MsgBox.Show("【課程代碼資料檔】代碼有誤有誤 \n 是否產生錯誤清單", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    GetErrorMessageFile();
                    return;
                }
                return;
            }
            //產生XML格式並更新
            MotherForm.SetStatusBarMessage("課程規劃表匯入中....");
            LuckUI();
            CreateXmlAndInsert();
            PrintZeroCredit();
            SmartSchool.Evaluation.GraduationPlan.GraduationPlan.Instance.Reflash();//同步 課程規劃表

            }
        }



        /// <summary>
        /// 驗證課程檔資料 代碼 是否又對應
        /// </summary>
        private void ValidateCourseFileContent()
        {
            //讀取 CSV 資料  
          
            //如果載入有誤
         
            //將資料轉成對應中文
            foreach (string keyCodeFromMOE in this._DicCourseInfo.Keys)
            {
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


                #region 轉換後中文欄位
                if (_DicCourseType.ContainsKey(CourseType)) //課程類型
                    _DicCourseInfo[keyCodeFromMOE].CourseTypeDetail = _DicCourseType[CourseType].IsCodeUniqe ? _DicCourseType[CourseType].Name : _DicCourseType[CourseType].DicForDuplicate[CourseType];
                else
                {
                    _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"課程類型 {CourseType} 不存在對照表中");
                }

                if (_DicGroupType.ContainsKey(GroupCode)) //群別代碼
                    if (_DicGroupType[GroupCode].DicForDuplicate.ContainsKey(CourseType))
                    {
                        _DicCourseInfo[keyCodeFromMOE].GroupCodeDetail = _DicGroupType[GroupCode].IsCodeUniqe ? _DicGroupType[GroupCode].Name : _DicGroupType[GroupCode].DicForDuplicate[CourseType];
                    }
                    else {

                        _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"群別代碼 {GroupCode} 與課程類型 對應有誤");
                    }
                else
                {
                    _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"群別代碼 {GroupCode} 不存在對照表中");

                }

                if (_DicDept.ContainsKey(Dept))//科別代碼
                    _DicCourseInfo[keyCodeFromMOE].DeptCodeDetail = _DicDept[Dept].IsCodeUniqe ? _DicDept[Dept].Name : _DicDept[Dept].DicForDuplicate[CourseType];
                else
                {
                    _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"科別代碼 {Dept} 不存在對照表中");

                }

                if (_DicClassGroup.ContainsKey(ClassGroup)) //班群
                    _DicCourseInfo[keyCodeFromMOE].ClassGroupDetail = _DicClassGroup[ClassGroup];
                else
                {
                    _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"班群代碼 {ClassGroup} 不存在對照表中");
                }


                if (_DicCourseClassified.ContainsKey(ClassClassified)) //課程類別 
                {
                    _DicCourseInfo[keyCodeFromMOE].ClassClassifiedDetail = _DicCourseClassified[ClassClassified];
                    _DicCourseInfo[keyCodeFromMOE].Required = _DicCourseInfo[keyCodeFromMOE].ClassClassifiedDetail.Contains("必修") ? "必修" : "選修"; //系統原始欄位
                    _DicCourseInfo[keyCodeFromMOE].RequiredBy = _DicCourseInfo[keyCodeFromMOE].ClassClassifiedDetail.Contains("部定") ? "部訂" : "校訂";//系統原始欄位
                }
                else
                {
                    _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"課程類別 {ClassClassified} 不存在對照表中");

                }

                if (_DicOpenWay.ContainsKey(OpenWay))//開課方式
                    _DicCourseInfo[keyCodeFromMOE].OpenWayDetail = _DicOpenWay[OpenWay];
                else
                {
                    _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"開課方式 {OpenWay} 不存在對照表中");
                }

                if (_DicSubjectAttribute.ContainsKey(SubjectAttribute)) //科目屬性
                {
                    _DicCourseInfo[keyCodeFromMOE].SubjectAttributeDetail = _DicSubjectAttribute[SubjectAttribute];
                    _DicCourseInfo[keyCodeFromMOE].Entry = _DicCourseInfo[keyCodeFromMOE].SubjectAttributeDetail == "一般科目" ? "學業" : _DicCourseInfo[keyCodeFromMOE].SubjectAttributeDetail; //系統原始欄位
                }
                else
                {
                    _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"開課方式 {SubjectAttribute} 不存在對照表中");
                }

                if (_DicFieldName.ContainsKey(FieldName))//領域名稱
                {
                    _DicCourseInfo[keyCodeFromMOE].FieldNameDetail = _DicFieldName[FieldName].IsCodeUniqe ? _DicFieldName[FieldName].Name : _DicFieldName[FieldName].DicForDuplicate[CourseType];
                    string filedNameOrigin = _DicCourseInfo[keyCodeFromMOE].FieldNameDetail;
                    //trim 掉 "領域兩字" 【領域特殊處理】
                    _DicCourseInfo[keyCodeFromMOE].FieldNameDetail = filedNameOrigin.Substring(filedNameOrigin.Length - 2, 2) == "領域" ? filedNameOrigin.Substring(0, filedNameOrigin.Length - 2) : filedNameOrigin;
                }
                else
                {
                    _DicCourseInfo[keyCodeFromMOE].ErrorMessage.Add($"領域名稱 {FieldName} 不存在對照表中");
                }
                #endregion


                //取得課程規劃表名稱
                _DicCourseInfo[keyCodeFromMOE].GetCurriiculumMapName();

                if (_DicCourseInfo[keyCodeFromMOE].ErrorMessage.Count > 0)
                {
                    _DicErrorCourse.Add(keyCodeFromMOE, _DicCourseInfo[keyCodeFromMOE]);

                }
                #endregion
            }
        }

        /// <summary>
        /// 驗證課程檔資料 代碼 是否又對應
        /// </summary>
        private void CreateXmlAndInsert()
        {

            // 讀取 CSV檔 並解析
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement graduationPlan = xmlDoc.CreateElement("GraduationPlan");

            int rows = 0;
            int count = 0; // 目前第幾筆 主要是要最後一筆時要儲存 AND 第一筆時要 寫入名稱做紀錄 
            foreach (string keyCodeFromMOE in this._DicCourseInfo.Keys)//將資料轉換加入中文欄位 組成XML 
            {
                count++;
                rows++;

                string EnterYear = _DicCourseInfo[keyCodeFromMOE].EnterYear;                    // 入學學年度 

                //取得課程規劃表名稱
                _DicCourseInfo[keyCodeFromMOE].GetCurriiculumMapName();

                if (_DicCourseInfo[keyCodeFromMOE].ErrorMessage.Count > 0)
                {
                    _DicErrorCourse.Add(keyCodeFromMOE, _DicCourseInfo[keyCodeFromMOE]);

                }

                if (_DicCourseInfo[keyCodeFromMOE].IsZeroCreditEachSem == true)
                {
                    this._DicZeroCreditEachSems.Add(_DicCourseInfo[keyCodeFromMOE].CourseCodeFromMOE, _DicCourseInfo[keyCodeFromMOE]);
                }

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

                int startLevel = this._DicLevel.ContainsKey(_DicCourseInfo[keyCodeFromMOE].SubjectName) ? this._DicLevel[_DicCourseInfo[keyCodeFromMOE].SubjectName] + 1 : 1;

                //放個學期的學分數
                Dictionary<int, string> DicCreditEachSemester = this._DicCourseInfo[keyCodeFromMOE].DicCreditEachSemester;

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
                    subject.SetAttribute("Required", _DicCourseInfo[keyCodeFromMOE].Required);
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
                 
                    grouping.SetAttribute("startLevel", startLevel.ToString());

                    subject.AppendChild(grouping);
                    #endregion
                }
                #endregion

                //最後一筆 課程資料可以儲存
                if (count == this._DicCourseInfo.Count)
                {
                    InsertGraduationPlan(CurrentCurriculumMappingName, xmlDoc.OuterXml);
                   
                }
            }

            //如果錯誤無錯誤訊息
       

            if (this._DicErrorCourse.Count > 0)
            {
                DialogResult result = MsgBox.Show("上傳資料有誤，是否開啟錯誤清單?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    GetErrorMessageFile();
                }
                else
                {
                    return;
                }
            }
        }

        private void PrintZeroCredit()
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("課程規劃表已匯入");
    
            if (_ListinSertSuccess.Count > 0)
            {
                string successInsert = String.Join("\n", this._ListinSertSuccess);
                MsgBox.Show($"課程規劃表 : \n{successInsert} \n 匯入成功 !");
            }
            //如果有每學期皆為0學分之課程 >印出
            if (_DicZeroCreditEachSems.Count > 0)
            {
                DialogResult result = MsgBox.Show($"有{_DicZeroCreditEachSems.Count}筆 每學期皆為0學分之課程，未匯入課程規表，是否印出?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    GetZeroCreditCourseFile();
                }
                else
                {
                    this.Close();
                }
            }
            this.Close();
        }

        /// <summary>
        /// 將匯入檔案讀進來  並解析
        /// </summary>
        private void LoadCSVFile(FileInfo _fileInfo)
        {
            _DicCourseInfo.Clear();
            using (StreamReader sr = new StreamReader(_fileInfo.OpenRead()))
            {
                string row;
                try
                {
                    while ((row = sr.ReadLine()) != null)
                    {
                        string[] itemInRow = row.Split(',');

                        //驗證長度 (課程代碼& 學期學分)
                        String  lenErr =VaildateEachColumn(itemInRow);

                        CourseInfo courseInfo = new CourseInfo(itemInRow[0], itemInRow[1], itemInRow[2]); //初始化 先裝入 課程代碼 / 課程名稱 / 學分

                        if (lenErr == "")
                        {
                            _DicCourseInfo.Add(courseInfo.CourseCodeFromMOE, courseInfo);
                        }
                        else
                        {
                            courseInfo.ErrorMessage .Add( lenErr);
                            _DictionaryLengthErr.Add($"{itemInRow[0]}", courseInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.Show($"讀取【課程代碼資料檔】發生錯誤 \n 錯誤訊息:{ex}");
                }
            }
        }

        /// <summary>
        /// load 班群資料 1.成功回傳true 2.不成功回傳 false
        /// </summary>
        /// <param name="fileInfo"></param>
        private Boolean LoadCSVFileForClassGroup(FileInfo fileInfo)
        {
            using (StreamReader sr = new StreamReader(_FileInfoForcClassGroup.OpenRead()))
            {
                string row;

                try
                {
                    while ((row = sr.ReadLine()) != null)
                    {
                        string[] itemInRow = row.Split(',');

                        if (this.rgx.IsMatch(itemInRow[4]))
                        {
                            if (!_DicClassGroup.ContainsKey(itemInRow[4]))
                                _DicClassGroup.Add(itemInRow[4], itemInRow[5]);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.Show($"讀取【班群代碼檔】發生錯誤 \n 錯誤訊息:{ex}");
                }
                return true;
            }
        }


        /// <summary>
        /// 載入代碼對照表 (與課程規劃表名稱有關之欄位) 來源:全國高級中等學校_課程計畫平台 (http://course.tchcvs.tw/QueryCode.asp?T=SCH)
        /// </summary>
        private void LoadCodeMapping()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList nodeList;

            #region 課程類別

            xmlDoc.LoadXml(Properties.Resources.CourseTypes);

            nodeList = xmlDoc.SelectNodes("CourseTypes/CourseType");

            foreach (XmlNode courseType in nodeList)
            {
                string code = ((XmlElement)courseType).GetAttribute("code");
                string name = ((XmlElement)courseType).GetAttribute("name");
                this._DicCourseType.Add(code, new MappingInfo(code, name));
            }
            #endregion


            #region 群別
            //*****code(代碼有重複)

            xmlDoc.LoadXml(Properties.Resources.GroupTypes);

            nodeList = xmlDoc.SelectNodes("GroupTypes/GroupType");


            //Console.WriteLine("印出課程群別對照表 數量");
            foreach (XmlNode courseType in nodeList)
            {

                //Console.WriteLine("有重複" + courseType);
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
                this._DicClassGroup.Add(code, name);
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

                //如果 同樣名稱已存在。
                if (existdRows.Rows.Count != 0 && existdRows != null)
                {
                    {
                        DialogResult result = MsgBox.Show($"{graduationPlanName}已經存在,是否更新?", MessageBoxButtons.YesNo);

                        if (result == DialogResult.No) //如果使用者 不要覆蓋
                        {
                            return;
                        }
                        else
                        {
                            string UpDateString = $"UPDATE graduation_plan SET  content ='{content}' WHERE  name = '{graduationPlanName}' RETURNING *";

                            _Qh.Select(UpDateString);
                        }
                    }
                }
                else //如果資料庫沒有就 新增

                {
                    string insertString = $"INSERT INTO graduation_plan (name , content ) VALUES ('{graduationPlanName}' , '{content}')  RETURNING name ";

                    DataTable insertedRows = _Qh.Select(insertString);

                    if (insertedRows.Rows.Count > 0)
                    {
                        string graduationPlan = insertedRows.Rows[0][0].ToString();
                        _ListinSertSuccess.Add(graduationPlan);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show("檔案匯入失敗:" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 取得錯誤清單
        /// </summary>
        private void GetErrorMessageFile()
        {
            Workbook template = new Workbook(new MemoryStream(Properties.Resources.高中課程規劃表匯入_錯誤清單));

            Worksheet wsheet = template.Worksheets[0];
            Style styleRed = new Style();
            styleRed.ForegroundColor = Color.Red;
            int i = 0;

            Dictionary<string, CourseInfo> exportTarget = this._DictionaryLengthErr.Count > 0 ? _DictionaryLengthErr : _DicErrorCourse;

            foreach (string errKey in exportTarget.Keys)
            {
                i++;
                wsheet.Cells[i, 0].PutValue(exportTarget[errKey].CourseCodeFromMOE);
                wsheet.Cells[i, 1].PutValue(exportTarget[errKey].SubjectName);
                wsheet.Cells[i, 2].PutValue(exportTarget[errKey].Credit);
                wsheet.Cells[i, 3].PutValue(String.Join(",", exportTarget[errKey].ErrorMessage));
                wsheet.Cells[i, 3].SetStyle(styleRed);
            }
            try
            {
                template.Save(Application.StartupPath + "\\Reports\\課程規劃表匯入_錯誤清單.xlsx", SaveFormat.Xlsx);
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\課程規劃表匯入_錯誤清單.xlsx");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.SaveFileDialog sd1 = new SaveFileDialog();
                sd1.Title = "另存新檔";
                sd1.FileName = "課程規劃表匯入_錯誤清單.xlsx";
                sd1.Filter = "Excel檔案 (*.xlsx)|*.xlsx|所有檔案 (*.*)|*.*";
                if (sd1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        template.Save(sd1.FileName, SaveFormat.Xlsx);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 產生0學分課程清單
        /// </summary>
        private void GetZeroCreditCourseFile()
        {
            LuckUI();
             Workbook template = new Workbook(new MemoryStream(Properties.Resources.課程規劃表匯入_0學分清單));
            Worksheet wsheet = template.Worksheets[0];

            int i = 0;
            foreach (string key in _DicZeroCreditEachSems.Keys)
            {
                i++;
                int j = 0;
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].CourseCodeFromMOE);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].SubjectName);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].Credit);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].EnterYear); //入學年度
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].CourseTypeDetail);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].GroupCodeDetail);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].DeptCodeDetail);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].ClassGroupDetail);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].ClassClassifiedDetail);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].OpenWayDetail);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].SubjectAttributeDetail);
                wsheet.Cells[i, j++].PutValue(_DicZeroCreditEachSems[key].FieldNameDetail);
            }

            try
            {
                template.Save(Application.StartupPath + "\\Reports\\課程規劃表匯入_0學分清單.xlsx", SaveFormat.Xlsx);
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\課程規劃表匯入_0學分清單.xlsx");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.SaveFileDialog sd1 = new SaveFileDialog();
                sd1.Title = "另存新檔";
                sd1.FileName = "課程規劃表匯入_0學分清單.xlsx";
                sd1.Filter = "Excel檔案 (*.xlsx)|*.xlsx|所有檔案 (*.*)|*.*";
                if (sd1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        template.Save(sd1.FileName, SaveFormat.Xlsx);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// 選擇 MD5檔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectedClassGroup_Click(object sender, EventArgs e)
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

                    _FileInfoForcClassGroup = new FileInfo(ofd.FileNames[0]);

                    textFileCassGroup.Text = _FileInfoForcClassGroup.Name;
                }
            }
        }


        /// <summary>
        /// 鎖住UI
        /// </summary>
        private void LuckUI()
        {
            btnImprt.Enabled = false;
            btnSelectFile.Enabled = false;
            btnSelectedClassGroup.Enabled = false;
            pbLoding.Visible = true;
        }

        private void UnlockUI()
        {
            btnImprt.Enabled = true;
            btnSelectFile.Enabled = true;
            btnSelectedClassGroup.Enabled = true;
            pbLoding.Visible = false;
        }

        /// <summary>
        /// 驗證個欄位長度
        /// </summary>
        /// <param name="colstring"></param>
        /// <param name="colNum"></param>
        /// <returns></returns>
        private  string   VaildateEachColumn( string[] itemInRow)
        {
            string lenStr = "";

            if (itemInRow[0].Length != 23)
            {
                lenStr= lenStr +"【課程代碼】長度有誤";
            }
             if (itemInRow[2].Length != 6)
            {
                lenStr = lenStr + "【授課學期學分】長度有誤";
            }
            return lenStr;
        }

        /// <summary>
        /// 清除字典
        /// </summary>
        private void ClearDic()
        {
            this._DicCourseInfo.Clear();
            this._DictionaryLengthErr.Clear();
            this._DicClassGroup.Clear();
            this._DicErrorCourse.Clear();
        }
    }
}
