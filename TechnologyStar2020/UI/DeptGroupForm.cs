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

namespace TechnologyStar2020.UI
{
    public partial class DeptGroupForm : BaseForm
    {
        public DeptGroupForm()
        {
            InitializeComponent();
        }

        private void DeptGroupForm_Load(object sender, EventArgs e)
        {

        }

        private void LoadData()
        {

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

        }

        private void btnSetGroup_Click(object sender, EventArgs e)
        {
            GroupForm gf = new GroupForm();
            gf.ShowDialog();
        }
    }
}
