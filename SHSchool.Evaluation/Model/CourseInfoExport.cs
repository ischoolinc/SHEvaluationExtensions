using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHSchool.Evaluation.Model
{

    /// <summary>
    /// 課程規劃表匯出用 Model
    /// </summary>
    class CourseInfoExport
    {
        public string ID { get; set; }
        public string 課程規劃表名稱 { get; set; }

        public string 領域名稱 { get; set; }

        public string 分項名稱 { get; set; }

        public string 年級 { get; set; }

        public string 學期 { get; set; }

        public string 科目名稱 { get; set; }

        public string 科目級別 { get; set; }

        public string 校訂部訂 { get; set; }

        public string 必選修 { get; set; }

        public string 學分數 { get; set; }
        public string 不計學分 { get; set; }

        public string 不需評分 { get; set; }

        public string 科目代碼 { get; set; }
    }
}
