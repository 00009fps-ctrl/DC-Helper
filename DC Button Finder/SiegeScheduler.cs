using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Threading.Tasks;

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
        private bool _launchScheduled = false; // Флаг, что запуск уже запланирован для этой осады

        // Расписание для серверов (групп)
        private readonly Dictionary<string, List<int>> _serverSchedules = new()
        {
            // Группа 1:
            { "I.Изначальный мир / V.Гьеди Прайм / XII.Пиксис", new List<int> { 3, 8, 13, 18, 23 } },
            
            // Группа 2:
            { "II.Западные территории / VI.Салуса Секундус / XI.Кайтайн / XIII.Вульпекула", new List<int> { 0, 4, 9, 14, 19 } },
            
            // Группа 3:
            { "III.Омикрон Прайм / VII.Лас Эквестрия / X.Ахернар / XIV.Эридан", new List<int> { 5, 10, 15, 20, 1 } },
            
            // Группа 4:
            { "IV.Оазис Судьбы / VIII.Регис / IX.Ричез", new List<int> { 2, 6, 11, 16, 21 } }
        };

        private const int CHECK_INTERVAL_MS = 30000; // 30 секунд

        private string _selectedServer = "IV.Оазис Судьбы / VIII.Регис / IX.Ричез";
        private bool _extendedSiege = false;
        private bool _startedByExtended = false;

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
                    _launchScheduled = false; // Сбрасываем флаг при отключении
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

            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _siegeTimer.Elapsed += CheckSiegeTime;
            _siegeTimer.AutoReset = true;
            _siegeTimer.Start();

            LogSiegeSchedule();
            _logger.Info("Планировщик осад инициализирован");
        }

        private async void CheckSiegeTime(object? sender, ElapsedEventArgs e)
        {
            if (!_isEnabled) return;

            try
            {
                var now = DateTime.Now;
                int currentHour = now.Hour;
                int currentMinute = now.Minute;
                int currentSecond = now.Second;
                var currentTime = now.TimeOfDay;

                // Получаем расписание для текущего сервера
                var siegeHours = GetSiegeHoursForServer(_selectedServer);

                // 1. ПРОВЕРЯЕМ НАЧАЛО ОСАДЫ (за 0-180 сек ДО начала)
                foreach (var siegeHour in siegeHours)
                {
                    var siegeStartTime = new TimeSpan(siegeHour, 0, 0);

                    // Рассчитываем время, когда нужно начать проверку
                    var checkStartTime = siegeStartTime - TimeSpan.FromSeconds(180); // За 3 минуты до

                    // Если текущее время между checkStartTime и siegeStartTime
                    if (currentTime >= checkStartTime && currentTime < siegeStartTime)
                    {
                        // Если ещё не запланировали запуск для этой осады
                        if (!_launchScheduled && !_botController.IsRunning && !_autoStarted)
                        {
                            // Вычисляем сколько секунд осталось до начала осады
                            var secondsUntilSiege = (siegeStartTime - currentTime).TotalSeconds;

                            // Запускаем с рандомной задержкой 0-180 сек, но не позже начала осады
                            int delaySeconds = _random.Next(0, Math.Min(181, (int)secondsUntilSiege + 1));

                            _logger.Info($"Осада в {siegeHour:00}:00. Запуск через {delaySeconds} сек (за {secondsUntilSiege - delaySeconds:F0} сек до начала)");

                            _launchScheduled = true; // Запоминаем, что запуск запланирован

                            if (delaySeconds > 0)
                            {
                                await Task.Delay(delaySeconds * 1000);
                            }

                            await StartAutoBot();
                            return;
                        }
                    }
                }

                // 2. ПРОВЕРЯЕМ ОКОНЧАНИЕ ОСАДЫ (0-60 сек ПОСЛЕ окончания)
                foreach (var siegeHour in siegeHours)
                {
                    var siegeEndTime = new TimeSpan((siegeHour + 1) % 24, 0, 0); // Осада длится 1 час
                    if (siegeEndTime == TimeSpan.Zero) siegeEndTime = new TimeSpan(24, 0, 0);

                    var checkEndTime = siegeEndTime + TimeSpan.FromSeconds(60); // 60 сек после окончания

                    // Если текущее время между окончанием осады и checkEndTime
                    if (currentTime >= siegeEndTime && currentTime < checkEndTime)
                    {
                        // И бот еще работает
                        if (_botController.IsRunning && _autoStarted)
                        {
                            // Вычисляем сколько секунд прошло после окончания
                            var secondsAfterSiege = (currentTime - siegeEndTime).TotalSeconds;

                            // Останавливаем с рандомной задержкой 0-60 сек
                            int remainingDelay = _random.Next(0, 61 - (int)secondsAfterSiege);

                            _logger.Info($"Осада закончилась в {siegeEndTime:hh\\:mm}. Остановка через {remainingDelay} сек");

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

            // По умолчанию возвращаем расписание
            return _serverSchedules["IV.Оазис Судьбы / VIII.Регис / IX.Ричез"];
        }

        private async Task StartAutoBot()
        {
            try
            {
                _autoStarted = true;
                _launchScheduled = false; // Сбрасываем флаг после запуска
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
                _startedByExtended = false;
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
                _startedByExtended = false;
                _launchScheduled = false; // Сбрасываем флаг при остановке

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

            foreach (var startHour in siegeHours)
            {
                int endHour = (startHour + 1) % 24;
                schedule.Add(new SiegeTimeSlot
                {
                    StartHour = startHour,
                    EndHour = endHour,
                    StartTime = new TimeSpan(startHour, 0, 0),
                    EndTime = new TimeSpan(endHour, 0, 0)
                });
            }
            return schedule;
        }

        public void AddSiegeTime(int hour)
        {
            var siegeHours = GetSiegeHoursForServer(_selectedServer);
            if (!siegeHours.Contains(hour))
            {
                siegeHours.Add(hour);
                siegeHours.Sort();
                _logger.Info($"Добавлено время осады: {hour:00}:00");
                LogSiegeSchedule();
            }
        }

        public void RemoveSiegeTime(int hour)
        {
            var siegeHours = GetSiegeHoursForServer(_selectedServer);
            if (siegeHours.Contains(hour))
            {
                siegeHours.Remove(hour);
                _logger.Info($"Удалено время осады: {hour:00}:00");
                LogSiegeSchedule();
            }
        }

        public bool IsSiegeTimeNow()
        {
            var now = DateTime.Now;
            int currentHour = now.Hour;
            int currentMinute = now.Minute;

            var siegeHours = GetSiegeHoursForServer(_selectedServer);
            return siegeHours.Contains(currentHour) && currentMinute < 55;
        }

        public TimeSpan GetTimeUntilNextSiege()
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;

            var siegeHours = GetSiegeHoursForServer(_selectedServer);

            // Находим ближайшую осаду сегодня
            var nextSiegeToday = siegeHours
                .Select(h => new TimeSpan(h, 0, 0))
                .FirstOrDefault(t => t > currentTime);

            if (nextSiegeToday != TimeSpan.Zero)
            {
                return nextSiegeToday - currentTime;
            }

            // Если осад сегодня больше не будет, берем первую завтра
            var firstSiegeTomorrow = new TimeSpan(siegeHours[0], 0, 0);
            return (firstSiegeTomorrow + TimeSpan.FromDays(1)) - currentTime;
        }

        private void LogSiegeSchedule()
        {
            _logger.Info($"=== ТЕКУЩЕЕ РАСПИСАНИЕ ОСАД (Сервер: {_selectedServer}) ===");
            foreach (var slot in GetSiegeSchedule())
            {
                _logger.Info($"  {slot.StartTime:hh\\:mm} - {slot.EndTime:hh\\:mm}");
            }
            _logger.Info("===============================");
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
            int currentHour = now.Hour;
            int currentMinute = now.Minute;

            var siegeHours = GetSiegeHoursForServer(_selectedServer);

            // Проверяем, идет ли сейчас осада
            foreach (var siegeHour in siegeHours)
            {
                int siegeEndHour = (siegeHour + 1) % 24;

                // Если текущий час равен часу осады
                if (currentHour == siegeHour)
                {
                    // Осада идет - считаем сколько осталось
                    var siegeEndTime = new TimeSpan(siegeEndHour, 0, 0);
                    if (siegeEndHour == 0) siegeEndTime = new TimeSpan(24, 0, 0); // Полночь

                    var timeRemaining = siegeEndTime - currentTime;
                    return new SiegeTimeInfo
                    {
                        IsSiegeActive = true,
                        TimeRemaining = timeRemaining
                    };
                }
            }

            // Осады нет - считаем до следующей
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
}