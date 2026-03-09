using OpenCvSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

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

            // 1. СНАЧАЛА ИЩЕМ В ОБЛАСТИ КНОПОК
            using (var buttonsScreenshot = _screenshotService.GetButtonsAreaScreenshot())
            {
                // 1. ATK (если включено)
                if (!string.IsNullOrEmpty(settings.AttackMode) && settings.AttackMode != "none")
                {
                    if (settings.OnlyScratch && HasAlreadyDamaged(buttonsScreenshot, settings))
                    {
                        _logger.Info("Урон уже нанесен - пропускаем ATK поиск");
                    }
                    else
                    {
                        _logger.Info($"Поиск кнопки атаки: {settings.AttackMode}");
                        var attackResult = await FindAndClickSingleButtonInAreaAsync(settings.AttackMode, buttonsScreenshot, settings, "buttons");
                        if (attackResult)
                        {
                            _lastClickedButton = settings.AttackMode;
                            if (settings.OnlyScratch)
                            {
                                _logger.Info("Проверяем нанесен ли урон после атаки");
                                await Task.Delay(1000);
                                using (var screenshotAfterAttack = _screenshotService.GetButtonsAreaScreenshot())
                                {
                                    if (HasAlreadyDamaged(screenshotAfterAttack, settings))
                                    {
                                        _logger.Info("Урон нанесен - продолжаем работу");
                                    }
                                }
                            }
                            foundSomething = true;
                            return;
                        }
                    }
                }

                // 2. НЕДЕЛЬНЫЕ БОССЫ
                string weekAttackMode = settings.WeekBossX1 ? "atk_1" : settings.WeekBossX3 ? "atk_3" : null;
                if (weekAttackMode != null)
                {
                    var weekResult = await FindAndClickBossWithAttackAsync(
                        _templateCache.WeekButtons, buttonsScreenshot, settings, "Недельный босс", weekAttackMode);
                    if (weekResult)
                    {
                        _lastClickedButton = "week_boss";
                        foundSomething = true;
                        return;
                    }
                }

                // 3. СКРЫТЫЕ БОССЫ
                string hiddenAttackMode = settings.HiddenBossX1 ? "atk_1" : settings.HiddenBossX3 ? "atk_3" : null;
                if (hiddenAttackMode != null)
                {
                    var hiddenBossResult = await FindAndClickBossWithAttackAsync(
                        _templateCache.HiddenBoss, buttonsScreenshot, settings, "Скрытый босс", hiddenAttackMode);
                    if (hiddenBossResult)
                    {
                        _lastClickedButton = "hidden_boss";
                        foundSomething = true;
                        return;
                    }
                }

                // 4. ПОЛЬЗОВАТЕЛЬСКАЯ ПОСЛЕДОВАТЕЛЬНОСТЬ
                var buttonSequence = ParseButtonSequence(settings.ButtonSequence);
                if (buttonSequence.Length > 0)
                {
                    foreach (var buttonName in buttonSequence)
                    {
                        if (!_isRunning) break;

                        var result = await FindAndClickSingleButtonInAreaAsync(buttonName, buttonsScreenshot, settings, "buttons");
                        if (result)
                        {
                            _lastClickedButton = buttonName;

                            // ОСОБАЯ ЛОГИКА ДЛЯ find_4
                            if (buttonName == "find_4")
                            {
                                _logger.Info("Найдено '4 из 4' - все слоты заняты");

                                // ШАГ 1: Сначала ищем в текущей позиции
                                _logger.Info("Поиск в текущей позиции");
                                bool foundInCurrentPosition = await SearchAllTargetsInScreenshotAsync(buttonsScreenshot, settings);
                                if (foundInCurrentPosition)
                                {
                                    foundSomething = true;
                                    return;
                                }

                                // ШАГ 2: Одна длинная прокрутка ВВЕРХ
                                _logger.Info("Прокрутка вверх для поиска в верхней позиции");
                                await _scrollController.ScrollUpLongAsync();

                                // ШАГ 3: Поиск после прокрутки
                                _logger.Info("Поиск после прокрутки вверх");
                                using (var screenshotAfterScroll = _screenshotService.GetButtonsAreaScreenshot())
                                {
                                    bool foundAfterScroll = await SearchAllTargetsInScreenshotAsync(screenshotAfterScroll, settings);
                                    if (foundAfterScroll)
                                    {
                                        foundSomething = true;
                                        return;
                                    }
                                }

                                // ШАГ 4: Ничего не найдено - завершаем итерацию
                                _logger.Info("Боссы/мобы не найдены - завершаем итерацию");
                                foundSomething = true;
                                return;
                            }

                            // ОСОБАЯ ЛОГИКА ДЛЯ find_3
                            if (buttonName == "find_3")
                            {
                                _logger.Info("Найдено '3 из 4' - есть свободная ячейка, прокручиваем вниз");

                                // Прокрутка вниз (одна длинная)
                                await _scrollController.ScrollDownLongAsync();

                                // Начинаем итерацию заново (ничего не ищем сразу)
                                // Следующая итерация найдет find_1 и продолжит поиск
                                foundSomething = true;
                                return;
                            }

                            // Для всех остальных кнопок
                            _logger.Success($"Нажата кнопка: {buttonName}");
                            foundSomething = true;
                            return;
                        }
                    }
                }
            }

            // 5. ПОИСК CROSS
            if (!foundSomething)
            {
                using (var crossScreenshot = _screenshotService.GetCrossAreaScreenshot())
                {
                    var crossResult = await FindAndClickSingleButtonInAreaAsync("cross", crossScreenshot, settings, "cross");
                    if (crossResult)
                    {
                        _lastClickedButton = "cross";
                        foundSomething = true;
                        return;
                    }
                }
            }

            // 6. ПОИСК МОБОВ
            if (!foundSomething)
            {
                await SearchMobsInCurrentPositionAsync(settings);
            }
        }
        private async Task<bool> FindAndClickBossWithAttackAsync(
    System.Collections.Generic.Dictionary<string, Mat> bossCollection,
    Mat areaScreenshot,
    BotSettings settings,
    string bossType,
    string attackMode)
        {
            double threshold = settings.ThresholdPercentage / 100.0;

            foreach (var kvp in bossCollection)
            {
                if (!_isRunning) break;

                var matchResult = _templateMatcher.FindTemplate(areaScreenshot, kvp.Value, kvp.Key, threshold);
                if (matchResult.Found)
                {
                    _logger.Info($"Найден {bossType}: {kvp.Key}");

                    await RandomDelayAsync(311, 437);

                    // Клик по боссу (от левого нижнего угла)
                    int bottomLeftX = matchResult.Location.X;
                    int bottomLeftY = matchResult.Location.Y + matchResult.TemplateSize.Height;

                    int targetX = bottomLeftX + 150;
                    int targetY = bottomLeftY + 70;

                    int minX = targetX - 30;
                    int maxX = targetX + 30;
                    int minY = targetY - 10;
                    int maxY = targetY + 10;

                    int x = _random.Next(minX, maxX + 1);
                    int y = _random.Next(minY, maxY + 1);

                    var clickPoint = new System.Drawing.Point(x, y);
                    clickPoint = _screenshotService.ConvertFromButtonsAreaCoords(clickPoint);
                    _clicker.ClickAtPosition(clickPoint);

                    await RandomDelayAsync(311, 437);

                    // Входим в цикл боя
                    bool combatResult = await EnterCombatLoopAsync(bossType, kvp.Key, settings, attackMode);

                    _lastClickedButton = combatResult ? "boss_killed" : "boss_failed";
                    return combatResult;
                }
            }

            return false;
        }

        private async Task<bool> FindAndClickMobWithAttackAsync(
            System.Collections.Generic.Dictionary<string, Mat> mobCollection,
            Mat areaScreenshot,
            BotSettings settings,
            string mobType,
            string attackMode)
        {
            double threshold = settings.ThresholdPercentage / 100.0;

            foreach (var kvp in mobCollection)
            {
                if (!_isRunning) break;

                var matchResult = _templateMatcher.FindTemplate(areaScreenshot, kvp.Value, kvp.Key, threshold);
                if (matchResult.Found)
                {
                    _logger.Info($"Найден {mobType}: {kvp.Key}");

                    await RandomDelayAsync(311, 437);

                    // Клик по мобу (от левого нижнего угла)
                    int bottomLeftX = matchResult.Location.X;
                    int bottomLeftY = matchResult.Location.Y + matchResult.TemplateSize.Height;

                    int targetX = bottomLeftX + 150;
                    int targetY = bottomLeftY + 70;

                    int minX = targetX - 30;
                    int maxX = targetX + 30;
                    int minY = targetY - 10;
                    int maxY = targetY + 10;

                    int x = _random.Next(minX, maxX + 1);
                    int y = _random.Next(minY, maxY + 1);

                    var clickPoint = new System.Drawing.Point(x, y);
                    clickPoint = _screenshotService.ConvertFromButtonsAreaCoords(clickPoint);
                    _clicker.ClickAtPosition(clickPoint);

                    await RandomDelayAsync(311, 437);

                    // Входим в цикл боя
                    bool combatResult = await EnterCombatLoopAsync(mobType, kvp.Key, settings, attackMode);

                    _lastClickedButton = combatResult ? "mob_killed" : "mob_failed";
                    return combatResult;
                }
            }

            return false;
        }
        private async Task<bool> SearchMobsInCurrentPositionAsync(BotSettings settings)
        {
            _logger.Info("Поиск мобов в текущей позиции");

            using (var buttonsScreenshot = _screenshotService.GetButtonsAreaScreenshot())
            {
                // Слабые мобы
                if (_templateCache.MobsEasy.Count > 0)
                {
                    string attackMode = settings.MobEasyX1 ? "atk_1" : settings.MobEasyX3 ? "atk_3" : null;
                    if (attackMode != null)
                    {
                        var mobsEasyResult = await FindAndClickMobWithAttackAsync(
                            _templateCache.MobsEasy, buttonsScreenshot, settings, "Слабый моб", attackMode);
                        if (mobsEasyResult)
                        {
                            _lastClickedButton = "mob_easy";
                            return true;
                        }
                    }
                }

                // Средние мобы
                if (_templateCache.MobsNormal.Count > 0)
                {
                    string attackMode = settings.MobNormalX1 ? "atk_1" : settings.MobNormalX3 ? "atk_3" : null;
                    if (attackMode != null)
                    {
                        var mobsNormalResult = await FindAndClickMobWithAttackAsync(
                            _templateCache.MobsNormal, buttonsScreenshot, settings, "Средний моб", attackMode);
                        if (mobsNormalResult)
                        {
                            _lastClickedButton = "mob_normal";
                            return true;
                        }
                    }
                }

                // Сильные мобы
                if (_templateCache.MobsStrong.Count > 0)
                {
                    string attackMode = settings.MobStrongX1 ? "atk_1" : settings.MobStrongX3 ? "atk_3" : null;
                    if (attackMode != null)
                    {
                        var mobsStrongResult = await FindAndClickMobWithAttackAsync(
                            _templateCache.MobsStrong, buttonsScreenshot, settings, "Сильный моб", attackMode);
                        if (mobsStrongResult)
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

        private async Task<bool> SearchAllTargetsInScreenshotAsync(Mat screenshot, BotSettings settings)
        {
            // 1. Недельные боссы
            string weekAttackMode = settings.WeekBossX1 ? "atk_1" : settings.WeekBossX3 ? "atk_3" : null;
            if (weekAttackMode != null)
            {
                var weekResult = await FindAndClickBossWithAttackAsync(
                    _templateCache.WeekButtons, screenshot, settings, "Недельный босс", weekAttackMode);
                if (weekResult) return true;
            }

            // 2. Скрытые боссы
            string hiddenAttackMode = settings.HiddenBossX1 ? "atk_1" : settings.HiddenBossX3 ? "atk_3" : null;
            if (hiddenAttackMode != null)
            {
                var hiddenBossResult = await FindAndClickBossWithAttackAsync(
                    _templateCache.HiddenBoss, screenshot, settings, "Скрытый босс", hiddenAttackMode);
                if (hiddenBossResult) return true;
            }

            // 3. Слабые мобы
            if (_templateCache.MobsEasy.Count > 0)
            {
                string attackMode = settings.MobEasyX1 ? "atk_1" : settings.MobEasyX3 ? "atk_3" : null;
                if (attackMode != null)
                {
                    var mobsEasyResult = await FindAndClickMobWithAttackAsync(
                        _templateCache.MobsEasy, screenshot, settings, "Слабый моб", attackMode);
                    if (mobsEasyResult) return true;
                }
            }

            // 4. Средние мобы
            if (_templateCache.MobsNormal.Count > 0)
            {
                string attackMode = settings.MobNormalX1 ? "atk_1" : settings.MobNormalX3 ? "atk_3" : null;
                if (attackMode != null)
                {
                    var mobsNormalResult = await FindAndClickMobWithAttackAsync(
                        _templateCache.MobsNormal, screenshot, settings, "Средний моб", attackMode);
                    if (mobsNormalResult) return true;
                }
            }

            // 5. Сильные мобы
            if (_templateCache.MobsStrong.Count > 0)
            {
                string attackMode = settings.MobStrongX1 ? "atk_1" : settings.MobStrongX3 ? "atk_3" : null;
                if (attackMode != null)
                {
                    var mobsStrongResult = await FindAndClickMobWithAttackAsync(
                        _templateCache.MobsStrong, screenshot, settings, "Сильный моб", attackMode);
                    if (mobsStrongResult) return true;
                }
            }

            return false;
        }

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

                var matchResult = _templateMatcher.FindTemplate(areaScreenshot, template, buttonName, threshold);

                if (matchResult.Found)
                {
                    if (buttonName == "find_4")
                    {
                        _logger.Info($"Обнаружено: {buttonName} - только логирование, клик не выполняется");
                        return true;
                    }

                    if (buttonName == "find_3")
                    {
                        var movePoint = matchResult.GetRandomPointInTemplate(_random);

                        if (areaType == "buttons")
                            movePoint = _screenshotService.ConvertFromButtonsAreaCoords(movePoint);
                        else if (areaType == "cross")
                            movePoint = _screenshotService.ConvertFromCrossAreaCoords(movePoint);

                        _clicker.MoveToPosition(movePoint);

                        await RandomDelayAsync(100, 300);
                        await _scrollController.ScrollDownAsync();
                        await RandomDelayAsync(100, 200);

                        _logger.Success($"{buttonName} обработан: курсор перемещен, прокрутка выполнена");
                        return true;
                    }

                    var clickPoint = matchResult.GetRandomPointInTemplate(_random);

                    if (areaType == "buttons")
                        clickPoint = _screenshotService.ConvertFromButtonsAreaCoords(clickPoint);
                    else if (areaType == "cross")
                        clickPoint = _screenshotService.ConvertFromCrossAreaCoords(clickPoint);

                    await HumanLikeClickAsync(clickPoint, settings);
                    OnButtonClicked?.Invoke($"Кнопка: {buttonName}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка обработки кнопки '{buttonName}': {ex.Message}");
            }

            return false;
        }

        private bool HasAlreadyDamaged(Mat screenshot, BotSettings settings)
        {
            try
            {
                string[] zeroDamageVariants = { "zeroDamage_0", "zeroDamage_1" };

                foreach (string variant in zeroDamageVariants)
                {
                    var zeroDamageTemplate = _templateCache.GetCachedButtonImage(variant, false);
                    if (zeroDamageTemplate != null)
                    {
                        var zeroDamageMatch = _templateMatcher.FindTemplate(screenshot, zeroDamageTemplate, variant, 0.96);

                        if (zeroDamageMatch.Found)
                        {
                            _logger.Info($"Найден {variant} - урон не нанесен (0 HP) - можно атаковать");
                            return false;
                        }
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
            // Задержка перед кликом: ±25% от IterationDelay
            int baseDelay = settings.IterationDelay;
            int minPreClick = baseDelay * 75 / 100;  // 75%
            int maxPreClick = baseDelay * 125 / 100; // 125%

            await RandomDelayAsync(minPreClick, maxPreClick);
            _clicker.ClickAtPosition(point);

           
        }
        private async Task RandomDelayAsync(int minMs, int maxMs)
        {
            int delay = _random.Next(minMs, maxMs + 1);
            await Task.Delay(delay);
        }

        private string[] ParseButtonSequence(string sequence)
        {
            return sequence.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .Where(s => !string.IsNullOrEmpty(s))
                          .ToArray();
        }

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

                // 1. Проверка урона (если включено "только царапать")
                if (settings.OnlyScratch)
                {
                    using (var checkScreenshot = _screenshotService.GetButtonsAreaScreenshot())
                    {
                        bool hasDamage = HasAlreadyDamaged(checkScreenshot, settings);

                        if (hasDamage)
                        {
                            if (!alreadyDamaged)
                            {
                                _logger.Info("Урон уже нанесен - выходим из боя");

                                await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед поиском back_1

                                var backTemplate = _templateCache.GetCachedButtonImage("back_1", false);
                                if (backTemplate != null)
                                {
                                    var backMatch = _templateMatcher.FindTemplate(checkScreenshot, backTemplate, "back_1", settings.ThresholdPercentage / 100.0);
                                    if (backMatch.Found)
                                    {
                                        await RandomDelayAsync(170, 237); // ЗАДЕРЖКА перед кликом

                                        var backPoint = backMatch.GetRandomPointInTemplate(_random);
                                        backPoint = _screenshotService.ConvertFromButtonsAreaCoords(backPoint);
                                        _clicker.ClickAtPosition(backPoint);

                                        await RandomDelayAsync(311, 437); // ЗАДЕРЖКА после клика
                                        _logger.Success($"Выход из боя с {targetType} (только царапать)");
                                        return true;
                                    }
                                }
                            }
                            alreadyDamaged = true;
                        }
                        else
                        {
                            _logger.Info("Урон не нанесен - можно атаковать");
                        }
                    }
                }

                // 2. Атакуем только если урон еще не нанесен
                bool shouldAttack = !settings.OnlyScratch || !alreadyDamaged;

                if (shouldAttack)
                {
                    using (var attackScreenshot = _screenshotService.GetButtonsAreaScreenshot())
                    {
                        var attackTemplate = _templateCache.GetCachedButtonImage(attackMode, false);
                        if (attackTemplate != null)
                        {
                            await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед поиском

                            var attackMatch = _templateMatcher.FindTemplate(attackScreenshot, attackTemplate, attackMode, settings.ThresholdPercentage / 100.0);
                            if (attackMatch.Found)
                            {
                                await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед кликом

                                var attackPoint = attackMatch.GetRandomPointInTemplate(_random);
                                attackPoint = _screenshotService.ConvertFromButtonsAreaCoords(attackPoint);
                                _clicker.ClickAtPosition(attackPoint);

                                _logger.Success($"Нажата {attackMode} в бою");
                                await RandomDelayAsync(311, 437); // ЗАДЕРЖКА после клика
                                actionPerformed = true;

                                if (settings.OnlyScratch)
                                {
                                    alreadyDamaged = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Если урон уже нанесен и мы не атакуем - ищем back_1 для выхода
                    using (var checkScreenshot = _screenshotService.GetButtonsAreaScreenshot())
                    {
                        await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед поиском back_1

                        var backTemplate = _templateCache.GetCachedButtonImage("back_1", false);
                        if (backTemplate != null)
                        {
                            var backMatch = _templateMatcher.FindTemplate(checkScreenshot, backTemplate, "back_1", settings.ThresholdPercentage / 100.0);
                            if (backMatch.Found)
                            {
                                await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед кликом

                                var backPoint = backMatch.GetRandomPointInTemplate(_random);
                                backPoint = _screenshotService.ConvertFromButtonsAreaCoords(backPoint);
                                _clicker.ClickAtPosition(backPoint);

                                await RandomDelayAsync(311, 437); // ЗАДЕРЖКА после клика
                                _logger.Success($"Выход из боя с {targetType}");
                                return true;
                            }
                        }
                    }
                }

                // 3. Проверяем bestAttack (монстр убит)
                using (var checkScreenshot = _screenshotService.GetButtonsAreaScreenshot())
                {
                    await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед поиском bestAttack

                    var bestTemplate = _templateCache.GetCachedButtonImage("bestAttack", false);
                    if (bestTemplate != null)
                    {
                        var bestMatch = _templateMatcher.FindTemplate(checkScreenshot, bestTemplate, "bestAttack", 0.8);
                        if (bestMatch.Found)
                        {
                            _logger.Info($"Обнаружена bestAttack - {targetType} убит");

                            await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед поиском back_1

                            var backTemplate = _templateCache.GetCachedButtonImage("back_1", false);
                            if (backTemplate != null)
                            {
                                var backMatch = _templateMatcher.FindTemplate(checkScreenshot, backTemplate, "back_1", settings.ThresholdPercentage / 100.0);
                                if (backMatch.Found)
                                {
                                    await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед кликом

                                    var backPoint = backMatch.GetRandomPointInTemplate(_random);
                                    backPoint = _screenshotService.ConvertFromButtonsAreaCoords(backPoint);
                                    _clicker.ClickAtPosition(backPoint);

                                    await RandomDelayAsync(311, 437); // ЗАДЕРЖКА после клика
                                    _logger.Success($"Выход из боя с {targetType}");

                                    OnButtonClicked?.Invoke($"{targetType}: {targetName} убит");
                                    return true;
                                }
                            }
                        }
                    }
                }

                // 4. Ищем cross
                using (var crossScreenshot = _screenshotService.GetCrossAreaScreenshot())
                {
                    await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед поиском cross

                    var crossTemplate = _templateCache.GetCachedButtonImage("cross", false);
                    if (crossTemplate != null)
                    {
                        var crossMatch = _templateMatcher.FindTemplate(crossScreenshot, crossTemplate, "cross", settings.ThresholdPercentage / 100.0);
                        if (crossMatch.Found)
                        {
                            await RandomDelayAsync(311, 437); // ЗАДЕРЖКА перед кликом

                            var crossPoint = crossMatch.GetRandomPointInTemplate(_random);
                            crossPoint = _screenshotService.ConvertFromCrossAreaCoords(crossPoint);
                            _clicker.ClickAtPosition(crossPoint);

                            _logger.Success("Закрыто окно результатов");
                            await RandomDelayAsync(311, 437); // ЗАДЕРЖКА после клика
                            actionPerformed = true;
                        }
                    }
                }

                // 5. Счетчик пустых итераций
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

                await Task.Delay(_random.Next(500, 800)); // ЗАДЕРЖКА между итерациями
            }

            return false;
        }
    }
}