using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LabCalculator
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private ListBox listBoxInput;
        private ListBox listBoxOutput;
        private Button btnImport;
        private Button btnCompute;
        private Button btnExport;
        private Button btnClose;
        private ComboBox comboOperation;
        private NumericUpDown numericOperand;
        private Label lblOperand;

        private string sessionLogPath = "Session log.txt";
        private StreamWriter sessionLogWriter;
        private bool sessionOpen = false;

        public MainForm()
        {
            Text = "Lab: GUI Calculator with Session Log";
            Width = 800;
            Height = 500;
            StartPosition = FormStartPosition.CenterScreen;

            InitializeComponents();
            OpenSession();
        }

        private void InitializeComponents()
        {
            listBoxInput = new ListBox { Left = 20, Top = 20, Width = 320, Height = 340 };
            listBoxOutput = new ListBox { Left = 440, Top = 20, Width = 320, Height = 340 };

            btnImport = new Button { Left = 20, Top = 380, Width = 140, Text = "Імпортувати вхідні дані" };
            btnImport.Click += BtnImport_Click;

            comboOperation = new ComboBox { Left = 180, Top = 380, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            comboOperation.Items.AddRange(new object[] { "+ (додати)", "- (відняти)", "* (помножити)", "/ (поділити)", "^ (піднести в степінь)" });
            comboOperation.SelectedIndex = 0;
            comboOperation.SelectedIndexChanged += ComboOperation_SelectedIndexChanged;

            lblOperand = new Label { Left = 320, Top = 384, Width = 60, Text = "Операнд:" };
            numericOperand = new NumericUpDown { Left = 380, Top = 380, Width = 80, DecimalPlaces = 2, Minimum = -1000000, Maximum = 1000000, Value = 1 };

            btnCompute = new Button { Left = 480, Top = 380, Width = 120, Text = "Обчислити вираз" };
            btnCompute.Click += BtnCompute_Click;

            btnExport = new Button { Left = 620, Top = 380, Width = 140, Text = "Експортувати результат" };
            btnExport.Click += BtnExport_Click;

            btnClose = new Button { Left = 620, Top = 420, Width = 140, Text = "Закрити додаток" };
            btnClose.Click += BtnClose_Click;

            Controls.AddRange(new Control[] { listBoxInput, listBoxOutput, btnImport, comboOperation, lblOperand, numericOperand, btnCompute, btnExport, btnClose });
        }

        private void OpenSession()
        {
            try
            {
                sessionLogWriter = new StreamWriter(sessionLogPath, append: true);
                sessionOpen = true;
                LogAction("Дія 1: додаток запущено");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося відкрити Session log: " + ex.Message);
            }
        }

        private void CloseSession()
        {
            if (sessionOpen)
            {
                LogAction("Дія: додаток закрито");
                sessionLogWriter?.Flush();
                sessionLogWriter?.Close();
                sessionOpen = false;
            }
        }

        private void LogAction(string action)
        {
            try
            {               var line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + action;
                sessionLogWriter?.WriteLine(line);
                sessionLogWriter?.Flush();
            }
            catch { /* ignore logging errors */ }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|All files|*.*";
            ofd.Title = "Обрати вхідний файл (числа по рядку)";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {      
                        var lines = File.ReadAllLines(ofd.FileName)
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .Select(l => l.Trim());

                    listBoxInput.Items.Clear();
                    foreach (var l in lines)
                        listBoxInput.Items.Add(l);

                    LogAction("Дія 2: обрано 'Імпортувати вхідні дані' (" + ofd.FileName + ")");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка читання файлу: " + ex.Message);
                }
            }
        }

        private void ComboOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            var op = comboOperation.SelectedItem?.ToString() ?? "";
            LogAction("Дія: обрано арифметичну операцію '" + op + "'");
        }

        private void BtnCompute_Click(object sender, EventArgs e)
        {
            if (listBoxInput.Items.Count == 0)
            {
                MessageBox.Show("Спочатку імпортуйте вхідні дані файлом.");
                return;
            }

            listBoxOutput.Items.Clear();

            double operand = (double)numericOperand.Value;
            string op = comboOperation.SelectedItem?.ToString() ?? "+";

            LogAction("Дія 3: обрано арифметичну операцію '" + op + "'");

            for (int i = 0; i < listBoxInput.Items.Count; i++)
            {
                string s = listBoxInput.Items[i].ToString();
                if (!double.TryParse(s, out double val))
                {
                    listBoxOutput.Items.Add($"{s} -> (помилка: не число)");
                    continue;
                }

                try
                {
                    double res = ApplyOperation(val, operand, op);
                    listBoxOutput.Items.Add($"{val} {GetOpSymbol(op)} {operand} = {res}");
                }
                catch (Exception ex)
                {
                    listBoxOutput.Items.Add($"{val} -> (помилка: {ex.Message})");
                }
            }

            LogAction("Дія 4: обрано 'Обчислити вираз'");
        }

        private double ApplyOperation(double value, double operand, string op)
        {
            // Determine operation by combo box text
            if (op.StartsWith("+")) return value + operand;
            if (op.StartsWith("-")) return value - operand;
            if (op.StartsWith("*")) return value * operand;
            if (op.StartsWith("/"))
            {
                if (operand == 0) throw new DivideByZeroException("Ділення на нуль");
                return value / operand;
            }
            if (op.StartsWith("^")) return Math.Pow(value, operand);

            return value + operand;
        }

        private string GetOpSymbol(string op)
        {
            if (op.StartsWith("+")) return "+";
            if (op.StartsWith("-")) return "-";
            if (op.StartsWith("*")) return "*";
            if (op.StartsWith("/")) return "/";
            if (op.StartsWith("^")) return "^";
            return "?";
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (listBoxOutput.Items.Count == 0)
            {
                MessageBox.Show("Немає результатів для експорту. Спочатку натисніть 'Обчислити вираз'.");
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text files (*.txt)|*.txt|All files|*.*";
            sfd.FileName = "result.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {                    File.WriteAllLines(sfd.FileName, listBoxOutput.Items.Cast<string>());
                    LogAction("Дія 7: обрано 'Експортувати результат у файл' (" + sfd.FileName + ")");
                    MessageBox.Show("Результати збережено у: " + sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка запису: " + ex.Message);
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            CloseSession();
        }
    }
}
