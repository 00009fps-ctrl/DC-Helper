using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

                // Режимы работы
                AlwaysMode = chkAlwaysMode.Checked,
                SiegeOnlyMode = chkSiegeOnlyMode.Checked,
                ExtendedSiege = chkExtendedSiege.Checked,
                OnlyScratch = chkOnlyScratch.Checked,

                // Ассистент
                AssistantX1 = chkAssistantX1.Checked,
                AssistantX3 = chkAssistantX3.Checked,

                // НОВЫЕ ПОЛЯ - режимы атаки для всех групп
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

                if (id == HOTKEY_ID_START)
                {
                    StartByHotkey();
                }
                else if (id == HOTKEY_ID_STOP)
                {
                    StopByHotkey();
                }
            }
            base.WndProc(ref m);
        }

        private async void StartByHotkey()
        {
            try
            {
                var settings = GetSettingsFromUIThreadSafe();
                bool isAssistantMode = chkAssistantX1.Checked || chkAssistantX3.Checked;

                if (isAssistantMode)
                {
                    await StartAssistantAsync(settings);
                }
                else
                {
                    ManualStartBot();
                }
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
            if (_assistantEngine.IsRunning)
            {
                StopAssistant();
            }
            else if (_botController.IsRunning)
            {
                ManualStopBot();
            }
            else
            {
                _logger.Warn("Ассистент или бот уже остановлены");
            }
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

            // НОВЫЕ ПОЛЯ - режимы атаки для всех групп
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

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _settings = _settingsManager.LoadSettings();
                ApplySettingsToUI(_settings);

                // Загружаем состояние чекбоксов и заметок
                LoadServerInfoTab();

                _siegeScheduler.SelectedServer = _settings.SelectedServer;
                _siegeScheduler.ExtendedSiege = _settings.ExtendedSiege;
                _siegeScheduler.IsEnabled = _settings.SiegeOnlyMode;

                await LoadAllTemplatesAsync(_settings);
                UpdateUIForRunningState(false);
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка инициализации: {ex.Message}");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Сохраняем состояние вкладки
                SaveServerInfoTab();

                _timerDisplay?.Dispose();
                _siegeScheduler?.Dispose();
                _botController.Stop();
                _assistantEngine?.Stop();

                _settings = GetSettingsFromUIThreadSafe();

                if (_settings != null)
                {
                    _settingsManager.SaveSettings(_settings, this);
                }

                UnregisterHotKey(Handle, HOTKEY_ID_START);
                UnregisterHotKey(Handle, HOTKEY_ID_STOP);

                _logger.Success("Программа завершена, настройки сохранены");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка при закрытии: {ex.Message}");
            }
        }

        private void cboWeekSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWeekSelection.SelectedItem != null)
            {
                string selectedWeek = cboWeekSelection.SelectedItem.ToString() ?? "Week 1-8. Универсальная неделя";

                if (_settings != null)
                {
                    _settings.SelectedWeek = selectedWeek;
                }

                _cache.LoadWeekButtons(selectedWeek, _logger);
                _logger.Info($"Выбрана неделя: {selectedWeek}");
            }
        }

        private void cboServerSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboServerSelection.SelectedItem != null)
            {
                string selectedServer = cboServerSelection.SelectedItem.ToString() ?? "IV.Оазис Судьбы / VIII.Регис / IX.Ричез";

                if (_settings != null)
                {
                    _settings.SelectedServer = selectedServer;
                }

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
            {
                _siegeScheduler.ExtendedSiege = chkExtendedSiege.Checked;
            }
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

        // Для недельных боссов
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

        // Для скрытых боссов
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

        // Для слабых мобов
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

        // Для средних мобов
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

        // Для сильных мобов
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

            if (isAssistantMode)
            {
                await StartAssistantAsync(settings);
            }
            else
            {
                ManualStartBot();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopByHotkey();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]

        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [System.Runtime.InteropServices.DllImport("user32.dll")]

        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private void lblWeekBoss_Click(object sender, EventArgs e)
        {

        }

        private void lblMobsStrong_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // Сохранить состояние вкладки "Сервер инфо"
        private void SaveServerInfoTab()
        {
            try
            {
                TabPage serverTab = null;
                foreach (TabPage page in tabControl1.TabPages)
                {
                    if (page.Text == "Сервер инфо") { serverTab = page; break; }
                }
                if (serverTab == null) return;

                Panel panel = null;
                foreach (Control ctrl in serverTab.Controls)
                {
                    if (ctrl is Panel p) { panel = p; break; }
                }
                if (panel == null) return;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is Panel row)
                    {
                        bool? box = null;
                        string note = "";
                        foreach (Control inner in row.Controls)
                        {
                            if (inner is CheckBox cb) box = cb.Checked;
                            if (inner is TextBox tb) note = tb.Text;
                        }
                        if (box.HasValue)
                        {
                            sb.AppendLine((box.Value ? "1" : "0") + "|" + note.Replace("\n", " ").Replace("\r", " "));
                        }
                    }
                }
                System.IO.File.WriteAllText("server_data.txt", sb.ToString());
                _logger.Info("Серверы сохранены");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка сохранения: {ex.Message}");
            }
        }

        // Загрузить состояние вкладки "Сервер инфо"
        private void LoadServerInfoTab()
        {
            try
            {
                if (!System.IO.File.Exists("server_data.txt")) return;

                string[] lines = System.IO.File.ReadAllLines("server_data.txt");
                if (lines.Length == 0) return;

                TabPage serverTab = null;
                foreach (TabPage page in tabControl1.TabPages)
                {
                    if (page.Text == "Сервер инфо") { serverTab = page; break; }
                }
                if (serverTab == null) return;

                Panel panel = null;
                foreach (Control ctrl in serverTab.Controls)
                {
                    if (ctrl is Panel p) { panel = p; break; }
                }
                if (panel == null) return;

                int idx = 0;
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is Panel row && idx < lines.Length)
                    {
                        string[] parts = lines[idx].Split('|');
                        if (parts.Length >= 2)
                        {
                            foreach (Control inner in row.Controls)
                            {
                                if (inner is CheckBox cb) cb.Checked = (parts[0] == "1");
                                if (inner is TextBox tb) tb.Text = parts[1];
                            }
                        }
                        idx++;
                    }
                }
                _logger.Info("Серверы загружены");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}