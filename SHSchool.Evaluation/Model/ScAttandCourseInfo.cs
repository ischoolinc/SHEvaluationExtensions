using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHSchool.Evaluation.Model
{
    class ScAttandCourseInfo
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="courseID"></param>
        internal ScAttandCourseInfo(string courseID)
        {
            this.ListScAttandCourseInfo = new List<ScAttendInfo>();
            this.CourseID = courseID;
        }

        public string CourseID { get; set; }
        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 科目
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ScAttendInfo> ListScAttandCourseInfo { get; set; }

        public void  AddScAttandCourseInfo(ScAttendInfo scAttandCourseInfo) 
        {
            this.ListScAttandCourseInfo.Add(scAttandCourseInfo);
        }
    }
}
