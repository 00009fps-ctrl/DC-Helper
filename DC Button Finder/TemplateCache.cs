using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DC_Button_Finder
{
    public sealed class TemplateCache : IDisposable
    {
        public Dictionary<string, Mat> FunctionButtons { get; } = new Dictionary<string, Mat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Mat> HiddenBoss { get; } = new Dictionary<string, Mat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Mat> WeekButtons { get; } = new Dictionary<string, Mat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Mat> MobsEasy { get; } = new Dictionary<string, Mat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Mat> MobsNormal { get; } = new Dictionary<string, Mat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Mat> MobsStrong { get; } = new Dictionary<string, Mat>(StringComparer.OrdinalIgnoreCase);

        public void LoadMobsEasy(Logger logger)
        {
            MobsEasy.Clear();
            string mobsEasyPath = "_resources/Mobs (easy)/";

            if (!Directory.Exists(mobsEasyPath))
            {
                logger.Warn($"Папка Mobs (easy) не найдена: {mobsEasyPath}");
                return;
            }

            string[] files = Directory.GetFiles(mobsEasyPath, "*.png");
            int loaded = 0;

            foreach (string file in files)
            {
                try
                {
                    string mobName = Path.GetFileNameWithoutExtension(file);
                    Mat template = Cv2.ImRead(file, ImreadModes.Unchanged);
                    if (template.Empty())
                    {
                        template.Dispose();
                        continue;
                    }

                    Mat processedTemplate = ProcessTemplate(template);
                    template.Dispose();

                    MobsEasy[mobName] = processedTemplate;
                    loaded++;
                }
                catch (Exception ex)
                {
                    logger.Error($"Ошибка загрузки моба {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            logger.Success($"Загружено {loaded} слабых мобов");
        }

        public void LoadMobsNormal(Logger logger)
        {
            MobsNormal.Clear();
            string mobsNormalPath = "_resources/Mobs (Normal)/";

            if (!Directory.Exists(mobsNormalPath))
            {
                logger.Warn($"Папка Mobs (Normal) не найдена: {mobsNormalPath}");
                return;
            }

            string[] files = Directory.GetFiles(mobsNormalPath, "*.png");
            int loaded = 0;

            foreach (string file in files)
            {
                try
                {
                    string mobName = Path.GetFileNameWithoutExtension(file);
                    Mat template = Cv2.ImRead(file, ImreadModes.Unchanged);
                    if (template.Empty())
                    {
                        template.Dispose();
                        continue;
                    }

                    Mat processedTemplate = ProcessTemplate(template);
                    template.Dispose();

                    MobsNormal[mobName] = processedTemplate;
                    loaded++;
                }
                catch (Exception ex)
                {
                    logger.Error($"Ошибка загрузки моба {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            logger.Success($"Загружено {loaded} средних мобов");
        }

        public void LoadMobsStrong(Logger logger)
        {
            MobsStrong.Clear();
            string mobsStrongPath = "_resources/Mobs (Strong)/";

            if (!Directory.Exists(mobsStrongPath))
            {
                logger.Warn($"Папка Mobs (Strong) не найдена: {mobsStrongPath}");
                return;
            }

            string[] files = Directory.GetFiles(mobsStrongPath, "*.png");
            int loaded = 0;

            foreach (string file in files)
            {
                try
                {
                    string mobName = Path.GetFileNameWithoutExtension(file);
                    Mat template = Cv2.ImRead(file, ImreadModes.Unchanged);
                    if (template.Empty())
                    {
                        template.Dispose();
                        continue;
                    }

                    Mat processedTemplate = ProcessTemplate(template);
                    template.Dispose();

                    MobsStrong[mobName] = processedTemplate;
                    loaded++;
                }
                catch (Exception ex)
                {
                    logger.Error($"Ошибка загрузки моба {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            logger.Success($"Загружено {loaded} сильных мобов");
        }

        public async Task LoadFunctionButtonsAsync(Logger logger)
        {
            FunctionButtons.Clear();
            string functionButtonsPath = "_resources/FunctionButtons/";

            if (!Directory.Exists(functionButtonsPath))
            {
                logger.Warn($"Папка FunctionButtons не найдена: {functionButtonsPath}");
                return;
            }

            string[] files = Directory.GetFiles(functionButtonsPath, "*.png");
            await Task.Run(() =>
            {
                foreach (string file in files)
                {
                    try
                    {
                        string buttonName = Path.GetFileNameWithoutExtension(file);
                        Mat template = Cv2.ImRead(file, ImreadModes.Unchanged);
                        if (template.Empty())
                        {
                            template.Dispose();
                            continue;
                        }

                        // Конвертируем в BGR если нужно
                        Mat processedTemplate = ProcessTemplate(template);
                        template.Dispose(); // Освобождаем оригинал

                        FunctionButtons[buttonName] = processedTemplate;
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Ошибка загрузки функциональной кнопки {Path.GetFileName(file)}: {ex.Message}");
                    }
                }
            });

            logger.Success($"Загружено {FunctionButtons.Count} функциональных кнопок");
        }

        public void LoadWeekButtons(string selectedWeek, Logger logger)
        {
            WeekButtons.Clear();
            if (string.IsNullOrWhiteSpace(selectedWeek))
            {
                logger.Warn("Неделя не выбрана — шаблоны не загружены");
                return;
            }

            string weekFolderPath = $"_resources/{selectedWeek}/";
            if (!Directory.Exists(weekFolderPath))
            {
                logger.Error($"Папка недели не найдена: {weekFolderPath}");
                return;
            }

            string[] files = Directory.GetFiles(weekFolderPath, "*.png");
            int loaded = 0;

            foreach (string file in files)
            {
                try
                {
                    string buttonName = Path.GetFileNameWithoutExtension(file);
                    Mat template = Cv2.ImRead(file, ImreadModes.Unchanged);
                    if (template.Empty())
                    {
                        template.Dispose();
                        continue;
                    }

                    // Конвертируем в BGR если нужно
                    Mat processedTemplate = ProcessTemplate(template);
                    template.Dispose(); // Освобождаем оригинал

                    WeekButtons[buttonName] = processedTemplate;
                    loaded++;
                }
                catch (Exception ex)
                {
                    logger.Error($"Ошибка загрузки шаблона недели {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            logger.Success($"Загружено {loaded} шаблонов из папки \"{selectedWeek}\"");
        }

        public void LoadHiddenBossButtons(Logger logger)
        {
            HiddenBoss.Clear();
            string hiddenBossPath = "_resources/HiddenBoss/";

            if (!Directory.Exists(hiddenBossPath))
            {
                logger.Warn($"Папка HiddenBoss не найдена: {hiddenBossPath}");
                return;
            }

            string[] files = Directory.GetFiles(hiddenBossPath, "*.png");
            int loaded = 0;

            foreach (string file in files)
            {
                try
                {
                    string buttonName = Path.GetFileNameWithoutExtension(file);
                    Mat template = Cv2.ImRead(file, ImreadModes.Unchanged);
                    if (template.Empty())
                    {
                        template.Dispose();
                        continue;
                    }

                    // Конвертируем в BGR если нужно
                    Mat processedTemplate = ProcessTemplate(template);
                    template.Dispose(); // Освобождаем оригинал

                    HiddenBoss[buttonName] = processedTemplate;
                    loaded++;
                }
                catch (Exception ex)
                {
                    logger.Error($"Ошибка загрузки скрытого босса {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            logger.Success($"Загружено {loaded} кнопок HiddenBoss");
        }

        private Mat ProcessTemplate(Mat template)
        {
            Mat result = new Mat();

            try
            {
                // 1. УДАЛЯЕМ АЛЬФА-КАНАЛ если есть
                if (template.Channels() == 4)
                    Cv2.CvtColor(template, result, ColorConversionCodes.BGRA2BGR);
                else if (template.Channels() == 1)
                    Cv2.CvtColor(template, result, ColorConversionCodes.GRAY2BGR);
                else
                    template.CopyTo(result);

                // 2. УБЕЖДАЕМСЯ ЧТО ИЗОБРАЖЕНИЕ В 8-BIT ФОРМАТЕ
                if (result.Depth() != MatType.CV_8U)
                    result.ConvertTo(result, MatType.CV_8U);

                // 3. ДОПОЛНИТЕЛЬНАЯ ОЧИСТКА - УДАЛЯЕМ ВОЗМОЖНЫЕ МЕТАДАННЫЕ
                // Создаем новую матрицу с теми же данными, но без метаданных
                Mat cleaned = new Mat();
                result.CopyTo(cleaned);

                // 4. ЯВНО УСТАНАВЛИВАЕМ РАЗМЕРЫ И ТИП
                if (cleaned.Width > 0 && cleaned.Height > 0)
                {
                    return cleaned;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки если нужно
            }

            // Если что-то пошло не так, возвращаем оригинал
            return result.Empty() ? template.Clone() : result;
        }

        public async Task ReloadAllTemplatesAsync(Logger logger, BotSettings settings)
        {
            Clear(); // Очищаем старые шаблоны

            await LoadFunctionButtonsAsync(logger);
            LoadWeekButtons(settings.SelectedWeek, logger);

            if (settings.SearchHiddenBoss)
                LoadHiddenBossButtons(logger);
            if (settings.SearchMobsEasy)
                LoadMobsEasy(logger);
            if (settings.SearchMobsNormal)
                LoadMobsNormal(logger);
            if (settings.SearchMobsStrong)
                LoadMobsStrong(logger);

            logger.Success("Все шаблоны перезагружены (очищены от метаданных)");
        }

        public Mat? GetCachedButtonImage(string buttonName, bool searchHiddenBoss)
        {
            // Поиск в FunctionButtons
            if (FunctionButtons.TryGetValue(buttonName, out Mat? functionButton) &&
                functionButton != null && !functionButton.Empty())
            {
                return functionButton;
            }

            // Поиск в HiddenBoss (если включено)
            if (searchHiddenBoss &&
                HiddenBoss.TryGetValue(buttonName, out Mat? hiddenBoss) &&
                hiddenBoss != null && !hiddenBoss.Empty())
            {
                return hiddenBoss;
            }

            // Поиск в WeekButtons
            if (WeekButtons.TryGetValue(buttonName, out Mat? weekButton) &&
                weekButton != null && !weekButton.Empty())
            {
                return weekButton;
            }

            return null;
        }

        public void Clear()
        {
            foreach (var mat in FunctionButtons.Values) mat?.Dispose();
            foreach (var mat in HiddenBoss.Values) mat?.Dispose();
            foreach (var mat in WeekButtons.Values) mat?.Dispose();
            foreach (var mat in MobsEasy.Values) mat?.Dispose();
            foreach (var mat in MobsNormal.Values) mat?.Dispose();
            foreach (var mat in MobsStrong.Values) mat?.Dispose();

            FunctionButtons.Clear();
            HiddenBoss.Clear();
            WeekButtons.Clear();
            MobsEasy.Clear();
            MobsNormal.Clear();
            MobsStrong.Clear();
        }
        public void Dispose()
        {
            Clear();
        }
    }
}