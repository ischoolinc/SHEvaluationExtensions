using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHEvaluationExtensions.DAO
{
    /// <summary>
    /// 學生修課
    /// </summary>
    public class SCAttendInfo
    {
        /// <summary>
        /// 修課ID
        /// </summary>
        public string ID { get; set; }
        public string StudentID { get; set; }
        public string CourseID { get; set; }
        /// <summary>
        /// 必選修
        /// </summary>
        public string IsRequired { get; set; }
        public string GradeYear { get; set; }

        /// <summary>
        /// 校部訂
        /// </summary>
        public string RequiredBy { get; set; }
        /// <summary>
        /// 及格標準
        /// </summary>
        public decimal? PassingStandard { get; set; }
        /// <summary>
        /// 補考標準
        /// </summary>
        public decimal? MakeupStandard { get; set; }
        /// <summary>
        /// 直接指定總成績
        /// </summary>
        public decimal? DesignateFinalScore { get; set; }

        /// <summary>
        /// 科目代碼
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        public bool IsPassingStandardCheck = false;
        public bool IsMakeupStandardCheck = false;
        public bool IsSubjectCodeCheck = false;
        public bool IsDesignateFinalScoreCheck = false;
        public bool IsRemarkCheck = false;
    }
}
