using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using TechnologyStar2020.DAO;

namespace TechnologyStar2020.UI
{
    public partial class GroupForm : BaseForm
    {
        AccessHelper accessHelper = new AccessHelper();
        Dictionary<string, string> defaultGroupDict = new Dictionary<string, string>();
        Dictionary<string, udtRegistrationGroup> sourceDict = new Dictionary<string, udtRegistrationGroup>();
        public GroupForm()
        {
            InitializeComponent();
            //
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = false;
            if (MsgBox.Show("當按「是」將清空群資料恢復成系統預設值，請問是否繼續?", "恢復系統預設值", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                this.LoadDefaultData();
                this.LoadData();
            }

            btnReset.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            if (CheckData())
                SaveData();

            btnSave.Enabled = true;
        }

        private bool CheckData()
        {
            bool value = true;
            List<string> idNameList = new List<string>();
            List<string> idList = new List<string>();
            List<string> NameList = new List<string>();
            // 檢查不能空白與不能重複
            foreach (DataGridViewRow dr in dgData.Rows)
            {
                if (dr.IsNewRow)
                    continue;
               
                string id = "";
                string name = "";
                if (dr.Cells[colID.Index].Value != null && dr.Cells[colID.Index].Value.ToString() != "")
                {
                    id = dr.Cells[colID.Index].Value.ToString();
                }

                if (dr.Cells[colName.Index].Value != null && dr.Cells[colName.Index].Value.ToString() != "")
                {
                    name = dr.Cells[colName.Index].Value.ToString();
                }

                if (id == "" || name == "")
                {
                    MsgBox.Show("第" + dr.Index + 1 + "列 有空白資料無法儲存。");

                    return false;
                }


                if (!idList.Contains(id))
                    idList.Add(id);
                else
                {
                    MsgBox.Show("第" + dr.Index + 1 + "列 " + id + name + "群代碼重複無法儲存。");

                    return false;
                }

                if (!NameList.Contains(name))
                    NameList.Add(name);
                else
                {
                    MsgBox.Show("第" + dr.Index + 1 + "列 " + id + name + "群名稱重複無法儲存。");

                    return false;
                }

                if (!idNameList.Contains(id + name))
                    idNameList.Add(id + name);
                else
                {
                    MsgBox.Show("第" + dr.Index + 1 + "列 " + id + name + "資料重複無法儲存。");

                    return false;
                }

            }
            return value;
        }

        /// <summary>
        /// 載入預設資料
        /// </summary>
        private void LoadDefaultData()
        {
            try
            {
                dgData.Rows.Clear();
                defaultGroupDict.Clear();
                defaultGroupDict.Add("01", "機械群");
                defaultGroupDict.Add("02", "動力機械群");
                defaultGroupDict.Add("03", "電機與電子群");
                defaultGroupDict.Add("04", "化工群");
                defaultGroupDict.Add("05", "土木與建築群");
                defaultGroupDict.Add("06", "商業與管理群");
                defaultGroupDict.Add("07", "外語群");
                defaultGroupDict.Add("08", "設計群");
                defaultGroupDict.Add("09", "農業群");
                defaultGroupDict.Add("10", "食品群");
                defaultGroupDict.Add("11", "家政群");
                defaultGroupDict.Add("12", "餐旅群");
                defaultGroupDict.Add("13", "水產群");
                defaultGroupDict.Add("14", "海事群");
                defaultGroupDict.Add("15", "藝術群");

                // 清空舊
                List<udtRegistrationGroup> groupList = accessHelper.Select<udtRegistrationGroup>();

                if (groupList.Count > 0)
                {
                    foreach (udtRegistrationGroup data in groupList)
                        data.Deleted = true;

                    groupList.SaveAll();
                }

                List<udtRegistrationGroup> NewDataList = new List<udtRegistrationGroup>();
                foreach (string id in defaultGroupDict.Keys)
                {
                    udtRegistrationGroup rg = new udtRegistrationGroup();
                    rg.GroupID = id;
                    rg.GroupName = defaultGroupDict[id];
                    NewDataList.Add(rg);
                }

                NewDataList.SaveAll();
            }
            catch (Exception ex)
            {
                MsgBox.Show("恢復系統預設發生錯誤：" + ex.Message);
            }

        }

        private void LoadData()
        {
            List<udtRegistrationGroup> groupList = accessHelper.Select<udtRegistrationGroup>();

            // 沒有資料載入預設
            if (groupList.Count < 1)
            {
                LoadDefaultData();
            }
            else
            {
                dgData.Rows.Clear();
                sourceDict.Clear();
                foreach(udtRegistrationGroup data in groupList)
                {
                    int rowIdx = dgData.Rows.Add();
                    dgData.Rows[rowIdx].Tag = data;
                    dgData.Rows[rowIdx].Cells[colID.Index].Value = data.GroupID;
                    dgData.Rows[rowIdx].Cells[colName.Index].Value = data.GroupName;
                    sourceDict.Add(data.UID,data);
                }
            }
        }

        private void SaveData()
        {

            // 讀取資料並儲存
            try
            {
                List<udtRegistrationGroup> delList = new List<udtRegistrationGroup>();
                List<string> uidList = new List<string>();
                List<udtRegistrationGroup> dataList = new List<udtRegistrationGroup>();
                foreach (DataGridViewRow drv in dgData.Rows)
                {
                    if (drv.IsNewRow)
                        continue;

                    udtRegistrationGroup data = drv.Tag as udtRegistrationGroup;
                    if (data == null)
                    {
                        data = new udtRegistrationGroup();
                    }
                    data.GroupID = drv.Cells[colID.Index].Value.ToString();
                    data.GroupName = drv.Cells[colName.Index].Value.ToString();

                    if (!string.IsNullOrEmpty(data.UID))
                        uidList.Add(data.UID);

                    dataList.Add(data);
                }

                dataList.SaveAll();

                foreach(string key in sourceDict.Keys)
                {
                    if (!uidList.Contains(key))
                    {
                        sourceDict[key].Deleted = true;
                        delList.Add(sourceDict[key]);
                    }                       
                }

                // 清除
                if (delList.Count > 0)
                    delList.SaveAll();

                


                MsgBox.Show("儲存完成。");
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.Show("儲存資料發生錯誤：" + ex.Message);
            }
        }

        private void GroupForm_Load(object sender, EventArgs e)
        {
            // 載入資料
            this.LoadData();
        }

    
    }
}
