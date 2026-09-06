using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Collections.Generic;

namespace DC_Button_Finder
{
    public partial class MainForm : Form
    {
        private AssistantEngine _assistantEngine;
        private readonly Logger _logger;
        private readonly TemplateCache _cache;
        private readonly SettingsManager _settingsManager;
        private readonly BotController _botController;
        private readonly SiegeScheduler _siegeScheduler;
        private readonly TimerDisplay _timerDisplay;
        private readonly Random _random;
        private readonly ScreenshotService _screenshotService;
        private BotSettings? _settings;
        private List<ServerInfo> _servers = new List<ServerInfo>();
        private Label _lblCounter;
        private DataGridView _dataGridTable;

        private const int HOTKEY_ID_START = 1;
        private const int HOTKEY_ID_STOP = 2;

        public MainForm()
        {
            InitializeComponent();

            _random = new Random();
            _logger = new Logger(txtLog);
            _cache = new TemplateCache();
            _settingsManager = new SettingsManager(_logger);
            _screenshotService = new ScreenshotService();
            var templateMatcher = new TemplateMatcher(_logger);
            var clicker = new Clicker(_logger, _random);
            var scrollController = new ScrollController(clicker, _logger, _random);
            var botEngine = new BotEngine(_logger, _screenshotService, _cache, templateMatcher, clicker, scrollController, _random);
            _botController = new BotController(botEngine);
            _assistantEngine = new AssistantEngine(
                _logger,
                _screenshotService,
                _cache,
                templateMatcher,
                clicker,
                _random
            );
            _siegeScheduler = new SiegeScheduler(
                _botController,
                _logger,
                GetSettingsFromUIThreadSafe,
                UpdateUIForRunningState
            );
            _timerDisplay = new TimerDisplay(_siegeScheduler, _botController, lblTimer);
            _siegeScheduler.OnSiegeStatusChanged += (status) => _logger.Info($"[ОСАДА] {status}");
            _siegeScheduler.OnAutoAction += (action) => _logger.Success($"[АВТО] {action}");
            botEngine.OnButtonClicked += (buttonName) =>
                _logger.Success($"Бот нажал: {buttonName}");
            botEngine.OnStatusChanged += (status) =>
                _logger.Info(status);

            txtLog.ReadOnly = true;
            RegisterHotKeys();
            ScheduleTab.Create(tabControl1, _servers);

            SiegeCountdownTab.Initialize(_logger);
        }

        private BotSettings GetSettingsFromUIThreadSafe()
        {
            if (InvokeRequired)
            {
                return (BotSettings)Invoke(new Func<BotSettings>(GetSettingsFromUIThreadSafe));
            }
            return GetSettingsFromUI();
        }

        private BotSettings GetSettingsFromUI()
        {
            return new BotSettings
            {
                ButtonSequence = txtButtonSequence.Text ?? string.Empty,
                ThresholdPercentage = (double)numThreshold.Value,
                IterationDelay = decimal.ToInt32(numIterationDelay.Value),
                SelectedWeek = cboWeekSelection.SelectedItem?.ToString() ?? "Week 1-8. Универсальная неделя",
                SelectedServer = cboServerSelection.SelectedItem?.ToString() ?? "IV.Оазис Судьбы / VIII.Регис / IX.Ричез",
                AlwaysMode = chkAlwaysMode.Checked,
                SiegeOnlyMode = chkSiegeOnlyMode.Checked,
                ExtendedSiege = chkExtendedSiege.Checked,
                OnlyScratch = chkOnlyScratch.Checked,
                AssistantX1 = chkAssistantX1.Checked,
                AssistantX3 = chkAssistantX3.Checked,
                WeekBossX1 = chkWeekBossX1.Checked,
                WeekBossX3 = chkWeekBossX3.Checked,
                HiddenBossX1 = chkHiddenBossX1.Checked,
                HiddenBossX3 = chkHiddenBossX3.Checked,
                MobEasyX1 = chkMobEasyX1.Checked,
                MobEasyX3 = chkMobEasyX3.Checked,
                MobNormalX1 = chkMobNormalX1.Checked,
                MobNormalX3 = chkMobNormalX3.Checked,
                MobStrongX1 = chkMobStrongX1.Checked,
                MobStrongX3 = chkMobStrongX3.Checked
            };
        }

        private void RegisterHotKeys()
        {
            try
            {
                RegisterHotKey(Handle, HOTKEY_ID_START, 0x0000, (uint)Keys.F1);
                RegisterHotKey(Handle, HOTKEY_ID_STOP, 0x0000, (uint)Keys.F2);
                _logger.Success("Горячие клавиши зарегистрированы: F1-старт, F2-стоп");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка регистрации горячих клавиш: {ex.Message}");
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                if (id == HOTKEY_ID_START) StartByHotkey();
                else if (id == HOTKEY_ID_STOP) StopByHotkey();
            }
            base.WndProc(ref m);
        }

        private async void StartByHotkey()
        {
            try
            {
                var settings = GetSettingsFromUIThreadSafe();
                bool isAssistantMode = chkAssistantX1.Checked || chkAssistantX3.Checked;
                if (isAssistantMode) await StartAssistantAsync(settings);
                else ManualStartBot();
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка запуска по горячей клавише: {ex.Message}");
            }
        }

        private async Task StartAssistantAsync(BotSettings settings)
        {
            if (_assistantEngine.IsRunning || _botController.IsRunning)
            {
                _logger.Warn("Ассистент или бот уже запущены");
                return;
            }

            try
            {
                _logger.Info("ЗАПУСК АССИСТЕНТА по горячей клавише");
                string assistantAttackMode = chkAssistantX1.Checked ? "atk_1" : "atk_3";
                UpdateUIForRunningState(true);
                _timerDisplay.BotStarted();
                await _assistantEngine.StartAsync(assistantAttackMode, settings.IterationDelay, (int)settings.ThresholdPercentage);
                _logger.Success($"Ассистент запущен (режим: {assistantAttackMode})");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка запуска ассистента: {ex.Message}");
                UpdateUIForRunningState(false);
                _timerDisplay.BotStopped();
            }
        }

        private void StopByHotkey()
        {
            if (_assistantEngine.IsRunning) StopAssistant();
            else if (_botController.IsRunning) ManualStopBot();
            else _logger.Warn("Ассистент или бот уже остановлены");
        }

        private void StopAssistant()
        {
            if (!_assistantEngine.IsRunning)
            {
                _logger.Warn("Ассистент уже остановлен");
                return;
            }
            _logger.Info("ОСТАНОВКА АССИСТЕНТА по горячей клавише");
            _assistantEngine.Stop();
            UpdateUIForRunningState(false);
            _timerDisplay.BotStopped();
            _logger.Info("Ассистент остановлен");
        }

        private async void ManualStartBot()
        {
            if (_botController.IsRunning)
            {
                _logger.Warn("Бот уже запущен");
                return;
            }

            try
            {
                _logger.Info("РУЧНОЙ ЗАПУСК по горячей клавише");
                var settings = GetSettingsFromUIThreadSafe();
                UpdateUIForRunningState(true);
                _timerDisplay.BotStarted();
                await _botController.StartAsync(settings);
                _logger.Success("Бот запущен вручную по горячей клавише F1");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка ручного запуска: {ex.Message}");
                UpdateUIForRunningState(false);
                _timerDisplay.BotStopped();
            }
        }

        private void ManualStopBot()
        {
            if (!_botController.IsRunning)
            {
                _logger.Warn("Бот уже остановлен");
                return;
            }
            _logger.Info("РУЧНАЯ ОСТАНОВКА по горячей клавише");
            _botController.Stop();
            UpdateUIForRunningState(false);
            _timerDisplay.BotStopped();
            _logger.Info("Бот остановлен вручную по горячей клавише F2");
        }

        private void ApplySettingsToUI(BotSettings settings)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<BotSettings>(ApplySettingsToUI), settings);
                return;
            }

            string[] weeks = {
                "Week 1-8. Универсальная неделя",
                "Week 0. Рунная неделя",
                "Week 0. Овощная неделя",
                "Week 0. Космическая неделя",
                "Week 0. Конфетная неделя"
            };

            cboWeekSelection.Items.Clear();
            cboWeekSelection.Items.AddRange(weeks);

            string[] servers = {
                "I.Изначальный мир / V.Гьеди Прайм / XII.Пиксис",
                "II.Западные территории / VI.Салуса Секундус / XI.Кайтайн / XIII.Вульпекула",
                "III.Омикрон Прайм / VII.Лас Эквестрия / X.Ахернар / XIV.Эридан",
                "IV.Оазис Судьбы / VIII.Регис / IX.Ричез"
            };

            cboServerSelection.Items.Clear();
            cboServerSelection.Items.AddRange(servers);

            txtButtonSequence.Text = settings.ButtonSequence;
            numThreshold.Value = (decimal)settings.ThresholdPercentage;
            numIterationDelay.Value = settings.IterationDelay;

            if (!string.IsNullOrEmpty(settings.SelectedWeek) && cboWeekSelection.Items.Contains(settings.SelectedWeek))
                cboWeekSelection.SelectedItem = settings.SelectedWeek;
            else
                cboWeekSelection.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(settings.SelectedServer) && cboServerSelection.Items.Contains(settings.SelectedServer))
                cboServerSelection.SelectedItem = settings.SelectedServer;
            else
                cboServerSelection.SelectedIndex = 0;

            chkAlwaysMode.Checked = settings.AlwaysMode;
            chkSiegeOnlyMode.Checked = settings.SiegeOnlyMode;
            chkExtendedSiege.Checked = settings.ExtendedSiege;
            chkExtendedSiege.Enabled = settings.SiegeOnlyMode;
            chkOnlyScratch.Checked = settings.OnlyScratch;
            chkAssistantX1.Checked = settings.AssistantX1;
            chkAssistantX3.Checked = settings.AssistantX3;
            chkWeekBossX1.Checked = settings.WeekBossX1;
            chkWeekBossX3.Checked = settings.WeekBossX3;
            chkHiddenBossX1.Checked = settings.HiddenBossX1;
            chkHiddenBossX3.Checked = settings.HiddenBossX3;
            chkMobEasyX1.Checked = settings.MobEasyX1;
            chkMobEasyX3.Checked = settings.MobEasyX3;
            chkMobNormalX1.Checked = settings.MobNormalX1;
            chkMobNormalX3.Checked = settings.MobNormalX3;
            chkMobStrongX1.Checked = settings.MobStrongX1;
            chkMobStrongX3.Checked = settings.MobStrongX3;

            if (settings.WindowLocation != Point.Empty && settings.WindowSize != Size.Empty)
            {
                StartPosition = FormStartPosition.Manual;
                Location = settings.WindowLocation;
                Size = settings.WindowSize;
            }
        }

        private async Task LoadAllTemplatesAsync(BotSettings settings)
        {
            try
            {
                await _cache.LoadFunctionButtonsAsync(_logger);
                _cache.LoadWeekButtons(settings.SelectedWeek, _logger);
                _cache.LoadHiddenBossButtons(_logger);
                _cache.LoadMobsEasy(_logger);
                _cache.LoadMobsNormal(_logger);
                _cache.LoadMobsStrong(_logger);
                _logger.Success("Все шаблоны загружены");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка загрузки шаблонов: {ex.Message}");
            }
        }

        private void UpdateUIForRunningState(bool isRunning)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(UpdateUIForRunningState), isRunning);
                return;
            }

            bool isAssistantRunning = _assistantEngine?.IsRunning ?? false;
            bool isBotRunning = _botController.IsRunning;

            btnStart.Enabled = !isRunning;
            btnStop.Enabled = isRunning;

            txtButtonSequence.ReadOnly = isRunning;
            txtButtonSequence.Enabled = !isRunning;
            numThreshold.Enabled = !isRunning;
            numIterationDelay.Enabled = !isRunning;
            cboWeekSelection.Enabled = !isRunning;
            cboServerSelection.Enabled = !isRunning;
            chkWeekBossX1.Enabled = !isRunning;
            chkWeekBossX3.Enabled = !isRunning;
            chkHiddenBossX1.Enabled = !isRunning;
            chkHiddenBossX3.Enabled = !isRunning;
            chkMobEasyX1.Enabled = !isRunning;
            chkMobEasyX3.Enabled = !isRunning;
            chkMobNormalX1.Enabled = !isRunning;
            chkMobNormalX3.Enabled = !isRunning;
            chkMobStrongX1.Enabled = !isRunning;
            chkMobStrongX3.Enabled = !isRunning;
            chkOnlyScratch.Enabled = !isRunning;
            chkAssistantX1.Enabled = !isRunning;
            chkAssistantX3.Enabled = !isRunning;
            chkAlwaysMode.Enabled = !isRunning;
            chkSiegeOnlyMode.Enabled = !isRunning;
            chkExtendedSiege.Enabled = !isRunning && chkSiegeOnlyMode.Checked;
        }

        private void LoadTableFromExcel(DataGridView dataGrid)
        {
            try
            {
                if (dataGrid == null || dataGrid.Columns.Count == 0)
                    return;

                string excelPath = Path.Combine(Application.StartupPath, "_resources", "ServerInfo", "server_data.xlsx");
                if (!File.Exists(excelPath))
                {
                    UpdateCounterLabel();
                    return;
                }

                using (var package = new ExcelPackage(new FileInfo(excelPath)))
                {
                    var dataSheet = package.Workbook.Worksheets["Таблица"];
                    if (dataSheet == null)
                    {
                        UpdateCounterLabel();
                        return;
                    }

                    bool hasData = false;
                    int checkRow = 2;
                    while (dataSheet.Cells[checkRow, 1].Value != null)
                    {
                        hasData = true;
                        break;
                    }

                    if (!hasData)
                    {
                        UpdateCounterLabel();
                        return;
                    }

                    var serverIds = new List<int>();
                    foreach (DataGridViewRow row in dataGrid.Rows)
                    {
                        if (row.Tag is int id)
                            serverIds.Add(id);
                    }

                    dataGrid.Rows.Clear();

                    dataGrid.Font = new Font("Segoe UI", 14, FontStyle.Regular);
                    dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    if (dataGrid.Columns.Contains("Server"))
                        dataGrid.Columns["Server"].DefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                    dataGrid.ColumnHeadersHeight = 45;
                    dataGrid.RowTemplate.Height = 30;

                    if (dataGrid.Columns.Contains("Server"))
                    {
                        dataGrid.Columns["Server"].Width = 295;
                        dataGrid.Columns["Server"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    }

                    foreach (DataGridViewColumn col in dataGrid.Columns)
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;

                    dataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    if (dataGrid.Columns.Contains("Server"))
                        dataGrid.Columns["Server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                    var checkboxes = new Dictionary<int, bool>();
                    var checkSheet = package.Workbook.Worksheets["CheckBox"];
                    if (checkSheet != null)
                    {
                        int row = 2;
                        while (checkSheet.Cells[row, 1].Value != null)
                        {
                            if (int.TryParse(checkSheet.Cells[row, 1].Text, out int id))
                            {
                                checkboxes[id] = checkSheet.Cells[row, 2].Text == "1";
                            }
                            row++;
                        }
                    }

                    int dataRow = 2;
                    while (dataSheet.Cells[dataRow, 1].Value != null)
                    {
                        dataGrid.Rows.Add();
                        dataRow++;
                    }

                    dataRow = 2;
                    int rowIndex = 0;
                    while (dataSheet.Cells[dataRow, 1].Value != null)
                    {
                        if (rowIndex >= dataGrid.Rows.Count)
                            dataGrid.Rows.Add();

                        string serverName = dataSheet.Cells[dataRow, 1].Text;
                        string voids = dataSheet.Cells[dataRow, 2].Text;
                        string cosmic = dataSheet.Cells[dataRow, 3].Text;
                        string potions = dataSheet.Cells[dataRow, 4].Text;
                        string izolda = dataSheet.Cells[dataRow, 5].Text;
                        string ap = dataSheet.Cells[dataRow, 6].Text;
                        string zod = dataSheet.Cells[dataRow, 7].Text;

                        if (dataGrid.Columns.Contains("Server"))
                            dataGrid.Rows[rowIndex].Cells["Server"].Value = serverName;
                        if (dataGrid.Columns.Contains("Voids"))
                            dataGrid.Rows[rowIndex].Cells["Voids"].Value = voids;
                        if (dataGrid.Columns.Contains("Cosmic"))
                            dataGrid.Rows[rowIndex].Cells["Cosmic"].Value = cosmic;
                        if (dataGrid.Columns.Contains("Potions"))
                            dataGrid.Rows[rowIndex].Cells["Potions"].Value = potions;
                        if (dataGrid.Columns.Contains("Izolda"))
                            dataGrid.Rows[rowIndex].Cells["Izolda"].Value = izolda;
                        if (dataGrid.Columns.Contains("AP"))
                            dataGrid.Rows[rowIndex].Cells["AP"].Value = ap;
                        if (dataGrid.Columns.Contains("ZOD"))
                            dataGrid.Rows[rowIndex].Cells["ZOD"].Value = zod;

                        if (rowIndex < serverIds.Count)
                            dataGrid.Rows[rowIndex].Tag = serverIds[rowIndex];

                        int serverId = rowIndex < serverIds.Count ? serverIds[rowIndex] : 0;
                        if (dataGrid.Columns.Contains("Checked") && serverId > 0)
                        {
                            dataGrid.Rows[rowIndex].Cells["Checked"].Value = checkboxes.ContainsKey(serverId) && checkboxes[serverId];
                        }
                        else if (dataGrid.Columns.Contains("Checked"))
                        {
                            dataGrid.Rows[rowIndex].Cells["Checked"].Value = false;
                        }

                        ApplyConditionalFormatting(dataGrid.Rows[rowIndex], cosmic, potions, izolda, ap, zod);

                        dataRow++;
                        rowIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка загрузки Excel: {ex.Message}");
            }
            finally
            {
                UpdateCounterLabel();
            }
        }
        private void ApplyConditionalFormatting(DataGridViewRow row, string cosmic, string potions, string izolda, string ap, string zod)
        {
            int cosmicVal = int.TryParse(cosmic, out int c) ? c : 0;
            int potionsVal = int.TryParse(potions, out int p) ? p : 0;
            int izoldaVal = int.TryParse(izolda, out int i) ? i : 0;
            int apVal = int.TryParse(ap, out int a) ? a : 0;
            int zodVal = int.TryParse(zod, out int z) ? z : 0;

            if (cosmicVal > 4)
                row.Cells["Cosmic"].Style.BackColor = Color.LightGreen;
            if (potionsVal > 1)
                row.Cells["Potions"].Style.BackColor = Color.LightGreen;
            if (izoldaVal == 0)
                row.Cells["Izolda"].Style.BackColor = Color.LightGreen;
            if (apVal == 0)
                row.Cells["AP"].Style.BackColor = Color.LightCoral;
            else if (apVal == 1)
                row.Cells["AP"].Style.BackColor = Color.LightGreen;
            if (zodVal == 0)
                row.Cells["ZOD"].Style.BackColor = Color.LightCoral;
            else if (zodVal == 1)
                row.Cells["ZOD"].Style.BackColor = Color.LightGreen;
        }

        private void SaveTableToExcel(DataGridView dataGrid)
        {
            try
            {
                _logger.Info("=== СОХРАНЕНИЕ В EXCEL ===");

                if (dataGrid == null)
                {
                    _logger.Warn("dataGrid == null, сохранение отменено");
                    return;
                }

                string excelPath = Path.Combine(Application.StartupPath, "_resources", "ServerInfo", "server_data.xlsx");
                _logger.Info($"Путь к Excel: {excelPath}");

                bool fileExists = File.Exists(excelPath);

                using (var package = fileExists ? new ExcelPackage(new FileInfo(excelPath)) : new ExcelPackage())
                {
                    var dataSheet = package.Workbook.Worksheets["Таблица"];
                    if (dataSheet == null)
                        dataSheet = package.Workbook.Worksheets.Add("Таблица");

                    dataSheet.Cells[1, 1].Value = "Сервер";
                    dataSheet.Cells[1, 2].Value = "Пустоты";
                    dataSheet.Cells[1, 3].Value = "Космик";
                    dataSheet.Cells[1, 4].Value = "Зелиек";
                    dataSheet.Cells[1, 5].Value = "Изольда";
                    dataSheet.Cells[1, 6].Value = "АП";
                    dataSheet.Cells[1, 7].Value = "ЗОД";

                    using (var range = dataSheet.Cells[1, 1, 1, 7])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    for (int i = 0; i < dataGrid.Rows.Count; i++)
                    {
                        var row = dataGrid.Rows[i];
                        dataSheet.Cells[i + 2, 1].Value = row.Cells["Server"].Value?.ToString() ?? "";
                        dataSheet.Cells[i + 2, 2].Value = row.Cells["Voids"].Value?.ToString() ?? "";
                        dataSheet.Cells[i + 2, 3].Value = row.Cells["Cosmic"].Value?.ToString() ?? "";
                        dataSheet.Cells[i + 2, 4].Value = row.Cells["Potions"].Value?.ToString() ?? "";
                        dataSheet.Cells[i + 2, 5].Value = row.Cells["Izolda"].Value?.ToString() ?? "";
                        dataSheet.Cells[i + 2, 6].Value = row.Cells["AP"].Value?.ToString() ?? "";
                        dataSheet.Cells[i + 2, 7].Value = row.Cells["ZOD"].Value?.ToString() ?? "";

                        for (int col = 0; col < 7; col++)
                        {
                            if (row.Cells[col].Style.BackColor != Color.Empty)
                            {
                                dataSheet.Cells[i + 2, col + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                dataSheet.Cells[i + 2, col + 1].Style.Fill.BackgroundColor.SetColor(row.Cells[col].Style.BackColor);
                            }
                            if (row.Cells[col].Style.ForeColor != Color.Empty)
                            {
                                dataSheet.Cells[i + 2, col + 1].Style.Font.Color.SetColor(row.Cells[col].Style.ForeColor);
                            }
                        }
                    }

                    dataSheet.Cells.AutoFitColumns();

                    var checkSheet = package.Workbook.Worksheets["CheckBox"];
                    if (checkSheet == null)
                        checkSheet = package.Workbook.Worksheets.Add("CheckBox");

                    checkSheet.Cells[1, 1].Value = "ID";
                    checkSheet.Cells[1, 2].Value = "Checked";

                    using (var range = checkSheet.Cells[1, 1, 1, 2])
                    {
                        range.Style.Font.Bold = true;
                    }

                    int checkRow = 2;
                    foreach (DataGridViewRow row in dataGrid.Rows)
                    {
                        if (row.Tag is int id)
                        {
                            bool isChecked = row.Cells["Checked"].Value is bool checkedVal && checkedVal;
                            checkSheet.Cells[checkRow, 1].Value = id;
                            checkSheet.Cells[checkRow, 2].Value = isChecked ? "1" : "0";
                            checkRow++;
                        }
                    }

                    checkSheet.Cells.AutoFitColumns();

                    if (!fileExists)
                    {
                        string directory = Path.GetDirectoryName(excelPath);
                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);
                    }

                    package.SaveAs(new FileInfo(excelPath));
                    _logger.Info($"Сохранено в Excel: {excelPath}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка сохранения Excel: {ex.Message}");
            }
            finally
            {
                _logger.Info("=== СОХРАНЕНИЕ В EXCEL ЗАВЕРШЕНО ===");
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _logger.Info("=== НАЧАЛО ЗАГРУЗКИ ПРОГРАММЫ ===");

                _settings = _settingsManager.LoadSettings();
                ApplySettingsToUI(_settings);

                _servers = ServerManager.LoadServers();
                _logger.Info($"Загружено серверов: {_servers.Count}");

                _logger.Info("Создаём вкладку 'Сервер инфо'...");
                LoadServerInfoTab();

                if (_dataGridTable == null)
                {
                    _logger.Error("Ошибка: _dataGridTable == null после LoadServerInfoTab()");
                    return;
                }
                _logger.Info("Таблица создана успешно");

                _logger.Info("Загружаем данные из Excel...");
                LoadTableFromExcel(_dataGridTable);

                _logger.Info("Обновляем счётчик...");
                UpdateCounterLabel();

                var checkboxes = ServerManager.LoadCheckboxes();
                _logger.Info($"Загружено чекбоксов: {checkboxes.Count}");
                foreach (var kvp in checkboxes)
                {
                    _logger.Info($"  Сервер {kvp.Key}: {(kvp.Value ? "☑" : "☐")}");
                }

                _logger.Info("Создаем вкладку 'Осады ⏳'...");
                SiegeCountdownTab.Create(tabControl1, _servers);

                _logger.Info("Создаем вкладку 'Расписание'...");
                ScheduleTab.Initialize(_logger);
                ScheduleTab.Create(tabControl1, _servers);

                _siegeScheduler.SelectedServer = _settings.SelectedServer;
                _siegeScheduler.ExtendedSiege = _settings.ExtendedSiege;
                _siegeScheduler.IsEnabled = _settings.SiegeOnlyMode;

                await LoadAllTemplatesAsync(_settings);
                UpdateUIForRunningState(false);

                _logger.Info("=== ЗАГРУЗКА ПРОГРАММЫ ЗАВЕРШЕНА ===");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка инициализации: {ex.Message}");
                _logger.Error($"StackTrace: {ex.StackTrace}");
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _logger.Info("=== НАЧАЛО ЗАКРЫТИЯ ПРОГРАММЫ ===");

                if (_dataGridTable != null)
                {
                    _logger.Info("Сохраняем таблицу в Excel...");
                    SaveTableToExcel(_dataGridTable);
                }

                _timerDisplay?.Dispose();
                _siegeScheduler?.Dispose();
                _botController.Stop();
                _assistantEngine?.Stop();

                _settings = GetSettingsFromUIThreadSafe();
                if (_settings != null)
                    _settingsManager.SaveSettings(_settings, this);

                UnregisterHotKey(Handle, HOTKEY_ID_START);
                UnregisterHotKey(Handle, HOTKEY_ID_STOP);

                _logger.Success("Программа завершена, настройки сохранены");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка при закрытии: {ex.Message}");
            }
        }

        private void LoadServerInfoTab()
        {
            try
            {
                _logger.Info("=== НАЧАЛО ЗАГРУЗКИ ВКЛАДКИ ===");

                if (_servers == null || _servers.Count == 0)
                {
                    _logger.Warn("_servers пуст, загружаем заново...");
                    _servers = ServerManager.LoadServers();
                }

                TabPage serverTab = null;
                foreach (TabPage page in tabControl1.TabPages)
                {
                    if (page.Text == "Сервер инфо")
                    {
                        serverTab = page;
                        break;
                    }
                }

                if (serverTab == null)
                {
                    _logger.Warn("Вкладка 'Сервер инфо' не найдена");
                    return;
                }

                serverTab.Controls.Clear();

                Panel mainContainer = new Panel { Dock = DockStyle.Fill };

                Panel topPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 50,
                    BackColor = Color.LightSteelBlue,
                    Padding = new Padding(10, 5, 10, 5)
                };

                _lblCounter = new Label
                {
                    Text = "✅ Выполнено: 0 / 0",
                    Location = new Point(10, 10),
                    Width = 240,
                    Height = 30,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.DarkBlue
                };

                Button btnReset = new Button
                {
                    Text = "🔄 Новая неделя",
                    Location = new Point(270, 8),
                    Width = 150,
                    Height = 32,
                    BackColor = Color.LightCoral,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold)
                };
                btnReset.Click += (s, e) => ResetAllCheckboxes();

                topPanel.Controls.Add(_lblCounter);
                topPanel.Controls.Add(btnReset);

                _dataGridTable = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    RowHeadersVisible = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 11, FontStyle.Regular)
                };

                _dataGridTable.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                _dataGridTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                _dataGridTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "Checked",
                    HeaderText = "☑",
                    Width = 40,
                    ReadOnly = false,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                };
                _dataGridTable.Columns.Add(chkColumn);

                _dataGridTable.Columns.Add("Server", "Сервер");
                _dataGridTable.Columns["Server"].Width = 295;
                _dataGridTable.Columns["Server"].ReadOnly = true;
                _dataGridTable.Columns["Server"].DefaultCellStyle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
                _dataGridTable.Columns["Server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                _dataGridTable.Columns["Server"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                _dataGridTable.Columns.Add("Voids", "Пустоты");
                _dataGridTable.Columns["Voids"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                _dataGridTable.Columns.Add("Cosmic", "Космик");
                _dataGridTable.Columns["Cosmic"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                _dataGridTable.Columns.Add("Potions", "Зелиек");
                _dataGridTable.Columns["Potions"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                _dataGridTable.Columns.Add("Izolda", "Изольда");
                _dataGridTable.Columns["Izolda"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                _dataGridTable.Columns.Add("AP", "АП");
                _dataGridTable.Columns["AP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                _dataGridTable.Columns.Add("ZOD", "ЗОД");
                _dataGridTable.Columns["ZOD"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                _dataGridTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                _dataGridTable.ColumnHeadersHeight = 45;
                _dataGridTable.RowTemplate.Height = 30;

                _logger.Info($"Добавляем {_servers.Count} строк в таблицу...");
                foreach (var server in _servers)
                {
                    int rowIndex = _dataGridTable.Rows.Add();
                    _dataGridTable.Rows[rowIndex].Cells["Server"].Value = server.Name;
                    _dataGridTable.Rows[rowIndex].Tag = server.Id;
                    _dataGridTable.Rows[rowIndex].Cells["Checked"].Value = false;
                }
                _logger.Info($"{_servers.Count} строк добавлено");

                _dataGridTable.CellValueChanged += (s, e) =>
                {
                    if (e.ColumnIndex == _dataGridTable.Columns["Checked"].Index)
                    {
                        UpdateCounterLabel();
                        SaveTableToExcel(_dataGridTable);
                        SiegeCountdownTab.Recreate(tabControl1, _servers);
                    }
                };

                _dataGridTable.CurrentCellDirtyStateChanged += (s, e) =>
                {
                    if (_dataGridTable.IsCurrentCellDirty)
                        _dataGridTable.CommitEdit(DataGridViewDataErrorContexts.Commit);
                };

                mainContainer.Controls.Add(_dataGridTable);
                mainContainer.Controls.Add(topPanel);
                serverTab.Controls.Add(mainContainer);

                _logger.Info("=== ВКЛАДКА ЗАГРУЖЕНА ===");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка загрузки вкладки: {ex.Message}");
                _logger.Error($"StackTrace: {ex.StackTrace}");
            }
        }
        private void ResetAllCheckboxes()
        {
            try
            {
                _logger.Info("=== СБРОС ЧЕКБОКСОВ ===");

                if (_dataGridTable == null)
                {
                    _logger.Warn("dataGrid == null, сброс отменён");
                    return;
                }

                foreach (DataGridViewRow row in _dataGridTable.Rows)
                {
                    row.Cells["Checked"].Value = false;
                }

                UpdateCounterLabel();
                _logger.Info("Все чекбоксы сброшены (Новая неделя)");
                SaveTableToExcel(_dataGridTable);
                SiegeCountdownTab.Recreate(tabControl1, _servers);

                _logger.Info("=== СБРОС ЗАВЕРШЕН ===");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка сброса: {ex.Message}");
            }
        }

        private void UpdateCounterLabel()
        {
            try
            {
                if (_dataGridTable == null)
                {
                    _logger.Warn("UpdateCounterLabel: _dataGridTable == null");
                    return;
                }

                int total = 0;
                int checkedCount = 0;

                foreach (DataGridViewRow row in _dataGridTable.Rows)
                {
                    if (row.Cells["Checked"].Value is bool isChecked)
                    {
                        total++;
                        if (isChecked) checkedCount++;
                    }
                }

                _logger.Info($"Счётчик: {checkedCount} / {total}");

                if (_lblCounter != null)
                {
                    _lblCounter.Text = $"✅ Выполнено: {checkedCount} / {total}";
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка обновления счётчика: {ex.Message}");
            }
        }

        private void cboWeekSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWeekSelection.SelectedItem != null)
            {
                string selectedWeek = cboWeekSelection.SelectedItem.ToString() ?? "Week 1-8. Универсальная неделя";
                if (_settings != null) _settings.SelectedWeek = selectedWeek;
                _cache.LoadWeekButtons(selectedWeek, _logger);
                _logger.Info($"Выбрана неделя: {selectedWeek}");
            }
        }

        private void cboServerSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboServerSelection.SelectedItem != null)
            {
                string selectedServer = cboServerSelection.SelectedItem.ToString() ?? "IV.Оазис Судьбы / VIII.Регис / IX.Ричез";
                if (_settings != null) _settings.SelectedServer = selectedServer;
                _siegeScheduler.SelectedServer = selectedServer;
                _logger.Info($"Выбран сервер: {selectedServer}");
            }
        }

        private void chkAlwaysMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAlwaysMode.Checked)
            {
                chkSiegeOnlyMode.Checked = false;
                chkExtendedSiege.Checked = false;
                chkExtendedSiege.Enabled = false;
                if (_settings != null)
                {
                    _settings.AlwaysMode = true;
                    _settings.SiegeOnlyMode = false;
                    _settings.ExtendedSiege = false;
                }
                _siegeScheduler.IsEnabled = false;
                _logger.Info("Режим 'Всегда' включен (планировщик отключен)");
            }
            else if (_settings != null)
            {
                _settings.AlwaysMode = false;
            }
        }

        private void chkSiegeOnlyMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSiegeOnlyMode.Checked)
            {
                chkAlwaysMode.Checked = false;
                chkExtendedSiege.Enabled = true;
                if (_settings != null)
                {
                    _settings.AlwaysMode = false;
                    _settings.SiegeOnlyMode = true;
                }
                _siegeScheduler.IsEnabled = true;
                _siegeScheduler.ExtendedSiege = chkExtendedSiege.Checked;
                _logger.Info("Режим 'Только осады' включен (планировщик активен)");
            }
            else
            {
                chkExtendedSiege.Enabled = false;
                chkExtendedSiege.Checked = false;
                if (_settings != null)
                {
                    _settings.SiegeOnlyMode = false;
                    _settings.ExtendedSiege = false;
                }
                _siegeScheduler.IsEnabled = false;
                _siegeScheduler.ExtendedSiege = false;
                _logger.Info("Режим 'Только осады' отключен");
            }
        }

        private void chkExtendedSiege_CheckedChanged(object sender, EventArgs e)
        {
            if (_settings != null) _settings.ExtendedSiege = chkExtendedSiege.Checked;
            if (_settings != null && _settings.SiegeOnlyMode)
                _siegeScheduler.ExtendedSiege = chkExtendedSiege.Checked;
            _logger.Info($"Расширенные осады: {(chkExtendedSiege.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkAssistantX1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAssistantX1.Checked)
            {
                chkAssistantX3.Checked = false;
                if (_settings != null)
                {
                    _settings.AssistantX1 = true;
                    _settings.AssistantX3 = false;
                }
            }
            _logger.Info($"Ассистент x1: {(chkAssistantX1.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkAssistantX3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAssistantX3.Checked)
            {
                chkAssistantX1.Checked = false;
                if (_settings != null)
                {
                    _settings.AssistantX1 = false;
                    _settings.AssistantX3 = true;
                }
            }
            _logger.Info($"Ассистент x3: {(chkAssistantX3.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkWeekBossX1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWeekBossX1.Checked)
            {
                chkWeekBossX3.Checked = false;
                if (_settings != null)
                {
                    _settings.WeekBossX1 = true;
                    _settings.WeekBossX3 = false;
                }
            }
            _logger.Info($"Недельные боссы x1: {(chkWeekBossX1.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkWeekBossX3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWeekBossX3.Checked)
            {
                chkWeekBossX1.Checked = false;
                if (_settings != null)
                {
                    _settings.WeekBossX1 = false;
                    _settings.WeekBossX3 = true;
                }
            }
            _logger.Info($"Недельные боссы x3: {(chkWeekBossX3.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkHiddenBossX1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHiddenBossX1.Checked)
            {
                chkHiddenBossX3.Checked = false;
                if (_settings != null)
                {
                    _settings.HiddenBossX1 = true;
                    _settings.HiddenBossX3 = false;
                }
            }
            _logger.Info($"Скрытые боссы x1: {(chkHiddenBossX1.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkHiddenBossX3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHiddenBossX3.Checked)
            {
                chkHiddenBossX1.Checked = false;
                if (_settings != null)
                {
                    _settings.HiddenBossX1 = false;
                    _settings.HiddenBossX3 = true;
                }
            }
            _logger.Info($"Скрытые боссы x3: {(chkHiddenBossX3.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkMobEasyX1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMobEasyX1.Checked)
            {
                chkMobEasyX3.Checked = false;
                if (_settings != null)
                {
                    _settings.MobEasyX1 = true;
                    _settings.MobEasyX3 = false;
                }
            }
            _logger.Info($"Слабые мобы x1: {(chkMobEasyX1.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkMobEasyX3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMobEasyX3.Checked)
            {
                chkMobEasyX1.Checked = false;
                if (_settings != null)
                {
                    _settings.MobEasyX1 = false;
                    _settings.MobEasyX3 = true;
                }
            }
            _logger.Info($"Слабые мобы x3: {(chkMobEasyX3.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkMobNormalX1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMobNormalX1.Checked)
            {
                chkMobNormalX3.Checked = false;
                if (_settings != null)
                {
                    _settings.MobNormalX1 = true;
                    _settings.MobNormalX3 = false;
                }
            }
            _logger.Info($"Средние мобы x1: {(chkMobNormalX1.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkMobNormalX3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMobNormalX3.Checked)
            {
                chkMobNormalX1.Checked = false;
                if (_settings != null)
                {
                    _settings.MobNormalX1 = false;
                    _settings.MobNormalX3 = true;
                }
            }
            _logger.Info($"Средние мобы x3: {(chkMobNormalX3.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkMobStrongX1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMobStrongX1.Checked)
            {
                chkMobStrongX3.Checked = false;
                if (_settings != null)
                {
                    _settings.MobStrongX1 = true;
                    _settings.MobStrongX3 = false;
                }
            }
            _logger.Info($"Сильные мобы x1: {(chkMobStrongX1.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkMobStrongX3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMobStrongX3.Checked)
            {
                chkMobStrongX1.Checked = false;
                if (_settings != null)
                {
                    _settings.MobStrongX1 = false;
                    _settings.MobStrongX3 = true;
                }
            }
            _logger.Info($"Сильные мобы x3: {(chkMobStrongX3.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private void chkOnlyScratch_CheckedChanged(object sender, EventArgs e)
        {
            if (_settings != null) _settings.OnlyScratch = chkOnlyScratch.Checked;
            _logger.Info($"Режим 'Только царапать': {(chkOnlyScratch.Checked ? "ВКЛ" : "ВЫКЛ")}");
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var settings = GetSettingsFromUIThreadSafe();
            bool isAssistantMode = chkAssistantX1.Checked || chkAssistantX3.Checked;
            if (isAssistantMode) await StartAssistantAsync(settings);
            else ManualStartBot();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopByHotkey();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}