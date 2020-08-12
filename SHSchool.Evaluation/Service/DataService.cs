using FISCA.Data;
using SHSchool.Evaluation.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace SHSchool.Evaluation.Service
{
    /// <summary>
    /// 資料庫更新 
    /// </summary>
    class DataService
    {
        QueryHelper _Qh = new QueryHelper();

        /// <summary>
        /// 新增 
        /// </summary>

        public void InsertGraduationPlan(string graduationPlanName, string content)
        {
            string insertString = $"INSERT INTO graduation_plan (name , content ) VALUES ('{graduationPlanName}' , '{content}')  RETURNING name ";

            DataTable insertedRows = _Qh.Select(insertString);

            if (insertedRows.Rows.Count > 0)
            {
                string graduationPlan = insertedRows.Rows[0][0].ToString();
                //_ListinSertSuccess.Add(graduationPlan);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="graduationPlanName"></param>
        /// <param name="content"></param>
        public void UpdateGraduationPlan(string id, string graduationPlanName, string content)
        {
            string updateSQL = $"UPDATE  graduation_plan   SET   content = '{content}'  WHERE id = {id}   RETURNING  id  ";

            _Qh.Select(updateSQL);
        }


        /// <summary>
        /// 取得全新課程規化表 之XML
        /// </summary>

        public string GetNewGraduationContent(GraduationPlanInfo graduationInfo)
        {
            // 2.產生各科課程規畫表資料
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlElement eleGraduationPlan = xmlDoc.CreateElement("GraduationPlan");

                Dictionary<string, int> dicSubjectLevel = new Dictionary<string, int>();
                int rows = 1;

                foreach (CourseInfo courseInfo in graduationInfo.GetAllCourseInfoList())
                {
                    eleGraduationPlan.SetAttribute("SchoolYear", courseInfo.EnterYear);
                    #region 填入XML

                    int startLevel = dicSubjectLevel.ContainsKey(courseInfo.NewSubjectName) ? dicSubjectLevel[courseInfo.NewSubjectName] + 1 : 1;

                    //放個學期的學分數
                    Dictionary<int, string> dicCreditEachSemester = courseInfo.DicCreditEachSemester;

                    int level = startLevel;
                    foreach (int semester in dicCreditEachSemester.Keys)
                    {
                        XmlElement eleSubject = xmlDoc.CreateElement("Subject");

                        #region 生成XML

                        eleSubject.SetAttribute("Category", "");
                        eleSubject.SetAttribute("Credit", dicCreditEachSemester[semester]);
                        eleSubject.SetAttribute("Domain", (courseInfo.領域名稱));
                        eleSubject.SetAttribute("Entry", courseInfo.Entry);
                        eleSubject.SetAttribute("GradeYear", Helper.GetGradeYear(semester).GradeYear.ToString());

                        if (!dicSubjectLevel.ContainsKey(courseInfo.NewSubjectName))
                        {
                            dicSubjectLevel.Add(courseInfo.NewSubjectName, level);
                            eleSubject.SetAttribute("Level", level.ToString());
                            eleSubject.SetAttribute("FullName", courseInfo.NewSubjectName + " " + Helper.GetRomaNumber(level));
                            level++;
                        }
                        else
                        {
                            dicSubjectLevel[courseInfo.NewSubjectName] = level;
                            eleSubject.SetAttribute("Level", level.ToString());

                            // todo  LEVEL 問題
                            // if (_DicForFullNameMap.ContainsKey(level))
                            {
                                courseInfo.NewSubjectNameWithLevel = courseInfo.NewSubjectName + " " + Helper.GetRomaNumber(level);
                                eleSubject.SetAttribute("FullName", courseInfo.NewSubjectNameWithLevel);
                                level++;
                            }
                        }
                        eleSubject.SetAttribute("NotIncludedInCalc", "False");
                        eleSubject.SetAttribute("NotIncludedInCredit", "False");
                        eleSubject.SetAttribute("Required", courseInfo.Required);
                        eleSubject.SetAttribute("RequiredBy", courseInfo.RequiredBy);
                        eleSubject.SetAttribute("Semester", Helper.GetGradeYear(semester).Semester.ToString());
                        eleSubject.SetAttribute("SubjectName", courseInfo.NewSubjectName);

                        eleSubject.SetAttribute("課程代碼", courseInfo.新課程代碼);
                        eleSubject.SetAttribute("課程類別", courseInfo.課程類別說明);
                        eleSubject.SetAttribute("開課方式", courseInfo.開課方式);
                        eleSubject.SetAttribute("科目屬性", courseInfo.科目屬性說明);
                        eleSubject.SetAttribute("領域名稱", courseInfo.領域名稱);
                        eleSubject.SetAttribute("課程名稱", courseInfo.NewSubjectName);
                        eleSubject.SetAttribute("學分", dicCreditEachSemester[semester]);
                        eleSubject.SetAttribute("授課學期學分", courseInfo.授課學期學分);
                        eleGraduationPlan.AppendChild(eleSubject);
                        {
                            XmlElement grouping = xmlDoc.CreateElement("Grouping");
                            grouping.SetAttribute("RowIndex", (rows).ToString());
                            grouping.SetAttribute("startLevel", startLevel.ToString());
                            eleSubject.AppendChild(grouping);
                        }
                        #endregion
                    }
                    #endregion
                    rows++;
                }
                return eleGraduationPlan.OuterXml;
            }
        }

        /// <summary>
        /// 匯出課程規劃表用
        /// </summary>
        public DataTable GetOldGraduationInfosByID(List<string> GraduationIDs)
        {
            string sql = @"
SELECT
      id
    , name
    , array_to_string(xpath('//Subject/@GradeYear', subject_ele), '')::text AS 年級
    , array_to_string(xpath('//Subject/@Semester', subject_ele), '')::text AS 學期
    , array_to_string(xpath('//Subject/@Entry', subject_ele), '')::text AS 分項
    , array_to_string(xpath('//Subject/@Domain', subject_ele), '')::text AS 領域
    , array_to_string(xpath('//Subject/@SubjectName', subject_ele), '')::text AS 科目
    , array_to_string(xpath('//Subject/@Level', subject_ele), '')::text AS 科目級別
    , array_to_string(xpath('//Subject/@Credit', subject_ele), '')::text AS 學分數
    , array_to_string(xpath('//Subject/@Required', subject_ele), '')::text AS 必選修
    , array_to_string(xpath('//Subject/@RequiredBy', subject_ele), '')::text AS 校部訂
	, array_to_string(xpath('//Subject/@NotIncludedInCalc', subject_ele), '')::text AS 不需評分
     , array_to_string(xpath('//Subject/@NotIncludedInCredit', subject_ele), '')::text  AS  不計學分
	  , array_to_string(xpath('//Subject/@課程代碼', subject_ele), '')::text  AS 課程代碼
FROM
    (
        SELECT 
            id
            , name
            , unnest(xpath('//GraduationPlan/Subject', xmlparse(content content))) as subject_ele
        FROM 
            graduation_plan
         WHERE id IN ({0})
    ) AS graduation_plan

";
            sql = string.Format(sql, string.Join(",", GraduationIDs));
            DataTable dt = this._Qh.Select(sql);
            return dt;
        }

        /// <summary>
        /// 取得須更新之xml
        /// </summary>
        /// <returns></returns>
        public DataTable GetScAttendUpdateTarget(List<string> subjectCodes)
        {
            #region SQL
            string sql = @"
WITH target AS 
(
	SELECT
		* 
	FROM 
		sc_attend 
	WHERE
	subject_code  IN  ('{0}')
)SELECT 
		target.id AS sc_attend_id 
		,ref_course_id 
		, course_name
		, subject
		, school_year
		, semester 
		, ref_student_id
		, student.student_number
		, class.class_name AS student_class
		, student.seat_no
		, student.name 
		, target.subject_code
	FROM 
		target
	LEFT JOIN 	course 
		ON course.id  =target.ref_course_id 
	LEFT JOIN student 
		ON  student.id =target.ref_student_id 
	LEFT JOIN  class
		ON class.id =student.ref_class_id
ORDER BY
	class.display_order
	,seat_no 

            ";
            #endregion

            sql = string.Format(sql, string.Join("','", subjectCodes));
            DataTable dt = this._Qh.Select(sql);
            return dt;
        }
    }
}
