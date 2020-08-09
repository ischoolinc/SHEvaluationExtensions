namespace SHSchool.Evaluation
{
    partial class GraduationPlanUpdateDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.cboGraduationName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.checkBoxShowOnly = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this._DomainIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.科目代碼_原 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.科目_原 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.授課學期學分 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.科目代碼_新 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.科目_新 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.授課學期學分_新 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.修改欄位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.備註 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._DomainIndex,
            this.科目代碼_原,
            this.科目_原,
            this.授課學期學分,
            this.科目代碼_新,
            this.科目_新,
            this.授課學期學分_新,
            this.Action,
            this.修改欄位,
            this.備註});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(12, 57);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.Size = new System.Drawing.Size(1229, 451);
            this.dataGridViewX1.TabIndex = 0;
            this.dataGridViewX1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellContentClick);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(1166, 521);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cboGraduationName
            // 
            this.cboGraduationName.DisplayMember = "GraduationName";
            this.cboGraduationName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboGraduationName.FormattingEnabled = true;
            this.cboGraduationName.ItemHeight = 19;
            this.cboGraduationName.Location = new System.Drawing.Point(163, 14);
            this.cboGraduationName.Name = "cboGraduationName";
            this.cboGraduationName.Size = new System.Drawing.Size(223, 25);
            this.cboGraduationName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboGraduationName.TabIndex = 2;
            this.cboGraduationName.SelectedIndexChanged += new System.EventHandler(this.cboGraduationName_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 14);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(145, 23);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "現有課程規劃表名稱:";
            // 
            // checkBoxShowOnly
            // 
            this.checkBoxShowOnly.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.checkBoxShowOnly.BackgroundStyle.Class = "";
            this.checkBoxShowOnly.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxShowOnly.Location = new System.Drawing.Point(412, 14);
            this.checkBoxShowOnly.Name = "checkBoxShowOnly";
            this.checkBoxShowOnly.Size = new System.Drawing.Size(148, 23);
            this.checkBoxShowOnly.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxShowOnly.TabIndex = 9;
            this.checkBoxShowOnly.Text = "只顯示變更項目";
            this.checkBoxShowOnly.CheckedChanged += new System.EventHandler(this.checkBoxShowOnly_CheckedChanged);
            // 
            // _DomainIndex
            // 
            this._DomainIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this._DomainIndex.HeaderText = "領域(原)";
            this._DomainIndex.Name = "_DomainIndex";
            this._DomainIndex.ReadOnly = true;
            this._DomainIndex.Width = 80;
            // 
            // 科目代碼_原
            // 
            this.科目代碼_原.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.科目代碼_原.HeaderText = "科目代碼(原)";
            this.科目代碼_原.Name = "科目代碼_原";
            this.科目代碼_原.ReadOnly = true;
            this.科目代碼_原.Width = 106;
            // 
            // 科目_原
            // 
            this.科目_原.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.科目_原.HeaderText = "科目(原)";
            this.科目_原.Name = "科目_原";
            this.科目_原.ReadOnly = true;
            this.科目_原.Width = 80;
            // 
            // 授課學期學分
            // 
            this.授課學期學分.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.授課學期學分.HeaderText = "授課學期學分(原)";
            this.授課學期學分.Name = "授課學期學分";
            this.授課學期學分.ReadOnly = true;
            this.授課學期學分.Visible = false;
            this.授課學期學分.Width = 132;
            // 
            // 科目代碼_新
            // 
            this.科目代碼_新.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.科目代碼_新.HeaderText = "科目代碼(新)";
            this.科目代碼_新.Name = "科目代碼_新";
            this.科目代碼_新.ReadOnly = true;
            this.科目代碼_新.Width = 106;
            // 
            // 科目_新
            // 
            this.科目_新.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.科目_新.HeaderText = "科目(新)";
            this.科目_新.Name = "科目_新";
            this.科目_新.ReadOnly = true;
            this.科目_新.Width = 80;
            // 
            // 授課學期學分_新
            // 
            this.授課學期學分_新.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.授課學期學分_新.HeaderText = "授課學期學分(新)";
            this.授課學期學分_新.Name = "授課學期學分_新";
            this.授課學期學分_新.ReadOnly = true;
            this.授課學期學分_新.Visible = false;
            this.授課學期學分_新.Width = 132;
            // 
            // Action
            // 
            this.Action.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Action.HeaderText = "動作";
            this.Action.Name = "Action";
            this.Action.Width = 59;
            // 
            // 修改欄位
            // 
            this.修改欄位.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.修改欄位.HeaderText = "修改欄位";
            this.修改欄位.Name = "修改欄位";
            this.修改欄位.Width = 85;
            // 
            // 備註
            // 
            this.備註.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.備註.HeaderText = "備註";
            this.備註.Name = "備註";
            this.備註.ReadOnly = true;
            this.備註.Width = 59;
            // 
            // GraduationPlanUpdateDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1253, 546);
            this.Controls.Add(this.checkBoxShowOnly);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cboGraduationName);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.dataGridViewX1);
            this.DoubleBuffered = true;
            this.Name = "GraduationPlanUpdateDetailForm";
            this.Text = "檢視更新細節";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboGraduationName;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxShowOnly;
        private System.Windows.Forms.DataGridViewTextBoxColumn _DomainIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn 科目代碼_原;
        private System.Windows.Forms.DataGridViewTextBoxColumn 科目_原;
        private System.Windows.Forms.DataGridViewTextBoxColumn 授課學期學分;
        private System.Windows.Forms.DataGridViewTextBoxColumn 科目代碼_新;
        private System.Windows.Forms.DataGridViewTextBoxColumn 科目_新;
        private System.Windows.Forms.DataGridViewTextBoxColumn 授課學期學分_新;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn 修改欄位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 備註;
    }
}