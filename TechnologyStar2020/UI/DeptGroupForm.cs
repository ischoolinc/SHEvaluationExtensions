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
using TechnologyStar2020.DAO;
using FISCA.UDT;

namespace TechnologyStar2020.UI
{
    public partial class DeptGroupForm : BaseForm
    {
        QueryData qd = new QueryData();
        AccessHelper accessHelper = new AccessHelper();
        List<DeptInfo> deptInfoList = new List<DeptInfo>();
        List<udtRegistrationGroup> groupList = new List<udtRegistrationGroup>();
        Dictionary<string, string> groupDict = new Dictionary<string, string>();

        public DeptGroupForm()
        {
            InitializeComponent();
        }

        private void DeptGroupForm_Load(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            dgData.Rows.Clear();
            // 取得系統內科別資料
            deptInfoList = qd.GetDeptInfoList();

            // 取得群資料
            groupList = accessHelper.Select<udtRegistrationGroup>();
            groupDict.Clear();
            foreach (udtRegistrationGroup data in groupList)
            {
                groupDict.Add(data.GroupID + " " + data.GroupName, data.GroupID);
            }

            dgData.Rows.Clear();

            foreach (DeptInfo di in deptInfoList)
            {
                int rowIdx = dgData.Rows.Add();
                dgData.Rows[rowIdx].Tag = di;
                dgData.Rows[rowIdx].Cells[colDeptName.Index].Value = di.DeptName;

                DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
                foreach (string key in groupDict.Keys)
                    cell.Items.Add(key);

                dgData.Rows[rowIdx].Cells[colRegGroupName.Index] = cell;
            }

        }

        private void SaveData()
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow drv in dgData.Rows)
            {
                Console.WriteLine(drv.Cells[colDeptName.Index].Value.ToString());
            }
        }

        private void btnSetGroup_Click(object sender, EventArgs e)
        {
            GroupForm gf = new GroupForm();
            gf.ShowDialog();
        }
    }
}
