using System.Collections.Generic;
using System.Data;
using System.Linq;
using Campus.DocumentValidator;
using FISCA.Data;
using K12.Data;
using SHSchool.Data;

namespace SHSchool.Evaluation
{
    /// <summary>
    /// 課程規劃表協助類別
    /// </summary>
    public static class ProgramPlanHelper
    {
        /// <summary>
        /// 取得RowIndex，科目名稱及屬性值相同視為相同群組
        /// </summary>
        /// <param name="Subjects">課程規劃科目列表</param>
        /// <param name="SubjectName"></param>
        /// <param name="Domain"></param>
        /// <param name="Entry"></param>
        /// <param name="RequiredBy"></param>
        /// <param name="Required"></param>
        /// <param name="NotIncludeInCalc"></param>
        /// <param name="NotIncludeInCredit"></param>
        /// <returns></returns>
        public static int GetRowIndex(this List<ProgramSubject> Subjects
            ,string SubjectName
            ,string Domain
            ,string Entry
            ,string RequiredBy
            ,bool Required
            ,bool NotIncludeInCalc
            ,bool NotIncludeInCredit)
        {
            #region 科目名稱及屬性值相同視為相同群組
            ProgramSubject FindSubject = Subjects.Find(m =>
            m.SubjectName.Equals(SubjectName) &&
            m.Domain.Equals(Domain) &&
            m.Entry.Equals(Entry) &&
            m.RequiredBy.Equals(RequiredBy) &&
            m.Required.Equals(Required) &&
            m.NotIncludedInCalc.Equals(NotIncludeInCalc) &&
            m.NotIncludedInCredit.Equals(NotIncludeInCredit));
            #endregion

            int RowIndex = 1;

            if (FindSubject != null)
                RowIndex = FindSubject.RowIndex;
            else if (Subjects.Count > 0)
                RowIndex = Subjects.Max(m => m.RowIndex) + 1;

            return RowIndex;
        }

        /// <summary>
        /// 根據列取得課程規劃表
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="mProgramPlanName"></param>
        /// <returns></returns>
        public static List<SHProgramPlanRecord> SelectProgramPlanByRows(List<IRowStream> Rows,string mProgramPlanName)
        {
            List<string> ProgramPlanNames = new List<string>();

            #region 取得課程規劃表名稱
            Rows.ForEach(x=>
                {
                    string ProgramPlanName = x.GetValue(mProgramPlanName);

                    if (!ProgramPlanNames.Contains(ProgramPlanName))
                        ProgramPlanNames.Add(ProgramPlanName);
                }
            );
            #endregion

            #region 根據名稱取得課程規劃系統編號
            QueryHelper Helper = new QueryHelper();

            DataTable Table = Helper.Select("select id from graduation_plan where name in ("+ string.Join(",",ProgramPlanNames.Select(x=>"'"+x+"'").ToArray()) +")");

            List<string> ProgramPlanIDs = new List<string>();

            foreach (DataRow Row in Table.Rows)
            {
                string ProgramPlanID = Row.Field<string>("id");
                ProgramPlanIDs.Add(ProgramPlanID);
            }
            #endregion

            return SHProgramPlan.SelectByIDs(ProgramPlanIDs); //根據課程規劃系統編號取得課程規劃物件列表
        }
    }
}