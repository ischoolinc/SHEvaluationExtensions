namespace TechnologyStar2020.UI
{
    partial class PrintForm111
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
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.ckPro = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.ckProReq = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.ckSkillDomain = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.ckSkillDomainReq = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.ckCEM = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.ckCEMReq = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel2.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.groupPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(69, 349);
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
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(164, 349);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(6, 6);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(234, 56);
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
            this.labelX2.Enabled = false;
            this.labelX2.Location = new System.Drawing.Point(299, 37);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "成績來源";
            this.labelX2.Visible = false;
            // 
            // cboSelectItem
            // 
            this.cboSelectItem.DisplayMember = "Text";
            this.cboSelectItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSelectItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectItem.Enabled = false;
            this.cboSelectItem.FormattingEnabled = true;
            this.cboSelectItem.ItemHeight = 19;
            this.cboSelectItem.Location = new System.Drawing.Point(366, 35);
            this.cboSelectItem.Name = "cboSelectItem";
            this.cboSelectItem.Size = new System.Drawing.Size(471, 25);
            this.cboSelectItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSelectItem.TabIndex = 5;
            this.cboSelectItem.Visible = false;
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Enabled = false;
            this.groupPanel1.Location = new System.Drawing.Point(411, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(212, 36);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = "";
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = "";
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = "";
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 15;
            this.groupPanel1.Text = "成績來源";
            this.groupPanel1.Visible = false;
            // 
            // groupPanel2
            // 
            this.groupPanel2.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.ckPro);
            this.groupPanel2.Controls.Add(this.ckProReq);
            this.groupPanel2.IsShadowEnabled = true;
            this.groupPanel2.Location = new System.Drawing.Point(4, 69);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(234, 86);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.Class = "";
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.Class = "";
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.Class = "";
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 15;
            this.groupPanel2.Text = "專業及實習";
            // 
            // ckPro
            // 
            this.ckPro.AutoSize = true;
            // 
            // 
            // 
            this.ckPro.BackgroundStyle.Class = "";
            this.ckPro.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckPro.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckPro.Location = new System.Drawing.Point(9, 33);
            this.ckPro.Name = "ckPro";
            this.ckPro.Size = new System.Drawing.Size(94, 21);
            this.ckPro.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckPro.TabIndex = 31;
            this.ckPro.Text = "專業及實習";
            // 
            // ckProReq
            // 
            this.ckProReq.AutoSize = true;
            // 
            // 
            // 
            this.ckProReq.BackgroundStyle.Class = "";
            this.ckProReq.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckProReq.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckProReq.Checked = true;
            this.ckProReq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckProReq.CheckValue = "Y";
            this.ckProReq.Location = new System.Drawing.Point(9, 5);
            this.ckProReq.Name = "ckProReq";
            this.ckProReq.Size = new System.Drawing.Size(156, 21);
            this.ckProReq.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckProReq.TabIndex = 29;
            this.ckProReq.Text = "專業及實習(部定必修)";
            // 
            // groupPanel3
            // 
            this.groupPanel3.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.ckSkillDomain);
            this.groupPanel3.Controls.Add(this.ckSkillDomainReq);
            this.groupPanel3.IsShadowEnabled = true;
            this.groupPanel3.Location = new System.Drawing.Point(4, 161);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(234, 86);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.Class = "";
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseDown.Class = "";
            this.groupPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseOver.Class = "";
            this.groupPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel3.TabIndex = 16;
            this.groupPanel3.Text = "技能領域";
            // 
            // ckSkillDomain
            // 
            this.ckSkillDomain.AutoSize = true;
            // 
            // 
            // 
            this.ckSkillDomain.BackgroundStyle.Class = "";
            this.ckSkillDomain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckSkillDomain.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckSkillDomain.Location = new System.Drawing.Point(9, 33);
            this.ckSkillDomain.Name = "ckSkillDomain";
            this.ckSkillDomain.Size = new System.Drawing.Size(80, 21);
            this.ckSkillDomain.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckSkillDomain.TabIndex = 31;
            this.ckSkillDomain.Text = "技能領域";
            // 
            // ckSkillDomainReq
            // 
            this.ckSkillDomainReq.AutoSize = true;
            // 
            // 
            // 
            this.ckSkillDomainReq.BackgroundStyle.Class = "";
            this.ckSkillDomainReq.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckSkillDomainReq.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckSkillDomainReq.Checked = true;
            this.ckSkillDomainReq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckSkillDomainReq.CheckValue = "Y";
            this.ckSkillDomainReq.Location = new System.Drawing.Point(9, 5);
            this.ckSkillDomainReq.Name = "ckSkillDomainReq";
            this.ckSkillDomainReq.Size = new System.Drawing.Size(143, 21);
            this.ckSkillDomainReq.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckSkillDomainReq.TabIndex = 29;
            this.ckSkillDomainReq.Text = "技能領域(部定必修)";
            // 
            // groupPanel4
            // 
            this.groupPanel4.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel4.Controls.Add(this.ckCEM);
            this.groupPanel4.Controls.Add(this.ckCEMReq);
            this.groupPanel4.IsShadowEnabled = true;
            this.groupPanel4.Location = new System.Drawing.Point(4, 253);
            this.groupPanel4.Name = "groupPanel4";
            this.groupPanel4.Size = new System.Drawing.Size(234, 86);
            // 
            // 
            // 
            this.groupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel4.Style.BackColorGradientAngle = 90;
            this.groupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderBottomWidth = 1;
            this.groupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderLeftWidth = 1;
            this.groupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderRightWidth = 1;
            this.groupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderTopWidth = 1;
            this.groupPanel4.Style.Class = "";
            this.groupPanel4.Style.CornerDiameter = 4;
            this.groupPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            // 
            // 
            // 
            this.groupPanel4.StyleMouseDown.Class = "";
            this.groupPanel4.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel4.StyleMouseOver.Class = "";
            this.groupPanel4.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel4.TabIndex = 17;
            this.groupPanel4.Text = "國英數";
            // 
            // ckCEM
            // 
            this.ckCEM.AutoSize = true;
            // 
            // 
            // 
            this.ckCEM.BackgroundStyle.Class = "";
            this.ckCEM.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckCEM.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckCEM.Location = new System.Drawing.Point(9, 33);
            this.ckCEM.Name = "ckCEM";
            this.ckCEM.Size = new System.Drawing.Size(67, 21);
            this.ckCEM.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckCEM.TabIndex = 31;
            this.ckCEM.Text = "國英數";
            // 
            // ckCEMReq
            // 
            this.ckCEMReq.AutoSize = true;
            // 
            // 
            // 
            this.ckCEMReq.BackgroundStyle.Class = "";
            this.ckCEMReq.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckCEMReq.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.ckCEMReq.Checked = true;
            this.ckCEMReq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckCEMReq.CheckValue = "Y";
            this.ckCEMReq.Location = new System.Drawing.Point(9, 5);
            this.ckCEMReq.Name = "ckCEMReq";
            this.ckCEMReq.Size = new System.Drawing.Size(129, 21);
            this.ckCEMReq.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckCEMReq.TabIndex = 29;
            this.ckCEMReq.Text = "國英數(部定必修)";
            // 
            // PrintForm111
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 377);
            this.Controls.Add(this.groupPanel4);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.cboSelectItem);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExport);
            this.DoubleBuffered = true;
            this.Name = "PrintForm111";
            this.Text = "技職繁星報名資料(111學年度適用)";
            this.Load += new System.EventHandler(this.PrintForm_Load);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.groupPanel4.ResumeLayout(false);
            this.groupPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSelectItem;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckPro;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckProReq;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckCEM;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckCEMReq;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckSkillDomain;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckSkillDomainReq;
    }
}