using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FISCA.Data;

namespace SHEvaluationExtensions.DAO
{
    public class DataAccess
    {

        /// <summary>
        /// 取得學生資料
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, StudentInfo> GetStudentInfoDictByStudentIDList(List<string> StudentIDList)
        {
            Dictionary<string, StudentInfo> value = new Dictionary<string, StudentInfo>();

            if (StudentIDList != null && StudentIDList.Count > 0)
            {
                string query = "SELECT " +
               "student.id AS student_id" +
               ",student.name AS student_name" +
               ",class_name" +
               ",seat_no" +
               ",student_number " +
               " FROM student LEFT JOIN class" +
               " ON student.ref_class_id = class.id" +
               " WHERE student.status IN(1,2) " +
               " AND student.id IN(" + string.Join(",", StudentIDList.ToArray()) + ")" +
               " ORDER BY class.grade_year,class.display_order,class.class_name,seat_no";

                QueryHelper qh = new QueryHelper();
                DataTable dt = qh.Select(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentInfo si = new StudentInfo();
                        si.StudentID = dr["student_id"].ToString();
                        si.Name = dr["student_name"].ToString();
                        si.StudentNumber = si.ClassName = si.SeatNo = "";

                        if (dr["student_number"] != null)
                            si.StudentNumber = dr["student_number"].ToString();

                        if (dr["class_name"] != null)
                            si.ClassName = dr["class_name"].ToString();

                        if (dr["seat_no"] != null)
                            si.SeatNo = dr["seat_no"].ToString();

                        if (!value.ContainsKey(si.StudentID))
                            value.Add(si.StudentID, si);
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 取得課程資料
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, CourseInfo> GetAllCourseInfoDict()
        {
            string query = "SELECT id ,course_name ,school_year ,semester FROM course";

            Dictionary<string, CourseInfo> value = new Dictionary<string, CourseInfo>();
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CourseInfo ci = new CourseInfo();
                    ci.CourseID = dr["id"].ToString();
                    ci.CourseName = dr["course_name"].ToString();
                    ci.SchoolYear = ci.Semester = "";
                    if (dr["school_year"] != null)
                        ci.SchoolYear = dr["school_year"].ToString();

                    if (dr["semester"] != null)
                        ci.Semester = dr["semester"].ToString();

                    string key = ci.SchoolYear + "_" + ci.Semester + "_" + ci.CourseName;
                    if (!value.ContainsKey(key))
                        value.Add(key, ci);
                }
            }

            return value;
        }


        public static Dictionary<string, Dictionary<string, SCAttendInfo>> GetSCAttendDictByStudentIDList(List<string> StudentIDList)
        {
            Dictionary<string, Dictionary<string, SCAttendInfo>> value = new Dictionary<string, Dictionary<string, SCAttendInfo>>();
            List<string> tmpKey = new List<string>();
            if (StudentIDList.Count > 0)
            {
                string query = "SELECT " +
                    "id AS sc_attend_id" +
                    ",ref_student_id AS student_id" +
                    ",ref_course_id AS course_id" +
                    ",is_required" +
                    ",grade_year" +
                    ",required_by" +
                    ",passing_standard" +
                    ",makeup_standard" +
                    ",designate_final_score" +
                    ",remark" +
                    ",subject_code" +
                    " FROM sc_attend" +
                    " WHERE ref_student_id IN(" + string.Join(",", StudentIDList.ToArray()) + ")";


                QueryHelper qh = new QueryHelper();
                DataTable dt = qh.Select(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SCAttendInfo si = new SCAttendInfo();
                        si.ID = dr["sc_attend_id"].ToString();
                        si.CourseID = dr["course_id"].ToString();
                        si.StudentID = dr["student_id"].ToString();
                        si.IsRequired = si.GradeYear = si.RequiredBy = "";

                        if (dr["is_required"] != null)
                            si.IsRequired = dr["is_required"].ToString();
                        if (dr["grade_year"] != null)
                            si.GradeYear = dr["grade_year"].ToString();
                        if (dr["required_by"] != null)
                            si.RequiredBy = dr["required_by"].ToString();

                        si.PassingStandard = null;
                        si.MakeupStandard = null;
                        si.DesignateFinalScore = null;
                        if (dr["passing_standard"] != null)
                        {
                            decimal pp;
                            if (decimal.TryParse(dr["passing_standard"].ToString(), out pp))
                            {
                                si.PassingStandard = pp;
                            }
                        }

                        if (dr["makeup_standard"] != null)
                        {
                            decimal mm;
                            if (decimal.TryParse(dr["makeup_standard"].ToString(), out mm))
                            {
                                si.MakeupStandard = mm;
                            }
                        }

                        if (dr["designate_final_score"] != null)
                        {
                            decimal mm;
                            if (decimal.TryParse(dr["designate_final_score"].ToString(), out mm))
                            {
                                si.DesignateFinalScore = mm;
                            }
                        }

                        si.SubjectCode = "";
                        if (dr["subject_code"] != null)
                            si.SubjectCode = dr["subject_code"].ToString();

                        si.Remark = "";
                        if (dr["remark"] != null)
                            si.Remark = dr["remark"].ToString();

                        string key = si.StudentID + "_" + si.CourseID;

                        if (!tmpKey.Contains(key))
                        {
                            if (!value.ContainsKey(si.StudentID))
                                value.Add(si.StudentID, new Dictionary<string, SCAttendInfo>());

                            if (!value[si.StudentID].ContainsKey(si.CourseID))
                                value[si.StudentID].Add(si.CourseID, si);

                            tmpKey.Add(key);
                        }
                    }
                }
            }
            return value;
        }

        public static void InsertSCAttendList(List<SCAttendInfo> dataList)
        {
            try
            {
                List<string> cmdList = new List<string>();
                foreach (SCAttendInfo si in dataList)
                {
                    string pSocre = "null", mScore = "null", dScore = "null";

                    if (si.PassingStandard.HasValue)
                        pSocre = si.PassingStandard.Value.ToString();

                    if (si.MakeupStandard.HasValue)
                        mScore = si.MakeupStandard.Value.ToString();

                    if (si.DesignateFinalScore.HasValue)
                        dScore = si.DesignateFinalScore.Value.ToString();

                    string query = "INSERT INTO sc_attend(" +
                        "ref_student_id" +
                        ",ref_course_id" +
                        ",passing_standard" +
                        ",makeup_standard" +
                        ",designate_final_score" +
                        ",remark" +
                        ",subject_code)" +
                        " VALUES(" + si.StudentID + "" +
                        "," + si.CourseID + "" +
                        "," + pSocre + "" +
                        "," + mScore + "" +
                        "," + dScore + "" +
                        ",'" + si.Remark + "'" +
                        ",'" + si.SubjectCode + "');";
                    cmdList.Add(query);
                }

                K12.Data.UpdateHelper uh = new K12.Data.UpdateHelper();
                uh.Execute(cmdList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void UpdateSCAttendList(List<SCAttendInfo> dataList)
        {
            try
            {
                List<string> cmdList = new List<string>();
                List<string> tmpList = new List<string>();
                foreach (SCAttendInfo si in dataList)
                {
                    string pSocre = "null", mScore = "null", dScore = "null";

                    if (si.PassingStandard.HasValue)
                        pSocre = si.PassingStandard.Value.ToString();

                    if (si.MakeupStandard.HasValue)
                        mScore = si.MakeupStandard.Value.ToString();

                    if (si.DesignateFinalScore.HasValue)
                        dScore = si.DesignateFinalScore.Value.ToString();

                    tmpList.Clear();

                    if (si.IsPassingStandardCheck)
                        tmpList.Add("passing_standard=" + pSocre);

                    if (si.IsMakeupStandardCheck)
                        tmpList.Add("makeup_standard = " + mScore);

                    if (si.IsDesignateFinalScoreCheck)
                        tmpList.Add("designate_final_score = " + dScore);

                    if (si.IsSubjectCodeCheck)
                        tmpList.Add("subject_code = '" + si.SubjectCode + "'");

                    if (si.IsRemarkCheck)
                        tmpList.Add("remark = '" + si.Remark + "'");

                    if (!string.IsNullOrEmpty(si.ID) && tmpList.Count > 0)
                    {
                        string query = "UPDATE sc_attend SET " + string.Join(",", tmpList.ToArray()) + "  WHERE ID = " + si.ID + ";";
                        cmdList.Add(query);
                    }
                }

                K12.Data.UpdateHelper uh = new K12.Data.UpdateHelper();
                uh.Execute(cmdList);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
