using System;
using System.Text;
using System.Windows.Forms;

namespace RemoveOddLengthWords
{
    public class MainForm : Form
    {
        //елементи інтерфейсу
        private TextBox inputBox;
        private Button processButton;
        private TextBox outputBox;
        private Label inputLabel;
        private Label outputLabel;

        public MainForm()
        {
        
            this.Text = "Видалення слів непарної довжини";
            this.Width = 600;
            this.Height = 300;

           
            inputLabel = new Label() { Text = "Введіть текст:", Top = 20, Left = 20, Width = 200 };
            inputBox = new TextBox() { Top = 50, Left = 20, Width = 540 };

      
            processButton = new Button() { Text = "Обробити", Top = 90, Left = 20, Width = 100 };
            processButton.Click += ProcessButton_Click;

           
            outputLabel = new Label() { Text = "Результат:", Top = 140, Left = 20, Width = 200 };
            outputBox = new TextBox() { Top = 170, Left = 20, Width = 540, Multiline = true, Height = 60 };

            
            this.Controls.Add(inputLabel);
            this.Controls.Add(inputBox);
            this.Controls.Add(processButton);
            this.Controls.Add(outputLabel);
            this.Controls.Add(outputBox);
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            string input = inputBox.Text;
            StringBuilder result = new StringBuilder();
            StringBuilder word = new StringBuilder();

            foreach (char c in input)
            {
                if (Char.IsLetterOrDigit(c)) // якщо символ частина слова
                {
                    word.Append(c);
                }
                else // роздільник
                {
                    if (word.Length > 0)
                    {
                        if (word.Length % 2 == 0)
                            result.Append(word + " ");

                        word.Clear();
                    }
                }
            }

            // Перевірка останнього слова
            if (word.Length > 0 && word.Length % 2 == 0)
                result.Append(word + " ");

            outputBox.Text = result.ToString().Trim();
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
