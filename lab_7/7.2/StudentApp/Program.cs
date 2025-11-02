using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StudentApp
{
    // Структура STUDENT
    public struct STUDENT
    {
        public string NAME;     // Прізвище та ініціали
        public string GROUP;    // Номер групи
        public int[] SUBJECT;   // Масив оцінок (5 предметів)
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

            // Заголовок
            this.labelTitle.Text = "Облік студентів";
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(130, 9);
            this.labelTitle.Size = new System.Drawing.Size(200, 30);

            // Поле "Прізвище"
            this.labelName.Text = "Прізвище та ініціали:";
            this.labelName.Location = new System.Drawing.Point(25, 50);
            this.txtName.Location = new System.Drawing.Point(180, 47);
            this.txtName.Size = new System.Drawing.Size(250, 23);

            // Поле "Група"
            this.labelGroup.Text = "Номер групи:";
            this.labelGroup.Location = new System.Drawing.Point(25, 85);
            this.txtGroup.Location = new System.Drawing.Point(180, 82);
            this.txtGroup.Size = new System.Drawing.Size(250, 23);

            // Поле "Оцінки"
            this.labelMarks.Text = "Оцінки (5 чисел через пробіл):";
            this.labelMarks.Location = new System.Drawing.Point(25, 120);
            this.txtMarks.Location = new System.Drawing.Point(230, 117);
            this.txtMarks.Size = new System.Drawing.Size(200, 23);

            // Кнопка "Додати"
            this.btnAdd.Location = new System.Drawing.Point(25, 160);
            this.btnAdd.Size = new System.Drawing.Size(130, 30);
            this.btnAdd.Text = "Додати студента";
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            // Кнопка "Сортувати"
            this.btnSort.Location = new System.Drawing.Point(170, 160);
            this.btnSort.Size = new System.Drawing.Size(100, 30);
            this.btnSort.Text = "Сортувати";
            this.btnSort.Click += new EventHandler(this.btnSort_Click);

            // Кнопка "Показати двійочників"
            this.btnShowTwos.Location = new System.Drawing.Point(280, 160);
            this.btnShowTwos.Size = new System.Drawing.Size(150, 30);
            this.btnShowTwos.Text = "Студенти з оцінкою 2";
            this.btnShowTwos.Click += new EventHandler(this.btnShowTwos_Click);

            // Список результатів
            this.listBoxResults.Location = new System.Drawing.Point(25, 210);
            this.listBoxResults.Size = new System.Drawing.Size(405, 180);

            // Вікно
            this.ClientSize = new System.Drawing.Size(460, 420);
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

                s.SUBJECT = parts.Select(int.Parse).ToArray();

                learners.Add(s);
                listBoxResults.Items.Add($"{s.NAME} ({s.GROUP}) — додано.");

                txtName.Clear();
                txtGroup.Clear();
                txtMarks.Clear();
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
                listBoxResults.Items.Add($"{s.NAME} — {s.GROUP}");
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
                    listBoxResults.Items.Add($"{s.NAME} — група {s.GROUP}");
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
