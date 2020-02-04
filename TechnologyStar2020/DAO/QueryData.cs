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
        public List<DeptInfo> GetDeptInfoList()
        {
            List<DeptInfo> value = new List<DeptInfo>();

            string qry = "select id,name,code from dept order by code,name;";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(qry);

            if (dt != null)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    DeptInfo di = new DeptInfo();
                    di.DeptID = dr["id"].ToString();
                    di.DeptName = dr["name"].ToString();
                    value.Add(di);
                }
            }
            return value;
        }
    }
}
