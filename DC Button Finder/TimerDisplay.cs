using System;
using System.Windows.Forms;

namespace DC_Button_Finder
{
    public sealed class TimerDisplay : IDisposable
    {
        private readonly SiegeScheduler _siegeScheduler;
        private readonly BotController _botController;
        private readonly Label _displayLabel;
        private readonly System.Windows.Forms.Timer _updateTimer;
        private DateTime _botStartTime;
        private bool _botIsRunning;

        public TimerDisplay(SiegeScheduler siegeScheduler, BotController botController, Label displayLabel)
        {
            _siegeScheduler = siegeScheduler;
            _botController = botController;
            _displayLabel = displayLabel;
            _botIsRunning = false;

            // Настраиваем таймер обновления
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 1000; // 1 секунда
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();

            // Инициализируем отображение
            UpdateDisplay();
        }

        public void BotStarted()
        {
            _botStartTime = DateTime.Now;
            _botIsRunning = true;
        }

        public void BotStopped()
        {
            _botIsRunning = false;
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_displayLabel.InvokeRequired)
            {
                _displayLabel.BeginInvoke(new Action(UpdateDisplay));
                return;
            }

            try
            {
                string displayText = "";

                if (_botIsRunning)
                {
                    // Показываем время работы бота
                    var runningTime = DateTime.Now - _botStartTime;
                    displayText = $"Работает: {runningTime:hh\\:mm\\:ss}";
                    _displayLabel.ForeColor = Color.DarkGreen;
                }
                else
                {
                    // Получаем время до начала или до конца осады
                    var timeInfo = _siegeScheduler.GetTimeUntilNextSiegeOrRemaining();

                    if (timeInfo.IsSiegeActive)
                    {
                        // Осада идет - показываем время до конца
                        displayText = $"До конца: {timeInfo.TimeRemaining:hh\\:mm\\:ss}";
                        _displayLabel.ForeColor = Color.DarkRed;
                    }
                    else
                    {
                        // Осады нет - показываем время до начала
                        if (timeInfo.TimeRemaining.TotalHours >= 1)
                        {
                            displayText = $"До осады: {timeInfo.TimeRemaining:hh\\:mm\\:ss}";
                        }
                        else if (timeInfo.TimeRemaining.TotalMinutes >= 1)
                        {
                            displayText = $"До осады: {timeInfo.TimeRemaining:mm\\:ss}";
                        }
                        else
                        {
                            displayText = $"До осады: {timeInfo.TimeRemaining:ss}с";
                        }

                        // Меняем цвет в зависимости от времени
                        if (timeInfo.TimeRemaining.TotalMinutes < 5)
                            _displayLabel.ForeColor = Color.DarkRed;
                        else if (timeInfo.TimeRemaining.TotalMinutes < 15)
                            _displayLabel.ForeColor = Color.DarkOrange;
                        else
                            _displayLabel.ForeColor = Color.DarkBlue;
                    }
                }

                _displayLabel.Text = displayText;
            }
            catch
            {
                _displayLabel.Text = "Таймер: ---";
            }
        }

        public void Dispose()
        {
            _updateTimer?.Stop();
            _updateTimer?.Dispose();
        }
    }
}