namespace DC_Button_Finder
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Основные элементы
        private System.Windows.Forms.RichTextBox txtLog;
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

        // Labels
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.Label lblSequence;
        private System.Windows.Forms.Label lblWeek;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblLine;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtLog = new RichTextBox();
            txtButtonSequence = new TextBox();
            numThreshold = new NumericUpDown();
            numIterationDelay = new NumericUpDown();
            cboWeekSelection = new ComboBox();
            cboServerSelection = new ComboBox();
            grpSearch = new GroupBox();
            lblHiddenBoss = new Label();
            chkHiddenBossX1 = new CheckBox();
            chkHiddenBossX3 = new CheckBox();
            lblWeekBoss = new Label();
            chkWeekBossX1 = new CheckBox();
            chkWeekBossX3 = new CheckBox();
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
            chkAssistantX3 = new CheckBox();
            chkAssistantX1 = new CheckBox();
            btnStart = new Button();
            btnStop = new Button();
            lblStatus = new Label();
            lblThreshold = new Label();
            lblDelay = new Label();
            lblSequence = new Label();
            lblWeek = new Label();
            lblServer = new Label();
            lblTimer = new Label();
            lblLine = new Label();
            ((System.ComponentModel.ISupportInitialize)numThreshold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numIterationDelay).BeginInit();
            grpSearch.SuspendLayout();
            grpMode.SuspendLayout();
            grpAssistant.SuspendLayout();
            SuspendLayout();
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 300);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(776, 331);
            txtLog.TabIndex = 0;
            txtLog.Text = "";
            // 
            // txtButtonSequence
            // 
            txtButtonSequence.Location = new Point(123, 56);
            txtButtonSequence.Name = "txtButtonSequence";
            txtButtonSequence.Size = new Size(665, 27);
            txtButtonSequence.TabIndex = 1;
            // 
            // numThreshold
            // 
            numThreshold.Location = new Point(123, 170);
            numThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numThreshold.Name = "numThreshold";
            numThreshold.Size = new Size(64, 27);
            numThreshold.TabIndex = 2;
            numThreshold.Value = new decimal(new int[] { 80, 0, 0, 0 });
            // 
            // numIterationDelay
            // 
            numIterationDelay.Location = new Point(123, 209);
            numIterationDelay.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numIterationDelay.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numIterationDelay.Name = "numIterationDelay";
            numIterationDelay.Size = new Size(64, 27);
            numIterationDelay.TabIndex = 3;
            numIterationDelay.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // cboWeekSelection
            // 
            cboWeekSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            cboWeekSelection.FormattingEnabled = true;
            cboWeekSelection.Location = new Point(123, 92);
            cboWeekSelection.Name = "cboWeekSelection";
            cboWeekSelection.Size = new Size(552, 28);
            cboWeekSelection.TabIndex = 4;
            cboWeekSelection.SelectedIndexChanged += cboWeekSelection_SelectedIndexChanged;
            // 
            // cboServerSelection
            // 
            cboServerSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            cboServerSelection.FormattingEnabled = true;
            cboServerSelection.Location = new Point(123, 131);
            cboServerSelection.Name = "cboServerSelection";
            cboServerSelection.Size = new Size(552, 28);
            cboServerSelection.TabIndex = 5;
            cboServerSelection.SelectedIndexChanged += cboServerSelection_SelectedIndexChanged;
            // 
            // grpSearch
            // 
            grpSearch.Controls.Add(lblHiddenBoss);
            grpSearch.Controls.Add(chkHiddenBossX1);
            grpSearch.Controls.Add(chkHiddenBossX3);
            grpSearch.Controls.Add(lblWeekBoss);
            grpSearch.Controls.Add(chkWeekBossX1);
            grpSearch.Controls.Add(chkWeekBossX3);
            grpSearch.Controls.Add(chkMobEasyX3);
            grpSearch.Controls.Add(chkMobNormalX3);
            grpSearch.Controls.Add(chkMobEasyX1);
            grpSearch.Controls.Add(lblMobsStrong);
            grpSearch.Controls.Add(lblMobsEasy);
            grpSearch.Controls.Add(chkMobNormalX1);
            grpSearch.Controls.Add(chkMobStrongX3);
            grpSearch.Controls.Add(lblMobsNormal);
            grpSearch.Controls.Add(chkMobStrongX1);
            grpSearch.Location = new Point(193, 167);
            grpSearch.Name = "grpSearch";
            grpSearch.Size = new Size(390, 127);
            grpSearch.TabIndex = 6;
            grpSearch.TabStop = false;
            grpSearch.Text = "Поиск";
            // 
            // lblHiddenBoss
            // 
            lblHiddenBoss.AutoSize = true;
            lblHiddenBoss.FlatStyle = FlatStyle.Flat;
            lblHiddenBoss.Font = new Font("Arial Narrow", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblHiddenBoss.Location = new Point(37, 21);
            lblHiddenBoss.Name = "lblHiddenBoss";
            lblHiddenBoss.Size = new Size(135, 31);
            lblHiddenBoss.TabIndex = 0;
            lblHiddenBoss.Text = "Hidden boss";
            // 
            // chkHiddenBossX1
            // 
            chkHiddenBossX1.FlatStyle = FlatStyle.System;
            chkHiddenBossX1.Font = new Font("Segoe UI", 9F);
            chkHiddenBossX1.Location = new Point(5, 21);
            chkHiddenBossX1.Name = "chkHiddenBossX1";
            chkHiddenBossX1.Size = new Size(37, 18);
            chkHiddenBossX1.TabIndex = 1;
            chkHiddenBossX1.Text = "x1";
            chkHiddenBossX1.CheckedChanged += chkHiddenBossX1_CheckedChanged;
            // 
            // chkHiddenBossX3
            // 
            chkHiddenBossX3.AutoSize = true;
            chkHiddenBossX3.FlatStyle = FlatStyle.System;
            chkHiddenBossX3.Font = new Font("Segoe UI", 9F);
            chkHiddenBossX3.Location = new Point(5, 37);
            chkHiddenBossX3.Name = "chkHiddenBossX3";
            chkHiddenBossX3.Size = new Size(43, 20);
            chkHiddenBossX3.TabIndex = 2;
            chkHiddenBossX3.Text = "x3";
            chkHiddenBossX3.CheckedChanged += chkHiddenBossX3_CheckedChanged;
            // 
            // lblWeekBoss
            // 
            lblWeekBoss.AutoSize = true;
            lblWeekBoss.Font = new Font("Arial Narrow", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblWeekBoss.Location = new Point(37, 74);
            lblWeekBoss.Name = "lblWeekBoss";
            lblWeekBoss.Size = new Size(122, 31);
            lblWeekBoss.TabIndex = 3;
            lblWeekBoss.Text = "Week boss";
            lblWeekBoss.Click += lblWeekBoss_Click;
            // 
            // chkWeekBossX1
            // 
            chkWeekBossX1.FlatStyle = FlatStyle.System;
            chkWeekBossX1.Font = new Font("Segoe UI", 9F);
            chkWeekBossX1.Location = new Point(5, 73);
            chkWeekBossX1.Name = "chkWeekBossX1";
            chkWeekBossX1.Size = new Size(37, 18);
            chkWeekBossX1.TabIndex = 4;
            chkWeekBossX1.Text = "x1";
            chkWeekBossX1.CheckedChanged += chkWeekBossX1_CheckedChanged;
            // 
            // chkWeekBossX3
            // 
            chkWeekBossX3.FlatStyle = FlatStyle.System;
            chkWeekBossX3.Font = new Font("Segoe UI", 9F);
            chkWeekBossX3.Location = new Point(5, 89);
            chkWeekBossX3.Name = "chkWeekBossX3";
            chkWeekBossX3.Size = new Size(40, 18);
            chkWeekBossX3.TabIndex = 5;
            chkWeekBossX3.Text = "x3";
            chkWeekBossX3.CheckedChanged += chkWeekBossX3_CheckedChanged;
            // 
            // chkMobEasyX3
            // 
            chkMobEasyX3.FlatStyle = FlatStyle.System;
            chkMobEasyX3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            chkMobEasyX3.Location = new Point(207, 29);
            chkMobEasyX3.Name = "chkMobEasyX3";
            chkMobEasyX3.Size = new Size(32, 15);
            chkMobEasyX3.TabIndex = 8;
            chkMobEasyX3.Text = "x3";
            chkMobEasyX3.CheckedChanged += chkMobEasyX3_CheckedChanged;
            // 
            // chkMobNormalX3
            // 
            chkMobNormalX3.FlatStyle = FlatStyle.System;
            chkMobNormalX3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            chkMobNormalX3.Location = new Point(207, 67);
            chkMobNormalX3.Name = "chkMobNormalX3";
            chkMobNormalX3.Size = new Size(29, 15);
            chkMobNormalX3.TabIndex = 11;
            chkMobNormalX3.Text = "x3";
            chkMobNormalX3.CheckedChanged += chkMobNormalX3_CheckedChanged;
            // 
            // chkMobEasyX1
            // 
            chkMobEasyX1.FlatStyle = FlatStyle.System;
            chkMobEasyX1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            chkMobEasyX1.Location = new Point(207, 16);
            chkMobEasyX1.Name = "chkMobEasyX1";
            chkMobEasyX1.Size = new Size(32, 14);
            chkMobEasyX1.TabIndex = 7;
            chkMobEasyX1.Text = "x1";
            chkMobEasyX1.CheckedChanged += chkMobEasyX1_CheckedChanged;
            // 
            // lblMobsStrong
            // 
            lblMobsStrong.AutoSize = true;
            lblMobsStrong.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblMobsStrong.Location = new Point(245, 94);
            lblMobsStrong.Name = "lblMobsStrong";
            lblMobsStrong.Size = new Size(119, 22);
            lblMobsStrong.TabIndex = 12;
            lblMobsStrong.Text = "Strong mobs";
            lblMobsStrong.Click += lblMobsStrong_Click;
            // 
            // lblMobsEasy
            // 
            lblMobsEasy.AutoSize = true;
            lblMobsEasy.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblMobsEasy.Location = new Point(245, 20);
            lblMobsEasy.Name = "lblMobsEasy";
            lblMobsEasy.Size = new Size(105, 22);
            lblMobsEasy.TabIndex = 6;
            lblMobsEasy.Text = "Easy mobs";
            // 
            // chkMobNormalX1
            // 
            chkMobNormalX1.FlatStyle = FlatStyle.System;
            chkMobNormalX1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            chkMobNormalX1.Location = new Point(207, 52);
            chkMobNormalX1.Name = "chkMobNormalX1";
            chkMobNormalX1.Size = new Size(32, 17);
            chkMobNormalX1.TabIndex = 10;
            chkMobNormalX1.Text = "x1";
            chkMobNormalX1.CheckedChanged += chkMobNormalX1_CheckedChanged;
            // 
            // chkMobStrongX3
            // 
            chkMobStrongX3.FlatStyle = FlatStyle.System;
            chkMobStrongX3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            chkMobStrongX3.Location = new Point(207, 104);
            chkMobStrongX3.Name = "chkMobStrongX3";
            chkMobStrongX3.Size = new Size(29, 18);
            chkMobStrongX3.TabIndex = 14;
            chkMobStrongX3.Text = "x3";
            chkMobStrongX3.CheckedChanged += chkMobStrongX3_CheckedChanged;
            // 
            // lblMobsNormal
            // 
            lblMobsNormal.AutoSize = true;
            lblMobsNormal.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblMobsNormal.Location = new Point(245, 56);
            lblMobsNormal.Name = "lblMobsNormal";
            lblMobsNormal.Size = new Size(123, 22);
            lblMobsNormal.TabIndex = 9;
            lblMobsNormal.Text = "Normal mobs";
            // 
            // chkMobStrongX1
            // 
            chkMobStrongX1.FlatStyle = FlatStyle.System;
            chkMobStrongX1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            chkMobStrongX1.Location = new Point(207, 89);
            chkMobStrongX1.Name = "chkMobStrongX1";
            chkMobStrongX1.Size = new Size(37, 17);
            chkMobStrongX1.TabIndex = 13;
            chkMobStrongX1.Text = "x1";
            chkMobStrongX1.CheckedChanged += chkMobStrongX1_CheckedChanged;
            // 
            // grpMode
            // 
            grpMode.Controls.Add(chkExtendedSiege);
            grpMode.Controls.Add(chkSiegeOnlyMode);
            grpMode.Controls.Add(chkAlwaysMode);
            grpMode.Controls.Add(chkOnlyScratch);
            grpMode.Location = new Point(589, 172);
            grpMode.Name = "grpMode";
            grpMode.Size = new Size(199, 122);
            grpMode.TabIndex = 7;
            grpMode.TabStop = false;
            grpMode.Text = "Режимы работы";
            // 
            // chkExtendedSiege
            // 
            chkExtendedSiege.AutoSize = true;
            chkExtendedSiege.Location = new Point(6, 70);
            chkExtendedSiege.Name = "chkExtendedSiege";
            chkExtendedSiege.Size = new Size(127, 24);
            chkExtendedSiege.TabIndex = 2;
            chkExtendedSiege.Text = "Расширенный";
            chkExtendedSiege.CheckedChanged += chkExtendedSiege_CheckedChanged;
            // 
            // chkSiegeOnlyMode
            // 
            chkSiegeOnlyMode.AutoSize = true;
            chkSiegeOnlyMode.Location = new Point(6, 45);
            chkSiegeOnlyMode.Name = "chkSiegeOnlyMode";
            chkSiegeOnlyMode.Size = new Size(124, 24);
            chkSiegeOnlyMode.TabIndex = 1;
            chkSiegeOnlyMode.Text = "Только осады";
            chkSiegeOnlyMode.CheckedChanged += chkSiegeOnlyMode_CheckedChanged;
            // 
            // chkAlwaysMode
            // 
            chkAlwaysMode.AutoSize = true;
            chkAlwaysMode.Location = new Point(6, 20);
            chkAlwaysMode.Name = "chkAlwaysMode";
            chkAlwaysMode.Size = new Size(74, 24);
            chkAlwaysMode.TabIndex = 0;
            chkAlwaysMode.Text = "Всегда";
            chkAlwaysMode.CheckedChanged += chkAlwaysMode_CheckedChanged;
            // 
            // chkOnlyScratch
            // 
            chkOnlyScratch.AutoSize = true;
            chkOnlyScratch.Location = new Point(6, 95);
            chkOnlyScratch.Name = "chkOnlyScratch";
            chkOnlyScratch.Size = new Size(146, 24);
            chkOnlyScratch.TabIndex = 3;
            chkOnlyScratch.Text = "Только царапать";
            chkOnlyScratch.CheckedChanged += chkOnlyScratch_CheckedChanged;
            // 
            // grpAssistant
            // 
            grpAssistant.Controls.Add(chkAssistantX3);
            grpAssistant.Controls.Add(chkAssistantX1);
            grpAssistant.Location = new Point(691, 95);
            grpAssistant.Name = "grpAssistant";
            grpAssistant.Size = new Size(92, 64);
            grpAssistant.TabIndex = 19;
            grpAssistant.TabStop = false;
            grpAssistant.Text = "Ассистент";
            // 
            // chkAssistantX3
            // 
            chkAssistantX3.AutoSize = true;
            chkAssistantX3.Location = new Point(49, 26);
            chkAssistantX3.Name = "chkAssistantX3";
            chkAssistantX3.Size = new Size(43, 24);
            chkAssistantX3.TabIndex = 1;
            chkAssistantX3.Text = "x3";
            chkAssistantX3.CheckedChanged += chkAssistantX3_CheckedChanged;
            // 
            // chkAssistantX1
            // 
            chkAssistantX1.AutoSize = true;
            chkAssistantX1.Location = new Point(6, 26);
            chkAssistantX1.Name = "chkAssistantX1";
            chkAssistantX1.Size = new Size(43, 24);
            chkAssistantX1.TabIndex = 0;
            chkAssistantX1.Text = "x1";
            chkAssistantX1.CheckedChanged += chkAssistantX1_CheckedChanged;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.LightGreen;
            btnStart.Location = new Point(603, 14);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(92, 40);
            btnStart.TabIndex = 9;
            btnStart.Text = "СТАРТ (F1)";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.LightCoral;
            btnStop.Enabled = false;
            btnStop.Location = new Point(701, 14);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(90, 40);
            btnStop.TabIndex = 10;
            btnStop.Text = "СТОП (F2)";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblStatus.ForeColor = Color.DarkRed;
            lblStatus.Location = new Point(603, 1);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(84, 13);
            lblStatus.TabIndex = 11;
            lblStatus.Text = "СТАТУС: ВЫКЛ";
            // 
            // lblThreshold
            // 
            lblThreshold.AutoSize = true;
            lblThreshold.Location = new Point(13, 172);
            lblThreshold.Name = "lblThreshold";
            lblThreshold.Size = new Size(102, 20);
            lblThreshold.TabIndex = 12;
            lblThreshold.Text = "Точность (%):";
            // 
            // lblDelay
            // 
            lblDelay.AutoSize = true;
            lblDelay.Location = new Point(11, 211);
            lblDelay.Name = "lblDelay";
            lblDelay.Size = new Size(111, 20);
            lblDelay.TabIndex = 13;
            lblDelay.Text = "Задержка (мс):";
            // 
            // lblSequence
            // 
            lblSequence.AutoSize = true;
            lblSequence.Location = new Point(11, 59);
            lblSequence.Name = "lblSequence";
            lblSequence.Size = new Size(103, 20);
            lblSequence.TabIndex = 14;
            lblSequence.Text = "Очерёдность:";
            // 
            // lblWeek
            // 
            lblWeek.AutoSize = true;
            lblWeek.Location = new Point(11, 95);
            lblWeek.Name = "lblWeek";
            lblWeek.Size = new Size(63, 20);
            lblWeek.TabIndex = 15;
            lblWeek.Text = "Неделя:";
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(13, 134);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(63, 20);
            lblServer.TabIndex = 16;
            lblServer.Text = "Сервер:";
            // 
            // lblTimer
            // 
            lblTimer.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTimer.ForeColor = Color.DarkBlue;
            lblTimer.Location = new Point(13, 7);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(282, 42);
            lblTimer.TabIndex = 17;
            lblTimer.Text = "До осады: --:--:--";
            // 
            // lblLine
            // 
            lblLine.BorderStyle = BorderStyle.Fixed3D;
            lblLine.Location = new Point(13, 297);
            lblLine.Name = "lblLine";
            lblLine.Size = new Size(776, 10);
            lblLine.TabIndex = 18;
            // 
            // MainForm
            // 
            ClientSize = new Size(795, 643);
            Controls.Add(lblLine);
            Controls.Add(lblTimer);
            Controls.Add(lblServer);
            Controls.Add(lblWeek);
            Controls.Add(lblSequence);
            Controls.Add(lblDelay);
            Controls.Add(lblThreshold);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(grpAssistant);
            Controls.Add(grpMode);
            Controls.Add(grpSearch);
            Controls.Add(cboServerSelection);
            Controls.Add(cboWeekSelection);
            Controls.Add(numIterationDelay);
            Controls.Add(numThreshold);
            Controls.Add(txtButtonSequence);
            Controls.Add(txtLog);
            Name = "MainForm";
            Text = "DC Button Finder Bot";
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
            ResumeLayout(false);
            PerformLayout();
        }
    }
}