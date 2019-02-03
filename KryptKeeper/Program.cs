using System;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace KryptKeeper
{
    internal static class Program
    {
        private static MainWindow _mainWindow;

        [STAThread]
        public static void Main(string[] args)
        {
            _mainWindow = new MainWindow();
            SingleInstanceApplication.Run(_mainWindow, NewInstanceHandler);
        }

        public static void NewInstanceHandler(object sender, StartupNextInstanceEventArgs e)
        {
            string file = e.CommandLine[1];
            e.BringToForeground = true;
            _mainWindow.AddFile(file);
        }
        
        public class SingleInstanceApplication : WindowsFormsApplicationBase
        {
            private SingleInstanceApplication()
            {
                IsSingleInstance = true;
            }

            public static void Run(Form form, StartupNextInstanceEventHandler startupHandler)
            {
                var app = new SingleInstanceApplication {MainForm = form};
                app.StartupNextInstance += startupHandler;
                app.Run(Environment.GetCommandLineArgs());
            }
        }
    }
}
