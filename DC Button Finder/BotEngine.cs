using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DC_Button_Finder
{
    public sealed class BotEngine
    {
        private readonly Logger _logger;
        private readonly ScreenshotService _screenshotService;
        private readonly TemplateCache _templateCache;
        private readonly TemplateMatcher _templateMatcher;
        private readonly Clicker _clicker;
        private readonly ScrollController _scrollController;
        private readonly Random _random;

        private bool _isRunning;
        private int _siegeBackCycleCount = 0;
        private string _lastClickedButton = "";

        public bool IsRunning => _isRunning;

        public event Action<string>? OnButtonClicked;
        public event Action<string>? OnStatusChanged;

        public BotEngine(
            Logger logger,
            ScreenshotService screenshotService,
            TemplateCache templateCache,
            TemplateMatcher templateMatcher,
            Clicker clicker,
            ScrollController scrollController,
            Random random)
        {
            _logger = logger;
            _screenshotService = screenshotService;
            _templateCache = templateCache;
            _templateMatcher = templateMatcher;
            _clicker = clicker;
            _scrollController = scrollController;
            _random = random;
        }

        public async Task StartAsync(BotSettings settings, CancellationToken cancellationToken)
        {
            if (_isRunning) return;

            _isRunning = true;
            OnStatusChanged?.Invoke("Бот запущен");
            _logger.Success("Бот запущен");

            await RunBotLoopAsync(settings, cancellationToken);
        }

        public void Stop()
        {
            _isRunning = false;
            OnStatusChanged?.Invoke("Бот остановлен");
            _logger.Info("Бот остановлен");
        }

        private async Task RunBotLoopAsync(BotSettings settings, CancellationToken cancellationToken)
        {
            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await RandomDelayAsync(50, 150, cancellationToken);
                    await ProcessSingleIterationAsync(settings);

                    int currentDelay = settings.IterationDelay;

                    if (_lastClickedButton == "siege_1" || _lastClickedButton == "back_1")
                    {
                        _siegeBackCycleCount++;
                        if (_siegeBackCycleCount > 1)
                        {
                            int additionalDelay = _random.Next(500, 901) * (_siegeBackCycleCount - 1);
                            currentDelay += additionalDelay;
                            _logger.Info($"Задержка увеличена на {additionalDelay}мс. Итого: {currentDelay}мс (цикл #{_siegeBackCycleCount})");
                        }
                    }
                    else if (!string.IsNullOrEmpty(_lastClickedButton))
                    {
                        if (_siegeBackCycleCount > 0)
                        {
                            _logger.Info($"Найдена другая кнопка ({_lastClickedButton}) - сброс счетчика ({_siegeBackCycleCount})");
                            _siegeBackCycleCount = 0;
                        }
                    }

                    await Task.Delay(currentDelay, cancellationToken);
                }
                catch (TaskCanceledException) { break; }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.Error($"Ошибка в основном цикле: {ex.Message}");
                    if (_isRunning && !cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, cancellationToken);
                    }
                }
            }

            _isRunning = false;
            _logger.Debug("Основной цикл бота завершен");
        }

        private async Task ProcessSingleIterationAsync(BotSettings settings)
        {
            bool foundSomething = false;
            _lastClickedButton = "";

            using (var rawScreenshot = _screenshotService.GetButtonsAreaScreenshot())
            using (var buttonsScreenshot = RemoveJudgeFromScreenshot(rawScreenshot, settings))
            {
                if (await TryBossesAsync(buttonsScreenshot, settings)) return;
                if (await TryUserSequenceAsync(buttonsScreenshot, settings)) return;
            }

            if (!foundSomething)
            {
                if (await TryCrossAsync(settings)) return;
            }

            if (!foundSomething)
            {
                await SearchMobsInCurrentPositionAsync(settings);
            }
        }

        #region Боссы

        private async Task<bool> TryBossesAsync(Mat buttonsScreenshot, BotSettings settings)
        {
            string weekAttackMode = settings.WeekBossX1 ? "atk_1" : settings.WeekBossX3 ? "atk_3" : null;
            if (weekAttackMode != null)
            {
                var weekResult = await FindAndClickTargetWithAttackAsync(
                    _templateCache.WeekButtons, buttonsScreenshot, settings, "Недельный босс", weekAttackMode);
                if (weekResult)
                {
                    _lastClickedButton = "week_boss";
                    return true;
                }
            }

            string hiddenAttackMode = settings.HiddenBossX1 ? "atk_1" : settings.HiddenBossX3 ? "atk_3" : null;
            if (hiddenAttackMode != null)
            {
                var hiddenBossResult = await FindAndClickTargetWithAttackAsync(
                    _templateCache.HiddenBoss, buttonsScreenshot, settings, "Скрытый босс", hiddenAttackMode);
                if (hiddenBossResult)
                {
                    _lastClickedButton = "hidden_boss";
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Пользовательская последовательность

        private async Task<bool> TryUserSequenceAsync(Mat buttonsScreenshot, BotSettings settings)
        {
            var buttonSequence = ParseButtonSequence(settings.ButtonSequence);
            if (buttonSequence.Length == 0) return false;

            foreach (var buttonName in buttonSequence)
            {
                if (!_isRunning) break;

                if (buttonName == "find_4")
                {
                    if (await HandleFind4Async(buttonsScreenshot, settings))
                    {
                        _lastClickedButton = "find_4";
                        return true;
                    }
                    continue;
                }

                if (buttonName == "find_3")
                {
                    if (await HandleFind3Async(buttonsScreenshot, settings))
                    {
                        _lastClickedButton = "find_3";
                        return true;
                    }
                    continue;
                }

                if (buttonName == "back_0" || buttonName == "back_1")
                {
                    if (await HandleBackButtonAsync(buttonsScreenshot, settings, buttonName))
                    {
                        _lastClickedButton = buttonName;
                        return true;
                    }
                    continue;
                }

                var result = await FindAndClickSingleButtonInAreaAsync(buttonName, buttonsScreenshot, settings, "buttons");
                if (result)
                {
                    _lastClickedButton = buttonName;
                    _logger.Success($"Нажата кнопка: {buttonName}");
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Обработчики find_4, find_3, back_0/back_1

        private async Task<bool> HandleFind4Async(Mat buttonsScreenshot, BotSettings settings)
        {
            var template = _templateCache.GetCachedButtonImage("find_4", settings.SearchHiddenBoss);
            if (template == null) return false;

            var match = _templateMatcher.FindTemplate(buttonsScreenshot, template, "find_4", settings.ThresholdPercentage / 100.0);
            if (!match.Found) return false;

            _logger.Info("Найдено '4 из 4' - все ячейки заняты");

            if (await FindAndAttackPersonalMobsAsync(buttonsScreenshot, settings)) return true;

            await _scrollController.ScrollUpLongAsync();

            using (var afterScroll = _screenshotService.GetButtonsAreaScreenshot())
            {
                if (await FindAndAttackPersonalMobsAsync(afterScroll, settings)) return true;
            }

            if (await SearchMobsInCurrentPositionAsync(settings)) return true;

            _logger.Info("Боссы/мобы не найдены - завершаем итерацию");
            return true;
        }

        private async Task<bool> HandleFind3Async(Mat buttonsScreenshot, BotSettings settings)
        {
            var template = _templateCache.GetCachedButtonImage("find_3", settings.SearchHiddenBoss);
            if (template == null) return false;

            var match = _templateMatcher.FindTemplate(buttonsScreenshot, template, "find_3", settings.ThresholdPercentage / 100.0);
            if (!match.Found) return false;

            _logger.Info("Найдено '3 из 4' - перемещаем курсор, кликаем и прокручиваем вниз");

            var basePoint = match.GetRandomPointInTemplate(_random);
            int offsetY = _random.Next(50, 101);
            System.Drawing.Point targetPoint = new System.Drawing.Point(basePoint.X, basePoint.Y - offsetY);
            targetPoint = _screenshotService.ConvertFromButtonsAreaCoords(targetPoint);

            _clicker.ClickAtPosition(targetPoint);
            await RandomDelayAsync(200, 350);

            await _scrollController.ScrollDownLongAsync();

            return true;
        }

        private async Task<bool> HandleBackButtonAsync(Mat buttonsScreenshot, BotSettings settings, string buttonName)
        {
            _logger.Info($"Достигнут {buttonName} - проверяем, есть ли мобы");

            if (await SearchMobsInCurrentPositionAsync(settings))
            {
                _logger.Info("Мобы найдены и атакованы, кнопка выхода не нажимается");
                return true;
            }

            _logger.Info($"Мобов нет, нажимаем {buttonName}");
            return await FindAndClickSingleButtonInAreaAsync(buttonName, buttonsScreenshot, settings, "buttons");
        }

        #endregion

        #region CROSS

        private async Task<bool> TryCrossAsync(BotSettings settings)
        {
            using (var crossScreenshot = _screenshotService.GetCrossAreaScreenshot())
            {
                var result = await FindAndClickSingleButtonInAreaAsync("cross", crossScreenshot, settings, "cross");
                if (result)
                {
                    _lastClickedButton = "cross";
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Атака цели (универсальный метод)

        private async Task<bool> FindAndClickTargetWithAttackAsync(
            Dictionary<string, Mat> targetCollection,
            Mat areaScreenshot,
            BotSettings settings,
            string targetType,
            string attackMode)
        {
            double threshold = settings.ThresholdPercentage / 100.0;

            foreach (var kvp in targetCollection)
            {
                if (kvp.Key.Contains("judge", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!_isRunning) break;

                var match = _templateMatcher.FindTemplate(areaScreenshot, kvp.Value, kvp.Key, threshold);
                if (!match.Found) continue;

                _logger.Info($"Найден {targetType}: {kvp.Key}");

                await RandomDelayAsync(311, 437);

                int bottomLeftX = match.Location.X;
                int bottomLeftY = match.Location.Y + match.TemplateSize.Height;

                int targetX = bottomLeftX + 150;
                int targetY = bottomLeftY + 70;

                int x = _random.Next(targetX - 30, targetX + 31);
                int y = _random.Next(targetY - 10, targetY + 11);

                System.Drawing.Point clickPoint = new System.Drawing.Point(x, y);
                clickPoint = _screenshotService.ConvertFromButtonsAreaCoords(clickPoint);
                _clicker.ClickAtPosition(clickPoint);

                await RandomDelayAsync(311, 437);

                bool combatResult = await EnterCombatLoopAsync(targetType, kvp.Key, settings, attackMode);

                _lastClickedButton = combatResult ? "target_killed" : "target_failed";
                return combatResult;
            }

            return false;
        }

        #endregion

        #region Поиск мобов

        private async Task<bool> SearchMobsInCurrentPositionAsync(BotSettings settings)
        {
            _logger.Info("Поиск мобов в текущей позиции");

            using (var buttonsScreenshot = _screenshotService.GetButtonsAreaScreenshot())
            {
                if (_templateCache.MobsEasy.Count > 0)
                {
                    string attackMode = settings.MobEasyX1 ? "atk_1" : settings.MobEasyX3 ? "atk_3" : null;
                    if (attackMode != null)
                    {
                        if (await FindAndClickTargetWithAttackAsync(_templateCache.MobsEasy, buttonsScreenshot, settings, "Слабый моб", attackMode))
                        {
                            _lastClickedButton = "mob_easy";
                            return true;
                        }
                    }
                }

                if (_templateCache.MobsNormal.Count > 0)
                {
                    string attackMode = settings.MobNormalX1 ? "atk_1" : settings.MobNormalX3 ? "atk_3" : null;
                    if (attackMode != null)
                    {
                        if (await FindAndClickTargetWithAttackAsync(_templateCache.MobsNormal, buttonsScreenshot, settings, "Средний моб", attackMode))
                        {
                            _lastClickedButton = "mob_normal";
                            return true;
                        }
                    }
                }

                if (_templateCache.MobsStrong.Count > 0)
                {
                    string attackMode = settings.MobStrongX1 ? "atk_1" : settings.MobStrongX3 ? "atk_3" : null;
                    if (attackMode != null)
                    {
                        if (await FindAndClickTargetWithAttackAsync(_templateCache.MobsStrong, buttonsScreenshot, settings, "Сильный моб", attackMode))
                        {
                            _lastClickedButton = "mob_strong";
                            return true;
                        }
                    }
                }
            }

            _logger.Info("Мобы не найдены");
            return false;
        }

        #endregion

        #region Личные мобы (personalMob)

        private async Task<bool> FindAndAttackPersonalMobsAsync(Mat buttonsScreenshot, BotSettings settings)
        {
            double threshold = settings.ThresholdPercentage / 100.0;
            var template = _templateCache.GetCachedButtonImage("personalMob", false);
            if (template == null) return false;

            var match = _templateMatcher.FindTemplate(buttonsScreenshot, template, "personalMob", threshold);
            if (!match.Found) return false;

            _logger.Info("Найден личный моб, проверяем область вокруг него");

            int searchX = Math.Max(0, match.Location.X - 140);
            int searchY = Math.Max(0, match.Location.Y - 70);
            int searchWidth = 200;
            int searchHeight = 100;

            using (var searchArea = new Mat(buttonsScreenshot, new OpenCvSharp.Rect(searchX, searchY, searchWidth, searchHeight)))
            {
                if (settings.MobEasyX1 || settings.MobEasyX3)
                {
                    string attackMode = settings.MobEasyX1 ? "atk_1" : "atk_3";
                    if (await FindAndClickMobInAreaAsync(_templateCache.MobsEasy, searchArea, settings, "Слабый моб", attackMode, searchX, searchY))
                        return true;
                }

                if (settings.MobNormalX1 || settings.MobNormalX3)
                {
                    string attackMode = settings.MobNormalX1 ? "atk_1" : "atk_3";
                    if (await FindAndClickMobInAreaAsync(_templateCache.MobsNormal, searchArea, settings, "Средний моб", attackMode, searchX, searchY))
                        return true;
                }

                if (settings.MobStrongX1 || settings.MobStrongX3)
                {
                    string attackMode = settings.MobStrongX1 ? "atk_1" : "atk_3";
                    if (await FindAndClickMobInAreaAsync(_templateCache.MobsStrong, searchArea, settings, "Сильный моб", attackMode, searchX, searchY))
                        return true;
                }
            }

            return false;
        }

        private async Task<bool> FindAndClickMobInAreaAsync(
            Dictionary<string, Mat> mobCollection,
            Mat searchArea,
            BotSettings settings,
            string mobType,
            string attackMode,
            int offsetX,
            int offsetY)
        {
            double threshold = settings.ThresholdPercentage / 100.0;

            foreach (var kvp in mobCollection)
            {
                if (kvp.Key.Contains("judge", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!_isRunning) break;

                var match = _templateMatcher.FindTemplate(searchArea, kvp.Value, kvp.Key, threshold);
                if (!match.Found) continue;

                int globalX = match.Location.X + offsetX;
                int globalY = match.Location.Y + offsetY;

                _logger.Info($"Найден {mobType}: {kvp.Key} (личный моб)");

                await RandomDelayAsync(311, 437);

                int bottomLeftX = globalX;
                int bottomLeftY = globalY + match.TemplateSize.Height;

                int targetX = bottomLeftX + 150;
                int targetY = bottomLeftY + 70;

                int x = _random.Next(targetX - 30, targetX + 31);
                int y = _random.Next(targetY - 10, targetY + 11);

                System.Drawing.Point clickPoint = new System.Drawing.Point(x, y);
                clickPoint = _screenshotService.ConvertFromButtonsAreaCoords(clickPoint);
                _clicker.ClickAtPosition(clickPoint);

                await RandomDelayAsync(311, 437);

                bool combatResult = await EnterCombatLoopAsync(mobType, kvp.Key, settings, attackMode);

                _lastClickedButton = combatResult ? "personal_mob_killed" : "personal_mob_failed";
                return true;
            }

            return false;
        }

        #endregion

        #region Удаление judge

        private Mat RemoveJudgeFromScreenshot(Mat screenshot, BotSettings settings)
        {
            double threshold = settings.ThresholdPercentage / 100.0;
            var template = _templateCache.GetCachedButtonImage("judge", false);
            if (template == null) return screenshot.Clone();

            var match = _templateMatcher.FindTemplate(screenshot, template, "judge", threshold);
            if (!match.Found) return screenshot.Clone();

            Mat result = screenshot.Clone();

            int x = match.Location.X - 20;
            int y = match.Location.Y - 20;
            int width = match.TemplateSize.Width + 120;
            int height = match.TemplateSize.Height + 120;

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x + width > result.Width) width = result.Width - x;
            if (y + height > result.Height) height = result.Height - y;

            var rect = new OpenCvSharp.Rect(x, y, width, height);
            Cv2.Rectangle(result, rect, new Scalar(0, 0, 0), -1);

            _logger.Info($"Область judge закрашена: ({x},{y}) {width}x{height}");
            return result;
        }

        #endregion

        #region Вспомогательные методы

        private async Task<bool> FindAndClickSingleButtonInAreaAsync(string buttonName, Mat areaScreenshot, BotSettings settings, string areaType)
        {
            try
            {
                double threshold = settings.ThresholdPercentage / 100.0;
                var template = _templateCache.GetCachedButtonImage(buttonName, settings.SearchHiddenBoss);

                if (template == null)
                {
                    _logger.Error($"Шаблон '{buttonName}' не найден в кэше");
                    return false;
                }

                var match = _templateMatcher.FindTemplate(areaScreenshot, template, buttonName, threshold);
                if (!match.Found) return false;

                System.Drawing.Point clickPoint = match.GetRandomPointInTemplate(_random);

                if (areaType == "buttons")
                    clickPoint = _screenshotService.ConvertFromButtonsAreaCoords(clickPoint);
                else if (areaType == "cross")
                    clickPoint = _screenshotService.ConvertFromCrossAreaCoords(clickPoint);

                await HumanLikeClickAsync(clickPoint, settings);
                OnButtonClicked?.Invoke($"Кнопка: {buttonName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка обработки кнопки '{buttonName}': {ex.Message}");
                return false;
            }
        }

        private bool HasAlreadyDamaged(Mat screenshot, BotSettings settings)
        {
            try
            {
                string[] variants = { "zeroDamage_0", "zeroDamage_1" };

                foreach (string variant in variants)
                {
                    var template = _templateCache.GetCachedButtonImage(variant, false);
                    if (template == null) continue;

                    var match = _templateMatcher.FindTemplate(screenshot, template, variant, 0.96);
                    if (match.Found)
                    {
                        _logger.Info($"Найден {variant} - урон не нанесен (0 HP) - можно атаковать");
                        return false;
                    }
                }

                _logger.Info("Обнаружен нанесенный урон - пропускаем ATK");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка проверки урона: {ex.Message}");
                return false;
            }
        }

        private async Task HumanLikeClickAsync(System.Drawing.Point point, BotSettings settings)
        {
            int baseDelay = settings.IterationDelay;
            await RandomDelayAsync(baseDelay * 75 / 100, baseDelay * 125 / 100);
            _clicker.ClickAtPosition(point);
        }

        private async Task RandomDelayAsync(int minMs, int maxMs, CancellationToken token = default)
        {
            int delay = _random.Next(minMs, maxMs + 1);
            try { await Task.Delay(delay, token); }
            catch (TaskCanceledException) { }
        }

        private string[] ParseButtonSequence(string sequence)
        {
            return sequence.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .Where(s => !string.IsNullOrEmpty(s))
                          .ToArray();
        }

        #endregion

        #region Цикл боя (EnterCombatLoop)

        private async Task<bool> EnterCombatLoopAsync(
            string targetType,
            string targetName,
            BotSettings settings,
            string attackMode)
        {
            _logger.Info($"Вход в бой с {targetType}: {targetName}");

            int maxIterations = 30;
            int iteration = 0;
            int noActionCounter = 0;
            const int maxNoAction = 10;
            bool alreadyDamaged = false;

            while (_isRunning && iteration < maxIterations)
            {
                iteration++;
                bool actionPerformed = false;

                // Проверка урона (только царапать)
                if (settings.OnlyScratch)
                {
                    using (var checkScreenshot = _screenshotService.GetButtonsAreaScreenshot())
                    {
                        bool hasDamage = HasAlreadyDamaged(checkScreenshot, settings);

                        if (hasDamage && !alreadyDamaged)
                        {
                            _logger.Info("Урон уже нанесен - выходим из боя");
                            await RandomDelayAsync(311, 437);

                            var backTemplate = _templateCache.GetCachedButtonImage("back_1", false);
                            if (backTemplate != null)
                            {
                                var backMatch = _templateMatcher.FindTemplate(checkScreenshot, backTemplate, "back_1", settings.ThresholdPercentage / 100.0);
                                if (backMatch.Found)
                                {
                                    await RandomDelayAsync(170, 237);
                                    System.Drawing.Point backPoint = backMatch.GetRandomPointInTemplate(_random);
                                    backPoint = _screenshotService.ConvertFromButtonsAreaCoords(backPoint);
                                    _clicker.ClickAtPosition(backPoint);
                                    await RandomDelayAsync(311, 437);
                                    _logger.Success($"Выход из боя с {targetType} (только царапать)");
                                    return true;
                                }
                            }
                        }
                        alreadyDamaged = hasDamage;
                    }
                }

                // Атака
                bool shouldAttack = !settings.OnlyScratch || !alreadyDamaged;
                if (shouldAttack)
                {
                    using (var attackScreenshot = _screenshotService.GetButtonsAreaScreenshot())
                    {
                        var attackTemplate = _templateCache.GetCachedButtonImage(attackMode, false);
                        if (attackTemplate != null)
                        {
                            var attackMatch = _templateMatcher.FindTemplate(attackScreenshot, attackTemplate, attackMode, settings.ThresholdPercentage / 100.0);
                            if (attackMatch.Found)
                            {
                                await RandomDelayAsync(311, 437);
                                System.Drawing.Point attackPoint = attackMatch.GetRandomPointInTemplate(_random);
                                attackPoint = _screenshotService.ConvertFromButtonsAreaCoords(attackPoint);
                                _clicker.ClickAtPosition(attackPoint);

                                _logger.Success($"Нажата {attackMode} в бою");
                                await RandomDelayAsync(311, 437);
                                actionPerformed = true;

                                if (settings.OnlyScratch) alreadyDamaged = true;
                            }
                        }
                    }
                }

                // bestAttack
                using (var checkScreenshot = _screenshotService.GetButtonsAreaScreenshot())
                {
                    await RandomDelayAsync(311, 437);
                    var bestTemplate = _templateCache.GetCachedButtonImage("bestAttack", false);
                    if (bestTemplate != null)
                    {
                        var bestMatch = _templateMatcher.FindTemplate(checkScreenshot, bestTemplate, "bestAttack", 0.8);
                        if (bestMatch.Found)
                        {
                            _logger.Info($"Обнаружена bestAttack - {targetType} убит");
                            await RandomDelayAsync(311, 437);

                            bool okPressed = false;
                            var okTemplate = _templateCache.GetCachedButtonImage("ok", false);
                            if (okTemplate != null)
                            {
                                var okMatch = _templateMatcher.FindTemplate(checkScreenshot, okTemplate, "ok", settings.ThresholdPercentage / 100.0);
                                if (okMatch.Found)
                                {
                                    await RandomDelayAsync(311, 437);
                                    System.Drawing.Point okPoint = okMatch.GetRandomPointInTemplate(_random);
                                    okPoint = _screenshotService.ConvertFromButtonsAreaCoords(okPoint);
                                    _clicker.ClickAtPosition(okPoint);
                                    _logger.Success("Нажата OK после победы");
                                    await RandomDelayAsync(311, 437);
                                    okPressed = true;
                                    actionPerformed = true;
                                }
                            }

                            var backTemplate = _templateCache.GetCachedButtonImage("back_1", false);
                            if (backTemplate != null)
                            {
                                var backMatch = _templateMatcher.FindTemplate(checkScreenshot, backTemplate, "back_1", settings.ThresholdPercentage / 100.0);
                                if (backMatch.Found)
                                {
                                    await RandomDelayAsync(311, 437);
                                    System.Drawing.Point backPoint = backMatch.GetRandomPointInTemplate(_random);
                                    backPoint = _screenshotService.ConvertFromButtonsAreaCoords(backPoint);
                                    _clicker.ClickAtPosition(backPoint);
                                    await RandomDelayAsync(311, 437);

                                    if (okPressed)
                                        _logger.Success($"Выход из боя с {targetType} (OK + назад)");
                                    else
                                        _logger.Warn($"OK не найден, но назад нажат — выход из боя с {targetType}");

                                    OnButtonClicked?.Invoke($"{targetType}: {targetName} убит");
                                    return true;
                                }
                                else
                                {
                                    _logger.Error($"Найдена bestAttack, но кнопка back_1 не обнаружена");
                                }
                            }
                        }
                    }
                }

                // Cross
                using (var crossScreenshot = _screenshotService.GetCrossAreaScreenshot())
                {
                    await RandomDelayAsync(311, 437);
                    var crossTemplate = _templateCache.GetCachedButtonImage("cross", false);
                    if (crossTemplate != null)
                    {
                        var crossMatch = _templateMatcher.FindTemplate(crossScreenshot, crossTemplate, "cross", settings.ThresholdPercentage / 100.0);
                        if (crossMatch.Found)
                        {
                            await RandomDelayAsync(311, 437);
                            System.Drawing.Point crossPoint = crossMatch.GetRandomPointInTemplate(_random);
                            crossPoint = _screenshotService.ConvertFromCrossAreaCoords(crossPoint);
                            _clicker.ClickAtPosition(crossPoint);
                            _logger.Success("Закрыто окно результатов");
                            await RandomDelayAsync(311, 437);
                            actionPerformed = true;
                        }
                    }
                }

                // Проверка на find_1 (кнопка поиска мобов)
                using (var checkScreenshot = _screenshotService.GetButtonsAreaScreenshot())
                {
                    var find1Template = _templateCache.GetCachedButtonImage("find_1", false);
                    if (find1Template != null)
                    {
                        var find1Match = _templateMatcher.FindTemplate(checkScreenshot, find1Template, "find_1", settings.ThresholdPercentage / 100.0);
                        if (find1Match.Found)
                        {
                            _logger.Info("Обнаружена кнопка поиска мобов (find_1) - бой завершен");
                            return true;
                        }
                    }
                }

                // Счетчик пустых итераций
                if (!actionPerformed)
                {
                    noActionCounter++;
                    if (noActionCounter >= maxNoAction)
                    {
                        _logger.Warn($"{maxNoAction} пустых итераций подряд - выход из боя");
                        return false;
                    }
                }
                else
                {
                    noActionCounter = 0;
                }

                await Task.Delay(_random.Next(500, 800));
            }

            return false;
        }

        #endregion
    }
}