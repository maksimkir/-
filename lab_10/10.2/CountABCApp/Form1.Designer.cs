namespace GraphMatricesApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private PictureBox pictureBox;
        private Button btnLoadImage;
        private Label lblVertices;
        private Label lblEdges;
        private TextBox txtVertices;
        private TextBox txtEdges;
        private Button btnBuild;
        private Button btnClear;
        private DataGridView dgvAdjacency;
        private DataGridView dgvIncidence;
        private Label lblAdj;
        private Label lblInc;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            btnLoadImage = new Button();
            lblVertices = new Label();
            lblEdges = new Label();
            txtVertices = new TextBox();
            txtEdges = new TextBox();
            btnBuild = new Button();
            btnClear = new Button();
            dgvAdjacency = new DataGridView();
            dgvIncidence = new DataGridView();
            lblAdj = new Label();
            lblInc = new Label();

            SuspendLayout();

            // picture
            pictureBox.Location = new Point(10, 10);
            pictureBox.Size = new Size(300, 300);
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // btnLoadImage
            btnLoadImage.Location = new Point(10, 320);
            btnLoadImage.Size = new Size(300, 30);
            btnLoadImage.Text = "Завантажити зображення";
            btnLoadImage.Click += btnLoadImage_Click;

            // lblVertices
            lblVertices.Location = new Point(330, 10);
            lblVertices.Text = "Вершини:";

            // txtVertices
            txtVertices.Location = new Point(330, 35);
            txtVertices.Size = new Size(300, 25);
            txtVertices.Text = "a,b,c,d,e,f";

            // lblEdges
            lblEdges.Location = new Point(330, 70);
            lblEdges.Text = "Ребра (u->v):";

            // txtEdges
            txtEdges.Location = new Point(330, 95);
            txtEdges.Size = new Size(300, 215);
            txtEdges.Multiline = true;
            txtEdges.ScrollBars = ScrollBars.Vertical;

            // btnBuild
            btnBuild.Location = new Point(330, 320);
            btnBuild.Size = new Size(140, 30);
            btnBuild.Text = "Побудувати матриці";
            btnBuild.Click += btnBuild_Click;

            // btnClear
            btnClear.Location = new Point(490, 320);
            btnClear.Size = new Size(140, 30);
            btnClear.Text = "Очистити";
            btnClear.Click += btnClear_Click;

            // lblAdj
            lblAdj.Location = new Point(10, 360);
            lblAdj.Text = "Матриця суміжності:";

            // dgvAdjacency
            dgvAdjacency.Location = new Point(10, 385);
            dgvAdjacency.Size = new Size(480, 240);
            dgvAdjacency.RowHeadersVisible = true;
            dgvAdjacency.AllowUserToAddRows = false;
            dgvAdjacency.ReadOnly = true;

            // lblInc
            lblInc.Location = new Point(500, 360);
            lblInc.Text = "Матриця інцидентності:";

            // dgvIncidence
            dgvIncidence.Location = new Point(500, 385);
            dgvIncidence.Size = new Size(480, 240);
            dgvIncidence.RowHeadersVisible = true;
            dgvIncidence.AllowUserToAddRows = false;
            dgvIncidence.ReadOnly = true;

            // Form
            ClientSize = new Size(1000, 650);
            Controls.Add(pictureBox);
            Controls.Add(btnLoadImage);
            Controls.Add(lblVertices);
            Controls.Add(txtVertices);
            Controls.Add(lblEdges);
            Controls.Add(txtEdges);
            Controls.Add(btnBuild);
            Controls.Add(btnClear);
            Controls.Add(lblAdj);
            Controls.Add(dgvAdjacency);
            Controls.Add(lblInc);
            Controls.Add(dgvIncidence);
            Text = "Graph Matrices (Adjacency & Incidence)";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
