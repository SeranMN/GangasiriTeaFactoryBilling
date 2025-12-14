namespace GangasiriTeaFactoryBilling.LineManagements
{
    partial class frmLines
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 
        private DataGridView linesGrid;
        private Button btnAddLine;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private TextBox txtSearch;
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Lines Management";
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Padding = new Padding(20);

            // Main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Header panel
            Panel headerPanel = CreateHeaderPanel();
            mainPanel.Controls.Add(headerPanel);

            // Data grid panel
            Panel gridPanel = CreateGridPanel();
            gridPanel.Dock = DockStyle.Fill;
            gridPanel.Margin = new Padding(0, headerPanel.Height + 10, 0, 0);
            mainPanel.Controls.Add(gridPanel);

            this.Controls.Add(mainPanel);
            this.ResumeLayout(false);
        }

        private Panel CreateHeaderPanel()
        {
            Panel header = new Panel
            {
                Height = 120,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            // Title
            Label titleLabel = new Label
            {
                Text = "🏭 Lines Management",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(0, 10),
                AutoSize = true
            };

            // Search panel
            Panel searchPanel = new Panel
            {
                Height = 40,
                Width = 400,
                Location = new Point(0, 60)
            };

            txtSearch = new TextBox
            {
                PlaceholderText = "Search lines...",
                Font = new Font("Segoe UI", 10),
                Size = new Size(300, 36),
                Location = new Point(0, 0),
                Padding = new Padding(10)
            };

            Button btnSearch = new Button
            {
                Text = "🔍 Search",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 36),
                Location = new Point(305, 0),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            // Action buttons panel
            Panel actionPanel = new Panel
            {
                Height = 40,
                Width = 500,
                Location = new Point(450, 60)
            };

            btnAddLine = new Button
            {
                Text = "➕ Add Line",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(120, 40),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddLine.FlatAppearance.BorderSize = 0;
            btnAddLine.Click += BtnAddLine_Click;

            btnEdit = new Button
            {
                Text = "✏️ Edit",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 40),
                Location = new Point(130, 0),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "🗑️ Delete",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 40),
                Location = new Point(240, 0),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new Button
            {
                Text = "🔄 Refresh",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 40),
                Location = new Point(350, 0),
                BackColor = Color.FromArgb(121, 85, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);

            actionPanel.Controls.Add(btnAddLine);
            actionPanel.Controls.Add(btnEdit);
            actionPanel.Controls.Add(btnDelete);
            actionPanel.Controls.Add(btnRefresh);

            header.Controls.Add(titleLabel);
            header.Controls.Add(searchPanel);
            header.Controls.Add(actionPanel);

            return header;
        }

        private Panel CreateGridPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5,120,5,5)
            };

            // Create DataGridView
            linesGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10)
            };

            // Style the grid
            linesGrid.EnableHeadersVisualStyles = false;
            linesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(63, 81, 181);
            linesGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            linesGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            linesGrid.ColumnHeadersHeight = 40;
            linesGrid.RowTemplate.Height = 35;

            // Add columns
            linesGrid.Columns.Add("LineID", "Line ID");
            linesGrid.Columns.Add("LineName", "Line Name");
            linesGrid.Columns.Add("Description", "Description");
            linesGrid.Columns.Add("TransportFee", "Transport Fee (₹)");
            linesGrid.Columns.Add("Status", "Status");

            // Style columns
            linesGrid.Columns["LineID"].Width = 100;
            linesGrid.Columns["LineName"].Width = 150;
            linesGrid.Columns["TransportFee"].DefaultCellStyle.Format = "N2";
            linesGrid.Columns["TransportFee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            linesGrid.Columns["Status"].Width = 100;
            linesGrid.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Add sample data
            LoadSampleData();

            panel.Controls.Add(linesGrid);

            return panel;
        }

        #endregion
    }
}