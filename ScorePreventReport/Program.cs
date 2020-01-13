using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA;
using FISCA.Permission;
using FISCA.Presentation;
using FISCA.Presentation.Controls;
using K12.Presentation;

namespace ScorePreventReport
{
    public class Program
    {
        [MainMethod("ScorePreventReport")]
        public static void Main()
        {
            #region 班級
            {
                string code = "Class-Score-Prevent-Report";
                RoleAclSource.Instance["班級"]["報表"].Add(new RibbonFeature(code, "班級成績預警通知單"));
                MotherForm.RibbonBarItems["班級", "資料統計"]["報表"]["成績相關報表"]["成績預警通知單"].Enable = UserAcl.Current[code].Executable;
                MotherForm.RibbonBarItems["班級", "資料統計"]["報表"]["成績相關報表"]["成績預警通知單"].Click += delegate
                {
                    if (NLDPanels.Class.SelectedSource.Count > 0)
                    {
                        (new ExportClassScorePreventReport(NLDPanels.Class.SelectedSource)).Export();
                    }
                    else
                    {
                        MsgBox.Show("請選擇要列印成績單的班級");
                    }
                };
            }
            #endregion
        }
    }
}
