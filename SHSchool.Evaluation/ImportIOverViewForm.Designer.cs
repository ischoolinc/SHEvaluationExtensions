namespace SHSchool.Evaluation
{
    partial class ImportGraduationPlanInfoForm
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
            this.btnCondouct = new DevComponents.DotNetBar.ButtonX();
            this.lab1 = new DevComponents.DotNetBar.LabelX();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.EntrySchoolYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourseType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GraduationPlanName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GraduatePlanCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldGraduPlanName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.已存在課程規劃表數量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.動作 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheakDetail = new DevComponents.DotNetBar.Controls.DataGridViewButtonXColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EntrySchoolYear,
            this.CourseType,
            this.GraduationPlanName,
            this.GraduatePlanCode,
            this.OldGraduPlanName,
            this.已存在課程規劃表數量,
            this.動作,
            this.CheakDetail});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(12, 42);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.Size = new System.Drawing.Size(1054, 525);
            this.dataGridViewX1.TabIndex = 1;
            this.dataGridViewX1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellContentClick);
            // 
            // btnCondouct
            // 
            this.btnCondouct.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCondouct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCondouct.BackColor = System.Drawing.Color.Transparent;
            this.btnCondouct.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCondouct.Location = new System.Drawing.Point(991, 586);
            this.btnCondouct.Name = "btnCondouct";
            this.btnCondouct.Size = new System.Drawing.Size(75, 23);
            this.btnCondouct.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCondouct.TabIndex = 2;
            this.btnCondouct.Text = "執行";
            this.btnCondouct.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // lab1
            // 
            this.lab1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lab1.BackgroundStyle.Class = "";
            this.lab1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lab1.Location = new System.Drawing.Point(12, 13);
            this.lab1.Name = "lab1";
            this.lab1.Size = new System.Drawing.Size(161, 23);
            this.lab1.TabIndex = 4;
            this.lab1.Text = "本次匯入課程規劃表：";
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(12, 586);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(142, 23);
            this.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "匯出現有課程規劃表";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // EntrySchoolYear
            // 
            this.EntrySchoolYear.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.EntrySchoolYear.HeaderText = "入學年度";
            this.EntrySchoolYear.Name = "EntrySchoolYear";
            this.EntrySchoolYear.ReadOnly = true;
            this.EntrySchoolYear.Width = 67;
            // 
            // CourseType
            // 
            this.CourseType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CourseType.HeaderText = "課程類型";
            this.CourseType.Name = "CourseType";
            this.CourseType.ReadOnly = true;
            this.CourseType.Width = 67;
            // 
            // GraduationPlanName
            // 
            this.GraduationPlanName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.GraduationPlanName.HeaderText = "課程規劃表名稱";
            this.GraduationPlanName.Name = "GraduationPlanName";
            this.GraduationPlanName.ReadOnly = true;
            this.GraduationPlanName.Width = 90;
            // 
            // GraduatePlanCode
            // 
            this.GraduatePlanCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.GraduatePlanCode.HeaderText = "課程規劃表識別代碼";
            this.GraduatePlanCode.Name = "GraduatePlanCode";
            this.GraduatePlanCode.ReadOnly = true;
            this.GraduatePlanCode.Width = 102;
            // 
            // OldGraduPlanName
            // 
            this.OldGraduPlanName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.OldGraduPlanName.HeaderText = "系統已存在之課程對照表名稱";
            this.OldGraduPlanName.Name = "OldGraduPlanName";
            this.OldGraduPlanName.ReadOnly = true;
            this.OldGraduPlanName.Width = 125;
            // 
            // 已存在課程規劃表數量
            // 
            this.已存在課程規劃表數量.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.已存在課程規劃表數量.HeaderText = "已存在課程規劃表數量";
            this.已存在課程規劃表數量.Name = "已存在課程規劃表數量";
            this.已存在課程規劃表數量.ReadOnly = true;
            this.已存在課程規劃表數量.Width = 102;
            // 
            // 動作
            // 
            this.動作.HeaderText = "動作(新增/更新)";
            this.動作.Name = "動作";
            this.動作.ReadOnly = true;
            // 
            // CheakDetail
            // 
            this.CheakDetail.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.CheakDetail.HeaderText = "檢視";
            this.CheakDetail.Name = "CheakDetail";
            this.CheakDetail.ReadOnly = true;
            this.CheakDetail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CheakDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CheakDetail.Text = "檢視";
            this.CheakDetail.UseColumnTextForButtonValue = true;
            // 
            // ImportGraduationPlanInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 621);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lab1);
            this.Controls.Add(this.btnCondouct);
            this.Controls.Add(this.dataGridViewX1);
            this.DoubleBuffered = true;
            this.Name = "ImportGraduationPlanInfoForm";
            this.Text = "匯入課程規劃表";
            this.Load += new System.EventHandler(this.ImportInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX btnCondouct;
        private DevComponents.DotNetBar.LabelX lab1;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private System.Windows.Forms.DataGridViewTextBoxColumn EntrySchoolYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn CourseType;
        private System.Windows.Forms.DataGridViewTextBoxColumn GraduationPlanName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GraduatePlanCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldGraduPlanName;
        private System.Windows.Forms.DataGridViewTextBoxColumn 已存在課程規劃表數量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 動作;
        private DevComponents.DotNetBar.Controls.DataGridViewButtonXColumn CheakDetail;
    }
}