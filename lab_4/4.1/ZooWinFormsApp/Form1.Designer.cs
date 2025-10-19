using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ZooFinder
{
    public partial class Form1 : Form
    {
        // Статична кількість колонок (параметрів) згідно умови:
        // 1 Назва тварини
        // 2 Кількість виду
        // 3 Поштовий індекс
        // 4 Країна
        // 5 Область
        // 6 Район
        // 7 Місто
        // 8 Вулиця
        // 9 Будинок
        // 10 Квартира
        // 11 Загальна кількість тварин
        // 12 Кількість працівників
        private const int COLS = 12;

        // Двовимірний масив для зберігання всіх записів після зчитування
        private string[,] dataArray = new string[0, COLS];

        public Form1()
        {
            InitializeCustomComponents();
        }

        // UI-компоненти створюємо програмно, щоб вставити код повністю в один файл
        private Button btnLoad;
        private Button btnFilter;
        private Button btnSave;
        private DataGridView dgvInput;
        private DataGridView dgvResult;
        private Label lblInput;
        private Label lblResult;

        private void InitializeCustomComponents()
        {
            this.Text = "Zoo Finder — Уссурійські тигри";
            this.Width = 1000;
            this.Height = 600;

            btnLoad = new Button() { Text = "Завантажити Input Data.txt", Left = 10, Top = 10, Width = 220 };
            btnLoad.Click += BtnLoad_Click;

            btnFilter = new Button() { Text = "Показати зоопарки з уссур. тиграми", Left = 240, Top = 10, Width = 260 };
            btnFilter.Click += BtnFilter_Click;

            btnSave = new Button() { Text = "Зберегти Output Data.txt", Left = 510, Top = 10, Width = 200 };
            btnSave.Click += BtnSave_Click;

            lblInput = new Label() { Text = "Вхідні дані:", Left = 10, Top = 50, Width = 200 };
            lblResult = new Label() { Text = "Результат (зоопарки з уссур. тиграми):", Left = 10, Top = 300, Width = 400 };

            dgvInput = new DataGridView() { Left = 10, Top = 70, Width = 960, Height = 220, ReadOnly = true, AllowUserToAddRows = false };
            dgvResult = new DataGridView() { Left = 10, Top = 320, Width = 960, Height = 220, ReadOnly = true, AllowUserToAddRows = false };

            this.Controls.Add(btnLoad);
            this.Controls.Add(btnFilter);
            this.Controls.Add(btnSave);
            this.Controls.Add(lblInput);
            this.Controls.Add(lblResult);
            this.Controls.Add(dgvInput);
            this.Controls.Add(dgvResult);
        }

        // Кнопка: завантажити вхідний файл
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            string filename = "Input Data.txt";
            if (!File.Exists(filename))
            {
                MessageBox.Show($"Файл '{filename}' не знайдено у теці програми.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var lines = new List<string>();
            // Послідовне зчитування (stream reading)
            using (var sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        lines.Add(line.Trim());
                }
            }

            // Кількість записів (рядків)
            int rows = lines.Count;

            // Виділяємо 2D масив з rows x COLS
            dataArray = new string[rows, COLS];

            for (int i = 0; i < rows; i++)
            {
                // Розділювач — ';'
                var parts = lines[i].Split(new char[] { ';' });
                // Якщо менше полів, доповнюємо порожніми
                for (int c = 0; c < COLS; c++)
                {
                    if (c < parts.Length)
                        dataArray[i, c] = parts[c].Trim();
                    else
                        dataArray[i, c] = string.Empty;
                }
            }

            // Показуємо у dgvInput
            PopulateDataGridViewFromArray(dgvInput, dataArray);
            MessageBox.Show($"Завантажено {rows} запис(ів).", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Кнопка: відфільтрувати зоопарки з уссурійськими тиграми
        private void BtnFilter_Click(object sender, EventArgs e)
        {
            if (dataArray == null || dataArray.GetLength(0) == 0)
            {
                MessageBox.Show("Спочатку завантажте вхідний файл.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var matched = new List<string[]>();
            int rows = dataArray.GetLength(0);

            for (int r = 0; r < rows; r++)
            {
                string animalName = dataArray[r, 0] ?? string.Empty;
                // нечутливий до регістру пошук підрядка "уссур"
                if (animalName.ToLower().Contains("уссур"))
                {
                    var row = new string[COLS];
                    for (int c = 0; c < COLS; c++) row[c] = dataArray[r, c];
                    matched.Add(row);
                }
            }

            if (matched.Count == 0)
            {
                MessageBox.Show("Не знайдено зоопарків з уссурійськими тиграми.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvResult.DataSource = null;
                return;
            }

            // Перетворимо matched у двовимірний масив для зручного заповнення dgv
            string[,] resultArr = new string[matched.Count, COLS];
            for (int r = 0; r < matched.Count; r++)
                for (int c = 0; c < COLS; c++)
                    resultArr[r, c] = matched[r][c];

            PopulateDataGridViewFromArray(dgvResult, resultArr);
            MessageBox.Show($"Знайдено {matched.Count} зоопарк(ів) з уссурійськими тиграми.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Кнопка: зберегти результат у Output Data.txt (тільки те, що зараз показано в dgvResult)
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dgvResult == null || dgvResult.Rows.Count == 0)
            {
                MessageBox.Show("Немає результату для збереження. Спочатку натисніть 'Показати зоопарки з уссур. тиграми'.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string outFile = "Output Data.txt";
            using (var sw = new StreamWriter(outFile, false))
            {
                // Записуємо кожний рядок як поля, розділені ';'
                foreach (DataGridViewRow row in dgvResult.Rows)
                {
                    var values = new List<string>();
                    for (int c = 0; c < dgvResult.Columns.Count; c++)
                        values.Add((row.Cells[c].Value ?? "").ToString());
                    sw.WriteLine(string.Join(";", values));
                }
            }

            MessageBox.Show($"Результат записано у '{outFile}'.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Допоміжний: заповнення DataGridView з 2D масиву
        private void PopulateDataGridViewFromArray(DataGridView dgv, string[,] arr)
        {
            dgv.Columns.Clear();
            dgv.Rows.Clear();

            // Додаємо назви колонок
            string[] headers = new string[]
            {
                "Назва тварини",
                "Кількість виду",
                "Поштовий індекс",
                "Країна",
                "Область",
                "Район",
                "Місто",
                "Вулиця",
                "Будинок",
                "Квартира",
                "Загальна кількість тварин",
                "Кількість працівників"
            };

            for (int c = 0; c < COLS; c++)
            {
                dgv.Columns.Add("c" + c.ToString(), headers[c]);
            }

            int rows = arr.GetLength(0);
            for (int r = 0; r < rows; r++)
            {
                var rowValues = new string[COLS];
                for (int c = 0; c < COLS; c++)
                    rowValues[c] = arr[r, c];
                dgv.Rows.Add(rowValues);
            }

            dgv.AutoResizeColumns();
        }
    }
}
