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
            this.textFileCassGroup = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSelectedClassGroup = new DevComponents.DotNetBar.ButtonX();
            this.lab1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.pbLoding = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoding)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            // 
            // 
            // 
            this.txtFileName.Border.Class = "TextBoxBorder";
            this.txtFileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFileName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFileName.Location = new System.Drawing.Point(59, 51);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(161, 25);
            this.txtFileName.TabIndex = 0;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectFile.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectFile.Location = new System.Drawing.Point(229, 53);
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
            this.btnImprt.Location = new System.Drawing.Point(229, 191);
            this.btnImprt.Name = "btnImprt";
            this.btnImprt.Size = new System.Drawing.Size(75, 23);
            this.btnImprt.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImprt.TabIndex = 2;
            this.btnImprt.Text = "匯入";
            this.btnImprt.Click += new System.EventHandler(this.btnImprt_Click);
            // 
            // textFileCassGroup
            // 
            // 
            // 
            // 
            this.textFileCassGroup.Border.Class = "TextBoxBorder";
            this.textFileCassGroup.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textFileCassGroup.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textFileCassGroup.Location = new System.Drawing.Point(59, 129);
            this.textFileCassGroup.Name = "textFileCassGroup";
            this.textFileCassGroup.Size = new System.Drawing.Size(161, 25);
            this.textFileCassGroup.TabIndex = 4;
            // 
            // btnSelectedClassGroup
            // 
            this.btnSelectedClassGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectedClassGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectedClassGroup.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectedClassGroup.Location = new System.Drawing.Point(229, 129);
            this.btnSelectedClassGroup.Name = "btnSelectedClassGroup";
            this.btnSelectedClassGroup.Size = new System.Drawing.Size(75, 23);
            this.btnSelectedClassGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSelectedClassGroup.TabIndex = 5;
            this.btnSelectedClassGroup.Text = "選擇檔案";
            this.btnSelectedClassGroup.Click += new System.EventHandler(this.btnSelectedClassGroup_Click);
            // 
            // lab1
            // 
            this.lab1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lab1.BackgroundStyle.Class = "";
            this.lab1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lab1.Location = new System.Drawing.Point(12, 100);
            this.lab1.Name = "lab1";
            this.lab1.Size = new System.Drawing.Size(189, 23);
            this.lab1.TabIndex = 6;
            this.lab1.Text = "步驟二：選擇【班群代碼檔】";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 22);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(208, 23);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "步驟一：選擇【課程代碼資料檔】";
            // 
            // pbLoding
            // 
            this.pbLoding.BackColor = System.Drawing.Color.Transparent;
            this.pbLoding.Image = global::SHSchool.Evaluation.Properties.Resources.loding;
            this.pbLoding.Location = new System.Drawing.Point(130, 177);
            this.pbLoding.Name = "pbLoding";
            this.pbLoding.Size = new System.Drawing.Size(33, 33);
            this.pbLoding.TabIndex = 8;
            this.pbLoding.TabStop = false;
            this.pbLoding.Visible = false;
            // 
            // ImportCurriculumMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 226);
            this.Controls.Add(this.pbLoding);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.lab1);
            this.Controls.Add(this.btnSelectedClassGroup);
            this.Controls.Add(this.textFileCassGroup);
            this.Controls.Add(this.btnImprt);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtFileName);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(353, 309);
            this.MinimumSize = new System.Drawing.Size(310, 184);
            this.Name = "ImportCurriculumMappingForm";
            this.Text = "匯入課程規劃表 (108年度開始適用)";
            this.Load += new System.EventHandler(this.ImportCurriculumMappingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoding)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtFileName;
        private DevComponents.DotNetBar.ButtonX btnSelectFile;
        private DevComponents.DotNetBar.ButtonX btnImprt;
        private DevComponents.DotNetBar.Controls.TextBoxX textFileCassGroup;
        private DevComponents.DotNetBar.ButtonX btnSelectedClassGroup;
        private DevComponents.DotNetBar.LabelX lab1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.PictureBox pbLoding;
    }
}