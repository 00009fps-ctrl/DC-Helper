using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DC_Button_Finder
{
    public static class SiegeCountdownTab
    {
        private static DataGridView _grid;
        private static System.Windows.Forms.Timer _timer;
        private static Logger _logger;

        public static void Initialize(Logger logger)
        {
            _logger = logger;
        }

        public static void Create(TabControl tabControl, List<ServerInfo> servers)
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                if (page.Text == "Осады ⏳")
                    return;
            }

            TabPage countdownTab = new TabPage("Осады ⏳");

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ColumnHeadersHeight = 35,
                RowTemplate = { Height = 35 },
                GridColor = Color.LightGray,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _grid.DefaultCellStyle.SelectionBackColor = _grid.DefaultCellStyle.BackColor;
            _grid.DefaultCellStyle.SelectionForeColor = _grid.DefaultCellStyle.ForeColor;

            _grid.Columns.Add("Server", "Сервер");
            _grid.Columns["Server"].DefaultCellStyle.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            _grid.Columns["Server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            _grid.Columns["Server"].SortMode = DataGridViewColumnSortMode.NotSortable;

            _grid.Columns.Add("Countdown", "До осады");
            _grid.Columns["Countdown"].DefaultCellStyle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            _grid.Columns["Countdown"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _grid.Columns["Countdown"].SortMode = DataGridViewColumnSortMode.NotSortable;

            _grid.Columns["Server"].FillWeight = 40;
            _grid.Columns["Countdown"].FillWeight = 60;

            _grid.ReadOnly = true;
            _grid.Enabled = false;

            var checkboxes = ServerManager.LoadCheckboxes();

            if (_logger != null)
            {
                _logger.Info($"Загружено чекбоксов: {checkboxes.Count}");
                foreach (var kvp in checkboxes)
                {
                    _logger.Info($"  Сервер {kvp.Key}: {(kvp.Value ? "☑" : "☐")}");
                }
            }

            var scheduleData = new[]
            {
                new { Server = "I. Изначальный мир", Times = new[] { "3:00", "8:00", "13:00", "18:00", "23:00" }, ServerId = 1 },
                new { Server = "II. Западные территории", Times = new[] { "4:00", "9:00", "14:00", "19:00", "00:00" }, ServerId = 2 },
                new { Server = "III. Омикрон Прайм", Times = new[] { "5:00", "10:00", "15:00", "20:00", "01:00" }, ServerId = 3 },
                new { Server = "IV. Оазис Судьбы", Times = new[] { "6:00", "11:00", "16:00", "21:00", "02:00" }, ServerId = 4 },
                new { Server = "V. Гьеди Прайм", Times = new[] { "3:00", "8:00", "13:00", "18:00", "23:00" }, ServerId = 5 },
                new { Server = "VI. Салуса Секундус", Times = new[] { "4:00", "9:00", "14:00", "19:00", "00:00" }, ServerId = 6 },
                new { Server = "VII. Лас Эквестрия", Times = new[] { "5:00", "10:00", "15:00", "20:00", "01:00" }, ServerId = 7 },
                new { Server = "VIII. Регис", Times = new[] { "6:00", "11:00", "16:00", "21:00", "02:00" }, ServerId = 8 },
                new { Server = "IX. Ричез", Times = new[] { "6:00", "11:00", "16:00", "21:00", "02:00" }, ServerId = 9 },
                new { Server = "X. Ахернар", Times = new[] { "5:00", "10:00", "15:00", "20:00", "01:00" }, ServerId = 10 },
                new { Server = "XI. Кайтайн", Times = new[] { "4:00", "9:00", "14:00", "19:00", "00:00" }, ServerId = 11 },
                new { Server = "XII. Пиксис", Times = new[] { "3:00", "8:00", "13:00", "18:00", "23:00" }, ServerId = 12 },
                new { Server = "XIII. Вульпекула", Times = new[] { "4:00", "9:00", "14:00", "19:00", "00:00" }, ServerId = 13 },
                new { Server = "XIV. Эридан", Times = new[] { "5:00", "10:00", "15:00", "20:00", "01:00" }, ServerId = 14 },
                new { Server = "XV. Кетус", Times = new[] { "6:00", "11:00", "16:00", "21:00", "02:00" }, ServerId = 15 },
                new { Server = "XVI. Аэтерис", Times = new[] { "6:00", "11:00", "16:00", "21:00", "02:00" }, ServerId = 16 },
                new { Server = "XVII. Талас", Times = new[] { "4:00", "9:00", "14:00", "19:00", "00:00" }, ServerId = 17 },
                new { Server = "XVIII. Наракин", Times = new[] { "5:00", "10:00", "15:00", "20:00", "01:00" }, ServerId = 18 },
                new { Server = "XIX. Эокс", Times = new[] { "6:00", "11:00", "16:00", "21:00", "02:00" }, ServerId = 19 }
            };

            int addedCount = 0;
            foreach (var item in scheduleData)
            {
                bool isChecked = checkboxes.ContainsKey(item.ServerId) && checkboxes[item.ServerId];

                if (isChecked)
                {
                    if (_logger != null)
                        _logger.Info($"Сервер {item.ServerId} ({item.Server}) — скрыт (галочка есть)");
                    continue;
                }

                int rowIndex = _grid.Rows.Add();
                _grid.Rows[rowIndex].Cells["Server"].Value = item.Server;
                _grid.Rows[rowIndex].Tag = item.Times;
                _grid.Rows[rowIndex].Cells["Countdown"].Value = "--:--:--";
                addedCount++;
            }

            if (_logger != null)
                _logger.Info($"Добавлено серверов в таблицу осад: {addedCount}");

            countdownTab.Controls.Add(_grid);
            tabControl.TabPages.Add(countdownTab);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += (s, e) => UpdateCountdown();
            _timer.Start();

            UpdateCountdown();
        }

        public static void Recreate(TabControl tabControl, List<ServerInfo> servers)
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                if (page.Text == "Осады ⏳")
                {
                    tabControl.TabPages.Remove(page);
                    _timer?.Stop();
                    _timer?.Dispose();
                    _grid?.Dispose();
                    break;
                }
            }

            Create(tabControl, servers);
        }

        private static void UpdateCountdown()
        {
            try
            {
                if (_grid == null || _grid.IsDisposed) return;

                var now = DateTime.Now;
                TimeZoneInfo mskZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
                DateTime mskNow = TimeZoneInfo.ConvertTime(now, mskZone);

                foreach (DataGridViewRow row in _grid.Rows)
                {
                    if (row.Tag is string[] times)
                    {
                        bool isSiegeActive = false;
                        TimeSpan? minDiff = null;

                        foreach (string timeStr in times)
                        {
                            var parts = timeStr.Split(':');
                            if (parts.Length == 2 && int.TryParse(parts[0], out int hour) && int.TryParse(parts[1], out int minute))
                            {
                                // ОСАДА ИДЕТ: если текущий час равен часу осады (от 00 до 00 следующего часа)
                                if (mskNow.Hour == hour)
                                {
                                    isSiegeActive = true;
                                    break;
                                }

                                DateTime todaySiege = new DateTime(mskNow.Year, mskNow.Month, mskNow.Day, hour, minute, 0);
                                if (todaySiege <= mskNow)
                                    todaySiege = todaySiege.AddDays(1);

                                TimeSpan diff = todaySiege - mskNow;
                                if (minDiff == null || diff < minDiff)
                                {
                                    minDiff = diff;
                                }
                            }
                        }

                        if (isSiegeActive)
                        {
                            row.Cells["Countdown"].Value = "🔴 Осада идёт!";
                            row.Cells["Countdown"].Style.ForeColor = Color.Red;
                            row.Cells["Countdown"].Style.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                        }
                        else if (minDiff.HasValue)
                        {
                            row.Cells["Countdown"].Value = FormatCountdown(minDiff.Value);

                            if (minDiff.Value.TotalMinutes < 30)
                                row.Cells["Countdown"].Style.ForeColor = Color.Red;
                            else if (minDiff.Value.TotalMinutes < 60)
                                row.Cells["Countdown"].Style.ForeColor = Color.Orange;
                            else
                                row.Cells["Countdown"].Style.ForeColor = Color.DarkGreen;
                        }
                    }
                }
            }
            catch { }
        }

        private static string FormatCountdown(TimeSpan time)
        {
            if (time.TotalHours >= 24)
                return $"{time.Days}д {time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}";
            if (time.TotalHours >= 1)
                return $"{time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}";
            return $"{time.Minutes:00}:{time.Seconds:00}";
        }
    }
}