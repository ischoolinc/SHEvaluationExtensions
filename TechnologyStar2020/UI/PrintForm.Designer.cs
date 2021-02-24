namespace TechnologyStar2020.UI
{
    partial class PrintForm
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
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cboSelectItem = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.AutoSize = true;
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(214, 170);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(91, 25);
            this.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "產生報名資料";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(315, 170);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(26, 16);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(369, 87);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "學生名單範圍：三年級一般狀態學生。\r\n產生報名資料：報名資料、比序資料。\r\n成績計算方式：加權平均。";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(26, 114);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "成績來源";
            // 
            // cboSelectItem
            // 
            this.cboSelectItem.DisplayMember = "Text";
            this.cboSelectItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSelectItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectItem.FormattingEnabled = true;
            this.cboSelectItem.ItemHeight = 19;
            this.cboSelectItem.Location = new System.Drawing.Point(93, 112);
            this.cboSelectItem.Name = "cboSelectItem";
            this.cboSelectItem.Size = new System.Drawing.Size(302, 25);
            this.cboSelectItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSelectItem.TabIndex = 5;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 213);
            this.Controls.Add(this.cboSelectItem);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExport);
            this.DoubleBuffered = true;
            this.Name = "PrintForm";
            this.Text = "技職繁星報名資料";
            this.Load += new System.EventHandler(this.PrintForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSelectItem;
    }
}