using System;
using System.Drawing;
using System.Windows.Forms;

namespace DC_Button_Finder
{
    public sealed class SettingsManager
    {
        private readonly Logger _logger;

        public SettingsManager(Logger logger)
        {
            _logger = logger;
        }

        public BotSettings LoadSettings()
        {
            var settings = new BotSettings();

            try
            {
                if (Properties.Settings.Default != null)
                {
                    // Основные настройки
                    settings.ButtonSequence = Properties.Settings.Default.savedText ?? string.Empty;
                    settings.ThresholdPercentage = Properties.Settings.Default.ThresholdPercentage > 0 ?
                        Convert.ToDouble(Properties.Settings.Default.ThresholdPercentage) : 80;
                    settings.IterationDelay = Properties.Settings.Default.IterationDelay > 0 ?
                        Convert.ToInt32(Properties.Settings.Default.IterationDelay) : 1000;
                    settings.SelectedWeek = Properties.Settings.Default.SelectedWeek ?? "Week 1-8. Универсальная неделя";
                    settings.OnlyScratch = Properties.Settings.Default.OnlyScratch;

                    // Недельные боссы
                    settings.WeekBossX1 = Properties.Settings.Default.WeekBossX1;
                    settings.WeekBossX3 = Properties.Settings.Default.WeekBossX3;

                    // Скрытые боссы
                    settings.SearchHiddenBoss = Properties.Settings.Default.SearchHiddenBoss;
                    settings.HiddenBossX1 = Properties.Settings.Default.HiddenBossX1;
                    settings.HiddenBossX3 = Properties.Settings.Default.HiddenBossX3;

                    // Мобы
                    settings.SearchMobsEasy = Properties.Settings.Default.SearchMobsEasy;
                    settings.SearchMobsNormal = Properties.Settings.Default.SearchMobsNormal;
                    settings.SearchMobsStrong = Properties.Settings.Default.SearchMobsStrong;

                    settings.MobEasyX1 = Properties.Settings.Default.MobEasyX1;
                    settings.MobEasyX3 = Properties.Settings.Default.MobEasyX3;
                    settings.MobNormalX1 = Properties.Settings.Default.MobNormalX1;
                    settings.MobNormalX3 = Properties.Settings.Default.MobNormalX3;
                    settings.MobStrongX1 = Properties.Settings.Default.MobStrongX1;
                    settings.MobStrongX3 = Properties.Settings.Default.MobStrongX3;

                    // Режимы работы
                    settings.AlwaysMode = Properties.Settings.Default.AlwaysMode;
                    settings.SiegeOnlyMode = Properties.Settings.Default.SiegeOnlyMode;
                    settings.ExtendedSiege = Properties.Settings.Default.ExtendedSiege;

                    // Сервер
                    settings.SelectedServer = Properties.Settings.Default.SelectedServer ?? "Сервер 1";

                    // Ассистент
                    settings.AssistantX1 = Properties.Settings.Default.AssistantX1;
                    settings.AssistantX3 = Properties.Settings.Default.AssistantX3;

                    // Оконные настройки
                    if (Properties.Settings.Default.WindowX != 0 && Properties.Settings.Default.WindowY != 0)
                    {
                        settings.WindowLocation = new Point(
                            Properties.Settings.Default.WindowX,
                            Properties.Settings.Default.WindowY
                        );

                        int width = Properties.Settings.Default.WindowW > 0 ?
                            Convert.ToInt32(Properties.Settings.Default.WindowW) : 800;
                        int height = Properties.Settings.Default.WindowH > 0 ?
                            Convert.ToInt32(Properties.Settings.Default.WindowH) : 600;
                        settings.WindowSize = new Size(width, height);
                    }
                }
                else
                {
                    _logger.Warn("Настройки по умолчанию не найдены, используются базовые значения");
                }

                _logger.Info("Настройки загружены");
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка загрузки настроек: {ex.Message}");
                settings = new BotSettings();
            }

            return settings;
        }

        public void SaveSettings(BotSettings settings, Form form)
        {
            try
            {
                if (Properties.Settings.Default != null)
                {
                    // Основные настройки
                    Properties.Settings.Default.savedText = settings.ButtonSequence;
                    Properties.Settings.Default.ThresholdPercentage = settings.ThresholdPercentage;
                    Properties.Settings.Default.IterationDelay = settings.IterationDelay;
                    Properties.Settings.Default.SelectedWeek = settings.SelectedWeek;
                    Properties.Settings.Default.OnlyScratch = settings.OnlyScratch;

                    // Недельные боссы
                    Properties.Settings.Default.WeekBossX1 = settings.WeekBossX1;
                    Properties.Settings.Default.WeekBossX3 = settings.WeekBossX3;

                    // Скрытые боссы
                    Properties.Settings.Default.SearchHiddenBoss = settings.SearchHiddenBoss;
                    Properties.Settings.Default.HiddenBossX1 = settings.HiddenBossX1;
                    Properties.Settings.Default.HiddenBossX3 = settings.HiddenBossX3;

                    // Мобы
                    Properties.Settings.Default.SearchMobsEasy = settings.SearchMobsEasy;
                    Properties.Settings.Default.SearchMobsNormal = settings.SearchMobsNormal;
                    Properties.Settings.Default.SearchMobsStrong = settings.SearchMobsStrong;

                    Properties.Settings.Default.MobEasyX1 = settings.MobEasyX1;
                    Properties.Settings.Default.MobEasyX3 = settings.MobEasyX3;
                    Properties.Settings.Default.MobNormalX1 = settings.MobNormalX1;
                    Properties.Settings.Default.MobNormalX3 = settings.MobNormalX3;
                    Properties.Settings.Default.MobStrongX1 = settings.MobStrongX1;
                    Properties.Settings.Default.MobStrongX3 = settings.MobStrongX3;

                    // Режимы работы
                    Properties.Settings.Default.AlwaysMode = settings.AlwaysMode;
                    Properties.Settings.Default.SiegeOnlyMode = settings.SiegeOnlyMode;
                    Properties.Settings.Default.ExtendedSiege = settings.ExtendedSiege;

                    // Сервер
                    Properties.Settings.Default.SelectedServer = settings.SelectedServer;

                    // Ассистент
                    Properties.Settings.Default.AssistantX1 = settings.AssistantX1;
                    Properties.Settings.Default.AssistantX3 = settings.AssistantX3;

                    // Оконные настройки
                    Properties.Settings.Default.WindowX = form.Location.X;
                    Properties.Settings.Default.WindowY = form.Location.Y;
                    Properties.Settings.Default.WindowW = form.Width;
                    Properties.Settings.Default.WindowH = form.Height;

                    Properties.Settings.Default.Save();
                    _logger.Success("Настройки сохранены");
                }
                else
                {
                    _logger.Error("Не удалось сохранить настройки - объект настроек не инициализирован");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка сохранения настроек: {ex.Message}");
            }
        }
    }
}