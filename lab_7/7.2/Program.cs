using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StudentApp
{
    public struct STUDENT
    {
        public string NAME;     // Прізвище та ініціали
        public string GROUP;    // Номер групи
        public int[] SUBJECT;   // Масив оцінок (5 предметів)
    }

    public class Form1 : Form
    {
        private TextBox txtName;
        private TextBox txtGroup;
        private TextBox txtMarks;
        private Button btnAdd;
        private Button btnSort;
        private Button btnShowTwos;
        private ListBox listBoxResults;
        private Label labelTitle;
        private Label labelName;
        private Label labelGroup;
        private Label labelMarks;

        private List<STUDENT> learners = new List<STUDENT>();

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtName = new TextBox();
            this.txtGroup = new TextBox();
            this.txtMarks = new TextBox();
            this.btnAdd = new Button();
            this.btnSort = new Button();
            this.btnShowTwos = new Button();
            this.listBoxResults = new ListBox();
            this.labelTitle = new Label();
            this.labelName = new Label();
            this.labelGroup = new Label();
            this.labelMarks = new Label();
            this.SuspendLayout();

            this.labelTitle.Text = "Облік студентів";
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(140, 9);
            this.labelTitle.Size = new System.Drawing.Size(200, 30);

            this.labelName.Text = "Прізвище та ініціали:"; 
            this.labelName.Location = new System.Drawing.Point(25, 50);
            this.labelName.Size = new System.Drawing.Size(150, 20); 
            this.txtName.Location = new System.Drawing.Point(180, 47);
            this.txtName.Size = new System.Drawing.Size(250, 23);

            this.labelGroup.Text = "Номер групи:";
            this.labelGroup.Location = new System.Drawing.Point(25, 85);
            this.labelGroup.Size = new System.Drawing.Size(150, 20); 
            this.txtGroup.Location = new System.Drawing.Point(180, 82);
            this.txtGroup.Size = new System.Drawing.Size(250, 23);

            this.labelMarks.Text = "Оцінки (5 чисел):";
            this.labelMarks.Location = new System.Drawing.Point(25, 120);
            this.labelMarks.Size = new System.Drawing.Size(150, 20); 
            this.txtMarks.Location = new System.Drawing.Point(180, 117);
            this.txtMarks.Size = new System.Drawing.Size(250, 23);

            this.btnAdd.Location = new System.Drawing.Point(25, 160);
            this.btnAdd.Size = new System.Drawing.Size(130, 30);
            this.btnAdd.Text = "Додати студента";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnSort.Location = new System.Drawing.Point(170, 160);
            this.btnSort.Size = new System.Drawing.Size(100, 30);
            this.btnSort.Text = "Сортувати";
            this.btnSort.Click += new EventHandler(this.btnSort_Click);

            this.btnShowTwos.Location = new System.Drawing.Point(280, 160);
            this.btnShowTwos.Size = new System.Drawing.Size(150, 30);
            this.btnShowTwos.Text = "Студенти з оцінкою 2";
            this.btnShowTwos.Click += new EventHandler(this.btnShowTwos_Click);

            this.listBoxResults.Location = new System.Drawing.Point(25, 210);
            this.listBoxResults.Size = new System.Drawing.Size(405, 180); 

            this.ClientSize = new System.Drawing.Size(460, 420)
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelGroup);
            this.Controls.Add(this.labelMarks);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtGroup);
            this.Controls.Add(this.txtMarks);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.btnShowTwos);
            this.Controls.Add(this.listBoxResults);
            this.Name = "Form1";
            this.Text = "Облік студентів";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Додавання студента
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                STUDENT s = new STUDENT();
                s.NAME = txtName.Text.Trim();
                s.GROUP = txtGroup.Text.Trim();

                string[] parts = txtMarks.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 5)
                {
                    MessageBox.Show("Введіть рівно 5 оцінок через пробіл!", "Помилка");
                    return;
                }

                // Додана перевірка на коректність введених оцінок
                s.SUBJECT = parts.Select(p => int.Parse(p)).ToArray();
                if (s.SUBJECT.Any(mark => mark < 2 || mark > 5))
                {
                    MessageBox.Show("Оцінки повинні бути в діапазоні від 2 до 5!", "Помилка");
                    return;
                }

                learners.Add(s);
                listBoxResults.Items.Add($"{s.NAME} ({s.GROUP}) — додано.");

                txtName.Clear();
                txtGroup.Clear();
                txtMarks.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Некоректний формат оцінок. Переконайтеся, що ви ввели числа.", "Помилка");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка введення: " + ex.Message);
            }
        }

        // Сортування студентів за алфавітом
        private void btnSort_Click(object sender, EventArgs e)
        {
            learners = learners.OrderBy(s => s.NAME).ToList();
            listBoxResults.Items.Clear();
            listBoxResults.Items.Add("Список відсортовано за алфавітом:");
            foreach (var s in learners)
            {
                // Форматуємо оцінки для відображення
                string marksString = string.Join(", ", s.SUBJECT);
                listBoxResults.Items.Add($"{s.NAME} — {s.GROUP} (Оцінки: {marksString})");
            }
        }

        // Пошук студентів з оцінкою "2"
        private void btnShowTwos_Click(object sender, EventArgs e)
        {
            var twos = learners.Where(s => s.SUBJECT.Contains(2)).ToList();

            listBoxResults.Items.Clear();

            if (twos.Count == 0)
            {
                listBoxResults.Items.Add("Немає студентів з оцінкою 2!");
            }
            else
            {
                listBoxResults.Items.Add("Студенти, які мають хоча б одну 2:");
                foreach (var s in twos)
                {
                    string marksString = string.Join(", ", s.SUBJECT);
                    listBoxResults.Items.Add($"{s.NAME} — група {s.GROUP} (Оцінки: {marksString})");
                }
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}