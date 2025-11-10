namespace QuizApplication
{
    // Частковий клас, що оголошує елементи GUI
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InputTextBox (Ввід даних користувача)
            // 
            this.InputTextBox.Location = new System.Drawing.Point(50, 50);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(200, 20);
            this.InputTextBox.TabIndex = 0;
            // 
            // SubmitButton (Кнопка для запуску логіки)
            // 
            this.SubmitButton.Location = new System.Drawing.Point(260, 48);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(150, 23);
            this.SubmitButton.TabIndex = 1;
            this.SubmitButton.Text = "Дізнатися / Завершити (0)";
            this.SubmitButton.UseVisualStyleBackColor = true;
            // 
            // OutputLabel (Відображення результатів)
            // 
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Location = new System.Drawing.Point(50, 100);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(176, 13);
            this.OutputLabel.TabIndex = 2;
            this.OutputLabel.Text = "Введіть пору року або 0 для виходу";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.InputTextBox);
            this.Name = "Form1";
            this.Text = "Вікторина: Пори року";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        // Оголошення змінних (MUST HAVE для Form1.cs)
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.Label OutputLabel;
    }
}