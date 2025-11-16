using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KomunalniBorgy
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }

    public class Form1 : Form
    {
     
        private Label label1, label2, label3, label4, label5;
        private TextBox txtName, txtDebt, txtService, txtAddress, txtSearch;
        private Button btnAdd, btnShowAll, btnCalculate, btnFind;
        private ListBox listBox1;

        private List<(string Name, double Debt, string Service, string Address)> abonents =
            new List<(string, double, string, string)>();

        public Form1()
        {

            this.Text = "Облік комунальних боргів";
            this.ClientSize = new System.Drawing.Size(720, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            int leftLabel = 20;
            int leftBox = 120;
            int top = 20;
            int step = 35;

           
            label1 = new Label() { Text = "Прізвище:", Left = leftLabel, Top = top + 3, Width = 90 };
            txtName = new TextBox() { Left = leftBox, Top = top, Width = 150 };

            label2 = new Label() { Text = "Борг (грн):", Left = leftLabel, Top = top + step + 3, Width = 90 };
            txtDebt = new TextBox() { Left = leftBox, Top = top + step, Width = 150 };

            label3 = new Label() { Text = "Послуга:", Left = leftLabel, Top = top + step * 2 + 3, Width = 90 };
            txtService = new TextBox() { Left = leftBox, Top = top + step * 2, Width = 150 };

            label4 = new Label() { Text = "Адреса:", Left = leftLabel, Top = top + step * 3 + 3, Width = 90 };
            txtAddress = new TextBox() { Left = leftBox, Top = top + step * 3, Width = 150 };

         
            int btnTop = top + step * 4 + 10;
            btnAdd = new Button() { Text = "Додати", Left = leftLabel, Top = btnTop, Width = 80 };
            btnAdd.Click += BtnAdd_Click;

            btnShowAll = new Button() { Text = "Показати всіх", Left = leftLabel + 90, Top = btnTop, Width = 100 };
            btnShowAll.Click += BtnShowAll_Click;

            btnCalculate = new Button() { Text = "Розрахувати", Left = leftLabel + 200, Top = btnTop, Width = 100 };
            btnCalculate.Click += BtnCalculate_Click;

     
            listBox1 = new ListBox()
            {
                Left = 330,
                Top = 20,
                Width = 370,
                Height = 200
            };

            // 🔹 Пошук знизу
            label5 = new Label() { Text = "Пошук (Прізвище):", Left = leftLabel, Top = btnTop + 50, Width = 120 };
            txtSearch = new TextBox() { Left = leftBox, Top = btnTop + 47, Width = 150 };
            btnFind = new Button() { Text = "Знайти", Left = leftBox + 160, Top = btnTop + 45, Width = 80 };
            btnFind.Click += BtnFind_Click;

            Controls.AddRange(new Control[]
            {
                label1, txtName,
                label2, txtDebt,
                label3, txtService,
                label4, txtAddress,
                btnAdd, btnShowAll, btnCalculate,
                listBox1,
                label5, txtSearch, btnFind
            });
        }

        //Додати абонента
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                string service = txtService.Text.Trim();
                string address = txtAddress.Text.Trim();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(service) || string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("Заповніть усі поля!");
                    return;
                }

                double debt = double.Parse(txtDebt.Text);
                abonents.Add((name, debt, service, address));

                listBox1.Items.Add($"{name} | {service} | {debt:F2} грн | {address}");

                txtName.Clear();
                txtDebt.Clear();
                txtService.Clear();
                txtAddress.Clear();
            }
            catch
            {
                MessageBox.Show("Некоректні дані! Перевірте поле 'Борг'.");
            }
        }

        //Показати всіх
        private void BtnShowAll_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var a in abonents)
            {
                listBox1.Items.Add($"{a.Name} | {a.Service} | {a.Debt:F2} грн | {a.Address}");
            }
        }

        //Розрахувати боржників
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            if (abonents.Count == 0)
            {
                MessageBox.Show("Список абонентів порожній!");
                return;
            }

            double totalDebt = abonents.Sum(a => a.Debt);
            double halfDebt = totalDebt / 2.0;
            int toDisconnect = abonents.Count(a => a.Debt >= halfDebt);

            MessageBox.Show(
                $"Загальна сума боргів: {totalDebt:F2} грн\n" +
                $"Половина суми: {halfDebt:F2} грн\n" +
                $"Абонентів для відключення: {toDisconnect}",
                "Результат");
        }

        //Пошук
        private void BtnFind_Click(object sender, EventArgs e)
        {
            string searchName = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("Введіть прізвище для пошуку!");
                return;
            }

            var found = abonents.FirstOrDefault(a =>
                a.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(found.Name))
            {
                MessageBox.Show(
                    $"Абонент: {found.Name}\n" +
                    $"Послуга: {found.Service}\n" +
                    $"Борг: {found.Debt:F2} грн\n" +
                    $"Адреса: {found.Address}",
                    "Знайдено");
            }
            else
            {
                MessageBox.Show("Абонента не знайдено!");
            }
        }
    }
}
