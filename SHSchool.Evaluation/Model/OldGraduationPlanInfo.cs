using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SHSchool.Evaluation.Model
{

    /// <summary>
    /// 系統原有 GraduationPlanInfo
    /// </summary>
    public class OldGraduationPlanInfo
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="id"> 系統ID </param>
        /// <param name="graduationName"></param>
        /// <param name="content"> XML</param>
        public OldGraduationPlanInfo(string id, string graduationName, string content)
        {
            this.NewCourseInfos = new List<CourseInfo>();
            this.SysID = id;
            this.GraduationName = graduationName;
            this.Content = content;
            this.ContverContentToXml();// 轉換成xmlElement格式 ;
            this.GraduationPlanKeys = new List<string>();
            this.OldCourseInfos = new List<UpdateCourseInfo>();
            this.UpdateCourseInfos = new List<UpdateCourseInfo>();
            this.InsertCourseInfos = new List<UpdateCourseInfo>();
            this.DeleteCourseInfos = new List<UpdateCourseInfo>();
        }


        public string SysID { get; set; }


        public string GraduationName { get; set; }

        /// <summary>
        /// 排除 綜合高中<M> 之科別:196()外 系統內可成1-16應該 與 GraduationPlanCode 應該成 1 對 1 有兩筆以上 GraduationPlanCode
        /// </summary>
        public List<string> GraduationPlanKeys { get; set; }

        /// <summary>
        /// GraduationPlankey  如果有多個以第一個為準
        /// </summary>
        public string GraduationPlanKey { get; set; }
        /// <summary>
        ///  解析出來有兩個以上課程規劃表key值
        /// </summary>
        public bool HasOverOneKey { get; set; }
        /// <summary>
        /// xml 內容
        /// </summary>
        public string Content { get; set; }

        public List<UpdateCourseInfo> OldCourseInfos { get; set; }

        /// <summary>
        /// 讀進來的CSV
        /// </summary>
        public List<CourseInfo> NewCourseInfos { get; set; }
        /// <summary>
        ///  Xml讀進來
        /// </summary>
        public System.Xml.XmlElement OldContentXml { get; set; }
        /// <summary>
        /// Update用XML
        /// </summary>
        public System.Xml.XmlElement UpdateContentXml { get; set; }
        /// <summary>
        /// 要更新之項目    
        /// </summary>
        public List<UpdateCourseInfo> UpdateCourseInfos { get; set; }
        /// <summary>
        /// 更新之項
        /// </summary>
        public List<UpdateCourseInfo> InsertCourseInfos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<UpdateCourseInfo> DeleteCourseInfos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void SetNewCourseInfo(List<CourseInfo> courseInfo)
        {
            this.NewCourseInfos.AddRange(courseInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        public UpdateCourseInfo GetUpdateCourseInfos(string oldSubjCode)
        {
            return this.UpdateCourseInfos.Where(x => x.OldSubjectCode == oldSubjCode).First();
        }

        /// <summary>
        /// 把資料整理加入
        /// </summary>
        /// <param name="courseInfo"></param>
        public void AddUpdateInfo(CourseInfo courseInfo)
        {
            #region 檢查授課學期學分

            if (courseInfo.Action == EnumAction.刪除)
            {
                UpdateCourseInfo oldCourseInfo = new UpdateCourseInfo(this.SysID, courseInfo);
                this.DeleteCourseInfos.Add(oldCourseInfo);
            }
            else
            {

                string CourseCodeFromMOE = courseInfo.OrginCourseCodeFromMOE != "" ? courseInfo.OrginCourseCodeFromMOE : courseInfo.新課程代碼;
                XmlElement ele = (XmlElement)this.OldContentXml.SelectSingleNode($"/GraduationPlan/Subject[@課程代碼='{CourseCodeFromMOE}']");
                if (ele != null)
                {
                    if (((ele.SelectSingleNode("@授課學期學分")) == null ? "" : (ele.SelectSingleNode("@授課學期學分")).Value.ToString()) != courseInfo.授課學期學分)
                    {
                        try
                        {
                            courseInfo.Action = EnumAction.修改;
                            UpdateCourseInfo oldCourseInfo = new UpdateCourseInfo(this.SysID, courseInfo);
                            oldCourseInfo.CollectIfDifferent("授課學期學分代碼");
                            oldCourseInfo.GenerateUpdateTarget(this.GetOneXmlElementBySubjectCode(oldCourseInfo.OldSubjectCode), courseInfo); // 取得更新之欄位
                            this.UpdateCourseInfos.Add(oldCourseInfo);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                #endregion
                if (courseInfo.Action == EnumAction.修改)
                {
                    try
                    {
                        UpdateCourseInfo oldCourseInfo;
                        if (this.UpdateCourseInfos.Any(x => x.OldSubjectCode == courseInfo.OrginCourseCodeFromMOE))
                        {
                            oldCourseInfo = GetUpdateCourseInfos(courseInfo.OrginCourseCodeFromMOE);
                        }
                        else
                        {
                            oldCourseInfo = new UpdateCourseInfo(this.SysID, courseInfo);
                            oldCourseInfo.GenerateUpdateTarget(this.GetOneXmlElementBySubjectCode(oldCourseInfo.OldSubjectCode), courseInfo); // 取得更新之欄位
                            this.UpdateCourseInfos.Add(oldCourseInfo);
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (courseInfo.Action == EnumAction.新增)
                {
                    if (this.IsContainSubjectCode(courseInfo.新課程代碼)) // 如果舊有課程規劃表 已經有本課程 => Action 調整為 "未調整"
                    {
                        courseInfo.Action = EnumAction.未調整;
                    }
                    else // 不包含課程規劃表
                    {
                        UpdateCourseInfo oldCourseInfo = new UpdateCourseInfo(this.SysID, courseInfo);
                        this.InsertCourseInfos.Add(oldCourseInfo);
                    }
                }




            }


        }

        /// <summary>
        /// 將ContentStringToXml
        /// </summary>
        internal void ContverContentToXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.Content);
            this.OldContentXml = (XmlElement)(doc.SelectSingleNode("GraduationPlan"));
        }


        /// <summary>
        /// 如果一個課程規劃表內解析出來有兩個以上值提想
        /// </summary>
        /// <param name="gPlanKey"></param>
        public void AddGPlanKey(string gPlanKey)
        {
            this.GraduationPlanKeys.Add(gPlanKey);
        }


        /// <summary>
        ///  XML 是否存在課程代碼
        /// </summary>
        /// <param name="SubjectCode"></param>
        /// <returns></returns>
        public bool IsContainSubjectCode(string SubjectCode)
        {
            //todo 
            return this.OldContentXml.SelectNodes($"/GraduationPlan/Subject[@課程代碼='{SubjectCode}']").Count > 0;
        }


        /// <summary>
        /// 找到第一個 符合的 xml
        /// </summary>
        /// <param name="SubjectCode"></param>
        /// <returns></returns>
        public XmlElement GetOneXmlElementBySubjectCode(string SubjectCode)
        {
            // Helper.GetMaxRow(this.OldContentXml);
            return (XmlElement)this.OldContentXml.SelectSingleNode($"/GraduationPlan/Subject[@課程代碼='{SubjectCode}']");
        }


        /// <summary>
        /// 找到第一個 符合的 xml=>找到多筆
        /// </summary>
        /// <param name="SubjectCode"></param>
        /// <returns></returns>
        public XmlNodeList GetOneXmlElementBySubjectCodes(XmlElement xmlElement, string SubjectCode)
        {
            return xmlElement.SelectNodes($"//Subject[@課程代碼='{SubjectCode}']");
        }

        // todo update

        /// <summary>
        /// 把資料整理成 易顯示且易更新之資料結構 
        /// </summary>
        internal void ManagerUpdateCourseInfo()
        {
            foreach (CourseInfo CourseInfo in this.NewCourseInfos)
            {
                this.AddUpdateInfo(CourseInfo);

                // 確認授課學期學分是否一致

            }
        }

        /// <summary>
        /// 將舊資料轉成要update的XML
        /// </summary>
        internal void MakeUpdateXml()
        {
            this.UpdateContentXml = (XmlElement)OldContentXml.Clone();
            // 修改
            foreach (UpdateCourseInfo courseInfo in UpdateCourseInfos)
            {
                // 取得要update的標的
                foreach (XmlElement element in this.GetOneXmlElementBySubjectCodes(this.UpdateContentXml, courseInfo.OldSubjectCode))
                {
                    this.SetAttributeByUpdate(element, "Domain", courseInfo.NewCourseInfo.領域名稱);

                    if (element.HasAttribute("FullName"))
                    {
                        string oldFullName = element.GetAttribute("FullName");
                        string newFullName = oldFullName.Replace(courseInfo.OldSujectName, courseInfo.NewCourseInfo.NewSubjectName);
                        this.SetAttributeByUpdate(element, "FullName", newFullName);
                    }
                    this.SetAttributeByUpdate(element, "Entry", courseInfo.NewCourseInfo.Entry);
                    this.SetAttributeByUpdate(element, "Required", courseInfo.NewCourseInfo.Required);
                    this.SetAttributeByUpdate(element, "RequiredBy", courseInfo.NewCourseInfo.RequiredBy);
                    this.SetAttributeByUpdate(element, "SubjectName", courseInfo.NewCourseInfo.NewSubjectName);
                    this.SetAttributeByUpdate(element, "課程代碼", courseInfo.NewCourseInfo.新課程代碼);
                    this.SetAttributeByUpdate(element, "課程類別", courseInfo.NewCourseInfo.課程類別說明);
                    this.SetAttributeByUpdate(element, "開課方式", courseInfo.NewCourseInfo.開課方式);
                    this.SetAttributeByUpdate(element, "科目屬性", courseInfo.NewCourseInfo.科目屬性說明);
                    this.SetAttributeByUpdate(element, "領域名稱", courseInfo.NewCourseInfo.領域名稱);
                    this.SetAttributeByUpdate(element, "課程名稱", courseInfo.NewCourseInfo.NewSubjectName);
                    this.SetAttributeByUpdate(element, "授課學期學分", courseInfo.NewCourseInfo.授課學期學分);
                }
            }

            // 刪除
            foreach (var courseInfo in DeleteCourseInfos)
            {
                // 取得標的
                foreach (XmlElement element in this.GetOneXmlElementBySubjectCodes(this.UpdateContentXml, courseInfo.NewSubjectCode))
                {
                    this.SetAttributeByUpdate(element, "課程代碼", "");
                }
            }
            // 新增 Append
            AddXmlElement(this.UpdateContentXml, InsertCourseInfos);
        }




        /// <summary>
        /// 複製
        /// </summary>
        /// <param name="subjectElement"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public System.Xml.XmlElement SetAttributeByUpdate(System.Xml.XmlElement subjectElement, string attributeName, string newAttrubuteValue, string oldAttributeValue = null)
        {

            subjectElement.SetAttribute(attributeName, newAttrubuteValue);

            return subjectElement;
        }

        /// <summary>
        /// 原有課程規劃表 加入科目  <subject></subject>
        /// </summary>
        /// <param name="orginXmlContent"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public System.Xml.XmlElement AddXmlElement(System.Xml.XmlElement orginXmlContent, List<UpdateCourseInfo> InsertCourseInfo)
        {
            XmlDocument doc = orginXmlContent.OwnerDocument;
            foreach (UpdateCourseInfo insertCourseInfo in InsertCourseInfo)
            {
                Dictionary<int, string> CreditSemesters = insertCourseInfo.NewCourseInfo.DicCreditEachSemester;
                string rowIndex = Helper.GetMaxRow(orginXmlContent).ToString();
                int level = 1;
                int startLevel = 0;
                foreach (int semester in CreditSemesters.Keys)
                {
                    XmlElement subjectNode = doc.CreateElement("Subject");
                    orginXmlContent.AppendChild(subjectNode);
                    subjectNode.SetAttribute("Category", "");
                    subjectNode.SetAttribute("Credit", CreditSemesters[semester]);
                    subjectNode.SetAttribute("Domain", insertCourseInfo.NewCourseInfo.領域名稱);
                    subjectNode.SetAttribute("Entry", Helper.GetEntryByCSVCodeDetail(insertCourseInfo.NewCourseInfo.科目屬性說明));
                    subjectNode.SetAttribute("GradeYear", Helper.GetGradeYear(semester).GradeYear.ToString());

                    // 處理一下級別
                    if (Helper.GetSubjectCount(orginXmlContent, insertCourseInfo.NewCourseInfo.NewSubjectName) > 0)
                    {

                        if (insertCourseInfo.NewCourseInfo.NewSubjectName.Contains("國語文"))
                        {
                        
                        }
                        level = Helper.GetSubjectCount(orginXmlContent, insertCourseInfo.NewCourseInfo.NewSubjectName) ;
                        if (startLevel == 0)
                        {
                            insertCourseInfo.NewCourseInfo.StartLevel = Helper.GetSubjectCount(orginXmlContent, insertCourseInfo.NewCourseInfo.NewSubjectName);
                            startLevel = insertCourseInfo.NewCourseInfo.StartLevel;
                        }

                    }
                    subjectNode.SetAttribute("Level", level.ToString());
                    subjectNode.SetAttribute("FullName", insertCourseInfo.NewCourseInfo.NewSubjectName + " " + Helper.GetRomaNumber(level));
                    subjectNode.SetAttribute("NotIncludedInCalc", "False"); 
                    subjectNode.SetAttribute("NotIncludedInCredit", "False");
                    subjectNode.SetAttribute("Required", insertCourseInfo.NewCourseInfo.Required);
                    subjectNode.SetAttribute("RequiredBy", insertCourseInfo.NewCourseInfo.RequiredBy);
                    subjectNode.SetAttribute("Semester", Helper.GetGradeYear(semester).Semester.ToString());
                    subjectNode.SetAttribute("SubjectName", insertCourseInfo.NewCourseInfo.NewSubjectName);
                    subjectNode.SetAttribute("課程代碼", insertCourseInfo.NewCourseInfo.新課程代碼);
                    subjectNode.SetAttribute("課程類別", insertCourseInfo.NewCourseInfo.課程類別說明);
                    subjectNode.SetAttribute("開課方式", insertCourseInfo.NewCourseInfo.開課方式);
                    subjectNode.SetAttribute("科目屬性", insertCourseInfo.NewCourseInfo.科目屬性說明);
                    subjectNode.SetAttribute("開課方式", insertCourseInfo.NewCourseInfo.開課方式);
                    subjectNode.SetAttribute("領域名稱", insertCourseInfo.NewCourseInfo.領域名稱);
                    subjectNode.SetAttribute("授課學期學分", insertCourseInfo.NewCourseInfo.授課學期學分);
                    // 建立Grouping XmlElemet
                    XmlElement groupNode = doc.CreateElement("Grouping");
                    subjectNode.AppendChild(groupNode);
                    groupNode.SetAttribute("RowIndex", rowIndex);

                    groupNode.SetAttribute("startLevel", insertCourseInfo.NewCourseInfo.StartLevel.ToString());

                    level++;
                }
            }
            return orginXmlContent;
        }
    }
}
