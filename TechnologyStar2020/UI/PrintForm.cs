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
    public partial class PrintForm : BaseForm
    {
        public PrintForm()
        {
            InitializeComponent();
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }

        private void btnSetDeptGroup_Click(object sender, EventArgs e)
        {
            DeptGroupForm dgf = new DeptGroupForm();
            dgf.ShowDialog();
        }
    }
}
