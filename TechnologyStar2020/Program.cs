﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using FISCA.Permission;
using FISCA.Presentation;

namespace TechnologyStar2020
{
    public class Program
    {
        [FISCA.MainMethod(FISCA.StartupPriority.LastAsynchronized)]
        public static void Main()
        {
            string guid = "99075CC9-91BD-48B7-94FE-C57CF299C93E";

            // 透過安裝這程式讓主要計算功能可以使用
            MotherForm.RibbonBarItems["教務作業", "批次作業/檢視"]["成績排名"]["技職繁星成績排名計算"].Visible = true;
            MotherForm.RibbonBarItems["教務作業", "批次作業/檢視"]["成績排名"]["技職繁星成績排名資料檢索"].Visible = true;

            RibbonBarItem rbItem1 = MotherForm.RibbonBarItems["教務作業", "資料統計"];
            rbItem1["報表"]["技職繁星報名資料"].Enable = UserAcl.Current[guid].Executable;
            rbItem1["報表"]["技職繁星報名資料"].Click += delegate
            {
                UI.PrintForm pr = new UI.PrintForm();
                pr.ShowDialog();
            };

            // 技職繁星
            Catalog catalog1a = RoleAclSource.Instance["教務作業"]["功能按鈕"];
            catalog1a.Add(new RibbonFeature(guid, "技職繁星報名資料"));
        }
    }
}
