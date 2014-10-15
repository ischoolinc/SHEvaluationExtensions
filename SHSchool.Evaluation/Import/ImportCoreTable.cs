using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Import;

namespace SHSchool.Evaluation
{
    /// <summary>
    /// 匯入自訂畢業應修及格科目表
    /// </summary>
    public class ImportCoreTable : ImportWizard
    {
        /// <summary>
        /// 支援匯入的動作，依『科目表名稱、科目名稱』新增或更新、刪除
        /// </summary>
        /// <returns></returns>
        public override ImportAction GetSupportActions()
        {
            return ImportAction.InsertOrUpdate | ImportAction.Delete;
        }

        /// <summary>
        /// 取得驗證規則描述檔
        /// </summary>
        /// <returns></returns>
        public override string GetValidateRule()
        {
            return Properties.Resources.CoreTable;
            //return "http://sites.google.com/a/kunhsiang.com/k12evaluation/validationrule/CoreTable.xml";        
        }

        /// <summary>
        /// 匯入前準備
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
        public override string Import(List<Campus.DocumentValidator.IRowStream> Rows)
        {
            throw new NotImplementedException();
        }
    }
}