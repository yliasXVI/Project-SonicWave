using System;
using System.Windows.Forms;
using ProjectSonicWave.UI;

namespace ProjectSonicWave
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}