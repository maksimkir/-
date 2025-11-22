using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GraphMatricesApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ==================== КНОПКА "Завантажити зображення" ====================
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp|All files|*.*";

            // Підставимо шлях до твого файла
            ofd.FileName = "/mnt/data/5920322e-cfa7-43b2-a855-a27b0f0cee4e.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox.Image = Image.FromFile(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вдалося відкрити зображення: " + ex.Message);
                }
            }
        }

        // ==================== КНОПКА "Побудувати матриці" ====================
        private void btnBuild_Click(object sender, EventArgs e)
        {
            var vertices = ParseVertices(txtVertices.Text);
            if (vertices.Count == 0)
            {
                MessageBox.Show("Вкажіть вершини!");
                return;
            }

            var edges = ParseEdges(txtEdges.Text);

            foreach (var (u, v) in edges)
            {
                if (!vertices.Contains(u) || !vertices.Contains(v))
                {
                    MessageBox.Show($"Ребро {u}->{v} містить невідому вершину.");
                    return;
                }
            }

            PopulateAdjacency(vertices, edges);
            PopulateIncidence(vertices, edges);
        }

        // ==================== КНОПКА "Очистити" ====================
        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvAdjacency.Columns.Clear();
            dgvAdjacency.Rows.Clear();
            dgvIncidence.Columns.Clear();
            dgvIncidence.Rows.Clear();
            pictureBox.Image = null;
        }


        // ==================== ПАРСИНГ ВЕРШИН ====================
        private List<string> ParseVertices(string text)
        {
            var list = new List<string>();
            var parts = Regex.Split(text.Trim(), @"[,\s;]+");
            foreach (var p in parts)
                if (p.Length > 0 && !list.Contains(p))
                    list.Add(p);
            return list;
        }

        // ==================== ПАРСИНГ РЕБЕР ====================
        private List<(string u, string v)> ParseEdges(string text)
        {
            var edges = new List<(string u, string v)>();
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var ln in lines)
            {
                var line = ln.Trim();
                if (line.Contains("->"))
                {
                    var parts = line.Split("->");
                    edges.Add((parts[0].Trim(), parts[1].Trim()));
                }
                else
                {
                    var parts = line.Split(' ');
                    if (parts.Length == 2)
                        edges.Add((parts[0], parts[1]));
                }
            }
            return edges;
        }

        // ==================== МАТРИЦЯ СУМІЖНОСТІ ====================
        private void PopulateAdjacency(List<string> vertices, List<(string u, string v)> edges)
        {
            dgvAdjacency.Columns.Clear();
            dgvAdjacency.Rows.Clear();

            int n = vertices.Count;

            dgvAdjacency.ColumnCount = n;
            for (int i = 0; i < n; i++)
            {
                dgvAdjacency.Columns[i].Name = vertices[i];
                dgvAdjacency.Columns[i].Width = 40;
            }

            dgvAdjacency.RowCount = n;
            for (int i = 0; i < n; i++)
                dgvAdjacency.Rows[i].HeaderCell.Value = vertices[i];

            // Заповнення нулями
            foreach (DataGridViewRow row in dgvAdjacency.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Value = 0;

            // Заповнення ребрами
            foreach (var (u, v) in edges)
            {
                int i = vertices.IndexOf(u);
                int j = vertices.IndexOf(v);
                dgvAdjacency[j, i].Value = 1;
            }
        }

        // ==================== МАТРИЦЯ ІНЦИДЕНТНОСТІ ====================
        private void PopulateIncidence(List<string> vertices, List<(string u, string v)> edges)
        {
            dgvIncidence.Columns.Clear();
            dgvIncidence.Rows.Clear();

            int n = vertices.Count;
            int m = edges.Count;

            for (int k = 0; k < m; k++)
                dgvIncidence.Columns.Add($"e{k + 1}", $"e{k + 1}");

            dgvIncidence.RowCount = n;
            for (int i = 0; i < n; i++)
                dgvIncidence.Rows[i].HeaderCell.Value = vertices[i];

            // Нулі
            foreach (DataGridViewRow row in dgvIncidence.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Value = 0;

            // Запис -1 (вихід), +1 (вхід)
            for (int k = 0; k < m; k++)
            {
                var (u, v) = edges[k];
                dgvIncidence[k, vertices.IndexOf(u)].Value = -1;
                dgvIncidence[k, vertices.IndexOf(v)].Value = +1;
            }
        }
    }
}
