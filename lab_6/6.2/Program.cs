using System;
using System.Drawing;
using System.Windows.Forms;

namespace AirTransportApp
{
    // === ІНТЕРФЕЙСИ ===
    public interface IAirTransport
    {
        void Fly();
        string GetInfo();
    }

    public interface IRoute
    {
        string StartRoute(string from, string to);
        string EndRoute();
    }

    // === КЛАС ВЕРТОЛІТ ===
    public class Helicopter : IAirTransport, IRoute
    {
        public string Name { get; set; }
        public int Speed { get; set; }
        public int Range { get; set; }
        public bool HasRotors { get; set; }

        public Helicopter(string name, int speed, int range, bool hasRotors)
        {
            Name = name;
            Speed = speed;
            Range = range;
            HasRotors = hasRotors;
        }

        public void Fly() { }

        public string GetInfo()
        {
            return $"🚁 Вертоліт: {Name}\nШвидкість: {Speed} км/год\nДальність: {Range} км\nРотори: {HasRotors}";
        }

        public string StartRoute(string from, string to)
        {
            return $"Вертоліт летить з {from} до {to}.";
        }

        public string EndRoute()
        {
            return "Вертоліт приземлився.";
        }

        // Власні методи
        public string Land() => "Вертоліт успішно сів.";
        public string Hover() => "Вертоліт зависає у повітрі.";
    }

    // === КЛАС ЛІТАК ===
    public class Airplane : IAirTransport, IRoute
    {
        public string Name { get; set; }
        public int Speed { get; set; }
        public int Range { get; set; }
        public int PassengerCapacity { get; set; }

        public Airplane(string name, int speed, int range, int capacity)
        {
            Name = name;
            Speed = speed;
            Range = range;
            PassengerCapacity = capacity;
        }

        public void Fly() { }

        public string GetInfo()
        {
            return $"✈️ Літак: {Name}\nШвидкість: {Speed} км/год\nДальність: {Range} км\nМісць: {PassengerCapacity}";
        }

        public string StartRoute(string from, string to)
        {
            return $"Літак вирушає з {from} до {to}.";
        }

        public string EndRoute()
        {
            return "Літак прибув у пункт призначення.";
        }

        // Власні методи
        public string TakeOff() => "Літак злетів!";
        public string Refuel() => "Літак дозаправлено.";
    }

    // === ГОЛОВНА ФОРМА ===
    public class MainForm : Form
    {
        private ComboBox comboType;
        private TextBox txtName;
        private TextBox txtSpeed;
        private TextBox txtRange;
        private Button btnCreate;
        private Label lblResult;

        public MainForm()
        {
            Text = "Повітряний транспорт";
            Size = new Size(500, 450);
            Font = new Font("Segoe UI", 10);
            BackColor = Color.WhiteSmoke;

            // Поле вибору типу
            comboType = new ComboBox
            {
                Location = new Point(30, 30),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboType.Items.AddRange(new object[] { "Вертоліт", "Літак" });

            txtName = new TextBox { Location = new Point(30, 80), Size = new Size(200, 30), PlaceholderText = "Назва" };
            txtSpeed = new TextBox { Location = new Point(30, 130), Size = new Size(200, 30), PlaceholderText = "Швидкість (км/год)" };
            txtRange = new TextBox { Location = new Point(30, 180), Size = new Size(200, 30), PlaceholderText = "Дальність (км)" };

            btnCreate = new Button
            {
                Text = "Створити транспорт",
                Location = new Point(30, 230),
                Size = new Size(200, 40),
                BackColor = Color.LightSkyBlue
            };
            btnCreate.Click += BtnCreate_Click;

            lblResult = new Label
            {
                Location = new Point(30, 290),
                Size = new Size(420, 120),
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            Controls.Add(comboType);
            Controls.Add(txtName);
            Controls.Add(txtSpeed);
            Controls.Add(txtRange);
            Controls.Add(btnCreate);
            Controls.Add(lblResult);
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            string type = comboType.Text;
            string name = txtName.Text;
            bool ok1 = int.TryParse(txtSpeed.Text, out int speed);
            bool ok2 = int.TryParse(txtRange.Text, out int range);

            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Оберіть тип транспорту!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(name) || !ok1 || !ok2)
            {
                MessageBox.Show("Введіть усі дані правильно!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string result = "";

            if (type == "Вертоліт")
            {
                Helicopter h = new Helicopter(name, speed, range, true);
                result = h.GetInfo() + "\n\n" + h.StartRoute("Київ", "Львів") + "\n" + h.Land();
            }
            else if (type == "Літак")
            {
                Airplane a = new Airplane(name, speed, range, 180);
                result = a.GetInfo() + "\n\n" + a.StartRoute("Київ", "Одеса") + "\n" + a.TakeOff();
            }

            lblResult.Text = result;
        }
    }

    // === ГОЛОВНИЙ ВХІД У ПРОГРАМУ ===
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // для .NET 6+ / Visual Studio 2022
            Application.Run(new MainForm());
        }
    }
}
