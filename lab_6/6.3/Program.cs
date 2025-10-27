using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WorkersApp
{
    
    public class Worker : IComparable<Worker>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }

        public Worker(string name, int age, double salary)
        {
            Name = name;
            Age = age;
            Salary = salary;
        }

        public int CompareTo(Worker other)
        {
            if (other == null) return 1;
            return Age.CompareTo(other.Age);
        }

        public override string ToString()
        {
            return $"{Name,-15} | Вік: {Age,3} | Зарплата: {Salary,8:F2}";
        }
    }

    public class WorkerSalaryComparer : IComparer<Worker>
    {
        public int Compare(Worker x, Worker y)
        {
            if (x == null || y == null) return 0;
            return x.Salary.CompareTo(y.Salary);
        }
    }

    public class WorkerCollection : IEnumerable<Worker>
    {
        private List<Worker> workers = new List<Worker>();

        public void Add(Worker worker)
        {
            workers.Add(worker);
        }

        public IEnumerator<Worker> GetEnumerator()
        {
            foreach (var w in workers)
                yield return w;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

       
        public void SortByAge()
        {
            workers.Sort(); 
        }

        public void SortBySalary()
        {
            workers.Sort(new WorkerSalaryComparer()); 
        }

        public override string ToString()
        {
            string result = "";
            foreach (var w in workers)
                result += w + "\n";
            return result;
        }
    }

    public class MainForm : Form
    {
        private WorkerCollection workers = new WorkerCollection();

        private TextBox txtName;
        private TextBox txtAge;
        private TextBox txtSalary;
        private Button btnAdd;
        private Button btnSortAge;
        private Button btnSortSalary;
        private ListBox listWorkers;
        private Label lblInfo;

        public MainForm()
        {
            Text = "Працівники — Лабораторна робота (IComparable, IComparer, IEnumerable)";
            Size = new Size(700, 500);
            Font = new Font("Segoe UI", 10);
            BackColor = Color.WhiteSmoke;

            Label lbl1 = new Label { Text = "Ім'я:", Location = new Point(30, 30), AutoSize = true };
            txtName = new TextBox { Location = new Point(100, 25), Width = 150 };

            Label lbl2 = new Label { Text = "Вік:", Location = new Point(30, 70), AutoSize = true };
            txtAge = new TextBox { Location = new Point(100, 65), Width = 150 };

            Label lbl3 = new Label { Text = "Зарплата:", Location = new Point(30, 110), AutoSize = true };
            txtSalary = new TextBox { Location = new Point(100, 105), Width = 150 };

            btnAdd = new Button
            {
                Text = "Додати працівника",
                Location = new Point(30, 150),
                Size = new Size(220, 35),
                BackColor = Color.LightGreen
            };
            btnAdd.Click += BtnAdd_Click;

            btnSortAge = new Button
            {
                Text = "Сортувати за віком",
                Location = new Point(30, 200),
                Size = new Size(220, 35),
                BackColor = Color.LightSkyBlue
            };
            btnSortAge.Click += BtnSortAge_Click;

            btnSortSalary = new Button
            {
                Text = "Сортувати за зарплатою",
                Location = new Point(30, 250),
                Size = new Size(220, 35),
                BackColor = Color.LightCoral
            };
            btnSortSalary.Click += BtnSortSalary_Click;

            listWorkers = new ListBox
            {
                Location = new Point(280, 25),
                Size = new Size(380, 350),
                Font = new Font("Consolas", 10)
            };

            lblInfo = new Label
            {
                Text = "Введіть дані працівників та натискайте «Додати».",
                Location = new Point(30, 310),
                Size = new Size(250, 100),
                AutoSize = false
            };

            Controls.Add(lbl1);
            Controls.Add(lbl2);
            Controls.Add(lbl3);
            Controls.Add(txtName);
            Controls.Add(txtAge);
            Controls.Add(txtSalary);
            Controls.Add(btnAdd);
            Controls.Add(btnSortAge);
            Controls.Add(btnSortSalary);
            Controls.Add(listWorkers);
            Controls.Add(lblInfo);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text;
                int age = int.Parse(txtAge.Text);
                double salary = double.Parse(txtSalary.Text);

                Worker w = new Worker(name, age, salary);
                workers.Add(w);

                UpdateList();
                txtName.Clear(); txtAge.Clear(); txtSalary.Clear();
            }
            catch
            {
                MessageBox.Show("Помилка введення даних! Перевірте формат чисел.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSortAge_Click(object sender, EventArgs e)
        {
            workers.SortByAge();
            UpdateList();
        }

        private void BtnSortSalary_Click(object sender, EventArgs e)
        {
            workers.SortBySalary();
            UpdateList();
        }

        private void UpdateList()
        {
            listWorkers.Items.Clear();
            foreach (var w in workers)
                listWorkers.Items.Add(w.ToString());
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
