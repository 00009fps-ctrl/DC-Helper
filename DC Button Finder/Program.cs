using System;
using System.Windows.Forms;

namespace DC_Button_Finder
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ВРЕМЕННО: запускаем тестовую форму вместо MainForm
            //Application.Run(new TestForm());

            // ПОТОМ вернуть обратно:
            Application.Run(new MainForm());
        }
    }
}