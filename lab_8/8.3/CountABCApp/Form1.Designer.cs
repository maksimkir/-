namespace SeasonsQuiz
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboBoxSeason;
        private Button btnShow;
        private Button btnExit;
        private TextBox textBoxResult;
        private Label label1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            comboBoxSeason = new ComboBox();
            btnShow = new Button();
            btnExit = new Button();
            textBoxResult = new TextBox();
            label1 = new Label();
            SuspendLayout();
          
            comboBoxSeason.Font = new Font("Segoe UI", 12F);
            comboBoxSeason.FormattingEnabled = true;
            comboBoxSeason.Items.AddRange(new object[] { "весна", "літо", "осінь", "зима" });
            comboBoxSeason.Location = new Point(30, 60);
            comboBoxSeason.Name = "comboBoxSeason";
            comboBoxSeason.Size = new Size(200, 29);
           
            btnShow.Font = new Font("Segoe UI", 12F);
            btnShow.Location = new Point(260, 60);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(120, 30);
            btnShow.Text = "Показати";
            btnShow.Click += btnShow_Click;
           
            btnExit.Font = new Font("Segoe UI", 12F);
            btnExit.Location = new Point(260, 300);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(120, 30);
            btnExit.Text = "Вийти";
            btnExit.Click += btnExit_Click;
           
            textBoxResult.Font = new Font("Segoe UI", 12F);
            textBoxResult.Location = new Point(30, 120);
            textBoxResult.Multiline = true;
            textBoxResult.Name = "textBoxResult";
            textBoxResult.ScrollBars = ScrollBars.Vertical;
            textBoxResult.Size = new Size(350, 160);
            
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(30, 20);
            label1.Name = "label1";
            label1.Size = new Size(250, 30);
            label1.Text = "Виберіть пору року:";
            
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 360);
            Controls.Add(label1);
            Controls.Add(textBoxResult);
            Controls.Add(btnExit);
            Controls.Add(btnShow);
            Controls.Add(comboBoxSeason);
            Name = "Form1";
            Text = "Вікторина – Пори року";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
