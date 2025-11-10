// Вміст для Form1.Designer.cs

namespace CharacterListProcessor
{
    partial class Form1
    {
        // Обов'язкова змінна дизайнера.
        private System.ComponentModel.IContainer components = null;

        // Звільнення ресурсів (виправлено помилку CS0501)
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматично згенерований конструктором форм Windows

        private void InitializeComponent()
        {
            // Створення та конфігурація елементів GUI
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.rtbOriginalList = new System.Windows.Forms.RichTextBox();
            this.rtbModifiedList = new System.Windows.Forms.RichTextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            // Налаштування кнопки Завантажити
            this.btnLoad.Location = new System.Drawing.Point(20, 20);
            this.btnLoad.Text = "1. Завантажити список";
            this.btnLoad.Size = new System.Drawing.Size(200, 30);
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click); // Прив'язка до методу в Form1.cs
            
            // Налаштування кнопки Обробка
            this.btnProcess.Location = new System.Drawing.Point(240, 20);
            this.btnProcess.Text = "2. Обробити та зберегти";
            this.btnProcess.Size = new System.Drawing.Size(200, 30);
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click); // Прив'язка до методу в Form1.cs
            
            // Налаштування мітки статусу
            this.lblStatus.Location = new System.Drawing.Point(20, 60);
            this.lblStatus.Size = new System.Drawing.Size(420, 20);
            this.lblStatus.Text = "Готово до роботи.";
            
            // Налаштування поля виводу оригінального списку
            this.rtbOriginalList.Location = new System.Drawing.Point(20, 90);
            this.rtbOriginalList.Size = new System.Drawing.Size(200, 200);
            
            // Налаштування поля виводу модифікованого списку
            this.rtbModifiedList.Location = new System.Drawing.Point(240, 90);
            this.rtbModifiedList.Size = new System.Drawing.Size(200, 200);

            // Налаштування самої форми
            this.ClientSize = new System.Drawing.Size(460, 320);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.rtbOriginalList);
            this.Controls.Add(this.rtbModifiedList);
            this.Name = "Form1";
            this.Text = "Обробка списку символів (ЛР)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // ОГОЛОШЕННЯ ЕЛЕМЕНТІВ GUI (Виправлення помилки CS0103)
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.RichTextBox rtbOriginalList;
        private System.Windows.Forms.RichTextBox rtbModifiedList;
        private System.Windows.Forms.Label lblStatus; 
    }
}