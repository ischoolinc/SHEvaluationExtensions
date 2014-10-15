using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.DocumentValidator;
using Campus.Import;
using K12.Data;
using SHSchool.Data;

namespace SHSchool.Evaluation
{
    /// <summary>
    /// 匯入課程規劃表
    /// </summary>
    public class ImportProgramPlan : ImportWizard
    {
        private ImportOption mOption;
        private StringBuilder mstrBuilder = new StringBuilder();
        private const string mProgramPlanName = "課程規劃表名稱";
        private const string mDomain = "領域名稱";
        private const string mEntry = "分項名稱";
        private const string mSubject = "科目名稱";
        private const string mGradeYar = "年級";
        private const string mSemester = "學期";
        private const string mLevel = "科目級別";
        private const string mCredit = "學分數";
        private const string mRequiredBy = "校定/部訂";
        private const string mRequired = "必選修";
        private const string mNotIncludeInCalc = "不需評分";
        private const string mNotIncludeInCredit = "不計學分";

        public ImportProgramPlan()
        {
            this.CustomValidate = (Rows, Messages) =>
            {
                Dictionary<string, List<IRowStream>> ProgramPlans = new Dictionary<string, List<IRowStream>>();

                #region 根據課程規劃名稱、科目名稱及科目屬性分類
                Rows.ForEach
                (x =>
                    {
                        string Level = x.GetValue(mLevel);

                        if (!string.IsNullOrEmpty(Level))
                        {
                            string ProgramPlanName = x.GetValue(mProgramPlanName);
                            string Subject = x.GetValue(mSubject);
                            string Domain = x.GetValue(mDomain);
                            string Entry = x.GetValue(mEntry);
                            string RequiredBy = x.GetValue(mRequiredBy);
                            string Required = x.GetValue(mRequired);
                            string NotIncludeInCalc = x.GetValue(mNotIncludeInCalc);
                            string NotIncludeInCredit = x.GetValue(mNotIncludeInCredit);

                            string SubjectKey = ProgramPlanName + "_" +
                                Subject + "_" + 
                                Domain + "_" + 
                                Entry + "_" + 
                                RequiredBy + "_" + 
                                Required + "_" +
                                NotIncludeInCalc + "_" + 
                                NotIncludeInCredit;

                            if (!ProgramPlans.ContainsKey(SubjectKey))
                                ProgramPlans.Add(SubjectKey, new List<IRowStream>());

                            ProgramPlans[SubjectKey].Add(x); 
                        }
                    }
                );
                #endregion

                #region 科目群組其級別是否連續
                foreach (string SubjectKey in ProgramPlans.Keys)
                {
                    List<IRowStream> SubjectRows = ProgramPlans[SubjectKey];

                    List<int> Levels = new List<int>();

                    bool IsContinuous = true;

                    #region 取得所有級別清單
                    SubjectRows.ForEach(x =>
                        {
                            int Level = K12.Data.Int.Parse(x.GetValue(mLevel));

                            if (!Levels.Contains(Level))
                                Levels.Add(Level);
                        }
                    );
                    #endregion

                    #region 判斷級別是否連續
                    int MaxLevel = Levels.Max();
                    int MinLevel = Levels.Min();

                    for (int i = MinLevel; i <= MaxLevel; i++)
                        if (!Levels.Contains(i))
                            IsContinuous = false;
                    #endregion

                    #region 若不連續寫入錯誤訊息
                    if (!IsContinuous)
                        SubjectRows.ForEach(x => Messages[x.Position].MessageItems.Add(new
                            Campus.Validator.MessageItem(Campus.Validator.ErrorType.Error, Campus.Validator.ValidatorType.Row, "科目『" + x.GetValue(mSubject) + "』相同屬性其級別必須連續！")));
                    #endregion
                }
                #endregion
            };
        }

        #region XML規格
        //<Subject 
        //    Category="一般科目" 
        //    Credit="1" 
        //
        //    Domain="生活" 
        //    Entry="學業" 
        //
        //    FullName="公民與社會 I"
        //
        //    SubjectName="公民與社會"
        //    Level="1" 
        //
        //    GradeYear="1" 
        //    Semester="1"
        //
        //    NotIncludedInCalc="False"
        //    NotIncludedInCredit="False" 
        //
        //    Required="必修"
        //    RequiredBy="部訂"
        //    >
        //    <Grouping RowIndex="1" startLevel="1"/>
        //</Subject>
        #endregion

        /// <summary>
        /// 取得支援的匯入動作
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate;
        }

        /// <summary>
        /// 取得驗證則
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return Properties.Resources.ProgramPlan;
            //return "http://sites.google.com/a/kunhsiang.com/k12evaluation/validationrule/ProgramPlan.xml";
        }

        /// <summary>
        /// 匯入前準備
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            mOption = Option;
        }
        
        /// <summary>
        /// 實際分批匯入
        /// </summary>
        /// <param name="Rows"></param>
        /// <returns></returns>
        public override string Import(List<IRowStream> Rows)
        {
            if (mOption.Action == ImportAction.InsertOrUpdate)
            {
                #region 尋找系統中已有課程規劃物件
                Dictionary<string, SHProgramPlanRecord> UpdateProgramPlans = ProgramPlanHelper
                    .SelectProgramPlanByRows(Rows, mProgramPlanName)
                    .ToDictionary(x=>x.Name);

                Dictionary<string, SHProgramPlanRecord> NewProgramPlans = new Dictionary<string, SHProgramPlanRecord>();
                #endregion

                #region 將IRowStream轉成對應的物件
                foreach(IRowStream x in Rows)
                {
                    #region 鍵值欄位（課程規劃表名稱、科目名稱、級別）
                    string ProgramPlanName = x.GetValue(mProgramPlanName);       //課程規劃表名稱
                    string SubjectName = x.GetValue(mSubject);                   //年級
                    string GradeYear = x.GetValue(mGradeYar);                    //年級
                    string Semester = x.GetValue(mSemester);                     //年級
                    #endregion

                    #region 科目屬性
                    string Domain = x.GetValue(mDomain);                                                   //領域
                    string Entry = x.GetValue(mEntry);                                                     //分項
                    string RequiredBy = x.GetValue(mRequiredBy).Equals("校") ? "校訂" : "部訂";              //校部訂
                    bool Required = x.GetValue(mRequired).Equals("必") ? true : false;                     //必選修
                    bool NotIncludeInCalc = x.GetValue(mNotIncludeInCalc).Equals("是") ? true : false;     //不需評分
                    bool NotIncludeInCredit = x.GetValue(mNotIncludeInCredit).Equals("是") ? true : false; //不計學分
                    #endregion

                    #region 取得課程規劃表物件，若不存在則新增
                    SHProgramPlanRecord ProgramPlanRecord = UpdateProgramPlans.ContainsKey(ProgramPlanName) ? UpdateProgramPlans[ProgramPlanName] : null;

                    if (ProgramPlanRecord == null)
                    {
                        ProgramPlanRecord = NewProgramPlans.ContainsKey(ProgramPlanName) ? NewProgramPlans[ProgramPlanName] : null;

                        if (ProgramPlanRecord == null)
                        {
                            ProgramPlanRecord = new SHProgramPlanRecord();
                            ProgramPlanRecord.Name = ProgramPlanName;
                            NewProgramPlans.Add(ProgramPlanRecord.Name, ProgramPlanRecord);
                        }
                    }
                    #endregion

                    #region 依『科目名稱』、『年級』及『學期』尋找科目，若無則新增
                    ProgramSubject Subject = ProgramPlanRecord.Subjects
                        .Find(v => v.SubjectName.Equals(SubjectName)
                            && (""+v.GradeYear).Equals(GradeYear) 
                            && (""+v.Semester).Equals(Semester));

                    if (Subject == null)
                    {
                        Subject = new ProgramSubject();
                        Subject.Category = string.Empty;
                        Subject.SubjectName = SubjectName;
                        Subject.GradeYear = K12.Data.Int.Parse(GradeYear);
                        Subject.Semester = K12.Data.Int.Parse(Semester);
                        Subject.RowIndex = ProgramPlanRecord.Subjects.GetRowIndex(
                            SubjectName,
                            Domain,
                            Entry,
                            RequiredBy,
                            Required,
                            NotIncludeInCalc,
                            NotIncludeInCredit);
                        ProgramPlanRecord.Subjects.Add(Subject);
                    }
                    #endregion

                    #region 將相關欄位值填入
                    if (mOption.SelectedFields.Contains(mLevel))
                    {
                        string Level = x.GetValue(mLevel);
                        int? intLevel = null;
                        if (!string.IsNullOrEmpty(Level))
                            intLevel = K12.Data.Int.Parse(Level);
                        Subject.Level = intLevel;

                        #region 處理StartLevel，只有考有填Level（級別）的情況
                        if (Subject.Level.HasValue)
                        {
                            List<ProgramSubject> LevelSubjects = ProgramPlanRecord.Subjects
                                .FindAll(m =>
                                    m.SubjectName.Equals(SubjectName) &&
                                    m.Level != null &&
                                    m.Domain == Domain &&
                                    m.Entry == Entry &&
                                    m.RequiredBy == RequiredBy &&
                                    m.Required == Required &&
                                    m.NotIncludedInCalc == NotIncludeInCalc &&
                                    m.NotIncludedInCredit == NotIncludeInCredit
                                    );

                            LevelSubjects.Add(Subject);

                            #region 全部重新計算StartLevel，確保值正確
                            int StartLevel = int.MaxValue;

                            LevelSubjects.ForEach(m =>
                                {
                                    if (m.Level < StartLevel)
                                        StartLevel = m.Level.Value;
                                });

                            if (StartLevel < int.MaxValue)
                                LevelSubjects.ForEach(m => m.StartLevel = StartLevel);
                            #endregion
                        }
                        #endregion
                    }

                    //組合成科目完成名稱
                    Subject.FullName = Subject.Level.HasValue ? Subject.SubjectName + " " + GetNumber(Subject.Level.Value) : Subject.SubjectName;
                    Subject.Domain = Domain;
                    Subject.Entry = Entry;
                    Subject.Credit = K12.Data.Decimal.Parse(x.GetValue(mCredit));
                    Subject.RequiredBy = RequiredBy;
                    Subject.Required = Required;
                    Subject.NotIncludedInCalc = NotIncludeInCalc;
                    Subject.NotIncludedInCredit = NotIncludeInCredit;
                    #endregion
                }

                #region 實際新增或更新課程規劃表
                if (UpdateProgramPlans.Count > 0)
                {
                    int UpdateCount = SHProgramPlan.Update(UpdateProgramPlans.Values);
                }

                if (NewProgramPlans.Count > 0)
                {
                    List<string> NewProgramIDs = SHProgramPlan.Insert(NewProgramPlans.Values);
                }
                #endregion

                #endregion
            }
            else if (mOption.Action == ImportAction.Delete)
            {
 
            }

            return mstrBuilder.ToString();
        }

        /// <summary>
        /// 根據級別數字取得級別字串
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetNumber(int p)
        {
            string levelNumber;
            switch (p)
            {
                #region 對應levelNumber
                case 1:
                    levelNumber = "I";
                    break;
                case 2:
                    levelNumber = "II";
                    break;
                case 3:
                    levelNumber = "III";
                    break;
                case 4:
                    levelNumber = "IV";
                    break;
                case 5:
                    levelNumber = "V";
                    break;
                case 6:
                    levelNumber = "VI";
                    break;
                case 7:
                    levelNumber = "VII";
                    break;
                case 8:
                    levelNumber = "VIII";
                    break;
                case 9:
                    levelNumber = "IX";
                    break;
                case 10:
                    levelNumber = "X";
                    break;
                default:
                    levelNumber = "" + (p);
                    break;
                #endregion
            }
            return levelNumber;
        }

    }
}