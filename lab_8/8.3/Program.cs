using System;
using System.Windows.Forms;

namespace QuizApplication
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Тут створюється та запускається ваша форма
            Application.Run(new Form1()); 
        }
    }
}