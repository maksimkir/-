using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace FermaApp
{
    public class Ferma
    {
        // 🔹 Закриті поля
        private string nazva;
        private string vlasnyk;
        private double ploshcha;
        private int kilkistTvaryn;
        private int kilkistPracivnykiv;
        private double richnyiDohid;
        private string typFerma;

        // 🔹 Властивості
        public string Nazva { get => nazva; set => nazva = value; }
        public string Vlasnyk { get => vlasnyk; set => vlasnyk = value; }
        public double Ploshcha { get => ploshcha; set => ploshcha = value; }
        public int KilkistTvaryn { get => kilkistTvaryn; set => kilkistTvaryn = value; }
        public int KilkistPracivnykiv { get => kilkistPracivnykiv; set => kilkistPracivnykiv = value; }
        public double RichnyiDohid { get => richnyiDohid; set => richnyiDohid = value; }
        public string TypFerma { get => typFerma; set => typFerma = value; }

        // 🔹 Конструктор без параметрів
        public Ferma()
        {
            nazva = "Невідома";
            vlasnyk = "Невідомий";
            ploshcha = 0;
            kilkistTvaryn = 0;
            kilkistPracivnykiv = 0;
            richnyiDohid = 0;
            typFerma = "Невідомий";
        }

        // 🔹 Методи класу
        public double DohidNaPracivnyka()
        {
            if (kilkistPracivnykiv == 0) return 0;
            return richnyiDohid / kilkistPracivnykiv;
        }

        public double ShylnistTvaryn()
        {
            if (ploshcha == 0) return 0;
            return kilkistTvaryn / ploshcha;
        }

        public void ZbilshytyDohid(double vidsotok)
        {
            richnyiDohid += richnyiDohid * (vidsotok / 100);
        }
    }

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class MainForm : Form
    {
        // 🔹 Елементи інтерфейсу
        private TextBox txtNazva, txtVlasnyk, txtPloshcha, txtTvaryny, txtPracivnyky, txtDohid, txtTyp;
        private Button btnCreate;
        private Label lblResult;

        public MainForm()
        {
            this.Text = "Ферма — Лабораторна робота";
            this.Width = 420;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Beige;

            Label lblTitle = new Label()
            {
                Text = "Введіть дані про ферму:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Top = 10,
                Left = 20,
                Width = 350
            };
            this.Controls.Add(lblTitle);

            int y = 50;
            int step = 40;

            // 🔹 Поля вводу
            AddLabelAndTextBox("Назва:", ref txtNazva, y); y += step;
            AddLabelAndTextBox("Власник:", ref txtVlasnyk, y); y += step;
            AddLabelAndTextBox("Площа (га):", ref txtPloshcha, y); y += step;
            AddLabelAndTextBox("Кількість тварин:", ref txtTvaryny, y); y += step;
            AddLabelAndTextBox("Працівників:", ref txtPracivnyky, y); y += step;
            AddLabelAndTextBox("Річний дохід (грн):", ref txtDohid, y); y += step;
            AddLabelAndTextBox("Тип ферми:", ref txtTyp, y); y += step + 10;

            btnCreate = new Button()
            {
                Text = "Створити об'єкт і зберегти",
                Top = y,
                Left = 100,
                Width = 200,
                BackColor = Color.LightGreen
            };
            btnCreate.Click += BtnCreate_Click;
            this.Controls.Add(btnCreate);

            y += step;
            lblResult = new Label()
            {
                Top = y,
                Left = 20,
                Width = 350,
                Height = 150,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            this.Controls.Add(lblResult);
        }

        private void AddLabelAndTextBox(string labelText, ref TextBox txt, int top)
        {
            Label lbl = new Label()
            {
                Text = labelText,
                Top = top + 5,
                Left = 20,
                Width = 120
            };
            this.Controls.Add(lbl);

            txt = new TextBox()
            {
                Top = top,
                Left = 150,
                Width = 220
            };
            this.Controls.Add(txt);
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Ferma f = new Ferma();
                f.Nazva = txtNazva.Text;
                f.Vlasnyk = txtVlasnyk.Text;
                f.Ploshcha = double.Parse(txtPloshcha.Text);
                f.KilkistTvaryn = int.Parse(txtTvaryny.Text);
                f.KilkistPracivnykiv = int.Parse(txtPracivnyky.Text);
                f.RichnyiDohid = double.Parse(txtDohid.Text);
                f.TypFerma = txtTyp.Text;

                // 🔹 Збереження у файл
                string data = $"Назва: {f.Nazva}\n" +
                              $"Власник: {f.Vlasnyk}\n" +
                              $"Площа: {f.Ploshcha} га\n" +
                              $"Кількість тварин: {f.KilkistTvaryn}\n" +
                              $"Працівників: {f.KilkistPracivnykiv}\n" +
                              $"Річний дохід: {f.RichnyiDohid} грн\n" +
                              $"Тип ферми: {f.TypFerma}\n";
                File.WriteAllText("ferma.txt", data);

                // 🔹 Виклик методів
                double dohidNaPracivnyka = f.DohidNaPracivnyka();
                double shylnist = f.ShylnistTvaryn();
                f.ZbilshytyDohid(10);

                lblResult.Text =
                    $"✅ Об'єкт створено та збережено у ferma.txt\n\n" +
                    $"Дохід на працівника: {dohidNaPracivnyka:F2} грн\n" +
                    $"Щільність тварин: {shylnist:F2} гол/га\n" +
                    $"Новий дохід (+10%): {f.RichnyiDohid:F2} грн";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка введення даних: " + ex.Message, "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }