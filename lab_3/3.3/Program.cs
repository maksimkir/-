using System;
using System.Text;
using System.Windows.Forms;


namespace CountABCApp
{
    public class MainForm : Form
    {
        private TextBox inputBox;
        private Button processButton;
        private TextBox outputBox;
        private Label inputLabel;
        private Label outputLabel;



        public MainForm()
        {
            this.Text = "Пошук 'abc'";
            this.Width = 400;
            this.Height = 200;

            inputLabel = new Label() { Text = "W", Top = 20, Left = 20, Width = 200 };
            inputBox = new TextBox() { Top = 50, Left = 20, Width = 300 };

            processButton = new Button() { Text = "text", Top = 90, Left = 20, Width = 100 };
            processButton.Click += ProcessButton_Click;

            outputLabel = new Label() { Text = "Результат:", Top = 140, Left = 20, Width = 200 };
            outputBox = new TextBox() { Top = 170, Left = 20, Width = 540, Multiline = true, Height = 60};

            this.Controls.Add(inputLabel);
            this.Controls.Add(inputBox);
            this.Controls.Add(processButton);
            this.Controls.Add(outputLabel);
            this.Controls.Add(outputBox);
        }
         