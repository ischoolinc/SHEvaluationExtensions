using SHSchool.Evaluation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHSchool.Evaluation
{
    /// <summary>
    /// 小幫手
    /// </summary>
    class Helper
    {
        private static readonly Dictionary<int, SchoolYearAndSemester> SchoolYearSemester 
        = new Dictionary<int, SchoolYearAndSemester>
            {
            { 1, new SchoolYearAndSemester(1,1)},
            { 2,  new SchoolYearAndSemester(1 ,2)},
            { 3,  new SchoolYearAndSemester(2 ,1)},
            { 4,  new SchoolYearAndSemester(2 ,2)},
            { 5,  new SchoolYearAndSemester(3 ,1)},
            { 6,  new SchoolYearAndSemester(3 ,2)},
            };
        //  SchoolYearAndSemester schoolSemester = new SchoolYearAndSemester();
        /// <summary>
        /// 取得年級
        /// </summary>
        /// <param name="semesterOfSix">1-6<(第幾個學期)</param>
        /// <returns></returns>
        public static SchoolYearAndSemester GetGradeYear(int semesterOfSix)
        {
            return SchoolYearSemester[semesterOfSix];
        }


        /// <summary>
        /// 科目最大的 row 數 以便新增時增加到最後一列
        /// </summary>
        /// <param name=""></param>
        public static int GetMaxRow(System.Xml.XmlElement orgainXml)
        {
            // 取得所有 
            int result;
            string MaxRow = orgainXml.SelectSingleNode("//Subject/Grouping[not(@RowIndex < //Subject/Grouping/@RowIndex)]/@RowIndex").Value;
            Int32.TryParse(MaxRow, out result); // 轉型
            return result+1;
        }

        /// <summary>
        /// 取得現有科目
        /// </summary>
        /// <param name="orgainXml"></param>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        public static int GetSubjectCount(System.Xml.XmlElement orgainXml ,string subjectName)
        {
            // 取得所有 
            int result = orgainXml.SelectNodes ($"//Subject[@SubjectName='{subjectName}']").Count;
            return result + 1;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="EntryString"></param>
        /// <returns></returns>
        public static string  GetEntryByCSVCodeDetail(string EntryString) 
        {

            string result = "";
            switch (EntryString)
            {
                case "不分屬性": //代碼 0
                    result = "學業";
                    break;
                case "一般科目": //代碼 1
                    result = "學業";
                    break;
                case "專業科目": //代碼 2
                    result = "專業科目";
                    break;
                case "實習科目": //代碼 3
                    result = "實習科目";
                    break;
                case "專精科目": //代碼 4
                    result = "學業";
                    break;
                case "專精科目(核心科目)": //代碼 5
                    result = "學業";
                    break;
                case "特殊需求領域": //代碼 6
                    result = "學業";
                    break;
                case "自主學習": //代碼 A
                    result = "學業";
                    break;
                case "選手培訓": //代碼 B
                    result = "學業";
                    break;
                case "充實(增廣)、補強性教學 [全學期、不授予學分]": //代碼 C
                    result = "學業";
                    break;
                case "充實(增廣)、補強性教學 [全學期、授予學分]": //代碼 D
                    result = "學業";
                    break;
                case "學校特色活動": //代碼 E
                    result = "學業";
                    break;
            }



            return result;

        }


        /// <summary>
        /// 取得羅馬數字
        /// </summary>
        /// <returns></returns>
        public static string GetRomaNumber( int Number) 
        {
            string result = "??";
            switch (Number)
            {
                case 1: 
                    result = "I";
                    break;
                case 2: 
                    result = "II";
                    break;
                case 3: 
                    result = "III";
                    break;
                case 4: 
                    result = "IV";
                    break;
                case 5: 
                    result = "V";
                    break;
                case 6:
                    result = "VI";
                    break;
        
            }
            return result;
        }


    }
}
