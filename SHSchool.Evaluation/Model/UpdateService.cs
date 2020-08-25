using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHSchool.Evaluation.Model
{

    //執行匯入更新使用
    class UpdateService
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public UpdateService() 
        {
            this.AllNewDictionary = new List<GraduationPlanInfo>();
            this.UpdateCourseInfo = new List<UpdateCourseInfo>();
            this.DeleteCourseInfo = new List<UpdateCourseInfo>();
            this.InsertCourseInfo = new List<UpdateCourseInfo>();
        }


        /// <summary>
        /// 要新增的Dictionary   key= id
        /// </summary>
         List<UpdateCourseInfo> UpdateCourseInfo;
        /// <summary>
        /// 
        /// </summary>
         List<UpdateCourseInfo> DeleteCourseInfo;
        /// <summary>
        /// 新增課程
        /// </summary>
        /// <returns></returns>
        List<UpdateCourseInfo> InsertCourseInfo;
        /// <summary>
        /// 整份課程規劃表更新
        /// </summary>
        List<GraduationPlanInfo> AllNewDictionary;





   

    }
}
