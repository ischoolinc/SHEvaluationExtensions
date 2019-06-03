using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHSchool.Evaluation.Model
{
    class MappingInfo
    {
        public string Code { get; set; }  //代碼

        public string Name { get; set; }  //代碼說明

        public string CourseTypeApplicable { get; set; } //適用課程代碼

        public Dictionary<string, string> DicForDuplicate { get; set; } // 如果資料重複就將資料裝在這裡 key 為 課程類型  value 為 適用課程類型

        public Boolean IsCodeUniqe { get; set; }  //資料來源 code不是唯一值 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">代碼</param>
        /// <param name="name">代碼說明</param>
        /// <param name="courseTypeApplicable">適用課程代碼:因代碼有重複問題 需再比對</param>
        public MappingInfo(string code, string name, string courseTypeApplicable="")
        {
            this.Code = code;
            this.Name = name;
            this.CourseTypeApplicable = courseTypeApplicable;
            this.IsCodeUniqe = true;   //預設為code 為唯一值 >>當 發現不是唯一值 時 變為false  並且新增 DicForDuplicate 對應
            this.GetCourseTypeApplicable(this.CourseTypeApplicable,this.Name);
        }


        //加入 課程類型對應 (因代碼重複)
        public void GetCourseTypeApplicable(string courseTypeApplicable,string name )
        {
            //如果沒有 適用課程類型 不用執行
            if (CourseTypeApplicable == "")
            {
                return;
            }
            else
            {
                this.CourseTypeApplicable = courseTypeApplicable;
                this.Name = name;
            }



            string[] courseTypes = CourseTypeApplicable.Substring(1, CourseTypeApplicable.Length-2).Split('|'); //將字串切成 Array

                // 放入 Dictionary
                foreach (string coursetype in courseTypes)
                {
                    if (this.DicForDuplicate == null) //還沒有建 存放 課程類型 對應 代碼說明
                    {
                        this.DicForDuplicate = new Dictionary<string, string>();
                        this.DicForDuplicate.Add(coursetype, this.Name);
                    }
                    else
                    {
                    if (!DicForDuplicate.ContainsKey(coursetype))
                    {
                        this.DicForDuplicate.Add(coursetype, this.Name);
                    }
                    else
                    {
                        if (DicForDuplicate[coursetype] == this.Name)
                        {
                            continue;
                        }
                        else //相同代碼  &  代碼說明 不同    適用課程類型 
                        {
                            DicForDuplicate[coursetype] += $" | { this.Name}";
                           
                        }
                    }
                    }
                }
        }
    }
}
