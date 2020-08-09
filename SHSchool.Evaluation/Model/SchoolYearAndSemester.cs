using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHSchool.Evaluation.Model
{
    class SchoolYearAndSemester
    {
        /// <summary>
        /// 建構子
        /// </summary>
       internal SchoolYearAndSemester(int grade ,int semester ) 
        {
            this.GradeYear = grade;
            this.Semester = semester;

        }
        /// <summary>
        /// 第幾學期
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        ///  學年度
        /// </summary>
        public int GradeYear { get; set; }

        /// <summary>
        ///  學期
        /// </summary>
        public int Semester { get; set; }

    }
}
