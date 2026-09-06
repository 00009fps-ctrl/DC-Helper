namespace DC_Button_Finder
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtButtonSequence;
        private System.Windows.Forms.NumericUpDown numThreshold;
        private System.Windows.Forms.NumericUpDown numIterationDelay;
        private System.Windows.Forms.ComboBox cboWeekSelection;
        private System.Windows.Forms.ComboBox cboServerSelection;

        // GroupBoxes
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.GroupBox grpAssistant;

        // Метки для групп (вместо чекбоксов включения)
        private System.Windows.Forms.Label lblHiddenBoss;
        private System.Windows.Forms.Label lblWeekBoss;
        private System.Windows.Forms.Label lblMobsEasy;
        private System.Windows.Forms.Label lblMobsNormal;
        private System.Windows.Forms.Label lblMobsStrong;

        // Чекбоксы атаки для недельных боссов
        private System.Windows.Forms.CheckBox chkWeekBossX1;
        private System.Windows.Forms.CheckBox chkWeekBossX3;

        // Чекбоксы атаки для скрытых боссов
        private System.Windows.Forms.CheckBox chkHiddenBossX1;
        private System.Windows.Forms.CheckBox chkHiddenBossX3;

        // Чекбоксы атаки для мобов
        private System.Windows.Forms.CheckBox chkMobEasyX1;
        private System.Windows.Forms.CheckBox chkMobEasyX3;
        private System.Windows.Forms.CheckBox chkMobNormalX1;
        private System.Windows.Forms.CheckBox chkMobNormalX3;
        private System.Windows.Forms.CheckBox chkMobStrongX1;
        private System.Windows.Forms.CheckBox chkMobStrongX3;

        // Чекбоксы Режимы работы
        private System.Windows.Forms.CheckBox chkAlwaysMode;
        private System.Windows.Forms.CheckBox chkSiegeOnlyMode;
        private System.Windows.Forms.CheckBox chkExtendedSiege;
        private System.Windows.Forms.CheckBox chkOnlyScratch;

        // Чекбоксы Ассистент
        private System.Windows.Forms.CheckBox chkAssistantX1;
        private System.Windows.Forms.CheckBox chkAssistantX3;

        // Кнопки
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.Label lblSequence;
        private System.Windows.Forms.Label lblWeek;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            txtButtonSequence = new TextBox();
            numThreshold = new NumericUpDown();
            numIterationDelay = new NumericUpDown();
            cboWeekSelection = new ComboBox();
            cboServerSelection = new ComboBox();
            grpSearch = new GroupBox();
            label7 = new Label();
            label6 = new Label();
            lblHiddenBoss = new Label();
            chkHiddenBossX1 = new CheckBox();
            chkHiddenBossX3 = new CheckBox();
            lblWeekBoss = new Label();
            chkWeekBossX1 = new CheckBox();
            chkWeekBossX3 = new CheckBox();
            label9 = new Label();
            label8 = new Label();
            label5 = new Label();
            chkMobEasyX3 = new CheckBox();
            chkMobNormalX3 = new CheckBox();
            chkMobEasyX1 = new CheckBox();
            lblMobsStrong = new Label();
            lblMobsEasy = new Label();
            chkMobNormalX1 = new CheckBox();
            chkMobStrongX3 = new CheckBox();
            lblMobsNormal = new Label();
            chkMobStrongX1 = new CheckBox();
            grpMode = new GroupBox();
            chkExtendedSiege = new CheckBox();
            chkSiegeOnlyMode = new CheckBox();
            chkAlwaysMode = new CheckBox();
            chkOnlyScratch = new CheckBox();
            grpAssistant = new GroupBox();
            label1 = new Label();
            chkAssistantX3 = new CheckBox();
            chkAssistantX1 = new CheckBox();
            btnStart = new Button();
            btnStop = new Button();
            lblThreshold = new Label();
            lblDelay = new Label();
            lblSequence = new Label();
            lblWeek = new Label();
            lblServer = new Label();
            lblTimer = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            groupBox1 = new GroupBox();
            tabPage3 = new TabPage();
            txtLog = new RichTextBox();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)numThreshold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numIterationDelay).BeginInit();
            grpSearch.SuspendLayout();
            grpMode.SuspendLayout();
            grpAssistant.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage3.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtButtonSequence
            // 
            resources.ApplyResources(txtButtonSequence, "txtButtonSequence");
            txtButtonSequence.Name = "txtButtonSequence";
            // 
            // numThreshold
            // 
            numThreshold.DecimalPlaces = 2;
            resources.ApplyResources(numThreshold, "numThreshold");
            numThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numThreshold.Name = "numThreshold";
            numThreshold.Value = new decimal(new int[] { 80, 0, 0, 0 });
            // 
            // numIterationDelay
            // 
            resources.ApplyResources(numIterationDelay, "numIterationDelay");
            numIterationDelay.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numIterationDelay.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numIterationDelay.Name = "numIterationDelay";
            numIterationDelay.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // cboWeekSelection
            // 
            resources.ApplyResources(cboWeekSelection, "cboWeekSelection");
            cboWeekSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            cboWeekSelection.FormattingEnabled = true;
            cboWeekSelection.Name = "cboWeekSelection";
            cboWeekSelection.SelectedIndexChanged += cboWeekSelection_SelectedIndexChanged;
            // 
            // cboServerSelection
            // 
            resources.ApplyResources(cboServerSelection, "cboServerSelection");
            cboServerSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            cboServerSelection.FormattingEnabled = true;
            cboServerSelection.Name = "cboServerSelection";
            cboServerSelection.SelectedIndexChanged += cboServerSelection_SelectedIndexChanged;
            // 
            // grpSearch
            // 
            resources.ApplyResources(grpSearch, "grpSearch");
            grpSearch.Controls.Add(label7);
            grpSearch.Controls.Add(label6);
            grpSearch.Controls.Add(lblHiddenBoss);
            grpSearch.Controls.Add(chkHiddenBossX1);
            grpSearch.Controls.Add(chkHiddenBossX3);
            grpSearch.Controls.Add(lblWeekBoss);
            grpSearch.Controls.Add(chkWeekBossX1);
            grpSearch.Controls.Add(chkWeekBossX3);
            grpSearch.Name = "grpSearch";
            grpSearch.TabStop = false;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // lblHiddenBoss
            // 
            resources.ApplyResources(lblHiddenBoss, "lblHiddenBoss");
            lblHiddenBoss.FlatStyle = FlatStyle.Flat;
            lblHiddenBoss.ForeColor = Color.DarkRed;
            lblHiddenBoss.Name = "lblHiddenBoss";
            // 
            // chkHiddenBossX1
            // 
            resources.ApplyResources(chkHiddenBossX1, "chkHiddenBossX1");
            chkHiddenBossX1.Name = "chkHiddenBossX1";
            chkHiddenBossX1.CheckedChanged += chkHiddenBossX1_CheckedChanged;
            // 
            // chkHiddenBossX3
            // 
            resources.ApplyResources(chkHiddenBossX3, "chkHiddenBossX3");
            chkHiddenBossX3.Name = "chkHiddenBossX3";
            chkHiddenBossX3.CheckedChanged += chkHiddenBossX3_CheckedChanged;
            // 
            // lblWeekBoss
            // 
            resources.ApplyResources(lblWeekBoss, "lblWeekBoss");
            lblWeekBoss.ForeColor = Color.DarkRed;
            lblWeekBoss.Name = "lblWeekBoss";
            // 
            // chkWeekBossX1
            // 
            resources.ApplyResources(chkWeekBossX1, "chkWeekBossX1");
            chkWeekBossX1.Name = "chkWeekBossX1";
            chkWeekBossX1.CheckedChanged += chkWeekBossX1_CheckedChanged;
            // 
            // chkWeekBossX3
            // 
            resources.ApplyResources(chkWeekBossX3, "chkWeekBossX3");
            chkWeekBossX3.Name = "chkWeekBossX3";
            chkWeekBossX3.CheckedChanged += chkWeekBossX3_CheckedChanged;
            // 
            // label9
            // 
            label9.BackColor = Color.Transparent;
            label9.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(label9, "label9");
            label9.Name = "label9";
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // chkMobEasyX3
            // 
            resources.ApplyResources(chkMobEasyX3, "chkMobEasyX3");
            chkMobEasyX3.Name = "chkMobEasyX3";
            chkMobEasyX3.CheckedChanged += chkMobEasyX3_CheckedChanged;
            // 
            // chkMobNormalX3
            // 
            resources.ApplyResources(chkMobNormalX3, "chkMobNormalX3");
            chkMobNormalX3.Name = "chkMobNormalX3";
            chkMobNormalX3.CheckedChanged += chkMobNormalX3_CheckedChanged;
            // 
            // chkMobEasyX1
            // 
            resources.ApplyResources(chkMobEasyX1, "chkMobEasyX1");
            chkMobEasyX1.Name = "chkMobEasyX1";
            chkMobEasyX1.CheckedChanged += chkMobEasyX1_CheckedChanged;
            // 
            // lblMobsStrong
            // 
            resources.ApplyResources(lblMobsStrong, "lblMobsStrong");
            lblMobsStrong.Name = "lblMobsStrong";
            // 
            // lblMobsEasy
            // 
            resources.ApplyResources(lblMobsEasy, "lblMobsEasy");
            lblMobsEasy.Name = "lblMobsEasy";
            // 
            // chkMobNormalX1
            // 
            resources.ApplyResources(chkMobNormalX1, "chkMobNormalX1");
            chkMobNormalX1.Name = "chkMobNormalX1";
            chkMobNormalX1.CheckedChanged += chkMobNormalX1_CheckedChanged;
            // 
            // chkMobStrongX3
            // 
            resources.ApplyResources(chkMobStrongX3, "chkMobStrongX3");
            chkMobStrongX3.Name = "chkMobStrongX3";
            chkMobStrongX3.CheckedChanged += chkMobStrongX3_CheckedChanged;
            // 
            // lblMobsNormal
            // 
            resources.ApplyResources(lblMobsNormal, "lblMobsNormal");
            lblMobsNormal.Name = "lblMobsNormal";
            // 
            // chkMobStrongX1
            // 
            resources.ApplyResources(chkMobStrongX1, "chkMobStrongX1");
            chkMobStrongX1.Name = "chkMobStrongX1";
            chkMobStrongX1.CheckedChanged += chkMobStrongX1_CheckedChanged;
            // 
            // grpMode
            // 
            grpMode.Controls.Add(chkExtendedSiege);
            grpMode.Controls.Add(chkSiegeOnlyMode);
            grpMode.Controls.Add(chkAlwaysMode);
            grpMode.Controls.Add(chkOnlyScratch);
            resources.ApplyResources(grpMode, "grpMode");
            grpMode.Name = "grpMode";
            grpMode.TabStop = false;
            // 
            // chkExtendedSiege
            // 
            resources.ApplyResources(chkExtendedSiege, "chkExtendedSiege");
            chkExtendedSiege.Name = "chkExtendedSiege";
            chkExtendedSiege.CheckedChanged += chkExtendedSiege_CheckedChanged;
            // 
            // chkSiegeOnlyMode
            // 
            resources.ApplyResources(chkSiegeOnlyMode, "chkSiegeOnlyMode");
            chkSiegeOnlyMode.Name = "chkSiegeOnlyMode";
            chkSiegeOnlyMode.CheckedChanged += chkSiegeOnlyMode_CheckedChanged;
            // 
            // chkAlwaysMode
            // 
            resources.ApplyResources(chkAlwaysMode, "chkAlwaysMode");
            chkAlwaysMode.Name = "chkAlwaysMode";
            chkAlwaysMode.CheckedChanged += chkAlwaysMode_CheckedChanged;
            // 
            // chkOnlyScratch
            // 
            resources.ApplyResources(chkOnlyScratch, "chkOnlyScratch");
            chkOnlyScratch.Name = "chkOnlyScratch";
            chkOnlyScratch.CheckedChanged += chkOnlyScratch_CheckedChanged;
            // 
            // grpAssistant
            // 
            grpAssistant.Controls.Add(label1);
            grpAssistant.Controls.Add(chkAssistantX3);
            grpAssistant.Controls.Add(chkAssistantX1);
            resources.ApplyResources(grpAssistant, "grpAssistant");
            grpAssistant.Name = "grpAssistant";
            grpAssistant.TabStop = false;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // chkAssistantX3
            // 
            resources.ApplyResources(chkAssistantX3, "chkAssistantX3");
            chkAssistantX3.Name = "chkAssistantX3";
            chkAssistantX3.CheckedChanged += chkAssistantX3_CheckedChanged;
            // 
            // chkAssistantX1
            // 
            resources.ApplyResources(chkAssistantX1, "chkAssistantX1");
            chkAssistantX1.Name = "chkAssistantX1";
            chkAssistantX1.CheckedChanged += chkAssistantX1_CheckedChanged;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.LightGreen;
            resources.ApplyResources(btnStart, "btnStart");
            btnStart.Name = "btnStart";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.LightCoral;
            resources.ApplyResources(btnStop, "btnStop");
            btnStop.Name = "btnStop";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // lblThreshold
            // 
            resources.ApplyResources(lblThreshold, "lblThreshold");
            lblThreshold.Name = "lblThreshold";
            // 
            // lblDelay
            // 
            resources.ApplyResources(lblDelay, "lblDelay");
            lblDelay.Name = "lblDelay";
            // 
            // lblSequence
            // 
            resources.ApplyResources(lblSequence, "lblSequence");
            lblSequence.Name = "lblSequence";
            // 
            // lblWeek
            // 
            resources.ApplyResources(lblWeek, "lblWeek");
            lblWeek.Name = "lblWeek";
            // 
            // lblServer
            // 
            resources.ApplyResources(lblServer, "lblServer");
            lblServer.Name = "lblServer";
            // 
            // lblTimer
            // 
            resources.ApplyResources(lblTimer, "lblTimer");
            lblTimer.ForeColor = Color.DarkBlue;
            lblTimer.Name = "lblTimer";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            resources.ApplyResources(tabControl1, "tabControl1");
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Tag = "";
            // 
            // tabPage1
            // 
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Controls.Add(groupBox1);
            tabPage2.Controls.Add(grpMode);
            tabPage2.Controls.Add(grpSearch);
            tabPage2.Controls.Add(lblTimer);
            tabPage2.Controls.Add(lblServer);
            tabPage2.Controls.Add(lblDelay);
            tabPage2.Controls.Add(lblWeek);
            tabPage2.Controls.Add(txtButtonSequence);
            tabPage2.Controls.Add(lblSequence);
            tabPage2.Controls.Add(numThreshold);
            tabPage2.Controls.Add(numIterationDelay);
            tabPage2.Controls.Add(lblThreshold);
            tabPage2.Controls.Add(cboWeekSelection);
            tabPage2.Controls.Add(cboServerSelection);
            tabPage2.Controls.Add(btnStop);
            tabPage2.Controls.Add(grpAssistant);
            tabPage2.Controls.Add(btnStart);
            tabPage2.Name = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(lblMobsEasy);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(chkMobStrongX1);
            groupBox1.Controls.Add(lblMobsNormal);
            groupBox1.Controls.Add(chkMobStrongX3);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(chkMobNormalX1);
            groupBox1.Controls.Add(lblMobsStrong);
            groupBox1.Controls.Add(chkMobEasyX1);
            groupBox1.Controls.Add(chkMobNormalX3);
            groupBox1.Controls.Add(chkMobEasyX3);
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(txtLog);
            resources.ApplyResources(tabPage3, "tabPage3");
            tabPage3.Name = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.Linen;
            resources.ApplyResources(txtLog, "txtLog");
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            resources.ApplyResources(menuStrip1, "menuStrip1");
            menuStrip1.Name = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimizeBox = false;
            Name = "MainForm";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)numThreshold).EndInit();
            ((System.ComponentModel.ISupportInitialize)numIterationDelay).EndInit();
            grpSearch.ResumeLayout(false);
            grpSearch.PerformLayout();
            grpMode.ResumeLayout(false);
            grpMode.PerformLayout();
            grpAssistant.ResumeLayout(false);
            grpAssistant.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage3.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private TabControl tabControl1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage1;
        private RichTextBox txtLog;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private Label label1;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private GroupBox groupBox1;
    }
}