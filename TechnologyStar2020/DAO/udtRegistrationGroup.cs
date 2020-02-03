using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace TechnologyStar2020.DAO
{
    /// <summary>
    /// 報名群
    /// </summary>
    [TableName("campus.technology_star.registration_group")]
    public class udtRegistrationGroup : ActiveRecord
    {
        /// <summary>
        /// 群代碼
        /// </summary>
        [Field(Field = "group_id", Indexed = false)]
        public string GroupID { get; set; }

        /// <summary>
        /// 群名稱
        /// </summary>
        [Field(Field = "group_name", Indexed = false)]
        public string GroupName { get; set; }
    }
}
