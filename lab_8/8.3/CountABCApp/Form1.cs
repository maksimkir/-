using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SeasonsQuiz
{
    public partial class Form1 : Form
    {
        Dictionary<Season, List<(string month, int days)>> seasons =
            new Dictionary<Season, List<(string, int)>>()
            {
                { Season.Весна, new List<(string, int)>
                    { ("Березень", 31), ("Квітень", 30), ("Травень", 31) } },

                { Season.Літо, new List<(string, int)>
                    { ("Червень", 30), ("Липень", 31), ("Серпень", 31) } },

                { Season.Осінь, new List<(string, int)>
                    { ("Вересень", 30), ("Жовтень", 31), ("Листопад", 30) } },

                { Season.Зима, new List<(string, int)>
                    { ("Грудень", 31), ("Січень", 31), ("Лютий", 28) } }
            };

        public Form1()
        {
            InitializeComponent();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            bool work = true;

            while (work)
            {
                string input = comboBoxSeason.Text.Trim().ToLower();
                Season season;

                switch (input)
                {
                    case "весна":
                        season = Season.Весна;
                        break;

                    case "літо":
                        season = Season.Літо;
                        break;

                    case "осінь":
                        season = Season.Осінь;
                        break;

                    case "зима":
                        season = Season.Зима;
                        break;

                    default:
                        MessageBox.Show("Помилка: введіть коректну пору року!");
                        return;
                }

                textBoxResult.Clear();
                foreach (var m in seasons[season])
                {
                    textBoxResult.AppendText($"{m.month} – {m.days} днів\n");
                }

                DialogResult answer = MessageBox.Show(
                    "Бажаєте продовжити?", 
                    "Вікторина", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (answer == DialogResult.No)
                    work = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
