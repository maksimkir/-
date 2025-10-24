// ZooFinder_WinForms_CSharp.cs
// .NET 6+ Windows Forms single-file example (Program + Form in one file for simplicity)
// Реалізація лабораторної роботи "Зоопарк"

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ZooFinder
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // .NET 6+ WinForms
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        // Статичний набір полів (кількість параметрів):
        // 0 - AnimalName (назва тварини)
        // 1 - SpeciesCount (кількість виду)
        // 2 - PostalCode (поштовий індекс)
        // 3 - Country
        // 4 - Region (область)
        // 5 - District (район)
        // 6 - City (місто)
        // 7 - Street (вулиця)
        // 8 - House (будинок)
        // 9 - Apartment (квартира)
        // 10 - TotalAnimals (загальна кількість тварин)
        // 11 - Employees (кількість працівників)
        const int COLUMNS = 12;

        private Button btnLoad;
        private Button btnProcessSave;
        private DataGridView dgvInput;
        private DataGridView dgvResults;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private Label lblInfo;

        private string[,] dataArray = null; // двовимірний масив даних
        private string inputFilePath = null;

        public MainForm()
        {
            Text = "ZooFinder — лабораторна: Зоопарк";
            Width = 1000;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;

            btnLoad = new Button { Text = "Load Input Data", Left = 10, Top = 10, Width = 140 };
            btnLoad.Click += BtnLoad_Click;

            btnProcessSave = new Button { Text = "Find Ussuri Tigers & Save Output", Left = 160, Top = 10, Width = 220 };
            btnProcessSave.Click += BtnProcessSave_Click;

            lblInfo = new Label { Left = 400, Top = 15, AutoSize = true };

            dgvInput = new DataGridView { Left = 10, Top = 50, Width = 960, Height = 300, ReadOnly = true, AllowUserToAddRows = false };
            dgvResults = new DataGridView { Left = 10, Top = 370, Width = 960, Height = 270, ReadOnly = true, AllowUserToAddRows = false };

            Controls.Add(btnLoad);
            Controls.Add(btnProcessSave);
            Controls.Add(lblInfo);
            Controls.Add(dgvInput);
            Controls.Add(dgvResults);

            openFileDialog = new OpenFileDialog { Filter = "Text Files|*.txt;*.csv|All Files|*.*", Title = "Open Input Data" };
            saveFileDialog = new SaveFileDialog { Filter = "Text Files|*.txt|All Files|*.*", Title = "Save Output Data" };

            SetupGridColumns(dgvInput);
            SetupGridColumns(dgvResults);
        }

        private void SetupGridColumns(DataGridView grid)
        {
            grid.Columns.Clear();
            grid.Columns.Add("AnimalName", "Назва тварини");
            grid.Columns.Add("SpeciesCount", "Кількість виду");
            grid.Columns.Add("PostalCode", "Поштовий індекс");
            grid.Columns.Add("Country", "Країна");
            grid.Columns.Add("Region", "Область");
            grid.Columns.Add("District", "Район");
            grid.Columns.Add("City", "Місто");
            grid.Columns.Add("Street", "Вулиця");
            grid.Columns.Add("House", "Будинок");
            grid.Columns.Add("Apartment", "Квартира");
            grid.Columns.Add("TotalAnimals", "Загальна кількість тварин");
            grid.Columns.Add("Employees", "Кількість працівників");
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            inputFilePath = openFileDialog.FileName;

            try
            {
                // Метод порядкового зчитування: читаємо весь файл, розбиваємо на токени
                string text = File.ReadAllText(inputFilePath);
                // Можливі роздільники: крапка з комою, новий рядок
                char[] separators = new char[] { ';', '\n', '\r' };
                var rawTokens = text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(t => t.Trim()).ToList();

                if (rawTokens.Count % COLUMNS != 0)
                {
                    MessageBox.Show($"Кількість токенів у файлі ({rawTokens.Count}) не ділиться на {COLUMNS}. Перевірте формат вводу.", "Помилка формату", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int rows = rawTokens.Count / COLUMNS;
                dataArray = new string[rows, COLUMNS];

                // Заповнюємо двовимірний масив порядково
                int idx = 0;
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < COLUMNS; c++)
                    {
                        dataArray[r, c] = rawTokens[idx++];
                    }
                }

                // Відобразимо в dgvInput
                dgvInput.Rows.Clear();
                for (int r = 0; r < rows; r++)
                {
                    var row = new string[COLUMNS];
                    for (int c = 0; c < COLUMNS; c++) row[c] = dataArray[r, c];
                    dgvInput.Rows.Add(row);
                }

                lblInfo.Text = $"Loaded: {rows} records from {Path.GetFileName(inputFilePath)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при зчитуванні файлу:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnProcessSave_Click(object sender, EventArgs e)
        {
            if (dataArray == null)
            {
                MessageBox.Show("Спочатку завантажте вхідний файл.", "Немає даних", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Знайдемо рядки, де назва тварини містить "уссур" і "тигр" або точну назву
            int rows = dataArray.GetLength(0);
            var matchedRows = new System.Collections.Generic.List<string[]>();

            for (int r = 0; r < rows; r++)
            {
                string animal = dataArray[r, 0] ?? "";
                string animalLower = animal.ToLowerInvariant();
                if (animalLower.Contains("уссур") || (animalLower.Contains("тигр") && animalLower.Contains("усс")) || animalLower.Contains("уссурійський") || animalLower.Contains("ussuri") )
                {
                    var row = new string[COLUMNS];
                    for (int c = 0; c < COLUMNS; c++) row[c] = dataArray[r, c];
                    matchedRows.Add(row);
                }
            }

            // Показати знайдені у dgvResults
            dgvResults.Rows.Clear();
            foreach (var row in matchedRows) dgvResults.Rows.Add(row);

            if (matchedRows.Count == 0)
            {
                MessageBox.Show("У вхідних даних не знайдено зоопарків з уссурійськими тиграми.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Запит на збереження результату у файл
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (var sw = new StreamWriter(saveFileDialog.FileName))
                {
                    // Записуємо у той же формат: кожен токен розділений крапкою з комою, запис послідовно
                    foreach (var row in matchedRows)
                    {
                        sw.WriteLine(string.Join("; ", row));
                    }
                }

                MessageBox.Show($"Output saved to {saveFileDialog.FileName}", "Збережено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при збереженні файлу:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

/*
README — як підготувати Input Data
Формат файлу: plain text. Поля записані послідовно як токени, розділені крапкою з комою (;) або новим рядком.
Кількість полів на один запис = 12 (статична). Приклад одного запису (усі поля через ";"):

Уссурійський тигр; 1; 690000; Ukraine; Kharkivska; Kharkivskyi; Kharkiv; Shevchenka; 10; 0; 5; 30

Кілька записів — кожен запис в окремому рядку або всі токени послідовно. Важливо: загальна кількість токенів у файлі має ділитися на 12.

Output Data: кожен знайдений запис записується в новий рядок у тому ж форматі (кожне поле через "; ").

Інструкції компіляції та запуску:
- Відкрийте Visual Studio (або будь-яке IDE). Створіть проект Windows Forms App (.NET) (версія .NET 6/7).
- Додайте цей файл як Program.cs (замініть стандартний код) або скопіюйте в відповідні місця.
- Побудуйте та запустіть.
*/
