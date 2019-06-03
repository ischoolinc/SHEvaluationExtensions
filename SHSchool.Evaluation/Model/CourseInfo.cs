
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

        public string CourseCodeFromMOE { get; set; }   //Source from 教育部課代碼

        public string SubjectName { get; set; }   //課程名稱 < 各校填報至課程計畫平台 之科目名稱 >

        public string Credit { get; set; } //學分



        //拆解課程代碼   參考【課程代碼對照表】 (長度)

        public string EnterYear { get; set; }   // 1 .適用入學年度 (3)

        public string SchoolCode { get; set; }  // 2. 校代碼  (6)

        public string CourseType { get; set; }    // 3.課程類型 ex H/普通型 V/技術型 (1)

        public string GroupCode { get; set; }     // 4.群別代碼 (2)

        public string DeptCode { get; set; }     // 5. 科別代碼  (3)

        public string ClassGroup { get; set; }     // 6.班群 每校自定義 有範例檔  (1)

        //-----------------------------------------------------------------------


        public string ClassClassified { get; set; }   // 課程類別（1）

        public string OpenWay { get; set; }   // 開課方式（1）

        public string SubjectAttribute { get; set; }   // 科目屬性（1）

        public string FieldName { get; set; }    //領域名稱（2）

        public string SubjectFixedCode { get; set; }     // 科目名稱代碼（2）


        //-------------------------------------------------------------------------

        public Dictionary<int ,string> DicCreditEachSemester { get; set; }   //存放個學期學分 


        //-------對應之中文之 ---------------------------------------------------------------------

      
        public string CourseTypeDetail { get; set; }    // 3.課程類型 ex H/普通型 V/技術型 (1)

        public string GroupCodeDetail { get; set; }     // 4.群別代碼 (2)

        public string DeptCodeDetail { get; set; }     // 5. 科別代碼  (3)

        public string ClassGroupDetail { get; set; }     // 6.班群 每校自定義 有範例檔  (1)

        //-----------------------------------------------------------------------


        public string ClassClassifiedDetail { get; set; }   // 課程類別（1）

        public string OpenWayDetail { get; set; }   // 開課方式（1）

        public string SubjectAttributeDetail { get; set; }   // 科目屬性（1）

        public string FieldNameDetail { get; set; }    //領域名稱（2） 會先將"領域兩字trim"

        public string SubjectFixedCodeDetail { get; set; }     // 科目名稱代碼（2）

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



        public CourseInfo(string courseCodeFromMOE ,string subjectName ,string credit )
        {
            this.CourseCodeFromMOE = courseCodeFromMOE;
            this.SubjectName = subjectName;
            this.Credit = credit;
            SliceToEachColumn();
        }

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
            foreach (char creaditEach in CreditEach)
            {
                if (creaditEach == '0') 
                {
                    semester++;
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
    }
}
