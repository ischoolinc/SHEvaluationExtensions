using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA;
using FISCA.Presentation;
using FISCA.Permission;
namespace SHEvaluationExtensions
{
    public class Program
    {
        [MainMethod()]
        public static void Main()
        {
            // 匯出課程修課學生
            RibbonBarItem rbItemCourseImportExport = K12.Presentation.NLDPanels.Course.RibbonBarItems["資料統計"];
            rbItemCourseImportExport["匯出"]["匯出課程修課學生"].Enable =UserAcl.Current["SHSchool.Course.ExportCourseStudent"].Executable;
            rbItemCourseImportExport["匯出"]["匯出課程修課學生"].Click += delegate
            {
                SmartSchool.API.PlugIn.Export.Exporter exporter = new SHEvaluationExtensions.Course.ExportCourseStudents("");
                SHEvaluationExtensions.Course.ExportStudentV2 wizard = new SHEvaluationExtensions.Course.ExportStudentV2(exporter.Text, exporter.Image);
                exporter.InitializeExport(wizard);
                wizard.ShowDialog();
            };

            // 匯入課程修課學生
            rbItemCourseImportExport["匯入"]["匯入課程修課學生"].Enable = UserAcl.Current["SHSchool.Course.ImportCourseStudent"].Executable;
            rbItemCourseImportExport["匯入"]["匯入課程修課學生"].Click += delegate
            {
                SmartSchool.API.PlugIn.Import.Importer importer = new SHEvaluationExtensions.Course.ImportCourseStudents("");
                SHEvaluationExtensions.Course.ImportStudentV2 wizard = new SHEvaluationExtensions.Course.ImportStudentV2(importer.Text, importer.Image);
                importer.InitializeImport(wizard);
                wizard.ShowDialog();
            };

            // 註冊
            // 匯出匯入課程修課學生
            Catalog catalog1 = RoleAclSource.Instance["課程"]["功能按鈕"];
            catalog1.Add(new RibbonFeature("SHSchool.Course.ExportCourseStudent", "匯出課程修課學生"));
            catalog1.Add(new RibbonFeature("SHSchool.Course.ImportCourseStudent", "匯入課程修課學生"));
        }
    }
}
