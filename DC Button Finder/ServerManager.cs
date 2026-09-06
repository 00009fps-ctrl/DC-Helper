using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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
        private static readonly string _excelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_resources", "ServerInfo", "server_data.xlsx");

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
                if (!File.Exists(_excelPath))
                    return result;

                using (var package = new ExcelPackage(new FileInfo(_excelPath)))
                {
                    var sheet = package.Workbook.Worksheets["CheckBox"];
                    if (sheet == null)
                        return result;

                    int row = 2;
                    while (sheet.Cells[row, 1].Value != null)
                    {
                        if (int.TryParse(sheet.Cells[row, 1].Text, out int id))
                        {
                            bool isChecked = sheet.Cells[row, 2].Text == "1";
                            result[id] = isChecked;
                        }
                        row++;
                    }
                }
            }
            catch { }

            return result;
        }

        public static void SaveCheckboxes(Dictionary<int, bool> checkboxes)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(_excelPath)))
                {
                    var sheet = package.Workbook.Worksheets["CheckBox"];
                    if (sheet == null)
                        sheet = package.Workbook.Worksheets.Add("CheckBox");

                    sheet.Cells[1, 1].Value = "ID";
                    sheet.Cells[1, 2].Value = "Checked";

                    using (var range = sheet.Cells[1, 1, 1, 2])
                    {
                        range.Style.Font.Bold = true;
                    }

                    int row = 2;
                    foreach (var kvp in checkboxes)
                    {
                        sheet.Cells[row, 1].Value = kvp.Key;
                        sheet.Cells[row, 2].Value = kvp.Value ? "1" : "0";
                        row++;
                    }

                    sheet.Cells.AutoFitColumns();
                    package.Save();
                }
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
                new ServerInfo { Id = 19, Name = "XIX. Эокс" }
            };
        }
    }
}