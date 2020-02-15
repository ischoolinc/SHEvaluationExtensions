using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Data;
using System.Data;

namespace TechnologyStar2020.DAO
{
    public class QueryData
    {
        /// <summary>
        /// 取得學生基本資料
        /// </summary>
        /// <returns></returns>
        public List<StudentInfo> GetStudentList()
        {
            List<StudentInfo> value = new List<StudentInfo>();

            try
            {
                // 取得三年級一般狀態學生
                string qry = @"
SELECT 
student.id AS student_id
,student_number
,class_name
,student.name AS student_name
,seat_no
, COALESCE(dept.name, '') AS dept_name 
FROM student 
LEFT OUTER JOIN class ON class.id = student.ref_class_id 
LEFT OUTER JOIN dept ON dept.id = COALESCE(student.ref_dept_id, class.ref_dept_id) 
WHERE student.status = 1 AND class.grade_year = 3 
ORDER BY class.display_order,class_name,seat_no";

                QueryHelper qh = new QueryHelper();
                DataTable dt = qh.Select(qry);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentInfo si = new StudentInfo();
                        si.StudentID = dr["student_id"].ToString();
                        si.StudentNumber = dr["student_number"].ToString();
                        si.ClassName = dr["class_name"].ToString();
                        si.Name = dr["student_name"].ToString();
                        si.SeatNo = dr["seat_no"].ToString();
                        si.DeptName = dr["dept_name"].ToString();
                        value.Add(si);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return value;
        }

        public Dictionary<string, Dictionary<string, StudentScoreInfo>> GetStudentScoreDict()
        {
            Dictionary<string, Dictionary<string, StudentScoreInfo>> value = new Dictionary<string, Dictionary<string, StudentScoreInfo>>();
            try
            {
                string qry = @"
SELECT
ref_student_id AS student_id
,item_type
,item_name
,rank_type
,rank_name
,matrix_count
,score
,rank
,pr
,percentile
FROM rank_matrix 
INNER JOIN rank_detail 
ON rank_matrix.id = rank_detail.ref_matrix_id
WHERE rank_matrix.item_type = '5學期/技職繁星比序' 
AND rank_type IN('學群排名','科排名') 
AND rank_matrix.is_alive = true 
";
                QueryHelper qh = new QueryHelper();
                DataTable dt = qh.Select(qry);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sid = dr["student_id"].ToString();
                        // item_name:國文,rank_type:學群排名,rank_name:外語群
                        string key = dr["item_name"].ToString() + "_" + dr["rank_type"].ToString();

                        if (!value.ContainsKey(sid))
                            value.Add(sid, new Dictionary<string, StudentScoreInfo>());                   

                        StudentScoreInfo ssi = new StudentScoreInfo();
                        ssi.StudentID = sid;
                        ssi.ItemType = dr["item_type"].ToString();
                        ssi.ItemName = dr["item_name"].ToString();
                        ssi.RankType = dr["rank_type"].ToString();
                        ssi.RankName = dr["rank_name"].ToString();
                        int m, r, pr, p;
                        decimal s;
                        if (int.TryParse(dr["matrix_count"].ToString(),out m))
                        {
                            ssi.MatrixCount = m;
                        }

                        if (int.TryParse(dr["rank"].ToString(), out r))
                        {
                            ssi.Rank = r;
                        }

                        if (int.TryParse(dr["pr"].ToString(), out pr))
                        {
                            ssi.PR = pr;
                        }

                        if (int.TryParse(dr["percentile"].ToString(), out p))
                        {
                            ssi.Percentile = p;
                        }

                        if(decimal.TryParse(dr["score"].ToString(),out s))
                        {
                            ssi.Score = s;
                        }

                        if (!value[sid].ContainsKey(key))
                            value[sid].Add(key, ssi);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

    }
}
