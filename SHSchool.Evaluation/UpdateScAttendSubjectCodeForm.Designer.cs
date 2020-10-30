namespace SHSchool.Evaluation
{
    partial class UpdateScAttendSubjectCodeForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.學年度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.學期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.課程名稱 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.科目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.班級 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.座號 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.學號 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.姓名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.學生人數 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExcute = new DevComponents.DotNetBar.ButtonX();
            this.cboShowWay = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
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
            this.學年度,
            this.學期,
            this.課程名稱,
            this.科目,
            this.班級,
            this.座號,
            this.學號,
            this.姓名,
            this.學生人數});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(12, 55);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.Size = new System.Drawing.Size(933, 374);
            this.dataGridViewX1.TabIndex = 0;
            // 
            // 學年度
            // 
            this.學年度.HeaderText = "學年度";
            this.學年度.Name = "學年度";
            this.學年度.ReadOnly = true;
            // 
            // 學期
            // 
            this.學期.HeaderText = "學期";
            this.學期.Name = "學期";
            this.學期.ReadOnly = true;
            // 
            // 課程名稱
            // 
            this.課程名稱.HeaderText = "課程名稱";
            this.課程名稱.Name = "課程名稱";
            this.課程名稱.ReadOnly = true;
            // 
            // 科目
            // 
            this.科目.HeaderText = "科目";
            this.科目.Name = "科目";
            this.科目.ReadOnly = true;
            // 
            // 班級
            // 
            this.班級.HeaderText = "班級";
            this.班級.Name = "班級";
            this.班級.ReadOnly = true;
            // 
            // 座號
            // 
            this.座號.HeaderText = "座號";
            this.座號.Name = "座號";
            this.座號.ReadOnly = true;
            // 
            // 學號
            // 
            this.學號.HeaderText = "學號";
            this.學號.Name = "學號";
            this.學號.ReadOnly = true;
            // 
            // 姓名
            // 
            this.姓名.HeaderText = "姓名";
            this.姓名.Name = "姓名";
            this.姓名.ReadOnly = true;
            // 
            // 學生人數
            // 
            this.學生人數.HeaderText = "學生人數";
            this.學生人數.Name = "學生人數";
            this.學生人數.ReadOnly = true;
            // 
            // btnExcute
            // 
            this.btnExcute.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExcute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcute.BackColor = System.Drawing.Color.Transparent;
            this.btnExcute.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExcute.Location = new System.Drawing.Point(870, 449);
            this.btnExcute.Name = "btnExcute";
            this.btnExcute.Size = new System.Drawing.Size(75, 23);
            this.btnExcute.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExcute.TabIndex = 1;
            this.btnExcute.Text = "執行";
            // 
            // cboShowWay
            // 
            this.cboShowWay.DisplayMember = "Text";
            this.cboShowWay.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboShowWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShowWay.FormattingEnabled = true;
            this.cboShowWay.ItemHeight = 19;
            this.cboShowWay.Location = new System.Drawing.Point(98, 13);
            this.cboShowWay.Name = "cboShowWay";
            this.cboShowWay.Size = new System.Drawing.Size(199, 25);
            this.cboShowWay.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboShowWay.TabIndex = 2;
            this.cboShowWay.SelectedIndexChanged += new System.EventHandler(this.comboBoxEx1_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(13, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(79, 23);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "顯示方式";
            // 
            // UpdateScAttendSubjectCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 473);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cboShowWay);
            this.Controls.Add(this.btnExcute);
            this.Controls.Add(this.dataGridViewX1);
            this.DoubleBuffered = true;
            this.Name = "UpdateScAttendSubjectCodeForm";
            this.Text = "UpdateScAttendSubjectCodeForm";
            this.Load += new System.EventHandler(this.UpdateScAttendSubjectCodeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 學年度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 學期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 課程名稱;
        private System.Windows.Forms.DataGridViewTextBoxColumn 科目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 班級;
        private System.Windows.Forms.DataGridViewTextBoxColumn 座號;
        private System.Windows.Forms.DataGridViewTextBoxColumn 學號;
        private System.Windows.Forms.DataGridViewTextBoxColumn 姓名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 學生人數;
        private DevComponents.DotNetBar.ButtonX btnExcute;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboShowWay;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}