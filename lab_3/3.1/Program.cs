using System;
using System.Windows.Forms;

namespace CountABCApp
{
    public class MainForm : Form
    {
        private TextBox inputBox;
        private Button checkButton;

        public MainForm()
        {
            this.Text = "Пошук 'abc'";
            this.Width = 400;
            this.Height = 200;

            Label label = new Label() { Text = "Введіть англійський текст:", Top = 20, Left = 20, Width = 200 };
            inputBox = new TextBox() { Top = 50, Left = 20, Width = 300 };

            checkButton = new Button() { Text = "Порахувати", Top = 90, Left = 20, Width = 100 };
            checkButton.Click += CheckButton_Click;

            this.Controls.Add(label);
            this.Controls.Add(inputBox);
            this.Controls.Add(checkButton);
        }

        private void CheckButton_Click(object? sender, EventArgs e)
        {
            string text = inputBox.Text;
            char[] chars = text.ToCharArray(); // Перетворюємо рядок у масив символів

            int count = 0;
            for (int i = 0; i < chars.Length - 2; i++)
            {
                if (chars[i] == 'a' && chars[i + 1] == 'b' && chars[i + 2] == 'c')
                {
                    count++;
                }
            }

            MessageBox.Show($"Група 'abc' зустрічається {count} раз(и).");
        }
[STAThread]
public static void Main()
{
    Application.Run(new MainForm());
}
    }
}
