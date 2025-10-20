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
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private Button btnLoad;
        private Button btnFind;
        private Button btnSave;
        private DataGridView dgvAll;
        private DataGridView dgvFound;
        private Label lblAll;
        private Label lblFound;

        // Статичний розмір колонок згідно умови (12 параметрів)
        private const int COLS = 12;
        private string[,] dataArray = new string[0, COLS]; // буде ініціалізовано після читання

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Zoo Finder — Уссурійські тигри";
            this.Width = 1000;
            this.Height = 700;

            btnLoad = new Button { Text = "Load Input Data", Left = 20, Top = 10, Width = 150 };
            btnFind = new Button { Text = "Find Ussuri Tigers", Left = 190, Top = 10, Width = 150 };
            btnSave = new Button { Text = "Save Output Data", Left = 360, Top = 10, Width = 150 };

            lblAll = new Label { Text = "All zoos (loaded):", Left = 20, Top = 50, Width = 200 };
            dgvAll = new DataGridView { Left = 20, Top = 75, Width = 940, Height = 260, ReadOnly = true, AllowUserToAddRows = false };

            lblFound = new Label { Text = "Zoos with Ussuri tigers:", Left = 20, Top = 350, Width = 300 };
            dgvFound = new DataGridView { Left = 20, Top = 375, Width = 940, Height = 260, ReadOnly = true, AllowUserToAddRows = false };

            this.Controls.Add(btnLoad);
            this.Controls.Add(btnFind);
            this.Controls.Add(btnSave);
            this.Controls.Add(lblAll);
            this.Controls.Add(dgvAll);
            this.Controls.Add(lblFound);
            this.Controls.Add(dgvFound);

            btnLoad.Click += BtnLoad_Click;
            btnFind.Click += BtnFind_Click;
            btnSave.Click += BtnSave_Click;

            // Колонки для DataGridView
            string[] headers = new string[]
            {
                "ZooName","PostalIndex","Country","Region","District","City","Street","Building","Apartment",
                "TotalAnimals","StaffCount","SpeciesList"
            };

            dgvAll.ColumnCount = COLS;
            dgvFound.ColumnCount = COLS;
            for (int i = 0; i < COLS; i++)
            {
                dgvAll.Columns[i].Name = headers[i];
                dgvFound.Columns[i].Name = headers[i];
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                ofd.Title = "Select Input Data file";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var lines = File.ReadAllLines(ofd.FileName)
                                        .Where(l => !string.IsNullOrWhiteSpace(l))
                                        .ToArray();
                        int rows = lines.Length;
                        dataArray = new string[rows, COLS];

                        dgvAll.Rows.Clear();

                        for (int r = 0; r < rows; r++)
                        {
                            // Розділяємо поля по ';'
                            var parts = lines[r].Split(';');
                            if (parts.Length != COLS)
                            {
                                MessageBox.Show($"Помилка: рядок {r + 1} має {parts.Length} полів, очікується {COLS}. Рядок ігнорується.", "Parsing error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continue;
                            }

                            for (int c = 0; c < COLS; c++)
                            {
                                dataArray[r, c] = parts[c].Trim();
                            }

                            // Додаємо в dgv (краще показати навіть якщо були пробіли)
                            dgvAll.Rows.Add(Enumerable.Range(0, COLS).Select(ci => dataArray[r, ci]).ToArray());
                        }

                        MessageBox.Show($"Завантажено {dgvAll.Rows.Count} записів.", "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка читання файлу: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            dgvFound.Rows.Clear();
            if (dataArray.Length == 0)
            {
                MessageBox.Show("Спочатку завантажте Input Data.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rows = dataArray.GetLength(0);
            for (int r = 0; r < rows; r++)
            {
                string speciesList = dataArray[r, 11] ?? ""; // колонка 11 — SpeciesList
                if (speciesList.IndexOf("уссурійський тигр", StringComparison.OrdinalIgnoreCase) >= 0
                    || speciesList.IndexOf("уссурійські тигри", StringComparison.OrdinalIgnoreCase) >= 0
                    || speciesList.IndexOf("ussuri", StringComparison.OrdinalIgnoreCase) >= 0) // на випадок англомовних
                {
                    // Додаємо рядок в результати
                    dgvFound.Rows.Add(Enumerable.Range(0, COLS).Select(ci => dataArray[r, ci]).ToArray());
                }
            }

            MessageBox.Show($"Знайдено {dgvFound.Rows.Count} зоопарків з уссурійськими тиграми.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (dgvFound.Rows.Count == 0)
            {
                MessageBox.Show("Немає знайдених записів для збереження. Спочатку натисніть Find Ussuri Tigers.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string outPath = Path.Combine(Directory.GetCurrentDirectory(), "Output Data.txt");
                using (StreamWriter sw = new StreamWriter(outPath, false))
                {
                    foreach (DataGridViewRow row in dgvFound.Rows)
                    {
                        // Збираємо колонки в рядок; використовуємо ';' як роздільник
                        string[] fields = new string[COLS];
                        for (int i = 0; i < COLS; i++)
                        {
                            fields[i] = row.Cells[i].Value?.ToString() ?? "";
                        }
                        sw.WriteLine(string.Join(";", fields));
                    }
                }

                MessageBox.Show($"Результати збережено в:\n{outPath}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка запису файлу: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
