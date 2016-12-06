using System;
using System.Windows.Forms;
using Yachtsolution.GUILayer;

namespace Yachtsolution
{
    /// <summary>
    /// This is the class Program.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// This is the main entry point for the windows form application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}