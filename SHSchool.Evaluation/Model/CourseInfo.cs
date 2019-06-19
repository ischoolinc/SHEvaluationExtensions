
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHSchool.Evaluation.Model
{


    // Q :如何知道是同一份課程規劃表 ?
    // Q :班群是甚麼?
    class CourseInfo
    {
        /// <summary>
        /// 教育部課程代碼
        /// </summary>
        public string CourseCodeFromMOE { get; set; }   //Source from 教育部課代碼
        /// <summary>
        /// 課程名稱
        /// </summary>
        public string SubjectName { get; set; }   //課程名稱 < 各校填報至課程計畫平台 之科目名稱 >
        /// <summary>
        /// 學分
        /// </summary>
        public string Credit { get; set; } //學分



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
        public Dictionary<int ,string> DicCreditEachSemester { get; set; }   //


        //-------對應之中文之 ---------------------------------------------------------------------

        /// <summary>
        /// 課程類型  H/普通型 V/技術型 (1)
        /// </summary>
        public string CourseTypeDetail { get; set; }
        /// <summary>
        /// 群別代碼 (2)
        /// </summary>
        public string GroupCodeDetail { get; set; }  
        /// <summary>
        /// 科別代碼  (3)
        /// </summary>
        public string DeptCodeDetail { get; set; }     

        /// <summary>
        /// 班群 每校自定義 有範例檔  (1)
        /// </summary>
        public string ClassGroupDetail { get; set; }


      
        /// <summary>
        /// 課程類別 
        /// </summary>
        public string ClassClassifiedDetail { get; set; }   
        /// <summary>
        ///  開課方式
        /// </summary>
        public string OpenWayDetail { get; set; }   
        /// <summary>
        /// 科目屬性
        /// </summary>
        public string SubjectAttributeDetail { get; set; }   
        /// <summary>
        /// 領域名稱
        /// </summary>
        public string FieldNameDetail { get; set; }   
        /// <summary>
        /// 科目名稱代碼
        /// </summary>
        public string SubjectFixedCodeDetail { get; set; }    

        /// <summary>
        /// level  
        /// </summary>
        public int  StartLevel { get; set; }


        /// <summary>
        /// 對應至原本欄位 Required  由 課程類別 轉換而來
        /// </summary>
        public string Required { get; set; }

        /// <summary>
        /// 對應至原本欄位 RequiredBy  由 課程類別 轉換而來
        /// </summary>
        public string  RequiredBy  { get; set; }

        /// <summary> 
        ///  對應至原本欄位  Entry 
        /// </summary>
        public string  Entry { get; set; }

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
        /// 初始化
        /// </summary>
        /// <param name="courseCodeFromMOE"></param>
        /// <param name="subjectName"></param>
        /// <param name="credit"></param>
        public CourseInfo(string courseCodeFromMOE ,string subjectName ,string credit )
        {
            this.CourseCodeFromMOE = courseCodeFromMOE;
            this.SubjectName = subjectName;
            this.Credit = credit;
            this.StartLevel = 1;
            this.ErrorMessage = new List<string>();
           
            SliceToEachColumn();
        }
        /// <summary>
        /// 解析對應代碼
        /// </summary>
        private void SliceToEachColumn()
        {
            this.EnterYear = CourseCodeFromMOE.Substring(0, 3);
            this.SchoolCode = CourseCodeFromMOE.Substring(3, 6);
            this.CourseType = CourseCodeFromMOE.Substring(9, 1);
            this.GroupCode = CourseCodeFromMOE.Substring(10, 2);
            this.DeptCode = CourseCodeFromMOE.Substring(12, 3);
            this.ClassGroup = CourseCodeFromMOE.Substring(15, 1);
            this.ClassClassified = CourseCodeFromMOE.Substring(16, 1);
            this.OpenWay = CourseCodeFromMOE.Substring(17, 1);
            this.SubjectAttribute = CourseCodeFromMOE.Substring(18, 1);
            this.FieldName = CourseCodeFromMOE.Substring(19, 2);
            this.SubjectFixedCode = CourseCodeFromMOE.Substring(21, 2);
            GetListCreditEachSemester();   //將學分代碼轉換 至各學期對應之dic

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

           char[] CreditEach  = this.Credit.ToCharArray();

            //放進各學期學分數的Dic裡
            int semester = 1;
            int creditZeroCount = 0;
            foreach (char creaditEach in CreditEach)
            {
                if (creaditEach == '0') 
                {
                    semester++;
                    creditZeroCount++; //計算0學分之數目 若六學期皆為0可印出
                    if (creditZeroCount==6)
                    {
                        this.IsZeroCreditEachSem = true;
                    }
                    continue;

                }else//不等於 0
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
        /// <returns></returns>
        public string GetCurriiculumMapName()
        {

           CurrucyCurriculumMapName = $"{ this.EnterYear }{this.DeptCodeDetail}{this.ClassGroupDetail}";
            return CurrucyCurriculumMapName;
        }
    }
}
