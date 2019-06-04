namespace SHSchool.Evaluation.InportForm
{
    partial class ImportCurriculumMappingForm
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
            this.txtFileName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSelectFile = new DevComponents.DotNetBar.ButtonX();
            this.btnImprt = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            // 
            // 
            // 
            this.txtFileName.Border.Class = "TextBoxBorder";
            this.txtFileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFileName.Location = new System.Drawing.Point(12, 23);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(161, 25);
            this.txtFileName.TabIndex = 0;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectFile.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectFile.Location = new System.Drawing.Point(201, 23);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSelectFile.TabIndex = 1;
            this.btnSelectFile.Text = "選擇檔案";
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnImprt
            // 
            this.btnImprt.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImprt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImprt.BackColor = System.Drawing.Color.Transparent;
            this.btnImprt.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImprt.Location = new System.Drawing.Point(201, 110);
            this.btnImprt.Name = "btnImprt";
            this.btnImprt.Size = new System.Drawing.Size(75, 23);
            this.btnImprt.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImprt.TabIndex = 2;
            this.btnImprt.Text = "開始上傳";
            this.btnImprt.Click += new System.EventHandler(this.btnImprt_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 71);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(253, 23);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "若有相同課程規劃表名稱將覆蓋原始資料";
            // 
            // ImportCurriculumMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 145);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnImprt);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtFileName);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(310, 184);
            this.MinimumSize = new System.Drawing.Size(310, 184);
            this.Name = "ImportCurriculumMappingForm";
            this.Text = "匯入課程規劃表 (108年度開始適用)";
            this.Load += new System.EventHandler(this.ImportCurriculumMappingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtFileName;
        private DevComponents.DotNetBar.ButtonX btnSelectFile;
        private DevComponents.DotNetBar.ButtonX btnImprt;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}