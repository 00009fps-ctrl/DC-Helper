using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DC_Button_Finder
{
    public sealed class Logger
    {
        private readonly RichTextBox _logTextBox;
        private const int MAX_LOG_ENTRIES = 1000;

        public Logger(RichTextBox logTextBox)
        {
            _logTextBox = logTextBox;
        }

        public void Debug(string message) => Log(message, Color.Gray, "DEBUG");
        public void Info(string message) => Log(message, Color.Black, "INFO");
        public void Success(string message) => Log(message, Color.Green, "SUCCESS");
        public void Warn(string message) => Log(message, Color.Orange, "WARN");
        public void Error(string message) => Log(message, Color.Red, "ERROR");
        public void Action(string message) => Log(message, Color.Blue, "ACTION");

        private void Log(string message, Color color, string level)
        {
            if (_logTextBox.InvokeRequired)
            {
                _logTextBox.BeginInvoke(new Action(() => AppendLog(message, color, level)));
            }
            else
            {
                AppendLog(message, color, level);
            }
        }

        private void AppendLog(string message, Color color, string level)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                string logEntry = $"[{timestamp}] [{level}] {message}";

                if (_logTextBox.Lines.Length >= MAX_LOG_ENTRIES)
                {
                    _logTextBox.Clear();
                }

                _logTextBox.SelectionStart = _logTextBox.TextLength;
                _logTextBox.SelectionColor = color;
                _logTextBox.AppendText(logEntry + Environment.NewLine);
                _logTextBox.ScrollToCaret();

                // Сохранение в файл
                SaveToFile(logEntry);
            }
            catch (Exception ex)
            {
                File.AppendAllText("error_log.txt", $"[{DateTime.Now:HH:mm:ss}] Ошибка логирования: {ex.Message}\n");
            }
        }

        private void SaveToFile(string logEntry)
        {
            try
            {
                string logFile = $"bot_log_{DateTime.Now:yyyyMMdd}.txt";
                File.AppendAllText(logFile, logEntry + Environment.NewLine);
            }
            catch
            {
                // Игнорируем ошибки записи в файл
            }
        }
    }
}