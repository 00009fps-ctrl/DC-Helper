using System;
using System.Threading;
using System.Threading.Tasks;

namespace DC_Button_Finder
{
    public sealed class BotController
    {
        private readonly BotEngine _botEngine;
        private CancellationTokenSource _cts;
        private bool _isRunning;

        public bool IsRunning => _isRunning;

        public event Action<string>? OnStatusChanged;

        public BotController(BotEngine botEngine)
        {
            _botEngine = botEngine;
            _cts = new CancellationTokenSource();
        }

        public async Task StartAsync(BotSettings settings)
        {
            if (_isRunning) return;

            _isRunning = true;
            _cts = new CancellationTokenSource();

            OnStatusChanged?.Invoke("Бот запускается...");

            // Запускаем бот в отдельной задаче
            _ = Task.Run(async () =>
            {
                try
                {
                    await _botEngine.StartAsync(settings, _cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // Ожидаемое исключение при отмене
                }
                catch (Exception ex)
                {
                    OnStatusChanged?.Invoke($"Ошибка бота: {ex.Message}");
                }
                finally
                {
                    _isRunning = false;
                    OnStatusChanged?.Invoke("Бот полностью остановлен");
                }
            });
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _cts.Cancel();
            _botEngine.Stop();
            OnStatusChanged?.Invoke("Бот останавливается...");
        }
    }
}