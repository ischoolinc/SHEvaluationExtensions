namespace TechnologyStar2020.UI
{
    partial class DeptGroupForm
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
            this.dgData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnSetGroup = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.colDeptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegDeptID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegDeptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegGroupName = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgData
            // 
            this.dgData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgData.BackgroundColor = System.Drawing.Color.White;
            this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDeptName,
            this.colRegDeptID,
            this.colRegDeptName,
            this.colRegGroupName});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgData.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgData.Location = new System.Drawing.Point(13, 23);
            this.dgData.Name = "dgData";
            this.dgData.RowTemplate.Height = 24;
            this.dgData.Size = new System.Drawing.Size(632, 183);
            this.dgData.TabIndex = 0;
            // 
            // btnSetGroup
            // 
            this.btnSetGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSetGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetGroup.AutoSize = true;
            this.btnSetGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnSetGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSetGroup.Location = new System.Drawing.Point(13, 222);
            this.btnSetGroup.Name = "btnSetGroup";
            this.btnSetGroup.Size = new System.Drawing.Size(78, 25);
            this.btnSetGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSetGroup.TabIndex = 1;
            this.btnSetGroup.Text = "設定群代碼";
            this.btnSetGroup.Click += new System.EventHandler(this.btnSetGroup_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(481, 222);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(572, 222);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // colDeptName
            // 
            this.colDeptName.HeaderText = "科別名稱";
            this.colDeptName.Name = "colDeptName";
            this.colDeptName.ReadOnly = true;
            this.colDeptName.Width = 150;
            // 
            // colRegDeptID
            // 
            this.colRegDeptID.HeaderText = "報名科別代碼";
            this.colRegDeptID.Name = "colRegDeptID";
            // 
            // colRegDeptName
            // 
            this.colRegDeptName.HeaderText = "報名科別名稱";
            this.colRegDeptName.Name = "colRegDeptName";
            this.colRegDeptName.Width = 150;
            // 
            // colRegGroupName
            // 
            this.colRegGroupName.DisplayMember = "Text";
            this.colRegGroupName.DropDownHeight = 106;
            this.colRegGroupName.DropDownWidth = 121;
            this.colRegGroupName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colRegGroupName.HeaderText = "報名群名稱";
            this.colRegGroupName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colRegGroupName.ItemHeight = 17;
            this.colRegGroupName.Name = "colRegGroupName";
            this.colRegGroupName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRegGroupName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colRegGroupName.Width = 150;
            // 
            // DeptGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 255);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSetGroup);
            this.Controls.Add(this.dgData);
            this.DoubleBuffered = true;
            this.Name = "DeptGroupForm";
            this.Text = "設定報名科群對照";
            this.Load += new System.EventHandler(this.DeptGroupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgData;
        private DevComponents.DotNetBar.ButtonX btnSetGroup;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegDeptID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegDeptName;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colRegGroupName;
    }
}