using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace TechnologyStar2020.DAO
{
    [TableName("campus.technology_star.registration_dept")]
    public class udtRegistrationDept : ActiveRecord
    {
        /// <summary>
        /// 科別名稱
        /// </summary>
        [Field(Field = "dept_name", Indexed = false)]
        public string DeptName { get; set; }

        /// <summary>
        /// 報名科代碼
        /// </summary>
        [Field(Field = "reg_dept_id", Indexed = false)]
        public string RegDeptID { get; set; }

        /// <summary>
        /// 報名科名稱
        /// </summary>
        [Field(Field = "reg_dept_name", Indexed = false)]
        public string RegDeptName { get; set; }

        /// <summary>
        /// 報名群代碼
        /// </summary>
        [Field(Field = "ref_reg_group_id", Indexed = false)]
        public string RefRegGroupID { get; set; }
    }
}
