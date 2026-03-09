using System.Drawing;

namespace DC_Button_Finder
{
    public class BotSettings
    {
        // Основные настройки
        public string ButtonSequence { get; set; } = string.Empty;
        public int ThresholdPercentage { get; set; } = 80;
        public int IterationDelay { get; set; } = 1000;
        public string SelectedWeek { get; set; } = "Week 1-8. Универсальная неделя";
        public Point WindowLocation { get; set; } = Point.Empty;
        public Size WindowSize { get; set; } = Size.Empty;
        public bool OnlyScratch { get; set; } = false;
        public string AttackMode { get; set; } = "none"; // Для обратной совместимости

        // Недельные боссы с режимами атаки
        public bool WeekBossX1 { get; set; } = false;
        public bool WeekBossX3 { get; set; } = false;

        // Скрытые боссы с режимами атаки
        public bool SearchHiddenBoss { get; set; } = false;
        public bool HiddenBossX1 { get; set; } = false;
        public bool HiddenBossX3 { get; set; } = false;

        // Мобы с режимами атаки
        public bool SearchMobsEasy { get; set; } = false;
        public bool SearchMobsNormal { get; set; } = false;
        public bool SearchMobsStrong { get; set; } = false;

        public bool MobEasyX1 { get; set; } = false;
        public bool MobEasyX3 { get; set; } = false;
        public bool MobNormalX1 { get; set; } = false;
        public bool MobNormalX3 { get; set; } = false;
        public bool MobStrongX1 { get; set; } = false;
        public bool MobStrongX3 { get; set; } = false;

        // Режимы работы
        public bool AlwaysMode { get; set; } = false;
        public bool SiegeOnlyMode { get; set; } = false;
        public bool ExtendedSiege { get; set; } = false;

        // Сервер
        public string SelectedServer { get; set; } = "Сервер 1";

        // Ассистент
        public bool AssistantX1 { get; set; } = false;
        public bool AssistantX3 { get; set; } = false;
    }
}