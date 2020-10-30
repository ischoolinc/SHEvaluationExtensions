using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHSchool.Evaluation.Model
{
    /// <summary>
    /// 更新修課紀錄用
    /// </summary>
    class ScAttendInfo
    {
      public  ScAttendInfo(string scAttendID)
        {
            this.ScAttendID = scAttendID;
        }
        public string ScAttendID { get; set; }
        public string RefCourseID { get; set; }
        public string CourseName { get; set; }
        public string Subject { get; set; }
        public string RefStudentID { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public string StudentName { get; set; }
        public string StudentNumber { get; set; }
        public string StudentClass { get; set; }
        public string SeatNo { get; set; }
        /// <summary>
        /// 舊課程代碼
        /// </summary>
        public string OldSubjectCode { get; set; }
        /// <summary>
        /// 新課程代碼        
        /// </summary>
        public string NewSubjectCode { get; set; }
    }
}
