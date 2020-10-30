
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SHSchool.Evaluation.Model
{


    // Q :如何知道是同一份課程規劃表 ?
    // Q :班群是甚麼?


    public class CourseInfo 
    {

        /// <summary>
        /// 教育部課程代碼
        /// </summary>
        public string 新課程代碼 { get; set; }   //Source from 教育部課代碼
        /// <summary>
        /// 課程名稱
        /// </summary>
        public string NewSubjectName { get; set; }   //課程名稱 < 各校填報至課程計畫平台 之科目名稱 >
        /// <summary>
        /// 學分
        /// </summary>
        public string 授課學期學分 { get; set; } //學分


        /// <summary>
        /// 科目名稱+級別   對應xml Attrubute : "FullName"
        /// </summary>
        public string  NewSubjectNameWithLevel { get; set; }
        

        public string GraduationPlanCode { get; set; } //識別 課程規劃表code

        //拆解課程代碼   參考【課程代碼對照表】 (長度)
        /// <summary>
        /// 適用入學年度 (3)
        /// </summary>
        public string EnterYear { get; set; }
        /// <summary>
        ///  校代碼  (6)
        /// </summary>
        public string SchoolCode { get; set; }
        /// <summary>
        /// 課程類型 ex H/普通型 V/技術型 (1)
        /// </summary>
        public string CourseType { get; set; }
        /// <summary>
        /// 群別代碼 (2)
        /// </summary>
        public string GroupCode { get; set; }
        /// <summary>
        ///  科別代碼  (3)
        /// </summary>
        public string DeptCode { get; set; }
        /// <summary>
        /// 班群 每校自定義 有範例檔  (1)
        /// </summary>
        public string ClassGroup { get; set; }

        //-----------------------------------------------------------------------

        /// <summary>
        /// 課程類別（1）
        /// </summary>
        public string ClassClassified { get; set; }
        /// <summary>
        ///  開課方式（1）
        /// </summary>
        public string OpenWay { get; set; }
        /// <summary>
        /// 科目屬性（1）
        /// </summary>
        public string SubjectAttribute { get; set; }
        /// <summary>
        /// 領域名稱（2）
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 科目名稱代碼（2）
        /// </summary>
        public string SubjectFixedCode { get; set; }


        //-------------------------------------------------------------------------

        /// <summary>
        /// 存放個學期學分 
        /// </summary>
        public Dictionary<int, string> DicCreditEachSemester { get; set; }


      //=================解析後對應中文===================

        /// <summary>
        /// 課程類型    【H/普通型 V/技術型 ...】
        /// </summary>
        public string 課程類型說明 { get; set; }
        public bool Is_課程類型_Update { get; set; }


        /// <summary>
        /// 群別代碼    【11/學術群 11/學術學程 21/機械群 22/動力機械群】
        /// </summary>
        public string 群別代碼說明 { get; set; }
        public bool Is_群別代碼_Update { get; set; }


        /// <summary>
        /// 科別代碼  099/其他 |M|  101/普通班 |H|V|C|S|  101/特色班 |H| 101/實驗班 |H|
        /// </summary>
        public string 科別代碼說明 { get; set; }
        public bool Is_科別代碼說明_Update { get; set; }


        /// <summary>
        /// 班群 每校自定義 0/不分班群 1/建教合作-輪調式
        /// </summary>
        public string 班群碼說明 { get; set; }
        public bool Is_班群碼說明_Update { get; set; }

        /// <summary>
        /// 課程類別  1/部定必修  2/校訂必修 3/選修-加深加廣選修 4/選修-補強性選修 5/選修-多元選修
        /// </summary>
        public string 課程類別說明 { get; set; }
        public bool Is_課程類別說明_Update { get; set; }


        /// <summary>
        ///  開課方式 0/原班級  1/跨班選修(班群開課) 2/綜高跨年級選修 A/同科單班選修 B/同科跨班選修
        /// </summary>
        public string 開課方式 { get; set; }
        public bool Is_開課方式_Update { get; set; }

        /// <summary>
        /// 科目屬性  0/不分屬性 1/一般科目 2/專業科目 3/實習科目 4/專精科目 5/專精科目(核心科目) 6/特殊需求領域 A/自主學習 B/選手培訓 
        /// </summary>
        public string 科目屬性說明 { get; set; }
        public bool Is_科目屬性_Update { get; set; }

        /// <summary>
        /// 領域名稱
        /// </summary>
        public string 領域名稱 { get; set; }
        public bool Is_領域名稱_Update { get; set; }

        /// <summary>
        /// 科目名稱代碼
        /// </summary>
        public string 科目名稱代碼 { get; set; }

        /// <summary>
        /// level  
        /// </summary>
        public int StartLevel { get; set; }


        /// <summary>
        /// 對應至原本欄位 Required  由 課程類別 轉換而來
        /// </summary>
        public string Required { get; set; }

        /// <summary>
        /// 對應至原本欄位 RequiredBy  由 課程類別 轉換而來
        /// </summary>
        public string RequiredBy { get; set; }

        /// <summary> 
        ///  對應至原本欄位  Entry 
        /// </summary>
        public string Entry { get; set; }

        /// <summary>
        /// 是否每學期接0學分
        /// </summary>
        public Boolean IsZeroCreditEachSem { get; set; }

        /// <summary>
        /// 課程規劃表名稱
        /// </summary>
        public string CurrucyCurriculumMapName { get; set; }

        /// <summary>
        /// 有錯誤之訊息
        /// </summary>
        public List<string> ErrorMessage { get; set; }


        /// <summary>
        /// CSV 讀進來後 根據規則判斷出 要執行那些動作
        /// </summary>
        public EnumAction Action { get; set; }

        public string OrginCourseCodeFromMOE { get; set; }

        public string OrginSubjectName { get; set; }

        /// <summary>
        /// 處初始化課程規劃表
        /// </summary>
        /// <param name="courseCodeFromMOE"></param>
        /// <param name="subjectName"></param>
        /// <param name="credit"></param>
        /// <param name="orginCourseCodeFromMOE"></param>
        /// <param name="orginSubjectName"></param>
        public CourseInfo(string courseCodeFromMOE, string subjectName, string credit, string orginCourseCodeFromMOE, string orginSubjectName)
        {
            if (courseCodeFromMOE == "" && subjectName == "")
            {
                this.Action = EnumAction.刪除;
                this.新課程代碼 = orginCourseCodeFromMOE;
                this.NewSubjectName = orginSubjectName;
            }
            else {

                this.新課程代碼 = courseCodeFromMOE;
                this.NewSubjectName = subjectName;
            }
         
            this.授課學期學分 = credit;
            this.GraduationPlanCode = this.新課程代碼.Substring(0, 16);
            this.StartLevel = 1; //開始級別設為1
            this.OrginCourseCodeFromMOE = orginCourseCodeFromMOE;
            this.OrginSubjectName = orginSubjectName;
            this.SetActionByCSV(courseCodeFromMOE, subjectName, credit, orginCourseCodeFromMOE, orginSubjectName);
            this.ErrorMessage = new List<string>();

            SliceToEachColumn();
        }
        /// <summary>
        /// 解析對應代碼
        /// </summary>
        private void SliceToEachColumn()
        {
            if (新課程代碼.Length == 23 )
            {
                this.EnterYear = 新課程代碼.Substring(0, 3);
                this.SchoolCode = 新課程代碼.Substring(3, 6);
                this.CourseType = 新課程代碼.Substring(9, 1);
                this.GroupCode = 新課程代碼.Substring(10, 2);
                this.DeptCode = 新課程代碼.Substring(12, 3);
                this.ClassGroup = 新課程代碼.Substring(15, 1);
                this.ClassClassified = 新課程代碼.Substring(16, 1);
                this.OpenWay = 新課程代碼.Substring(17, 1);
                this.SubjectAttribute = 新課程代碼.Substring(18, 1);
                this.FieldName = 新課程代碼.Substring(19, 2);
                this.SubjectFixedCode = 新課程代碼.Substring(21, 2);
                GetListCreditEachSemester();   //將學分代碼轉換 至各學期對應之dic
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 將學分之代碼拆解至各學期
        /// </summary>
        private void GetListCreditEachSemester()
        {
            this.DicCreditEachSemester = new Dictionary<int, string>();

            Dictionary<char, string> creditConvert = new Dictionary<char, string>(); //針對對開課程 A-D(代碼)/1-4(學分) 放對照數
            creditConvert.Add('A', "1");
            creditConvert.Add('B', "2");
            creditConvert.Add('C', "3");
            creditConvert.Add('D', "4");

            char[] CreditEach = this.授課學期學分.ToCharArray();

            //放進各學期學分數的Dic裡
            int semester = 1;
            int creditZeroCount = 0;
            foreach (char creaditEach in CreditEach)
            {
                if (creaditEach == '0')
                {
                    semester++;
                    creditZeroCount++; //計算0學分之數目 若六學期皆為0可印出
                    if (creditZeroCount == 6)
                    {
                        this.IsZeroCreditEachSem = true;
                    }
                    continue;

                }
                else//不等於 0
                {
                    //為ABCD
                    if (creaditEach == 'A' || creaditEach == 'B' || creaditEach == 'C' || creaditEach == 'D')
                    {
                        this.DicCreditEachSemester.Add(semester, creditConvert[creaditEach]);
                        semester++;
                    }
                    else
                    {
                        this.DicCreditEachSemester.Add(semester, creaditEach.ToString());
                        semester++;
                    }
                }
            }
        }

        /// <summary>
        /// 取得課程規劃表名稱
        /// </summary>
        /// <returns></returns>GetCurriiculumMapName
        public string GetCurriiculumMapName()
        {
            
            CurrucyCurriculumMapName = $"{ this.EnterYear }{this.科別代碼說明}{this.班群碼說明}";
            return CurrucyCurriculumMapName;
        }


        public string GetGradustionPlanKey()
        {
            return this.GraduationPlanCode;
        }


        /// <summary>
        /// 根據CSV檔 判斷本次動作課程執行動作
        /// </summary>
        internal void SetActionByCSV(string courseCodeFromMOE, string subjectName, string credit, string orginCourseCodeFromMOE, string orginSubjectName)
        {
            // 如果後面4 5皆有東西
            if (!string.IsNullOrEmpty(orginCourseCodeFromMOE) && !string.IsNullOrEmpty(orginSubjectName))  // 如果讀取進來第四列第五列有東西其中一列有資料
            {
                if (this.Action != EnumAction.刪除)
                {
                    this.Action = EnumAction.修改;
                }

            }
            else if (string.IsNullOrEmpty(orginCourseCodeFromMOE) && string.IsNullOrEmpty(orginSubjectName)) //如果 4 5 行皆空白
            {
                this.Action = EnumAction.新增;
            }
        }


        /// <summary>
        /// 與系統內部進行比對 確認動作 (主要用於確認 新增/未調整科目 項目)
        /// </summary>
        /// <param name="xmlElement"></param>
        internal void SetActionByCompareToData(OldGraduationPlanInfo oldGPlanInfo)
        {
            bool ContainNewCourseMOECode = oldGPlanInfo.IsContainSubjectCode(this.新課程代碼);

            if (ContainNewCourseMOECode)  //此科目已經存 如果此科目已經存在
            {
                this.Action = EnumAction.未調整;

            }
        }

        public CourseInfo Clone()
        {
            //建立物件的淺層複製
            return (CourseInfo)this.MemberwiseClone();
        }

    }
}
