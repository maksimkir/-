using System;
using System.Windows.Forms;
using System.Text;

namespace LabWorkGUI
{
    public abstract class Фотоапарат
    {
        protected string модель;
        protected double zoom;
        protected string матеріалКорпусу;

        // Конструктор
        public Фотоапарат(string модель, double zoom, string матеріалКорпусу)
        {
            if (zoom < 1 || zoom > 35)
                throw new ArgumentOutOfRangeException("Zoom", "Zoom має бути в межах від 1 до 35.");
            if (матеріалКорпусу.ToLower() != "метал" && матеріалКорпусу.ToLower() != "пластик")
                throw new ArgumentException("Матеріал корпусу має бути 'метал' або 'пластик'.", "матеріалКорпусу");

            this.модель = модель;
            this.zoom = zoom;
            this.матеріалКорпусу = матеріалКорпусу.ToLower();
        }

        public virtual double Вартість()
        {
            double baseCost = (zoom + 2) * (матеріалКорпусу == "пластик" ? 10 : 15);
            return baseCost;
        }

        public bool Дорогий()
        {
            return Вартість() > 200;
        }

        public virtual string Інформація()
        {
            return $"Модель: {модель}, Zoom: {zoom:F1}, Вартість: {Вартість():F2} $";
        }
    }

    public class ЦифровийФотоапарат : Фотоапарат
    {
        protected int мегапікселі;

        public ЦифровийФотоапарат(string модель, double zoom, string матеріалКорпусу, int мегапікселі)
            : base(модель, zoom, матеріалКорпусу)
        {
            if (мегапікселі <= 0)
                throw new ArgumentOutOfRangeException("мегапікселі", "Кількість мегапікселів має бути більше нуля.");
            this.мегапікселі = мегапікселі;
        }
        public override double Вартість()
        {
            return base.Вартість() * мегапікселі;
        }

        public void ОновленняМоделі()
        {
            мегапікселі += 2;
        }

        public override string Інформація()
        {
            string baseInfo = ((Фотоапарат)this).Інформація(); 
            return $"{baseInfo}, Мегапікселі: {мегапікселі} MP, Дорогий: {Дорогий()}";
        }
    }

    public class Камера : ЦифровийФотоапарат
    {
        private string типКамери;

        public Камера(string модель, double zoom, string матеріалКорпусу, int мегапікселі, string типКамери)
            : base(модель, zoom, матеріалКорпусу, мегапікселі)
        {
            this.типКамери = типКамери;
        }

        public override double Вартість()
        {
            // Вартість = Базова вартість Фотоапарата * Мегапікселі * 10
            double baseCameraCost = ((Фотоапарат)this).Вартість(); 
            return baseCameraCost * мегапікселі * 10;
        }

        public new void ОновленняМоделі()
        {
            мегапікселі += 20; 
        }

        public override string Інформація()
        {
             // Викликаємо інформацію від Цифрового фотоапарата, але додаємо тип камери
            return $"{base.Інформація()}, Тип камери: {типКамери}";
        }
    }

    public partial class Form1 : Form
    {

        private Фотоапарат camera;
        private ЦифровийФотоапарат digitalCamera;
        private Камера camcorder;

        public Form1()
        {

            InitializeComponent(); 
            btnUpdate.Enabled = false;
        }

        private void btnCreateObjects_Click(object sender, EventArgs e)
        {
            try
            {

                string model = txtInputModel.Text;
                double zoom = double.Parse(txtInputZoom.Text);
                string material = txtInputMaterial.Text;
                int megapixels = int.Parse(txtInputMegapixels.Text);
                string camcorderType = txtInputCamcorderType.Text;

                camera = new Фотоапарат(model + " (Базовий)", zoom, material);
                digitalCamera = new ЦифровийФотоапарат(model + " (Цифровий)", zoom, material, megapixels);
                camcorder = new Камера(model + " (Камера)", zoom, material, megapixels, camcorderType);

                DisplayInformation(false);
                btnUpdate.Enabled = true; 
            }
            catch (FormatException)
            {
                MessageBox.Show("Помилка вводу: Zoom та Мегапікселі мають бути коректними числами.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Помилка вводу: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невідома помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayInformation(bool isUpdate)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(isUpdate ? "--- ОНОВЛЕНА ІНФОРМАЦІЯ ---" : "--- ПОЧАТКОВА ІНФОРМАЦІЯ ПРО ОБ'ЄКТИ ---");

            // Виведення інформації
            sb.AppendLine($"[Фотоапарат]: {camera.Інформація()}");
            sb.AppendLine($"[Цифровий Фотоапарат]: {digitalCamera.Інформація()}");
            sb.AppendLine($"[Камера]: {camcorder.Інформація()}");
            
            if (isUpdate)
            {
                 txtOutput.AppendText(Environment.NewLine + sb.ToString());
            }
            else
            {
                txtOutput.Text = sb.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (camera == null) return;

            digitalCamera.ОновленняМоделі(); 
            camcorder.ОновленняМоделі();    

            // Вивід оновленої інформації
            DisplayInformation(true);
        }
    }
}