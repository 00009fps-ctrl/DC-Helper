using OpenCvSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace DC_Button_Finder
{
    public sealed class AssistantEngine
    {
        private readonly Logger _logger;
        private readonly ScreenshotService _screenshotService;
        private readonly TemplateCache _templateCache;
        private readonly TemplateMatcher _templateMatcher;
        private readonly Clicker _clicker;
        private readonly Random _random;

        private bool _isRunning;
        private CancellationTokenSource _cts;
        private string? _selectedAttackMode;

        public bool IsRunning => _isRunning;

        public AssistantEngine(
            Logger logger,
            ScreenshotService screenshotService,
            TemplateCache templateCache,
            TemplateMatcher templateMatcher,
            Clicker clicker,
            Random random)
        {
            _logger = logger;
            _screenshotService = screenshotService;
            _templateCache = templateCache;
            _templateMatcher = templateMatcher;
            _clicker = clicker;
            _random = random;
            _cts = new CancellationTokenSource();
        }

        public async Task StartAsync(string attackMode, int iterationDelay, int thresholdPercentage)
        {
            if (_isRunning) return;

            _isRunning = true;
            _selectedAttackMode = attackMode;
            _cts = new CancellationTokenSource();

            _logger.Info($"Ассистент запущен (режим: {attackMode})");

            _ = RunAssistantLoopAsync(iterationDelay, thresholdPercentage, _cts.Token);
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _cts.Cancel();
            _logger.Info("Ассистент остановлен");
        }

        private async Task RunAssistantLoopAsync(int iterationDelay, int thresholdPercentage, CancellationToken cancellationToken)
        {
            double threshold = thresholdPercentage / 100.0;

            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Человеческая задержка перед скриншотом (имитация реакции)
                    await RandomDelayAsync(150, 350, cancellationToken);

                    bool foundAndClicked = await ProcessSingleAssistantIterationAsync(threshold);

                    if (!foundAndClicked)
                    {
                        await Task.Delay(iterationDelay, cancellationToken);
                    }
                }
                catch (TaskCanceledException) { break; }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.Error($"Ошибка ассистента: {ex.Message}");
                    if (_isRunning && !cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, cancellationToken);
                    }
                }
            }

            _isRunning = false;
            _logger.Debug("Цикл ассистента завершен");
        }

        private async Task<bool> ProcessSingleAssistantIterationAsync(double threshold)
        {
            // 1. Поиск ATK кнопки (x1 или x3) - в основной области
            using (var buttonsScreenshot = _screenshotService.GetButtonsAreaScreenshot())
            {
                string atkButtonName = _selectedAttackMode == "atk_1" ? "atk_1" : "atk_3";
                var atkFound = await FindAndClickButtonInAreaAsync(atkButtonName, buttonsScreenshot, threshold, "buttons");
                if (atkFound) return true;

                // 2. Поиск bestAttack (если найден → ищем и нажимаем back_1) - в основной области
                var bestAttackFound = await FindBestAttackAndHandleAsync(buttonsScreenshot, threshold);
                if (bestAttackFound) return true;
            }

            // 3. Поиск CROSS - в отдельной области (верхний правый угол)
            using (var crossScreenshot = _screenshotService.GetCrossAreaScreenshot())
            {
                var crossFound = await FindAndClickButtonInAreaAsync("cross", crossScreenshot, threshold, "cross");
                if (crossFound) return true;
            }

            return false;
        }

        private async Task<bool> FindAndClickButtonInAreaAsync(string buttonName, Mat screenshot, double threshold, string areaType)
        {
            try
            {
                var template = _templateCache.GetCachedButtonImage(buttonName, false);
                if (template == null) return false;

                var matchResult = _templateMatcher.FindTemplate(screenshot, template, buttonName, threshold);

                if (matchResult.Found)
                {
                    await RandomDelayAsync(311, 437);

                    var clickPoint = matchResult.GetRandomPointInTemplate(_random);

                    // Преобразуем координаты в зависимости от области
                    if (areaType == "buttons")
                        clickPoint = _screenshotService.ConvertFromButtonsAreaCoords(clickPoint);
                    else if (areaType == "cross")
                        clickPoint = _screenshotService.ConvertFromCrossAreaCoords(clickPoint);

                    _clicker.ClickAtPosition(clickPoint);

                    await RandomDelayAsync(311, 437);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка поиска кнопки '{buttonName}': {ex.Message}");
            }

            return false;
        }

        private async Task<bool> FindBestAttackAndHandleAsync(Mat screenshot, double threshold)
        {
            try
            {
                var template = _templateCache.GetCachedButtonImage("bestAttack", false);
                if (template == null) return false;

                var matchResult = _templateMatcher.FindTemplate(screenshot, template, "bestAttack", threshold);

                if (matchResult.Found)
                {
                    _logger.Info("Найдена лучшая атака (моб убит) - ищем кнопку назад");

                    var backTemplate = _templateCache.GetCachedButtonImage("back_1", false);
                    if (backTemplate != null)
                    {
                        var backMatch = _templateMatcher.FindTemplate(screenshot, backTemplate, "back_1", threshold);
                        if (backMatch.Found)
                        {
                            await RandomDelayAsync(311, 437);

                            var clickPoint = backMatch.GetRandomPointInTemplate(_random);
                            clickPoint = _screenshotService.ConvertFromButtonsAreaCoords(clickPoint);

                            _clicker.ClickAtPosition(clickPoint);

                            await RandomDelayAsync(311, 437);
                            _logger.Success("Нажата back_1 после bestAttack");
                            return true;
                        }
                        else
                        {
                            _logger.Warn("Найден bestAttack, но не найдена back_1");
                        }
                    }
                    else
                    {
                        _logger.Warn("Шаблон back_1 не загружен");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка обработки bestAttack: {ex.Message}");
            }

            return false;
        }

        private async Task RandomDelayAsync(int minMs, int maxMs, CancellationToken token = default)
        {
            int delay = _random.Next(minMs, maxMs + 1);
            try
            {
                await Task.Delay(delay, token);
            }
            catch (TaskCanceledException) { }
        }
    }
}