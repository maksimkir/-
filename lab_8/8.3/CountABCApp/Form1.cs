using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuizApplication
{
    // Частковий клас, що містить логіку
    public partial class Form1 : Form
    {
        // 1. Змінна стану для керування циклом while (вимога)
        private bool continueQuiz = true;

        // Enum для чіткого представлення пори року
        public enum Season
        {
            Spring, 
            Summer, 
            Autumn, 
            Winter  
        }

        // Dictionary для зберігання даних
        public static readonly Dictionary<string, Dictionary<string, int>> SeasonsData =
            new Dictionary<string, Dictionary<string, int>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Весна", new Dictionary<string, int> { { "Березень", 31 }, { "Квітень", 30 }, { "Травень", 31 } } },
            { "Літо", new Dictionary<string, int> { { "Червень", 30 }, { "Липень", 31 }, { "Серпень", 31 } } },
            { "Осінь", new Dictionary<string, int> { { "Вересень", 30 }, { "Жовтень", 31 }, { "Листопад", 30 } } },
            { "Зима", new Dictionary<string, int> { { "Грудень", 31 }, { "Січень", 31 }, { "Лютий", 28 } } } 
        };

        // Конструктор форми
        public Form1()
        {
            InitializeComponent(); 
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
        }

        // 2. Метод, що використовує SWITCH (вимога)
        public string GetSeasonDetails(string seasonName)
        {
            if (Enum.TryParse(seasonName, true, out Season selectedSeason))
            {
                // Оператор switch на enum (шаблон констант)
                switch (selectedSeason)
                {
                    case Season.Spring:
                        return FormatDetails("Весна");
                    case Season.Summer:
                        return FormatDetails("Літо");
                    case Season.Autumn:
                        return FormatDetails("Осінь");
                    case Season.Winter:
                        return FormatDetails("Зима");
                    default:
                        return "Невідома пора року."; 
                }
            }
            else
            {
                return "Некоректний ввід. Введіть одну з назв або 0 для виходу.";
            }
        }

        // Допоміжний метод для форматування
        private string FormatDetails(string key)
        {
            var details = SeasonsData[key];
            var result = new StringBuilder($"**{key}** – ");
            foreach (var month in details)
            {
                result.Append($"{month.Key} ({month.Value} днів), ");
            }
            return result.ToString().TrimEnd(' ', ',');
        }

        // 3. Обробник кнопки, що імітує цикл WHILE (вимога)
        private void SubmitButton_Click(object? sender, EventArgs e) 
        {
            // Отримання даних користувача
            string userInput = InputTextBox.Text.Trim();

            // Перевірка умови завершення циклу
            if (userInput == "0")
            {
                continueQuiz = false; 
            }

            // Імітація тіла циклу while
            if (continueQuiz)
            {
                string result = GetSeasonDetails(userInput); 
                OutputLabel.Text = result;
            }
            else
            {
                OutputLabel.Text = "Робота програми завершена (введено '0').";
                SubmitButton.Enabled = false; 
                InputTextBox.Enabled = false;
            }
            
            InputTextBox.Clear();
            InputTextBox.Focus();
        }
    }
}