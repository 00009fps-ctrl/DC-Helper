using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace DC_Button_Finder
{
    public sealed class Clicker
    {
        private readonly Logger _logger;
        private readonly Random _random;

        public Clicker(Logger logger, Random random)
        {
            _logger = logger;
            _random = random;
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, IntPtr dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;

        // НОВЫЙ МЕТОД: ПЕРЕМЕЩЕНИЕ КУРСОРА БЕЗ КЛИКА
        public void MoveToPosition(int x, int y)
        {
            try
            {
                // Добавляем небольшой рандом к координатам для естественности
                int jitterX = _random.Next(-3, 4);
                int jitterY = _random.Next(-3, 4);

                SetCursorPos(x + jitterX, y + jitterY);
                _logger.Debug($"Курсор перемещен в ({x},{y})");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка при перемещении курсора: {ex.Message}");
            }
        }

        public void MoveToPosition(Point point)
        {
            MoveToPosition(point.X, point.Y);
        }

        public void ClickAtPosition(int x, int y)
        {
            try
            {
                // Добавляем небольшой рандом к координатам для естественности
                int jitterX = _random.Next(-3, 4);
                int jitterY = _random.Next(-3, 4);

                SetCursorPos(x + jitterX, y + jitterY);

                // Человеческие задержки
                Thread.Sleep(_random.Next(20, 60)); // Задержка перед кликом
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
                Thread.Sleep(_random.Next(10, 30)); // Задержка между нажатием и отпусканием
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);

                _logger.Action($"Клик по координатам ({x},{y})");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка при клике: {ex.Message}");
            }
        }

        public void ClickAtPosition(Point point)
        {
            ClickAtPosition(point.X, point.Y);
        }

        public void WheelScroll(int amount)
        {
            try
            {
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, amount, IntPtr.Zero);
                _logger.Debug($"Прокрутка колеса: {amount}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка прокрутки колеса: {ex.Message}");
            }
        }
    }
}