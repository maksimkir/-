using System;
using System.Windows.Forms;

namespace GraphMatricesApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // Якщо .NET Framework — заміни на EnableVisualStyles()
            Application.Run(new Form1());
        }
    }
}
