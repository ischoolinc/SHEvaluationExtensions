using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHSchool.Evaluation.Extension;
namespace SHSchool.Evaluation.Model
{
    /// <summary>
    ///  裝課程規劃表 csv讀進來的
    /// </summary>
    public class GraduationPlanInfo
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseInfo">課程資訊</param>
        /// <param name="mTypecourseInfos"></param>
        public GraduationPlanInfo(CourseInfo courseInfo)
        {
           
            this.GraduationName = courseInfo.GetCurriiculumMapName();
            this.EntryYear = courseInfo.EnterYear;
            this.CourseType = courseInfo.CourseType;
            this.GraduationPlanKey = courseInfo.GraduationPlanCode;
            this.CourseInfos = new Dictionary<string, CourseInfo>();
            this.GraduationPlanKeys = new List<string>();
            this.ListCourseInfos = new List<CourseInfo>();

            // this.MTypeGrade1CourseInfo = mTypecourseInfos;
            // this.ListCourseInfos.AddRange( courseInfos);
            this.GetGraduationPlanCode(); // 取的PlanCode
            this.DicOldGraduationPlanInfos = new Dictionary<string, OldGraduationPlanInfo>();
        }

        public bool Has196CourseInfo { get; set; } //檔案讀進來原始版

        public string GraduationName { get; set; }

        /// <summary>
        /// 課程規劃表下的課程
        /// </summary>
        public List<CourseInfo> ListCourseInfos { get; set; }

        /// <summary>
        /// 入學年度
        /// </summary>
        public string EntryYear { get; set; }

        /// <summary>
        ///  課程類型 應該是一樣的
        /// </summary>
        public string CourseType { get; set; }
        /// <summary>
        /// 課程規劃表key值 
        /// </summary>
        public string GraduationPlanKey { get; set; }

        /// <summary>
        ///  一年級不分班群之課程(檔案讀進來原始版)
        /// </summary>
        public List<CourseInfo> MTypeGrade1CourseInfo { get; set; }
        /// <summary>
        /// M 綜合型高中 可能包含1年級  所以會有兩組key值
        /// </summary>
        public List<string> GraduationPlanKeys { get; set; }

        /// <summary>
        /// 課程規劃表名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 有兩個以上的key 不太正常
        /// </summary>

        public bool HasOverOneKey { get; set; }

        /// <summary>
        /// 課程規劃表下的課程資訊 key 課程規壞表名稱
        /// </summary>
        Dictionary<string, CourseInfo> CourseInfos { get; set; }

        /// <summary>
        /// 對應舊資料 OldGraduationInfo key =id 
        /// </summary>
        public Dictionary<string, OldGraduationPlanInfo> DicOldGraduationPlanInfos { get; set; }

        /// <summary>
        /// 把 同學年度學期的加進來
        /// </summary>
        public void AddMtypeCourses(List<CourseInfo> MtypcourseInfos)
        {
            this.MTypeGrade1CourseInfo = MtypcourseInfos;
            this.ListCourseInfos.InsertRange(0, MtypcourseInfos.ConvertAll(courseInfo => courseInfo.Clone()));// 複製到課程規劃表裡
        }


        /// <summary>
        ///  加入課程
        /// </summary>
        /// <param name="courseInfo"></param>
        public void AddCourseInfo(CourseInfo courseInfo)
        {
            this.ListCourseInfos.Add(courseInfo);
        }

        /// <summary>
        /// 取得所有課程清單
        /// </summary>
        public List<CourseInfo> GetAllCourseInfoList()
        {
            return this.ListCourseInfos;
        }

        /// <summary>
        /// 取得課程規劃表code
        /// </summary>
        private void GetGraduationPlanCode()
        {
            foreach (string courseName in this.CourseInfos.Keys)
            {
                // 處理 GraduationCode
                if (CourseInfos[courseName].GraduationPlanCode.Substring(12, 3) != "196")
                {
                    if (this.GraduationPlanKey != "")
                    {
                        this.GraduationPlanKey = CourseInfos[courseName].GraduationPlanCode;
                    }
                }
            }
        }

        /// <summary>
        /// 將舊課程計畫資料加入Dic
        /// </summary>
        /// <param name="oldGraduationPlanInfo"></param>
        public void AddOldGraduationPlan(OldGraduationPlanInfo oldGraduationPlanInfo)
        {
            if (!this.DicOldGraduationPlanInfos.ContainsKey(oldGraduationPlanInfo.SysID))  // 如果不包含systemCode
            {
                oldGraduationPlanInfo.NewCourseInfos = this.ListCourseInfos.ConvertAll(courseInfo => courseInfo.Clone());
                oldGraduationPlanInfo.ManagerUpdateCourseInfo();
                this.DicOldGraduationPlanInfos.Add(oldGraduationPlanInfo.SysID, oldGraduationPlanInfo); //

            }
            else // 如果已經有相同Id 代表一張課程規劃表下 有兩組以上課程規劃表識別欄位 有異常 
            {



            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, OldGraduationPlanInfo> GetAllOldGraduationPlan()
        {
            return this.DicOldGraduationPlanInfos;
        }


        /// <summary>
        /// 依據系統ID 取得課程規劃表
        /// </summary>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public OldGraduationPlanInfo GetOldGPlanBySysID(string systemID)
        {
            OldGraduationPlanInfo result = null;
            if (this.DicOldGraduationPlanInfos.ContainsKey(systemID))
            {
                return DicOldGraduationPlanInfos[systemID];
            }
            return result;
        }

        /// <summary>
        /// 如果 課程規劃表contain 特定系統編號課程規劃表 
        /// </summary>
        /// <returns></returns>
        public bool DicOldGPlansContain(string systemID)
        {
            return this.DicOldGraduationPlanInfos.ContainsKey(systemID);
        }






    }
}
