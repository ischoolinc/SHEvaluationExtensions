using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SHSchool.Evaluation.Model
{
    /// <summary>
    ///  課程規劃表中原有項目
    /// </summary>
    public class UpdateCourseInfo
    {
        /// <summary>
        /// 裝異動資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newCourseInfo"></param>
        public UpdateCourseInfo(string id, CourseInfo newCourseInfo)
        {
            this.GPlanSysID = id;
            this.NewCourseInfo = newCourseInfo;
            this.UpdateTargets = new List<string>();
            this.ErrorList = new List<string>();

            if (newCourseInfo.Action == EnumAction.修改)
            {
                this.OldSubjectCode = newCourseInfo.OrginCourseCodeFromMOE;
                this.OldSujectName = newCourseInfo.OrginSubjectName;
                this.NewSubjectCode = newCourseInfo.New課程代碼;
                this.NewSubjectName = newCourseInfo.NewSubjectName;

                // todo  判斷 科目代碼 及 科目皆無修改

                // 如果 科目代碼 及科目名稱皆無修改 
                if (newCourseInfo.OrginCourseCodeFromMOE == newCourseInfo.New課程代碼 && (newCourseInfo.OrginSubjectName == newCourseInfo.NewSubjectName))
                {
                    newCourseInfo.Action = EnumAction.修改授課學期學分_代碼;
                }

            }
            else if (newCourseInfo.Action == EnumAction.新增)
            {
                this.NewSubjectCode = newCourseInfo.New課程代碼;
                this.NewSubjectName = newCourseInfo.NewSubjectName;

            }
            else if (newCourseInfo.Action == EnumAction.刪除)
            {
                this.NewSubjectCode = newCourseInfo.New課程代碼;
                this.OldSujectName = newCourseInfo.NewSubjectName;
            }



        }


        public string GPlanSysID { get; set; }
        /// <summary>
        /// 系統原有課程代碼 
        /// </summary>
        public string OldSubjectCode { get; set; }

        /// <summary>
        /// 科目名稱
        /// </summary>

        public string OldSujectName { get; set; }

        /// <summary>
        /// 學分
        /// </summary>
        public string EachSemsterStatus { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string NewSubjectCode { get; set; }

        public string NewSubjectName { get; set; }

        public string NewEachSemsterStatus { get; set; }

        /// <summary>
        ///  CSV 讀進來異動資料
        /// </summary>
        public CourseInfo NewCourseInfo { get; set; }

        /// <summary>
        /// 更新 Target 
        /// </summary>
        public List<string> UpdateTargets { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public List<string> ErrorList { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetUpdateColumns()
        {
            return string.Join(" , ", this.UpdateTargets);


        }

        public string GetErrorInfo()
        {
            return string.Join(" , ", this.ErrorList);
        }


        public void AddErrorList(string errorInfo)
        {
            this.ErrorList.Add(errorInfo);
        }

        /// <summary>
        /// 產生 更新的 targetlist 
        /// </summary>
        /// <param name="oldXmlElement"></param>
        public void GenerateUpdateTarget(XmlElement oldXmlElement, CourseInfo courseInfo) // 第一筆
        {
            // 1.比較實際差異  (與級別和學分相關的不比對)
            /*
        <Subject 
        
            ="" 
           Credit="4"  ==> 差異比對時不會更新
           Domain="數學"                          
           Entry="學業"               
           GradeYear="1"  ==> 差異比對時不會更新 
           Level="1"  ==> 差異比對時不會更新
           FullName="數學 I"
           NotIncludedInCalc="False"  => 不影響
           NotIncludedInCredit="False" => 不影響
           Required="必修" 
           RequiredBy="部訂"
           Semester="1"
           SubjectName="數學" 
           課程代碼="108070406M1119601010203"
           課程類別="部定必修" 
           開課方式="原班級" 
           科目屬性="一般科目"
           領域名稱="數學" 
           課程名稱="數學"  ==>  與前面藍未重複 (subjectName) 可以廢除
           學分="4"   ==> 與前面欄位重複(Credit) 可以廢除
           >   差異比對時不會更新
           授課學期學分 ⇒ 本次增加
            <Grouping RowIndex="3" startLevel="1" />
        </Subject>
           */


            if (oldXmlElement == null)
            {
                ErrorList.Add($"{courseInfo.OrginCourseCodeFromMOE} 系統內找不到對應 ");
                return;
            }


            // 1. 領域
            this.CollectIfDifferent("領域", oldXmlElement.GetAttribute("Domain"), courseInfo.領域名稱);
            // todo
            // 2. 課程全名 (課成全名 因需比對 對應之level 需全部下去跑 ) 所以這邊邏輯為科目名稱有改 及代表 課程全名也會變更 但是更新時會較為複雜 還是要補寫邏輯
            this.CollectIfDifferent("課程全名", oldXmlElement.GetAttribute("SubjectName"), courseInfo.NewSubjectName);
            // 3. 必修選修
            this.CollectIfDifferent("必修選修", oldXmlElement.GetAttribute("Required"), courseInfo.Required);
            // 4. 校定部定 
            this.CollectIfDifferent("校定部定", oldXmlElement.GetAttribute("RequiredBy"), courseInfo.RequiredBy);
            // 5. 科目名稱
            this.CollectIfDifferent("科目名稱", oldXmlElement.GetAttribute("SubjectName"), courseInfo.NewSubjectName);
            // 6. 課程代碼
            this.CollectIfDifferent("課程代碼(課程計畫平台)", oldXmlElement.GetAttribute("課程代碼"), courseInfo.New課程代碼);
            // 課程類別
            this.CollectIfDifferent("課程類別(課程計畫平台)", oldXmlElement.GetAttribute("課程類別"), courseInfo.課程類別說明);
            // 7. 開課方式
            this.CollectIfDifferent("開課方式(課程計畫平台)", oldXmlElement.GetAttribute("開課方式"), courseInfo.開課方式);
            // 8. 科目屬性
            this.CollectIfDifferent("科目屬性(課程計畫平台)", oldXmlElement.GetAttribute("科目屬性"), courseInfo.科目屬性說明);
            // 9.領域名稱
            this.CollectIfDifferent("領域名稱(課程計畫平台)", oldXmlElement.GetAttribute("領域名稱"), courseInfo.領域名稱);
            // 10
            if (oldXmlElement.HasAttribute("授課學期學分"))
            {
                this.CollectIfDifferent("授課學期學分代碼", oldXmlElement.GetAttribute("授課學期學分"), courseInfo.授課學期學分);
            }
            else
            {
                this.CollectIfDifferent("授課學期學分代碼", "", courseInfo.授課學期學分);
            }
        }



        /// <summary>
        /// 比較新舊是否有差異 如有差異 增加到異動清單裡
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="oldOne"></param>
        /// <param name="newOne"></param>
        public void CollectIfDifferent(string columnName, string oldOne, string newOne)
        {
            if (oldOne != newOne)
            {
                this.UpdateTargets.Add(columnName);
            }
        }

        /// <summary>
        /// 取得值
        /// </summary>
        /// <returns></returns>
        public string GetAttributeValue()
        {

            return null;
        }
    }
}
