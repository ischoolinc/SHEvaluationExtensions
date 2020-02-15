using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnologyStar2020.DAO
{
    public class StudentScoreInfo
    {
        public string StudentID { get; set; }

        public string ItemType { get; set; }

        public string ItemName { get; set; }

        public string RankType { get; set; }

        public string RankName { get; set; }

        public int? MatrixCount  { get; set; }

        public decimal? Score  { get; set; }

        public int? Rank  { get; set; }

        public int? PR  { get; set; }

        public int? Percentile  { get; set; }
    }
}
