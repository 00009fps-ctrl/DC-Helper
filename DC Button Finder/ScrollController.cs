using System;
using System.Threading.Tasks;

namespace DC_Button_Finder
{
    public sealed class ScrollController
    {
        private readonly Clicker _clicker;
        private readonly Logger _logger;
        private readonly Random _random;

        public ScrollController(Clicker clicker, Logger logger, Random random)
        {
            _clicker = clicker;
            _logger = logger;
            _random = random;
        }

        public async Task ScrollDownAsync()
        {
            try
            {
                int scrollCount = _random.Next(5, 8);
                _logger.Debug($"Прокрутка вниз: {scrollCount} щелчков");

                for (int i = 0; i < scrollCount; i++)
                {
                    int scrollAmount = -(_random.Next(300, 500));
                    int delayMs = _random.Next(20, 30);

                    _clicker.WheelScroll(scrollAmount);
                    await Task.Delay(delayMs);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка прокрутки вниз: {ex.Message}");
            }
        }

        public async Task ScrollUpAsync()
        {
            try
            {
                int scrollCount = _random.Next(5, 8);
                _logger.Debug($"Прокрутка вверх: {scrollCount} щелчков");

                for (int i = 0; i < scrollCount; i++)
                {
                    int scrollAmount = _random.Next(300, 500);
                    int delayMs = _random.Next(20, 30);

                    _clicker.WheelScroll(scrollAmount);
                    await Task.Delay(delayMs);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка прокрутки вверх: {ex.Message}");
            }
        }

        // НОВЫЙ МЕТОД: Длинная прокрутка вниз
        public async Task ScrollDownLongAsync()
        {
            try
            {
                int scrollCount = _random.Next(15, 20); // Одна длинная прокрутка
                _logger.Debug($"Длинная прокрутка вниз: {scrollCount} щелчков");

                for (int i = 0; i < scrollCount; i++)
                {
                    int scrollAmount = -(_random.Next(300, 500));
                    int delayMs = _random.Next(20, 30);

                    _clicker.WheelScroll(scrollAmount);
                    await Task.Delay(delayMs);
                }

                await Task.Delay(_random.Next(300, 500)); // Задержка после прокрутки
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка длинной прокрутки вниз: {ex.Message}");
            }
        }

        // НОВЫЙ МЕТОД: Длинная прокрутка вверх
        public async Task ScrollUpLongAsync()
        {
            try
            {
                int scrollCount = _random.Next(15, 20); // Одна длинная прокрутка
                _logger.Debug($"Длинная прокрутка вверх: {scrollCount} щелчков");

                for (int i = 0; i < scrollCount; i++)
                {
                    int scrollAmount = _random.Next(300, 500);
                    int delayMs = _random.Next(20, 30);

                    _clicker.WheelScroll(scrollAmount);
                    await Task.Delay(delayMs);
                }

                await Task.Delay(_random.Next(300, 500)); // Задержка после прокрутки
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка длинной прокрутки вверх: {ex.Message}");
            }
        }

        public async Task ScrollToBottomAsync()
        {
            try
            {
                _logger.Info("Прокрутка до низа");
                for (int i = 0; i < 5; i++)
                {
                    await ScrollDownAsync();
                    await Task.Delay(200);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка прокрутки до низа: {ex.Message}");
            }
        }

        public async Task ScrollToTopAsync()
        {
            try
            {
                _logger.Info("Прокрутка до верха");
                for (int i = 0; i < 5; i++)
                {
                    await ScrollUpAsync();
                    await Task.Delay(200);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка прокрутки до верха: {ex.Message}");
            }
        }
    }
}