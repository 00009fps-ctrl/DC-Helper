using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DC_Button_Finder
{
    public class ServerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public static class ServerManager
    {
        private static readonly string _serverListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "servers.txt");
        private static readonly string _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server_data.txt");

        public static List<ServerInfo> LoadServers()
        {
            var result = new List<ServerInfo>();

            try
            {
                if (File.Exists(_serverListPath))
                {
                    var lines = File.ReadAllLines(_serverListPath);
                    int id = 1;
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            result.Add(new ServerInfo { Id = id, Name = line.Trim() });
                            id++;
                        }
                    }
                }
                else
                {
                    return GetDefaultServers();
                }
            }
            catch
            {
                return GetDefaultServers();
            }

            return result.Count > 0 ? result : GetDefaultServers();
        }

        public static Dictionary<int, bool> LoadCheckboxes()
        {
            var result = new Dictionary<int, bool>();
            try
            {
                if (!File.Exists(_dataPath)) return result;

                var lines = File.ReadAllLines(_dataPath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 3 && int.TryParse(parts[0], out int id))
                    {
                        result[id] = parts[1] == "1";
                    }
                }
            }
            catch { }
            return result;
        }

        public static Dictionary<int, string> LoadNotes()
        {
            var result = new Dictionary<int, string>();
            try
            {
                if (!File.Exists(_dataPath)) return result;

                var lines = File.ReadAllLines(_dataPath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 3 && int.TryParse(parts[0], out int id))
                    {
                        result[id] = parts.Length > 2 ? parts[2] : "";
                    }
                }
            }
            catch { }
            return result;
        }

        // ===== ЗАГРУЗКА СЧЁТЧИКОВ =====
        public static Dictionary<string, decimal> LoadCounters()
        {
            var result = new Dictionary<string, decimal>();
            try
            {
                string countersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server_counters.txt");
                if (!File.Exists(countersPath)) return result;

                var lines = File.ReadAllLines(countersPath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2 && decimal.TryParse(parts[1], out decimal value))
                    {
                        result[parts[0]] = value;
                    }
                }
            }
            catch { }
            return result;
        }

        // ===== СОХРАНЕНИЕ ВСЕХ ДАННЫХ (ЧЕКБОКСЫ + ЗАМЕТКИ + СЧЁТЧИКИ) =====
        public static void SaveData(
            List<ServerInfo> servers,
            Dictionary<int, bool> checkboxes,
            Dictionary<int, string> notes,
            Dictionary<string, decimal> counters)
        {
            try
            {
                // Сохраняем чекбоксы и заметки
                var lines = new List<string>();
                foreach (var server in servers)
                {
                    bool isChecked = checkboxes.ContainsKey(server.Id) && checkboxes[server.Id];
                    string note = notes.ContainsKey(server.Id) ? notes[server.Id] : "";
                    lines.Add($"{server.Id}|{(isChecked ? "1" : "0")}|{note}");
                }
                File.WriteAllLines(_dataPath, lines);

                // Сохраняем счётчики
                var counterLines = counters.Select(kvp => $"{kvp.Key}|{kvp.Value}").ToList();
                string countersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server_counters.txt");
                File.WriteAllLines(countersPath, counterLines);
            }
            catch { }
        }

        private static List<ServerInfo> GetDefaultServers()
        {
            return new List<ServerInfo>
            {
                new ServerInfo { Id = 1, Name = "I. Изначальный мир" },
                new ServerInfo { Id = 2, Name = "II. Западные территории" },
                new ServerInfo { Id = 3, Name = "III. Омикрон Прайм" },
                new ServerInfo { Id = 4, Name = "IV. Оазис Судьбы" },
                new ServerInfo { Id = 5, Name = "V. Гьеди Прайм" },
                new ServerInfo { Id = 6, Name = "VI. Салуса Секундус" },
                new ServerInfo { Id = 7, Name = "VII. Лас Эквестрия" },
                new ServerInfo { Id = 8, Name = "VIII. Регис" },
                new ServerInfo { Id = 9, Name = "IX. Ричез" },
                new ServerInfo { Id = 10, Name = "X. Ахернар" },
                new ServerInfo { Id = 11, Name = "XI. Кайтайн" },
                new ServerInfo { Id = 12, Name = "XII. Пиксис" },
                new ServerInfo { Id = 13, Name = "XIII. Вульпекула" },
                new ServerInfo { Id = 14, Name = "XIV. Эридан" },
                new ServerInfo { Id = 15, Name = "XV. Кетус" },
                new ServerInfo { Id = 16, Name = "XVI. Аэтерис" },
                new ServerInfo { Id = 17, Name = "XVII. Талас" },
                new ServerInfo { Id = 18, Name = "XVIII. Наракин" },
                new ServerInfo { Id = 19, Name = "XIX. Новый сервер" }
            };
        }
    }
}