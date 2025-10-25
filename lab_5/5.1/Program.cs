using System;
using System.IO;
using System.Windows.Forms;

namespace FermaApp
{
    public class Ferma
    {
        private string nazva;
        private string vlasnyk;
        private double ploshcha;
        private int kilkistTvaryn;
        private int kilkistPracivnykiv;
        private double richnyiDohid;
        private string typFerma;

        public string Nazva { get => nazva; set => nazva = value; }
        public string Vlasnyk { get => vlasnyk; set => vlasnyk = value; }
        public double Ploshcha { get => ploshcha; set => ploshcha = value; }
        public int KilkistTvaryn { get => kilkistTvaryn; set => kilkistTvaryn = value; }
        public int KilkistPracivnykiv { get => kilkistPracivnykiv; set => kilkistPracivnykiv = value; }
        public double RichnyiDohid { get => richnyiDohid; set => richnyiDohid = value; }
        public string TypFerma { get => typFerma; set => typFerma = value; }

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

    public class MainForm : Form
    {
        private TextBox txtNazva, txtVlasnyk, txtPloshcha, txtTvaryny, txtPracivnyky, txtDohid, txtTyp;
        private Button btnCreate;
        private Label lblResult;

        public MainForm()
        {
            this.Text = "Ферма - Введення даних";
            this.Width = 400;
            this.Height = 520;

            Label lblTitle = new Label()
            {
                Text = "Введіть дані про ферму:",
                Top = 10,
                Left = 20,
                Width = 300
            };
            this.Controls.Add(lblTitle);

            int y = 40;
            int step = 40;

            // Поля вводу
            this.Controls.Add(new Label() { Text = "Назва:", Top = y, Left = 20 });
            txtNazva = new TextBox() { Top = y, Left = 150, Width = 200 };
            this.Controls.Add(txtNazva);

            y += step;
            this.Controls.Add(new Label() { Text = "Власник:", Top = y, Left = 20 });
            txtVlasnyk = new TextBox() { Top = y, Left = 150, Width = 200 };
            this.Controls.Add(txtVlasnyk);

            y += step;
            this.Controls.Add(new Label() { Text = "Площа (га):", Top = y, Left = 20 });
            txtPloshcha = new TextBox() { Top = y, Left = 150, Width = 200 };
            this.Controls.Add(txtPloshcha);

            y += step;
            this.Controls.Add(new Label() { Text = "Кількість тварин:", Top = y, Left = 20 });
            txtTvaryny = new TextBox() { Top = y, Left = 150, Width = 200 };
            this.Controls.Add(txtTvaryny);

            y += step;
            this.Controls.Add(new Label() { Text = "Працівників:", Top = y, Left = 20 });
            txtPracivnyky = new TextBox() { Top = y, Left = 150, Width = 200 };
            this.Controls.Add(txtPracivnyky);

            y += step;
            this.Controls.Add(new Label() { Text = "Річний дохід (грн):", Top = y, Left = 20 });
            txtDohid = new TextBox() { Top = y, Left = 150, Width = 200 };
            this.Controls.Add(txtDohid);

            y += step;
            this.Controls.Add(new Label() { Text = "Тип ферми:", Top = y, Left = 20 });
            txtTyp = new TextBox() { Top = y, Left = 150, Width = 200 };
            this.Controls.Add(txtTyp);

            y += step + 10;
            btnCreate = new Button()
            {
                Text = "Створити об'єкт і зберегти",
                Top = y,
                Left = 100,
                Width = 180
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
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblResult);
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

                string data = $"Назва: {f.Nazva}\n" +
                              $"Власник: {f.Vlasnyk}\n" +
                              $"Площа: {f.Ploshcha} га\n" +
                              $"Кількість тварин: {f.KilkistTvaryn}\n" +
                              $"Працівників: {f.KilkistPracivnykiv}\n" +
                              $"Річний дохід: {f.RichnyiDohid} грн\n" +
                              $"Тип ферми: {f.TypFerma}\n";

                File.WriteAllText("ferma.txt", data);

                double dohidNaPracivnyka = f.DohidNaPracivnyka();
                double shylnist = f.ShylnistTvaryn();
                f.ZbilshytyDohid(10);

                lblResult.Text =
                    $"✅ Об'єкт створено та збережено у ferma.txt\n\n" +
                    $"Дохід на працівника: {dohidNaPracivnyka:F2} грн\n" +
                    $"Щільність тварин: {shylnist:F2} гол/га\n" +
                    $"Новий дохід (після +10%): {f.RichnyiDohid:F2} грн";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка введення даних: " + ex.Message);
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
