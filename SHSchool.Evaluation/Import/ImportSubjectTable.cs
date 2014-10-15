using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Import;
using Campus.DocumentValidator;

namespace SHSchool.Evaluation
{
    /// <summary>
    /// 匯入學程科目表
    /// </summary>
    public class ImportSubjectTable : ImportWizard
    {
        /// <summary>
        /// 支援匯入的動作，新增或更新、刪除
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate | ImportAction.Delete;
        }

        /// <summary>
        /// 驗證規則描述檔
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return Properties.Resources.SubjectTable;
            //return "http://sites.google.com/a/kunhsiang.com/k12evaluation/validationrule/SubjectTable.xml";
        }

        /// <summary>
        /// 準備匯入
        /// </summary>
        /// <param name="Option"></param>
        public override void Prepare(ImportOption Option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 實際分批匯入
        /// </summary>
        /// <param name="Rows"></param>
        /// <returns></returns>
        public override string Import(List<IRowStream> Rows)
        {
            throw new NotImplementedException();
        }
    }
}