using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SymbolListApp
{
    public class Form1 : Form
    {
        private Button btnLoad, btnProcess, btnSave;
        private TextBox txtOriginal, txtProcessed;
        private List<char> symbols = new List<char>();

        public Form1()
        {
            this.Text = "Обробка списку символів";
            this.Width = 800;
            this.Height = 600;

            btnLoad = new Button() { Text = "Зчитати файл", Left = 20, Top = 20, Width = 200 };
            btnProcess = new Button() { Text = "Обробити", Left = 240, Top = 20, Width = 200 };
            btnSave = new Button() { Text = "Зберегти у файл", Left = 460, Top = 20, Width = 200 };

            txtOriginal = new TextBox()
            {
                Left = 20,
                Top = 70,
                Width = 350,
                Height = 450,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            txtProcessed = new TextBox()
            {
                Left = 400,
                Top = 70,
                Width = 350,
                Height = 450,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            btnLoad.Click += BtnLoad_Click;
            btnProcess.Click += BtnProcess_Click;
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(btnLoad);
            this.Controls.Add(btnProcess);
            this.Controls.Add(btnSave);
            this.Controls.Add(txtOriginal);
            this.Controls.Add(txtProcessed);
        }

        // 1) Зчитування файлу
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string content = File.ReadAllText(ofd.FileName);

                symbols = content.ToList();

                txtOriginal.Text = string.Join("", symbols);
            }
        }

        // 2) Видалення повторів
        private void BtnProcess_Click(object sender, EventArgs e)
        {
            if (symbols.Count == 0)
            {
                MessageBox.Show("Спершу зчитайте файл!");
                return;
            }

            List<char> unique = new List<char>();

            foreach (char c in symbols)
                if (!unique.Contains(c))
                    unique.Add(c);

            symbols = unique;

            txtProcessed.Text = string.Join("", symbols);
        }

        // 3) Збереження у файл (по 8 символів у рядку)
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (symbols.Count == 0)
            {
                MessageBox.Show("Немає даних для збереження!");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text files (*.txt)|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    for (int i = 0; i < symbols.Count; i++)
                    {
                        sw.Write(symbols[i]);

                        if ((i + 1) % 8 == 0)
                            sw.WriteLine();
                    }
                }

                MessageBox.Show("Файл успішно збережено!");
            }
        }
    }
}
