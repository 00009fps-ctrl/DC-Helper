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
            panel1 = new Panel();
            panel19 = new Panel();
            textBox18 = new TextBox();
            checkBox18 = new CheckBox();
            panel18 = new Panel();
            textBox17 = new TextBox();
            checkBox17 = new CheckBox();
            panel17 = new Panel();
            textBox16 = new TextBox();
            checkBox16 = new CheckBox();
            panel16 = new Panel();
            textBox15 = new TextBox();
            checkBox15 = new CheckBox();
            panel15 = new Panel();
            textBox14 = new TextBox();
            checkBox14 = new CheckBox();
            panel14 = new Panel();
            textBox13 = new TextBox();
            checkBox13 = new CheckBox();
            panel13 = new Panel();
            textBox12 = new TextBox();
            checkBox12 = new CheckBox();
            panel12 = new Panel();
            textBox11 = new TextBox();
            checkBox11 = new CheckBox();
            panel11 = new Panel();
            textBox10 = new TextBox();
            checkBox10 = new CheckBox();
            panel10 = new Panel();
            textBox9 = new TextBox();
            checkBox9 = new CheckBox();
            panel9 = new Panel();
            textBox8 = new TextBox();
            checkBox8 = new CheckBox();
            panel8 = new Panel();
            textBox7 = new TextBox();
            checkBox7 = new CheckBox();
            panel7 = new Panel();
            textBox6 = new TextBox();
            checkBox6 = new CheckBox();
            panel6 = new Panel();
            textBox5 = new TextBox();
            checkBox5 = new CheckBox();
            panel5 = new Panel();
            textBox4 = new TextBox();
            checkBox4 = new CheckBox();
            panel4 = new Panel();
            textBox3 = new TextBox();
            checkBox3 = new CheckBox();
            panel3 = new Panel();
            textBox2 = new TextBox();
            checkBox2 = new CheckBox();
            label3 = new Label();
            label2 = new Label();
            panel2 = new Panel();
            textBox1 = new TextBox();
            checkBox1 = new CheckBox();
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
            tabPage1.SuspendLayout();
            panel1.SuspendLayout();
            panel19.SuspendLayout();
            panel18.SuspendLayout();
            panel17.SuspendLayout();
            panel16.SuspendLayout();
            panel15.SuspendLayout();
            panel14.SuspendLayout();
            panel13.SuspendLayout();
            panel12.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            panel9.SuspendLayout();
            panel8.SuspendLayout();
            panel7.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage3.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtButtonSequence
            // 
            txtButtonSequence.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtButtonSequence.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtButtonSequence.Location = new Point(118, 111);
            txtButtonSequence.Name = "txtButtonSequence";
            txtButtonSequence.Size = new Size(648, 27);
            txtButtonSequence.TabIndex = 1;
            // 
            // numThreshold
            // 
            numThreshold.DecimalPlaces = 2;
            numThreshold.Location = new Point(123, 8);
            numThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numThreshold.Name = "numThreshold";
            numThreshold.Size = new Size(64, 27);
            numThreshold.TabIndex = 2;
            numThreshold.Value = new decimal(new int[] { 80, 0, 0, 0 });
            // 
            // numIterationDelay
            // 
            numIterationDelay.Location = new Point(123, 45);
            numIterationDelay.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numIterationDelay.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numIterationDelay.Name = "numIterationDelay";
            numIterationDelay.Size = new Size(64, 27);
            numIterationDelay.TabIndex = 3;
            numIterationDelay.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // cboWeekSelection
            // 
            cboWeekSelection.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboWeekSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            cboWeekSelection.FormattingEnabled = true;
            cboWeekSelection.Location = new Point(118, 153);
            cboWeekSelection.Name = "cboWeekSelection";
            cboWeekSelection.Size = new Size(648, 28);
            cboWeekSelection.TabIndex = 4;
            cboWeekSelection.SelectedIndexChanged += cboWeekSelection_SelectedIndexChanged;
            // 
            // cboServerSelection
            // 
            cboServerSelection.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboServerSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            cboServerSelection.FormattingEnabled = true;
            cboServerSelection.Location = new Point(118, 192);
            cboServerSelection.Name = "cboServerSelection";
            cboServerSelection.Size = new Size(648, 28);
            cboServerSelection.TabIndex = 5;
            cboServerSelection.SelectedIndexChanged += cboServerSelection_SelectedIndexChanged;
            // 
            // grpSearch
            // 
            grpSearch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpSearch.Controls.Add(label7);
            grpSearch.Controls.Add(label6);
            grpSearch.Controls.Add(lblHiddenBoss);
            grpSearch.Controls.Add(chkHiddenBossX1);
            grpSearch.Controls.Add(chkHiddenBossX3);
            grpSearch.Controls.Add(lblWeekBoss);
            grpSearch.Controls.Add(chkWeekBossX1);
            grpSearch.Controls.Add(chkWeekBossX3);
            grpSearch.Location = new Point(6, 238);
            grpSearch.Name = "grpSearch";
            grpSearch.Size = new Size(178, 225);
            grpSearch.TabIndex = 6;
            grpSearch.TabStop = false;
            grpSearch.Text = "Поиск боссов";
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.BorderStyle = BorderStyle.FixedSingle;
            label7.Location = new Point(39, 26);
            label7.Name = "label7";
            label7.Size = new Size(2, 37);
            label7.TabIndex = 24;
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.BorderStyle = BorderStyle.FixedSingle;
            label6.Location = new Point(39, 98);
            label6.Name = "label6";
            label6.Size = new Size(2, 37);
            label6.TabIndex = 23;
            // 
            // lblHiddenBoss
            // 
            lblHiddenBoss.AutoSize = true;
            lblHiddenBoss.FlatStyle = FlatStyle.Flat;
            lblHiddenBoss.Font = new Font("Times New Roman", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblHiddenBoss.ForeColor = Color.DarkRed;
            lblHiddenBoss.Location = new Point(43, 23);
            lblHiddenBoss.Name = "lblHiddenBoss";
            lblHiddenBoss.Size = new Size(93, 44);
            lblHiddenBoss.TabIndex = 0;
            lblHiddenBoss.Text = "Скрытые\r\nбоссы";
            lblHiddenBoss.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkHiddenBossX1
            // 
            chkHiddenBossX1.Anchor = AnchorStyles.Left;
            chkHiddenBossX1.FlatStyle = FlatStyle.System;
            chkHiddenBossX1.Font = new Font("Tahoma", 9F);
            chkHiddenBossX1.Location = new Point(5, 26);
            chkHiddenBossX1.Name = "chkHiddenBossX1";
            chkHiddenBossX1.Size = new Size(30, 20);
            chkHiddenBossX1.TabIndex = 1;
            chkHiddenBossX1.Text = "x1";
            chkHiddenBossX1.CheckedChanged += chkHiddenBossX1_CheckedChanged;
            // 
            // chkHiddenBossX3
            // 
            chkHiddenBossX3.Anchor = AnchorStyles.Left;
            chkHiddenBossX3.FlatStyle = FlatStyle.System;
            chkHiddenBossX3.Font = new Font("Tahoma", 9F);
            chkHiddenBossX3.Location = new Point(5, 46);
            chkHiddenBossX3.Name = "chkHiddenBossX3";
            chkHiddenBossX3.Size = new Size(30, 20);
            chkHiddenBossX3.TabIndex = 2;
            chkHiddenBossX3.Text = "x3";
            chkHiddenBossX3.CheckedChanged += chkHiddenBossX3_CheckedChanged;
            // 
            // lblWeekBoss
            // 
            lblWeekBoss.AutoSize = true;
            lblWeekBoss.Font = new Font("Times New Roman", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblWeekBoss.ForeColor = Color.DarkRed;
            lblWeekBoss.Location = new Point(43, 91);
            lblWeekBoss.Name = "lblWeekBoss";
            lblWeekBoss.Size = new Size(108, 44);
            lblWeekBoss.TabIndex = 3;
            lblWeekBoss.Text = "Недельные\r\nбоссы";
            lblWeekBoss.TextAlign = ContentAlignment.MiddleLeft;
            lblWeekBoss.Click += lblWeekBoss_Click;
            // 
            // chkWeekBossX1
            // 
            chkWeekBossX1.Anchor = AnchorStyles.Left;
            chkWeekBossX1.FlatStyle = FlatStyle.System;
            chkWeekBossX1.Font = new Font("Tahoma", 9F);
            chkWeekBossX1.Location = new Point(5, 95);
            chkWeekBossX1.Name = "chkWeekBossX1";
            chkWeekBossX1.Size = new Size(30, 20);
            chkWeekBossX1.TabIndex = 4;
            chkWeekBossX1.Text = "x1";
            chkWeekBossX1.CheckedChanged += chkWeekBossX1_CheckedChanged;
            // 
            // chkWeekBossX3
            // 
            chkWeekBossX3.Anchor = AnchorStyles.Left;
            chkWeekBossX3.FlatStyle = FlatStyle.System;
            chkWeekBossX3.Font = new Font("Tahoma", 9F);
            chkWeekBossX3.Location = new Point(5, 115);
            chkWeekBossX3.Name = "chkWeekBossX3";
            chkWeekBossX3.Size = new Size(30, 20);
            chkWeekBossX3.TabIndex = 5;
            chkWeekBossX3.Text = "x3";
            chkWeekBossX3.CheckedChanged += chkWeekBossX3_CheckedChanged;
            // 
            // label9
            // 
            label9.BackColor = Color.Transparent;
            label9.BorderStyle = BorderStyle.FixedSingle;
            label9.Location = new Point(52, 98);
            label9.Name = "label9";
            label9.Size = new Size(2, 37);
            label9.TabIndex = 26;
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.BorderStyle = BorderStyle.FixedSingle;
            label8.Location = new Point(52, 27);
            label8.Name = "label8";
            label8.Size = new Size(2, 37);
            label8.TabIndex = 25;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.BorderStyle = BorderStyle.FixedSingle;
            label5.Location = new Point(52, 164);
            label5.Name = "label5";
            label5.Size = new Size(2, 37);
            label5.TabIndex = 22;
            // 
            // chkMobEasyX3
            // 
            chkMobEasyX3.FlatStyle = FlatStyle.System;
            chkMobEasyX3.Font = new Font("Tahoma", 9F);
            chkMobEasyX3.Location = new Point(16, 47);
            chkMobEasyX3.Name = "chkMobEasyX3";
            chkMobEasyX3.Size = new Size(30, 20);
            chkMobEasyX3.TabIndex = 8;
            chkMobEasyX3.Text = "x3";
            chkMobEasyX3.CheckedChanged += chkMobEasyX3_CheckedChanged;
            // 
            // chkMobNormalX3
            // 
            chkMobNormalX3.FlatStyle = FlatStyle.System;
            chkMobNormalX3.Font = new Font("Tahoma", 9F);
            chkMobNormalX3.Location = new Point(16, 116);
            chkMobNormalX3.Name = "chkMobNormalX3";
            chkMobNormalX3.Size = new Size(30, 20);
            chkMobNormalX3.TabIndex = 11;
            chkMobNormalX3.Text = "x3";
            chkMobNormalX3.CheckedChanged += chkMobNormalX3_CheckedChanged;
            // 
            // chkMobEasyX1
            // 
            chkMobEasyX1.FlatStyle = FlatStyle.System;
            chkMobEasyX1.Font = new Font("Tahoma", 9F);
            chkMobEasyX1.Location = new Point(16, 27);
            chkMobEasyX1.Name = "chkMobEasyX1";
            chkMobEasyX1.Size = new Size(30, 20);
            chkMobEasyX1.TabIndex = 7;
            chkMobEasyX1.Text = "x1";
            chkMobEasyX1.CheckedChanged += chkMobEasyX1_CheckedChanged;
            // 
            // lblMobsStrong
            // 
            lblMobsStrong.AutoSize = true;
            lblMobsStrong.Font = new Font("Times New Roman", 14.25F);
            lblMobsStrong.Location = new Point(56, 160);
            lblMobsStrong.Name = "lblMobsStrong";
            lblMobsStrong.Size = new Size(82, 42);
            lblMobsStrong.TabIndex = 12;
            lblMobsStrong.Text = "Сильные\r\nмобы";
            lblMobsStrong.TextAlign = ContentAlignment.MiddleLeft;
            lblMobsStrong.Click += lblMobsStrong_Click;
            // 
            // lblMobsEasy
            // 
            lblMobsEasy.AutoSize = true;
            lblMobsEasy.Font = new Font("Times New Roman", 14.25F);
            lblMobsEasy.Location = new Point(56, 26);
            lblMobsEasy.Name = "lblMobsEasy";
            lblMobsEasy.Size = new Size(71, 42);
            lblMobsEasy.TabIndex = 6;
            lblMobsEasy.Text = "Слабые\r\nмобы\r\n";
            lblMobsEasy.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkMobNormalX1
            // 
            chkMobNormalX1.FlatStyle = FlatStyle.System;
            chkMobNormalX1.Font = new Font("Tahoma", 9F);
            chkMobNormalX1.Location = new Point(16, 96);
            chkMobNormalX1.Name = "chkMobNormalX1";
            chkMobNormalX1.Size = new Size(30, 20);
            chkMobNormalX1.TabIndex = 10;
            chkMobNormalX1.Text = "x1";
            chkMobNormalX1.CheckedChanged += chkMobNormalX1_CheckedChanged;
            // 
            // chkMobStrongX3
            // 
            chkMobStrongX3.FlatStyle = FlatStyle.System;
            chkMobStrongX3.Font = new Font("Tahoma", 9F);
            chkMobStrongX3.Location = new Point(16, 182);
            chkMobStrongX3.Name = "chkMobStrongX3";
            chkMobStrongX3.Size = new Size(30, 20);
            chkMobStrongX3.TabIndex = 14;
            chkMobStrongX3.Text = "x3";
            chkMobStrongX3.CheckedChanged += chkMobStrongX3_CheckedChanged;
            // 
            // lblMobsNormal
            // 
            lblMobsNormal.AutoSize = true;
            lblMobsNormal.Font = new Font("Times New Roman", 14.25F);
            lblMobsNormal.Location = new Point(56, 95);
            lblMobsNormal.Name = "lblMobsNormal";
            lblMobsNormal.Size = new Size(79, 42);
            lblMobsNormal.TabIndex = 9;
            lblMobsNormal.Text = "Средние\r\nмобы";
            lblMobsNormal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkMobStrongX1
            // 
            chkMobStrongX1.FlatStyle = FlatStyle.System;
            chkMobStrongX1.Font = new Font("Tahoma", 9F);
            chkMobStrongX1.Location = new Point(16, 162);
            chkMobStrongX1.Name = "chkMobStrongX1";
            chkMobStrongX1.Size = new Size(30, 20);
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
            grpMode.Location = new Point(426, 238);
            grpMode.Name = "grpMode";
            grpMode.Size = new Size(192, 225);
            grpMode.TabIndex = 7;
            grpMode.TabStop = false;
            grpMode.Text = "Режимы работы";
            // 
            // chkExtendedSiege
            // 
            chkExtendedSiege.AutoSize = true;
            chkExtendedSiege.Location = new Point(6, 60);
            chkExtendedSiege.Name = "chkExtendedSiege";
            chkExtendedSiege.Size = new Size(173, 24);
            chkExtendedSiege.TabIndex = 2;
            chkExtendedSiege.Text = "Расширенные осады";
            chkExtendedSiege.CheckedChanged += chkExtendedSiege_CheckedChanged;
            // 
            // chkSiegeOnlyMode
            // 
            chkSiegeOnlyMode.AutoSize = true;
            chkSiegeOnlyMode.Location = new Point(6, 40);
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
            chkOnlyScratch.Location = new Point(6, 80);
            chkOnlyScratch.Name = "chkOnlyScratch";
            chkOnlyScratch.Size = new Size(146, 24);
            chkOnlyScratch.TabIndex = 3;
            chkOnlyScratch.Text = "Только царапать";
            chkOnlyScratch.CheckedChanged += chkOnlyScratch_CheckedChanged;
            // 
            // grpAssistant
            // 
            grpAssistant.Controls.Add(label1);
            grpAssistant.Controls.Add(chkAssistantX3);
            grpAssistant.Controls.Add(chkAssistantX1);
            grpAssistant.Location = new Point(627, 238);
            grpAssistant.Name = "grpAssistant";
            grpAssistant.Size = new Size(139, 76);
            grpAssistant.TabIndex = 19;
            grpAssistant.TabStop = false;
            grpAssistant.Text = "Ассистент (beta)";
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(68, 21);
            label1.Name = "label1";
            label1.Size = new Size(1, 45);
            label1.TabIndex = 20;
            // 
            // chkAssistantX3
            // 
            chkAssistantX3.AutoSize = true;
            chkAssistantX3.Location = new Point(84, 30);
            chkAssistantX3.Name = "chkAssistantX3";
            chkAssistantX3.Size = new Size(43, 24);
            chkAssistantX3.TabIndex = 1;
            chkAssistantX3.Text = "x3";
            chkAssistantX3.CheckedChanged += chkAssistantX3_CheckedChanged;
            // 
            // chkAssistantX1
            // 
            chkAssistantX1.AutoSize = true;
            chkAssistantX1.Location = new Point(19, 30);
            chkAssistantX1.Name = "chkAssistantX1";
            chkAssistantX1.Size = new Size(43, 24);
            chkAssistantX1.TabIndex = 0;
            chkAssistantX1.Text = "x1";
            chkAssistantX1.CheckedChanged += chkAssistantX1_CheckedChanged;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.LightGreen;
            btnStart.Location = new Point(674, 0);
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
            btnStop.Location = new Point(674, 41);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(90, 40);
            btnStop.TabIndex = 10;
            btnStop.Text = "СТОП (F2)";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // lblThreshold
            // 
            lblThreshold.AutoSize = true;
            lblThreshold.Location = new Point(11, 10);
            lblThreshold.Name = "lblThreshold";
            lblThreshold.Size = new Size(102, 20);
            lblThreshold.TabIndex = 12;
            lblThreshold.Text = "Точность (%):";
            // 
            // lblDelay
            // 
            lblDelay.AutoSize = true;
            lblDelay.Location = new Point(12, 47);
            lblDelay.Name = "lblDelay";
            lblDelay.Size = new Size(111, 20);
            lblDelay.TabIndex = 13;
            lblDelay.Text = "Задержка (мс):";
            // 
            // lblSequence
            // 
            lblSequence.AutoSize = true;
            lblSequence.Location = new Point(6, 114);
            lblSequence.Name = "lblSequence";
            lblSequence.Size = new Size(103, 20);
            lblSequence.TabIndex = 14;
            lblSequence.Text = "Очерёдность:";
            // 
            // lblWeek
            // 
            lblWeek.AutoSize = true;
            lblWeek.Location = new Point(6, 156);
            lblWeek.Name = "lblWeek";
            lblWeek.Size = new Size(63, 20);
            lblWeek.TabIndex = 15;
            lblWeek.Text = "Неделя:";
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(8, 195);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(63, 20);
            lblServer.TabIndex = 16;
            lblServer.Text = "Сервер:";
            // 
            // lblTimer
            // 
            lblTimer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTimer.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblTimer.ForeColor = Color.DarkBlue;
            lblTimer.Location = new Point(193, 3);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(475, 68);
            lblTimer.TabIndex = 17;
            lblTimer.Text = "До осады: --:--:--";
            lblTimer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 28);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(780, 779);
            tabControl1.TabIndex = 20;
            tabControl1.Tag = "";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(772, 746);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "Сервер инфо";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.AllowDrop = true;
            panel1.AutoScroll = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.BackgroundImageLayout = ImageLayout.Zoom;
            panel1.Controls.Add(panel19);
            panel1.Controls.Add(panel18);
            panel1.Controls.Add(panel17);
            panel1.Controls.Add(panel16);
            panel1.Controls.Add(panel15);
            panel1.Controls.Add(panel14);
            panel1.Controls.Add(panel13);
            panel1.Controls.Add(panel12);
            panel1.Controls.Add(panel11);
            panel1.Controls.Add(panel10);
            panel1.Controls.Add(panel9);
            panel1.Controls.Add(panel8);
            panel1.Controls.Add(panel7);
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(766, 740);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // panel19
            // 
            panel19.AutoSize = true;
            panel19.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel19.Controls.Add(textBox18);
            panel19.Controls.Add(checkBox18);
            panel19.Location = new Point(5, 689);
            panel19.Name = "panel19";
            panel19.Size = new Size(746, 33);
            panel19.TabIndex = 19;
            // 
            // textBox18
            // 
            textBox18.BorderStyle = BorderStyle.FixedSingle;
            textBox18.Location = new Point(193, 3);
            textBox18.Name = "textBox18";
            textBox18.Size = new Size(550, 27);
            textBox18.TabIndex = 1;
            // 
            // checkBox18
            // 
            checkBox18.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox18.Location = new Point(3, 4);
            checkBox18.Name = "checkBox18";
            checkBox18.Size = new Size(177, 24);
            checkBox18.TabIndex = 0;
            checkBox18.Text = "XVIII. Наракин";
            checkBox18.UseVisualStyleBackColor = true;
            // 
            // panel18
            // 
            panel18.AutoSize = true;
            panel18.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel18.Controls.Add(textBox17);
            panel18.Controls.Add(checkBox17);
            panel18.Location = new Point(5, 652);
            panel18.Name = "panel18";
            panel18.Size = new Size(746, 33);
            panel18.TabIndex = 18;
            // 
            // textBox17
            // 
            textBox17.BorderStyle = BorderStyle.FixedSingle;
            textBox17.Location = new Point(193, 3);
            textBox17.Name = "textBox17";
            textBox17.Size = new Size(550, 27);
            textBox17.TabIndex = 1;
            // 
            // checkBox17
            // 
            checkBox17.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox17.Location = new Point(3, 4);
            checkBox17.Name = "checkBox17";
            checkBox17.Size = new Size(177, 24);
            checkBox17.TabIndex = 0;
            checkBox17.Text = "XVII. Талас";
            checkBox17.UseVisualStyleBackColor = true;
            // 
            // panel17
            // 
            panel17.AutoSize = true;
            panel17.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel17.Controls.Add(textBox16);
            panel17.Controls.Add(checkBox16);
            panel17.Location = new Point(5, 615);
            panel17.Name = "panel17";
            panel17.Size = new Size(746, 33);
            panel17.TabIndex = 17;
            // 
            // textBox16
            // 
            textBox16.BorderStyle = BorderStyle.FixedSingle;
            textBox16.Location = new Point(193, 3);
            textBox16.Name = "textBox16";
            textBox16.Size = new Size(550, 27);
            textBox16.TabIndex = 1;
            // 
            // checkBox16
            // 
            checkBox16.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox16.Location = new Point(3, 4);
            checkBox16.Name = "checkBox16";
            checkBox16.Size = new Size(177, 24);
            checkBox16.TabIndex = 0;
            checkBox16.Text = "XVI. Аэтерис";
            checkBox16.UseVisualStyleBackColor = true;
            // 
            // panel16
            // 
            panel16.AutoSize = true;
            panel16.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel16.Controls.Add(textBox15);
            panel16.Controls.Add(checkBox15);
            panel16.Location = new Point(5, 578);
            panel16.Name = "panel16";
            panel16.Size = new Size(746, 33);
            panel16.TabIndex = 16;
            // 
            // textBox15
            // 
            textBox15.BorderStyle = BorderStyle.FixedSingle;
            textBox15.Location = new Point(193, 3);
            textBox15.Name = "textBox15";
            textBox15.Size = new Size(550, 27);
            textBox15.TabIndex = 1;
            // 
            // checkBox15
            // 
            checkBox15.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox15.Location = new Point(3, 4);
            checkBox15.Name = "checkBox15";
            checkBox15.Size = new Size(177, 24);
            checkBox15.TabIndex = 0;
            checkBox15.Text = "XV. Кетус";
            checkBox15.UseVisualStyleBackColor = true;
            // 
            // panel15
            // 
            panel15.AutoSize = true;
            panel15.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel15.Controls.Add(textBox14);
            panel15.Controls.Add(checkBox14);
            panel15.Location = new Point(5, 541);
            panel15.Name = "panel15";
            panel15.Size = new Size(746, 33);
            panel15.TabIndex = 15;
            // 
            // textBox14
            // 
            textBox14.BorderStyle = BorderStyle.FixedSingle;
            textBox14.Location = new Point(193, 3);
            textBox14.Name = "textBox14";
            textBox14.Size = new Size(550, 27);
            textBox14.TabIndex = 1;
            // 
            // checkBox14
            // 
            checkBox14.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox14.Location = new Point(3, 4);
            checkBox14.Name = "checkBox14";
            checkBox14.Size = new Size(177, 24);
            checkBox14.TabIndex = 0;
            checkBox14.Text = "XIV. Эридан";
            checkBox14.UseVisualStyleBackColor = true;
            // 
            // panel14
            // 
            panel14.AutoSize = true;
            panel14.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel14.Controls.Add(textBox13);
            panel14.Controls.Add(checkBox13);
            panel14.Location = new Point(5, 504);
            panel14.Name = "panel14";
            panel14.Size = new Size(746, 33);
            panel14.TabIndex = 14;
            // 
            // textBox13
            // 
            textBox13.BorderStyle = BorderStyle.FixedSingle;
            textBox13.Location = new Point(193, 3);
            textBox13.Name = "textBox13";
            textBox13.Size = new Size(550, 27);
            textBox13.TabIndex = 1;
            // 
            // checkBox13
            // 
            checkBox13.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox13.Location = new Point(3, 4);
            checkBox13.Name = "checkBox13";
            checkBox13.Size = new Size(177, 24);
            checkBox13.TabIndex = 0;
            checkBox13.Text = "XIII. Вульпекула";
            checkBox13.UseVisualStyleBackColor = true;
            // 
            // panel13
            // 
            panel13.AutoSize = true;
            panel13.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel13.Controls.Add(textBox12);
            panel13.Controls.Add(checkBox12);
            panel13.Location = new Point(4, 467);
            panel13.Name = "panel13";
            panel13.Size = new Size(747, 33);
            panel13.TabIndex = 13;
            // 
            // textBox12
            // 
            textBox12.BorderStyle = BorderStyle.FixedSingle;
            textBox12.Location = new Point(194, 3);
            textBox12.Name = "textBox12";
            textBox12.Size = new Size(550, 27);
            textBox12.TabIndex = 1;
            // 
            // checkBox12
            // 
            checkBox12.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox12.Location = new Point(4, 4);
            checkBox12.Name = "checkBox12";
            checkBox12.Size = new Size(177, 24);
            checkBox12.TabIndex = 0;
            checkBox12.Text = "XII. Пиксис";
            checkBox12.UseVisualStyleBackColor = true;
            // 
            // panel12
            // 
            panel12.AutoSize = true;
            panel12.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel12.Controls.Add(textBox11);
            panel12.Controls.Add(checkBox11);
            panel12.Location = new Point(5, 430);
            panel12.Name = "panel12";
            panel12.Size = new Size(746, 33);
            panel12.TabIndex = 12;
            // 
            // textBox11
            // 
            textBox11.BorderStyle = BorderStyle.FixedSingle;
            textBox11.Location = new Point(193, 3);
            textBox11.Name = "textBox11";
            textBox11.Size = new Size(550, 27);
            textBox11.TabIndex = 1;
            // 
            // checkBox11
            // 
            checkBox11.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox11.Location = new Point(3, 4);
            checkBox11.Name = "checkBox11";
            checkBox11.Size = new Size(177, 24);
            checkBox11.TabIndex = 0;
            checkBox11.Text = "XI. Кайтайн";
            checkBox11.UseVisualStyleBackColor = true;
            // 
            // panel11
            // 
            panel11.AutoSize = true;
            panel11.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel11.Controls.Add(textBox10);
            panel11.Controls.Add(checkBox10);
            panel11.Location = new Point(5, 393);
            panel11.Name = "panel11";
            panel11.Size = new Size(746, 33);
            panel11.TabIndex = 11;
            // 
            // textBox10
            // 
            textBox10.BorderStyle = BorderStyle.FixedSingle;
            textBox10.Location = new Point(193, 3);
            textBox10.Name = "textBox10";
            textBox10.Size = new Size(550, 27);
            textBox10.TabIndex = 1;
            // 
            // checkBox10
            // 
            checkBox10.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox10.Location = new Point(3, 4);
            checkBox10.Name = "checkBox10";
            checkBox10.Size = new Size(177, 24);
            checkBox10.TabIndex = 0;
            checkBox10.Text = "X. Ахернар";
            checkBox10.UseVisualStyleBackColor = true;
            // 
            // panel10
            // 
            panel10.AutoSize = true;
            panel10.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel10.Controls.Add(textBox9);
            panel10.Controls.Add(checkBox9);
            panel10.Location = new Point(5, 356);
            panel10.Name = "panel10";
            panel10.Size = new Size(746, 33);
            panel10.TabIndex = 10;
            // 
            // textBox9
            // 
            textBox9.BorderStyle = BorderStyle.FixedSingle;
            textBox9.Location = new Point(193, 3);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(550, 27);
            textBox9.TabIndex = 1;
            // 
            // checkBox9
            // 
            checkBox9.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox9.Location = new Point(3, 4);
            checkBox9.Name = "checkBox9";
            checkBox9.Size = new Size(177, 24);
            checkBox9.TabIndex = 0;
            checkBox9.Text = "IX. Ричез";
            checkBox9.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            panel9.AutoSize = true;
            panel9.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel9.Controls.Add(textBox8);
            panel9.Controls.Add(checkBox8);
            panel9.Location = new Point(5, 319);
            panel9.Name = "panel9";
            panel9.Size = new Size(746, 33);
            panel9.TabIndex = 9;
            // 
            // textBox8
            // 
            textBox8.BorderStyle = BorderStyle.FixedSingle;
            textBox8.Location = new Point(193, 3);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(550, 27);
            textBox8.TabIndex = 1;
            // 
            // checkBox8
            // 
            checkBox8.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox8.Location = new Point(3, 4);
            checkBox8.Name = "checkBox8";
            checkBox8.Size = new Size(177, 24);
            checkBox8.TabIndex = 0;
            checkBox8.Text = "VIII. Регис";
            checkBox8.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            panel8.AutoSize = true;
            panel8.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel8.Controls.Add(textBox7);
            panel8.Controls.Add(checkBox7);
            panel8.Location = new Point(5, 282);
            panel8.Name = "panel8";
            panel8.Size = new Size(746, 33);
            panel8.TabIndex = 8;
            // 
            // textBox7
            // 
            textBox7.BorderStyle = BorderStyle.FixedSingle;
            textBox7.Location = new Point(193, 3);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(550, 27);
            textBox7.TabIndex = 1;
            // 
            // checkBox7
            // 
            checkBox7.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox7.Location = new Point(3, 4);
            checkBox7.Name = "checkBox7";
            checkBox7.Size = new Size(177, 24);
            checkBox7.TabIndex = 0;
            checkBox7.Text = "VII. Лас Эквестрия";
            checkBox7.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            panel7.AutoSize = true;
            panel7.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel7.Controls.Add(textBox6);
            panel7.Controls.Add(checkBox6);
            panel7.Location = new Point(5, 245);
            panel7.Name = "panel7";
            panel7.Size = new Size(746, 33);
            panel7.TabIndex = 7;
            // 
            // textBox6
            // 
            textBox6.BorderStyle = BorderStyle.FixedSingle;
            textBox6.Location = new Point(193, 3);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(550, 27);
            textBox6.TabIndex = 1;
            // 
            // checkBox6
            // 
            checkBox6.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox6.Location = new Point(3, 4);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(177, 24);
            checkBox6.TabIndex = 0;
            checkBox6.Text = "VI. Салуса Секундус";
            checkBox6.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            panel6.AutoSize = true;
            panel6.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel6.Controls.Add(textBox5);
            panel6.Controls.Add(checkBox5);
            panel6.Location = new Point(5, 208);
            panel6.Name = "panel6";
            panel6.Size = new Size(746, 33);
            panel6.TabIndex = 6;
            // 
            // textBox5
            // 
            textBox5.BorderStyle = BorderStyle.FixedSingle;
            textBox5.Location = new Point(193, 3);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(550, 27);
            textBox5.TabIndex = 1;
            // 
            // checkBox5
            // 
            checkBox5.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox5.Location = new Point(3, 4);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(177, 24);
            checkBox5.TabIndex = 0;
            checkBox5.Text = "V. Гьеди Прайм";
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            panel5.AutoSize = true;
            panel5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel5.Controls.Add(textBox4);
            panel5.Controls.Add(checkBox4);
            panel5.Location = new Point(5, 171);
            panel5.Name = "panel5";
            panel5.Size = new Size(746, 33);
            panel5.TabIndex = 5;
            // 
            // textBox4
            // 
            textBox4.BorderStyle = BorderStyle.FixedSingle;
            textBox4.Location = new Point(193, 3);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(550, 27);
            textBox4.TabIndex = 1;
            // 
            // checkBox4
            // 
            checkBox4.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox4.Location = new Point(3, 4);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(177, 24);
            checkBox4.TabIndex = 0;
            checkBox4.Text = "IV. Оазис Судьбы";
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.AutoSize = true;
            panel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel4.Controls.Add(textBox3);
            panel4.Controls.Add(checkBox3);
            panel4.Location = new Point(5, 134);
            panel4.Name = "panel4";
            panel4.Size = new Size(746, 33);
            panel4.TabIndex = 4;
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.Location = new Point(193, 3);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(550, 27);
            textBox3.TabIndex = 1;
            // 
            // checkBox3
            // 
            checkBox3.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox3.Location = new Point(3, 4);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(177, 24);
            checkBox3.TabIndex = 0;
            checkBox3.Text = "III. Омикрон Прайм";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel3.Controls.Add(textBox2);
            panel3.Controls.Add(checkBox2);
            panel3.Location = new Point(5, 97);
            panel3.Name = "panel3";
            panel3.Size = new Size(746, 33);
            panel3.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Location = new Point(193, 3);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(550, 27);
            textBox2.TabIndex = 1;
            // 
            // checkBox2
            // 
            checkBox2.Font = new Font("Arial Narrow", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            checkBox2.Location = new Point(3, 4);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(204, 24);
            checkBox2.TabIndex = 0;
            checkBox2.Text = "II. Западные территории";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 18F, FontStyle.Underline, GraphicsUnit.Point, 204);
            label3.Location = new Point(198, 18);
            label3.Name = "label3";
            label3.Size = new Size(120, 27);
            label3.TabIndex = 3;
            label3.Text = "Описание:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 18F, FontStyle.Underline, GraphicsUnit.Point, 204);
            label2.Location = new Point(5, 18);
            label2.Name = "label2";
            label2.Size = new Size(91, 27);
            label2.TabIndex = 2;
            label2.Text = "Сервер:";
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(checkBox1);
            panel2.Location = new Point(5, 60);
            panel2.Name = "panel2";
            panel2.Size = new Size(746, 33);
            panel2.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Location = new Point(193, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(550, 27);
            textBox1.TabIndex = 1;
            // 
            // checkBox1
            // 
            checkBox1.Font = new Font("Arial Narrow", 12F, FontStyle.Bold);
            checkBox1.Location = new Point(3, 4);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(177, 24);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "I. Изначальный мир";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.BackgroundImageLayout = ImageLayout.Zoom;
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
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(772, 746);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Параметры";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
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
            groupBox1.Location = new Point(217, 238);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(178, 225);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "Поиск мобов";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(txtLog);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(772, 746);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Логи";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.Linen;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Location = new Point(3, 3);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(766, 740);
            txtLog.TabIndex = 1;
            txtLog.Text = "";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(780, 28);
            menuStrip1.TabIndex = 21;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(79, 24);
            toolStripMenuItem1.Text = "Справка";
            // 
            // MainForm
            // 
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(780, 807);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
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
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel19.ResumeLayout(false);
            panel19.PerformLayout();
            panel18.ResumeLayout(false);
            panel18.PerformLayout();
            panel17.ResumeLayout(false);
            panel17.PerformLayout();
            panel16.ResumeLayout(false);
            panel16.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            panel14.ResumeLayout(false);
            panel14.PerformLayout();
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel11.ResumeLayout(false);
            panel11.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private Panel panel1;
        private Panel panel2;
        private TextBox textBox1;
        private CheckBox checkBox1;
        private Label label2;
        private Label label3;
        private Panel panel7;
        private TextBox textBox6;
        private CheckBox checkBox6;
        private Panel panel6;
        private TextBox textBox5;
        private CheckBox checkBox5;
        private Panel panel5;
        private TextBox textBox4;
        private CheckBox checkBox4;
        private Panel panel4;
        private TextBox textBox3;
        private CheckBox checkBox3;
        private Panel panel3;
        private TextBox textBox2;
        private CheckBox checkBox2;
        private Panel panel19;
        private TextBox textBox18;
        private CheckBox checkBox18;
        private Panel panel18;
        private TextBox textBox17;
        private CheckBox checkBox17;
        private Panel panel17;
        private TextBox textBox16;
        private CheckBox checkBox16;
        private Panel panel16;
        private TextBox textBox15;
        private CheckBox checkBox15;
        private Panel panel15;
        private TextBox textBox14;
        private CheckBox checkBox14;
        private Panel panel14;
        private TextBox textBox13;
        private CheckBox checkBox13;
        private Panel panel13;
        private TextBox textBox12;
        private CheckBox checkBox12;
        private Panel panel12;
        private TextBox textBox11;
        private CheckBox checkBox11;
        private Panel panel11;
        private TextBox textBox10;
        private CheckBox checkBox10;
        private Panel panel10;
        private TextBox textBox9;
        private CheckBox checkBox9;
        private Panel panel9;
        private TextBox textBox8;
        private CheckBox checkBox8;
        private Panel panel8;
        private TextBox textBox7;
        private CheckBox checkBox7;
        private Label label1;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private GroupBox groupBox1;
    }
}