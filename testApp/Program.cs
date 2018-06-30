using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.GameEngine.System;

namespace testApp
{
    static class Program
    {
        public static Label labelDebug;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            GameApplication.SetCrashHandler(new GameCrashHandler(true));
            Game gameWindow = new Game("TestGameApp", 1280, 720);
            labelDebug = new Label { BackColor = Color.Transparent, ForeColor = Color.Yellow, AutoSize = true };
            //labelDebug.BringToFront();
            GameApplication.Run(gameWindow);
            GameApplication.Log(craftersmine.GameEngine.Utils.LogEntryType.Info, "Game exited!");
        }
    }
}
