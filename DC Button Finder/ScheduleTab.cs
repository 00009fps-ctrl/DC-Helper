using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DC_Button_Finder
{
    public static class ScheduleTab
    {
        private static DataGridView _grid;
        private static List<ServerInfo> _servers;
        private static Logger _logger;

        public static void Initialize(Logger logger)
        {
            _logger = logger;
        }

        public static void Create(TabControl tabControl, List<ServerInfo> servers)
        {
            if (servers == null)
            {
                _logger?.Error("ScheduleTab.Create: servers == null!");
                return;
            }

            _servers = servers;
            _logger?.Info($"ScheduleTab.Create: получено {_servers.Count} серверов");

            // Удаляем старую вкладку, если есть
            TabPage existingTab = null;
            foreach (TabPage page in tabControl.TabPages)
            {
                if (page.Text == "Расписание")
                {
                    existingTab = page;
                    break;
                }
            }

            if (existingTab != null)
            {
                _logger?.Info("Удаляем старую вкладку 'Расписание'");
                tabControl.TabPages.Remove(existingTab);
                existingTab.Dispose();
            }

            TabPage scheduleTab = new TabPage("Расписание");

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                GridColor = Color.LightGray,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            _grid.DefaultCellStyle.SelectionBackColor = _grid.DefaultCellStyle.BackColor;
            _grid.DefaultCellStyle.SelectionForeColor = _grid.DefaultCellStyle.ForeColor;

            _grid.Columns.Add("Hour", "Час");
            _grid.Columns["Hour"].Width = 60;
            _grid.Columns["Hour"].DefaultCellStyle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            _grid.Columns["Hour"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _grid.Columns["Hour"].SortMode = DataGridViewColumnSortMode.NotSortable;
            _grid.Columns["Hour"].ReadOnly = true;

            _grid.Columns.Add("Server", "Сервер");
            _grid.Columns["Server"].DefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            _grid.Columns["Server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            _grid.Columns["Server"].SortMode = DataGridViewColumnSortMode.NotSortable;

            var checkboxes = ServerManager.LoadCheckboxes();
            _logger?.Info($"Загружено чекбоксов: {checkboxes.Count}");

            _grid.Rows.Clear();

            for (int hour = 0; hour < 24; hour++)
            {
                int rowIndex = _grid.Rows.Add();
                _grid.Rows[rowIndex].Cells["Hour"].Value = $"{hour:00}:00";

                DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                comboCell.Items.Add("—");

                foreach (var server in _servers)
                {
                    if (checkboxes.ContainsKey(server.Id) && checkboxes[server.Id])
                        continue;
                    comboCell.Items.Add(server.Name);
                }

                comboCell.Value = "—";
                _grid.Rows[rowIndex].Cells["Server"] = comboCell;
            }

            _logger?.Info($"Добавлено 24 строки в таблицу");

            scheduleTab.Controls.Add(_grid);
            tabControl.TabPages.Add(scheduleTab);

            _logger?.Info($"Вкладка 'Расписание' добавлена. Всего вкладок: {tabControl.TabPages.Count}");
        }
    }
}