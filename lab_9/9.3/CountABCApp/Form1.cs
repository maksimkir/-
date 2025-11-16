using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ParenBalanceApp
{
    public class MainForm : Form
    {
        private Button btnLoadFile;
        private Button btnCheck;
        private TextBox txtExpression;
        private ListBox lstOutput;
        private Label lblExpr;
        private Label lblOutput;

        // Масив-стек і покажчик вершини стеку реалізовані всередині класу ArrayStack
        private class ArrayStack
        {
            private int[] arr;
            private int top; // індекс вершини, -1 коли порожній
            public int Capacity => arr.Length;

            public ArrayStack(int capacity)
            {
                arr = new int[capacity];
                top = -1;
            }

            public bool IsEmpty() => top == -1;
            public bool IsFull() => top == arr.Length - 1;

            public void Push(int value)
            {
                if (IsFull())
                    throw new InvalidOperationException("Переповнення стеку (push на повний стек).");
                arr[++top] = value;
            }

            public int Pop()
            {
                if (IsEmpty())
                    throw new InvalidOperationException("Витяг з порожнього стеку (pop).");
                return arr[top--];
            }

            public int Peek()
            {
                if (IsEmpty())
                    throw new InvalidOperationException("Peek з порожнього стеку.");
                return arr[top];
            }

            // для налагодження
            public int Count => top + 1;
        }

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Перевірка балансу круглих дужок";
            this.Width = 800;
            this.Height = 500;

            lblExpr = new Label() { Left = 12, Top = 12, Text = "Вираз:", AutoSize = true };
            txtExpression = new TextBox()
            {
                Left = 12,
                Top = 32,
                Width = 760,
                Height = 80,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            btnLoadFile = new Button() { Left = 12, Top = 120, Width = 140, Text = "Завантажити з файлу" };
            btnCheck = new Button() { Left = 160, Top = 120, Width = 140, Text = "Перевірити баланс" };

            lblOutput = new Label() { Left = 12, Top = 160, Text = "Результат:", AutoSize = true };
            lstOutput = new ListBox()
            {
                Left = 12,
                Top = 180,
                Width = 760,
                Height = 260
            };

            btnLoadFile.Click += BtnLoadFile_Click;
            btnCheck.Click += BtnCheck_Click;

            this.Controls.Add(lblExpr);
            this.Controls.Add(txtExpression);
            this.Controls.Add(btnLoadFile);
            this.Controls.Add(btnCheck);
            this.Controls.Add(lblOutput);
            this.Controls.Add(lstOutput);
        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                string text = File.ReadAllText(ofd.FileName);
                txtExpression.Text = text;
                lstOutput.Items.Clear();
                lstOutput.Items.Add($"Файл '{Path.GetFileName(ofd.FileName)}' завантажено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка читання файлу: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            lstOutput.Items.Clear();
            string expr = txtExpression.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(expr))
            {
                MessageBox.Show("Введіть або завантажте вираз у вікні вище.", "Немає даних", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var result = CheckParenthesesBalance(expr);
                if (!result.IsBalanced)
                {
                    lstOutput.Items.Add("Дужки НЕ збалансовані.");
                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                        lstOutput.Items.Add("Причина: " + result.ErrorMessage);
                }
                else
                {
                    lstOutput.Items.Add("Дужки збалансовані.");
                    lstOutput.Items.Add("Пари (позиція відкриваючої, позиція закриваючої) — впорядковані за зростанням позиції закриваючої дужки:");
                    foreach (var pair in result.Pairs)
                    {
                        lstOutput.Items.Add($"({pair.OpenPos}, {pair.ClosePos})");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Під час перевірки сталася помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Результат перевірки
        private class CheckResult
        {
            public bool IsBalanced { get; set; }
            public List<(int OpenPos, int ClosePos)> Pairs { get; set; } = new List<(int, int)>();
            public string ErrorMessage { get; set; } = "";
        }

        /// <summary>
        /// Перевіряє збалансованість круглих дужок у виразі.
        /// Використовує стек, реалізований на масиві цілих (позиції відповідних відкритих дужок).
        /// Повертає пари позицій (1-based). Сортує по позиції закриваючих дужок зростаюче.
        /// </summary>
        private CheckResult CheckParenthesesBalance(string expr)
        {
            var res = new CheckResult();

            // capacity = довжина виразу — цього достатньо для стеку позицій відкритих дужок
            ArrayStack stack = new ArrayStack(expr.Length);
            var pairs = new List<(int OpenPos, int ClosePos)>();

            // Проходимо символи зліва направо, індексація у тексті — 1-based
            for (int i = 0; i < expr.Length; i++)
            {
                char ch = expr[i];
                int pos = i + 1; // позиція у тексті (1-based)

                if (ch == '(')
                {
                    // кладемо позицію відкриваючої дужки в стек
                    try
                    {
                        stack.Push(pos);
                    }
                    catch (InvalidOperationException)
                    {
                        // Переповнення стеку — теоретично не повинно статися, бо capacity = expr.Length
                        res.IsBalanced = false;
                        res.ErrorMessage = "Переповнення стеку при спробі додати відкриваючу дужку.";
                        return res;
                    }
                }
                else if (ch == ')')
                {
                    // при зустрічі закриваючої — перевіряємо, чи є у стеку відповідна відкрита
                    if (stack.IsEmpty())
                    {
                        res.IsBalanced = false;
                        res.ErrorMessage = $"Знайдена закриваюча дужка без відповідної відкриваючої на позиції {pos}.";
                        return res;
                    }
                    else
                    {
                        int openPos = stack.Pop();
                        pairs.Add((openPos, pos));
                    }
                }
                // інші символи ігноруємо
            }

            // Після проходу по всіх символах стек повинен бути порожній
            if (!stack.IsEmpty())
            {
                res.IsBalanced = false;
                // якщо залишились відкриті дужки — повідомляємо позицію першої (зверху стеку)
                int remainingOpenPos = stack.Pop(); // можна показати верхню (останню відкриту)
                res.ErrorMessage = $"Є незакриті відкриваючі дужки. Остання незакрита на позиції {remainingOpenPos}.";
                return res;
            }

            // Якщо сюди дійшли — все збалансовано
            // Потрібно вивести пари по зростанню позиції закриваючої дужки
            pairs.Sort((a, b) => a.ClosePos.CompareTo(b.ClosePos));
            res.IsBalanced = true;
            res.Pairs = pairs;
            return res;
        }
    }
}
