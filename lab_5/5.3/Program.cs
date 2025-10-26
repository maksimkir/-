using System;
using System.Windows.Forms;

namespace PhotoApp
{
    public partial class MainForm : Form
    {
        PhotoCamera photo;
        DigitalCamera digital;
        Camera camera;

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string model = textBoxModel.Text;
                double zoom = double.Parse(textBoxZoom.Text);
                string material = comboBoxMaterial.Text;
                int mp = int.Parse(textBoxMegaPixels.Text);
                string type = textBoxType.Text;

                photo = new PhotoCamera(model, zoom, material);
                digital = new DigitalCamera(model + "_D", zoom + 2, material, mp);
                camera = new Camera(model + "_C", zoom + 5, material, mp + 5, type);

                textBoxOutput.Text =
                    "=== Початкова інформація ===" + Environment.NewLine +
                    photo.Info() + Environment.NewLine +
                    digital.Info() + Environment.NewLine +
                    camera.Info();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (photo == null)
            {
                MessageBox.Show("Спочатку створіть об’єкти!");
                return;
            }

            digital.UpdateModel();
            camera.UpdateModel();

            textBoxOutput.Text =
                "=== Після оновлення моделей ===" + Environment.NewLine +
                digital.Info() + Environment.NewLine +
                camera.Info();
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.textBoxModel = new TextBox();
            this.label2 = new Label();
            this.textBoxZoom = new TextBox();
            this.label3 = new Label();
            this.comboBoxMaterial = new ComboBox();
            this.label4 = new Label();
            this.textBoxMegaPixels = new TextBox();
            this.label5 = new Label();
            this.textBoxType = new TextBox();
            this.buttonCreate = new Button();
            this.buttonUpdate = new Button();
            this.textBoxOutput = new TextBox();
            this.SuspendLayout();
            
            this.label1.Text = "Модель:";
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.AutoSize = true;
            
            this.textBoxModel.Location = new System.Drawing.Point(120, 20);
            this.textBoxModel.Width = 150;
            
            this.label2.Text = "Zoom:";
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.AutoSize = true;
            
            this.textBoxZoom.Location = new System.Drawing.Point(120, 60);
            this.textBoxZoom.Width = 150;

            this.label3.Text = "Матеріал:";
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.AutoSize = true;
            
            this.comboBoxMaterial.Location = new System.Drawing.Point(120, 100);
            this.comboBoxMaterial.Width = 150;
            this.comboBoxMaterial.Items.AddRange(new object[] { "пластик", "метал" });
            
            this.label4.Text = "Мегапікселі:";
            this.label4.Location = new System.Drawing.Point(20, 140);
            this.label4.AutoSize = true;
           
            this.textBoxMegaPixels.Location = new System.Drawing.Point(120, 140);
            this.textBoxMegaPixels.Width = 150;
           
            this.label5.Text = "Тип камери:";
            this.label5.Location = new System.Drawing.Point(20, 180);
            this.label5.AutoSize = true;
        
            this.textBoxType.Location = new System.Drawing.Point(120, 180);
            this.textBoxType.Width = 150;
           
            this.buttonCreate.Text = "Створити об’єкти";
            this.buttonCreate.Location = new System.Drawing.Point(20, 230);
            this.buttonCreate.Width = 120;
            this.buttonCreate.Click += new EventHandler(this.buttonCreate_Click);
            
            this.buttonUpdate.Text = "Оновити моделі";
            this.buttonUpdate.Location = new System.Drawing.Point(150, 230);
            this.buttonUpdate.Width = 120;
            this.buttonUpdate.Click += new EventHandler(this.buttonUpdate_Click);
            
            this.textBoxOutput.Location = new System.Drawing.Point(20, 270);
            this.textBoxOutput.Width = 400;
            this.textBoxOutput.Height = 200;
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.ScrollBars = ScrollBars.Vertical;
            
            this.ClientSize = new System.Drawing.Size(450, 500);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxModel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxZoom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxMaterial);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxMegaPixels);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxType);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.textBoxOutput);
            this.Text = "Фотоапарати – лабораторна робота";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label label1;
        private TextBox textBoxModel;
        private Label label2;
        private TextBox textBoxZoom;
        private Label label3;
        private ComboBox comboBoxMaterial;
        private Label label4;
        private TextBox textBoxMegaPixels;
        private Label label5;
        private TextBox textBoxType;
        private Button buttonCreate;
        private Button buttonUpdate;
        private TextBox textBoxOutput;
    }


    public class PhotoCamera
    {
        public string Model { get; set; }
        public double Zoom { get; set; }
        public string Material { get; set; }

        public PhotoCamera(string model, double zoom, string material)
        {
            Model = model;
            Zoom = zoom;
            Material = material.ToLower();
        }

        public virtual double Cost()
        {
            if (Material == "пластик")
                return (Zoom + 2) * 10;
            else
                return (Zoom + 2) * 15;
        }

        public bool IsExpensive()
        {
            return Cost() > 200;
        }

        public virtual string Info()
        {
            return $"Модель: {Model}, Zoom: {Zoom}, Матеріал: {Material}, Вартість: {Cost()}$, Дорогий: {IsExpensive()}";
        }
    }

    public class DigitalCamera : PhotoCamera
    {
        public int MegaPixels { get; set; }

        public DigitalCamera(string model, double zoom, string material, int megaPixels)
            : base(model, zoom, material)
        {
            MegaPixels = megaPixels;
        }

        public override double Cost()
        {
            return base.Cost() * MegaPixels;
        }

        public void UpdateModel()
        {
            MegaPixels += 2;
        }

        public override string Info()
        {
            return $"[Цифровий] Модель: {Model}, Zoom: {Zoom}, Матеріал: {Material}, МП: {MegaPixels}, Вартість: {Cost()}$, Дорогий: {IsExpensive()}";
        }
    }

    public class Camera : DigitalCamera
    {
        public string Type { get; set; }

        public Camera(string model, double zoom, string material, int megaPixels, string type)
            : base(model, zoom, material, megaPixels)
        {
            Type = type;
        }

        public override double Cost()
        {
            return base.Cost() * 10;
        }

        public new void UpdateModel()
        {
            MegaPixels += 20;
        }

        public override string Info()
        {
            return $"[Камера] Модель: {Model}, Zoom: {Zoom}, Матеріал: {Material}, МП: {MegaPixels}, Тип: {Type}, Вартість: {Cost()}$, Дорогий: {IsExpensive()}";
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
