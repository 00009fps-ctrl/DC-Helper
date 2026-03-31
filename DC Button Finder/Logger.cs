using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DC_Button_Finder
{
    public sealed class Logger
    {
        private readonly RichTextBox _logTextBox;
        private readonly string _sessionLogFile;
        private const int MAX_LOG_ENTRIES = 1000;

        public Logger(RichTextBox logTextBox)
        {
            _logTextBox = logTextBox;

            // Создаём папку Logs, если её нет
            string logFolder = Path.Combine(Application.StartupPath, "Logs");
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            // Имя файла для этой сессии: Лог_ГГГГ-ММ-ДД_ЧЧ-ММ-СС.txt
            string fileName = "Лог_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
            _sessionLogFile = Path.Combine(logFolder, fileName);

            // Записываем начало сессии
            File.AppendAllText(_sessionLogFile, $"[{DateTime.Now:HH:mm:ss}] [INFO] === НАЧАЛО СЕССИИ ==={Environment.NewLine}");
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

                // Ограничиваем количество строк в RichTextBox
                if (_logTextBox.Lines.Length >= MAX_LOG_ENTRIES)
                {
                    _logTextBox.Clear();
                }

                _logTextBox.SelectionStart = _logTextBox.TextLength;
                _logTextBox.SelectionColor = color;
                _logTextBox.AppendText(logEntry + Environment.NewLine);
                _logTextBox.ScrollToCaret();

                // Сохраняем в файл этой сессии
                try
                {
                    File.AppendAllText(_sessionLogFile, logEntry + Environment.NewLine);
                }
                catch { }
            }
            catch (Exception ex)
            {
                File.AppendAllText("error_log.txt", $"[{DateTime.Now:HH:mm:ss}] Ошибка логирования: {ex.Message}\n");
            }
        }

        // Вызови этот метод при закрытии программы, чтобы записать конец сессии
        public void CloseSession()
        {
            try
            {
                File.AppendAllText(_sessionLogFile, $"[{DateTime.Now:HH:mm:ss}] [INFO] === КОНЕЦ СЕССИИ ==={Environment.NewLine}");
            }
            catch { }
        }
    }
}