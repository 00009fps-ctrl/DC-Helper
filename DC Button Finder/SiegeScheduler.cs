using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace DC_Button_Finder
{
    public sealed class SiegeScheduler : IDisposable
    {
        private readonly BotController _botController;
        private readonly Logger _logger;
        private readonly Func<BotSettings> _getSettings;
        private readonly Action<bool> _updateUI;

        private System.Timers.Timer _siegeTimer;
        private bool _autoStarted = false;
        private bool _isEnabled = true;
        private readonly Random _random = new Random();
        private bool _launchScheduled = false;

        private readonly Dictionary<string, List<int>> _serverSchedules = new()
        {
            { "I.Изначальный мир / V.Гьеди Прайм / XII.Пиксис", new List<int> { 3, 8, 13, 18, 23 } },
            { "II.Западные территории / VI.Салуса Секундус / XI.Кайтайн / XIII.Вульпекула", new List<int> { 0, 4, 9, 14, 19 } },
            { "III.Омикрон Прайм / VII.Лас Эквестрия / X.Ахернар / XIV.Эридан", new List<int> { 5, 10, 15, 20, 1 } },
            { "IV.Оазис Судьбы / VIII.Регис / IX.Ричез", new List<int> { 2, 6, 11, 16, 21 } }
        };

        private const int CHECK_INTERVAL_MS = 30000;
        private const int EXTENDED_SIEGE_OFFSET_MINUTES = 30;

        private string _selectedServer = "IV.Оазис Судьбы / VIII.Регис / IX.Ричез";
        private bool _extendedSiege = false;
        private Dictionary<int, ScheduledAction> _schedule = new Dictionary<int, ScheduledAction>();

        public event Action<string>? OnSiegeStatusChanged;
        public event Action<string>? OnAutoAction;

        public string SelectedServer
        {
            get => _selectedServer;
            set
            {
                if (_serverSchedules.ContainsKey(value))
                {
                    _selectedServer = value;
                    _logger.Info($"Выбран сервер: {value}");
                    OnSiegeStatusChanged?.Invoke($"Сервер изменен на {value}");
                }
            }
        }

        public bool ExtendedSiege
        {
            get => _extendedSiege;
            set
            {
                _extendedSiege = value;
                _logger.Info($"Расширенные осады: {(value ? "ВКЛ" : "ВЫКЛ")}");
                OnSiegeStatusChanged?.Invoke($"Расширенные осады: {(value ? "ВКЛ" : "ВЫКЛ")}");
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                if (!value && _autoStarted)
                {
                    StopAutoBot();
                }
                if (!value)
                {
                    _launchScheduled = false;
                }
                _logger.Info($"Планировщик осад: {(value ? "ВКЛ" : "ВЫКЛ")}");
            }
        }

        public SiegeScheduler(
            BotController botController,
            Logger logger,
            Func<BotSettings> getSettings,
            Action<bool> updateUI)
        {
            _botController = botController;
            _logger = logger;
            _getSettings = getSettings;
            _updateUI = updateUI;
            _siegeTimer = new System.Timers.Timer(CHECK_INTERVAL_MS);

            // Загружаем расписание
            LoadSchedule();

            InitializeTimer();
        }

        private void LoadSchedule()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "schedule.dat");
                if (!File.Exists(path)) return;

                _schedule = new Dictionary<int, ScheduledAction>();

                using (var stream = new FileStream(path, FileMode.Open))
                using (var reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        int hour = reader.ReadInt32();
                        string server = reader.ReadString();
                        string action = reader.ReadString();
                        _schedule[hour] = new ScheduledAction { ServerName = server, Action = action };
                    }
                }

                _logger.Info($"Загружено расписание: {_schedule.Count} записей");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка загрузки расписания: {ex.Message}");
            }
        }

        public Dictionary<int, ScheduledAction> GetSchedule()
        {
            return _schedule;
        }

        public void RefreshSchedule()
        {
            LoadSchedule();
        }

        private void InitializeTimer()
        {
            _siegeTimer.Elapsed += CheckSiegeTime;
            _siegeTimer.AutoReset = true;
            _siegeTimer.Start();

            _logger.Info("Планировщик осад инициализирован");
        }

        private async void CheckSiegeTime(object? sender, ElapsedEventArgs e)
        {
            if (!_isEnabled) return;

            try
            {
                var now = DateTime.Now;
                var currentTime = now.TimeOfDay;

                // ===== ПРОВЕРКА РАСПИСАНИЯ =====
                if (_schedule.TryGetValue(now.Hour, out var scheduledAction))
                {
                    if (scheduledAction.Action == "Остановить")
                    {
                        if (_botController.IsRunning || _autoStarted)
                        {
                            _logger.Info($"Остановка по расписанию в {now.Hour:00}:00");
                            StopAutoBot();
                        }
                        return;
                    }

                    // Переключаем сервер
                    if (!string.IsNullOrEmpty(scheduledAction.ServerName) && scheduledAction.ServerName != "—")
                    {
                        if (_selectedServer != scheduledAction.ServerName)
                        {
                            _selectedServer = scheduledAction.ServerName;
                            _logger.Info($"Сервер переключен на {scheduledAction.ServerName} по расписанию");
                            OnSiegeStatusChanged?.Invoke($"Сервер изменен на {scheduledAction.ServerName} (по расписанию)");
                        }
                    }

                    // Запускаем бота (если не запущен)
                    if (!_botController.IsRunning && !_autoStarted)
                    {
                        _logger.Info($"Запуск бота по расписанию в {now.Hour:00}:00 ({scheduledAction.Action})");
                        await StartAutoBot();
                        return;
                    }
                }

                // ===== ОБЫЧНАЯ ЛОГИКА ОСАД =====
                var siegeHours = GetSiegeHoursForServer(_selectedServer);
                int startOffsetMinutes = _extendedSiege ? EXTENDED_SIEGE_OFFSET_MINUTES : 0;

                foreach (var siegeHour in siegeHours)
                {
                    var siegeStartTime = new TimeSpan(siegeHour, 0, 0);
                    var actualStartTime = siegeStartTime - TimeSpan.FromMinutes(startOffsetMinutes);
                    var checkStartTime = actualStartTime - TimeSpan.FromSeconds(180);

                    if (currentTime >= checkStartTime && currentTime < actualStartTime)
                    {
                        if (!_launchScheduled && !_botController.IsRunning && !_autoStarted)
                        {
                            var secondsUntilSiege = (actualStartTime - currentTime).TotalSeconds;
                            int delaySeconds = _random.Next(0, Math.Min(181, (int)secondsUntilSiege + 1));

                            _logger.Info($"Осада в {siegeHour:00}:00. Запуск через {delaySeconds} сек");
                            _launchScheduled = true;

                            if (delaySeconds > 0)
                            {
                                await Task.Delay(delaySeconds * 1000);
                            }

                            await StartAutoBot();
                            return;
                        }
                    }
                }

                foreach (var siegeHour in siegeHours)
                {
                    int endOffsetMinutes = _extendedSiege ? EXTENDED_SIEGE_OFFSET_MINUTES : 0;
                    var siegeEndTime = new TimeSpan((siegeHour + 1) % 24, 0, 0);
                    if (siegeEndTime == TimeSpan.Zero) siegeEndTime = new TimeSpan(24, 0, 0);

                    var actualEndTime = siegeEndTime + TimeSpan.FromMinutes(endOffsetMinutes);
                    var checkEndTime = actualEndTime + TimeSpan.FromSeconds(60);

                    if (currentTime >= actualEndTime && currentTime < checkEndTime)
                    {
                        if (_botController.IsRunning && _autoStarted)
                        {
                            var secondsAfterSiege = (currentTime - actualEndTime).TotalSeconds;
                            int remainingDelay = _random.Next(0, 61 - (int)secondsAfterSiege);

                            _logger.Info($"Осада закончилась в {actualEndTime:hh\\:mm}. Остановка через {remainingDelay} сек");

                            if (remainingDelay > 0)
                            {
                                await Task.Delay(remainingDelay * 1000);
                            }

                            StopAutoBot();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка в планировщике осад: {ex.Message}");
            }
        }

        private List<int> GetSiegeHoursForServer(string server)
        {
            if (_serverSchedules.TryGetValue(server, out var hours))
                return hours;
            return _serverSchedules["IV.Оазис Судьбы / VIII.Регис / IX.Ричез"];
        }

        private async Task StartAutoBot()
        {
            try
            {
                _autoStarted = true;
                _launchScheduled = false;
                OnSiegeStatusChanged?.Invoke("Обнаружено начало осады");
                OnAutoAction?.Invoke("Автоматический запуск бота");

                var settings = _getSettings();
                _updateUI(true);

                await _botController.StartAsync(settings);

                _logger.Success("Бот автоматически запущен по расписанию осады");
                OnSiegeStatusChanged?.Invoke("Бот запущен автоматически");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка автоматического запуска: {ex.Message}");
                _autoStarted = false;
                _launchScheduled = false;
                _updateUI(false);
                OnSiegeStatusChanged?.Invoke($"Ошибка запуска: {ex.Message}");
            }
        }

        private void StopAutoBot()
        {
            try
            {
                OnSiegeStatusChanged?.Invoke("Осада заканчивается");
                OnAutoAction?.Invoke("Автоматическая остановка бота");

                _botController.Stop();
                _updateUI(false);
                _autoStarted = false;
                _launchScheduled = false;

                _logger.Success("Бот автоматически остановлен по окончанию осады");
                OnSiegeStatusChanged?.Invoke("Бот остановлен автоматически");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка автоматической остановки: {ex.Message}");
                OnSiegeStatusChanged?.Invoke($"Ошибка остановки: {ex.Message}");
            }
        }

        public List<SiegeTimeSlot> GetSiegeSchedule()
        {
            var schedule = new List<SiegeTimeSlot>();
            var siegeHours = GetSiegeHoursForServer(_selectedServer);
            int offsetMinutes = _extendedSiege ? EXTENDED_SIEGE_OFFSET_MINUTES : 0;

            foreach (var startHour in siegeHours)
            {
                int endHour = (startHour + 1) % 24;
                schedule.Add(new SiegeTimeSlot
                {
                    StartHour = startHour,
                    EndHour = endHour,
                    StartTime = new TimeSpan(startHour, 0, 0) - TimeSpan.FromMinutes(offsetMinutes),
                    EndTime = new TimeSpan(endHour, 0, 0) + TimeSpan.FromMinutes(offsetMinutes)
                });
            }
            return schedule;
        }

        public bool IsSiegeTimeNow()
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var siegeHours = GetSiegeHoursForServer(_selectedServer);
            int offsetMinutes = _extendedSiege ? EXTENDED_SIEGE_OFFSET_MINUTES : 0;

            foreach (var siegeHour in siegeHours)
            {
                var startTime = new TimeSpan(siegeHour, 0, 0) - TimeSpan.FromMinutes(offsetMinutes);
                var endHour = (siegeHour + 1) % 24;
                var endTime = new TimeSpan(endHour, 0, 0) + TimeSpan.FromMinutes(offsetMinutes);
                if (endTime == TimeSpan.Zero) endTime = new TimeSpan(24, 0, 0);

                if (currentTime >= startTime && currentTime < endTime)
                {
                    return true;
                }
            }
            return false;
        }

        public TimeSpan GetTimeUntilNextSiege()
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            int offsetMinutes = _extendedSiege ? EXTENDED_SIEGE_OFFSET_MINUTES : 0;

            var siegeHours = GetSiegeHoursForServer(_selectedServer);

            var nextSiegeToday = siegeHours
                .Select(h => new TimeSpan(h, 0, 0) - TimeSpan.FromMinutes(offsetMinutes))
                .Where(t => t > currentTime)
                .OrderBy(t => t)
                .FirstOrDefault();

            if (nextSiegeToday != TimeSpan.Zero)
            {
                return nextSiegeToday - currentTime;
            }

            var firstSiegeTomorrow = new TimeSpan(siegeHours[0], 0, 0) - TimeSpan.FromMinutes(offsetMinutes);
            return (firstSiegeTomorrow + TimeSpan.FromDays(1)) - currentTime;
        }

        public void Dispose()
        {
            _siegeTimer?.Stop();
            _siegeTimer?.Dispose();
        }

        public SiegeTimeInfo GetTimeUntilNextSiegeOrRemaining()
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            int offsetMinutes = _extendedSiege ? EXTENDED_SIEGE_OFFSET_MINUTES : 0;

            var siegeHours = GetSiegeHoursForServer(_selectedServer);

            foreach (var siegeHour in siegeHours)
            {
                var startTime = new TimeSpan(siegeHour, 0, 0) - TimeSpan.FromMinutes(offsetMinutes);
                var endHour = (siegeHour + 1) % 24;
                var endTime = new TimeSpan(endHour, 0, 0) + TimeSpan.FromMinutes(offsetMinutes);
                if (endTime == TimeSpan.Zero) endTime = new TimeSpan(24, 0, 0);

                if (currentTime >= startTime && currentTime < endTime)
                {
                    return new SiegeTimeInfo
                    {
                        IsSiegeActive = true,
                        TimeRemaining = endTime - currentTime
                    };
                }
            }

            return new SiegeTimeInfo
            {
                IsSiegeActive = false,
                TimeRemaining = GetTimeUntilNextSiege()
            };
        }

        public class SiegeTimeInfo
        {
            public bool IsSiegeActive { get; set; }
            public TimeSpan TimeRemaining { get; set; }
        }
    }

    public class SiegeTimeSlot
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class ScheduledAction
    {
        public string ServerName { get; set; } = "";
        public string Action { get; set; } = "";
    }
}