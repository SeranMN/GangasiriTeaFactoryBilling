namespace GangasiriTeaFactoryBilling
{
    partial class frmSuppliars
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
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Suppliers Management";
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Padding = new Padding(20);

            // Main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            // Header panel
            Panel headerPanel = CreateHeaderPanel();
            mainPanel.Controls.Add(headerPanel);

            // Data grid panel
            Panel gridPanel = CreateGridPanel();
            gridPanel.Location = new Point(0, headerPanel.Height + 10);
            gridPanel.Size = new Size(mainPanel.Width, mainPanel.Height - headerPanel.Height - 20);
            mainPanel.Controls.Add(gridPanel);

            this.Controls.Add(mainPanel);
            this.ResumeLayout(false);
        }

        private Panel CreateHeaderPanel()
        {
            Panel header = new Panel
            {
                Height = 100,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };

            // Title
            Label titleLabel = new Label
            {
                Text = "Suppliers Management",
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
                Location = new Point(0, 50)
            };

            txtSearch = new TextBox
            {
                PlaceholderText = "Search suppliers by name or ID...",
                Font = new Font("Segoe UI", 10),
                Size = new Size(300, 36),
                Location = new Point(0, 0),
                Padding = new Padding(10)
            };

            btnSearch = new Button
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

            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);

            // Action buttons panel
            Panel actionPanel = new Panel
            {
                Height = 40,
                Width = 400,
                Location = new Point(450, 50)
            };

            btnAddSupplier = new Button
            {
                Text = "➕ Add New Supplier",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddSupplier.FlatAppearance.BorderSize = 0;
            btnAddSupplier.Click += BtnAddSupplier_Click;

            btnExport = new Button
            {
                Text = "📁 Export",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 40),
                Location = new Point(190, 0),
                BackColor = Color.FromArgb(96, 125, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            btnRefresh = new Button
            {
                Text = "🔄 Refresh",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 40),
                Location = new Point(300, 0),
                BackColor = Color.FromArgb(121, 85, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            actionPanel.Controls.Add(btnAddSupplier);
            actionPanel.Controls.Add(btnExport);
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
                Padding = new Padding(10,100,10,100)
            };

            // Create DataGridView
            suppliersGrid = new DataGridView
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
            suppliersGrid.EnableHeadersVisualStyles = false;
            suppliersGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
            suppliersGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            suppliersGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            suppliersGrid.ColumnHeadersHeight = 40;
            suppliersGrid.RowTemplate.Height = 35;

            // Add columns
            suppliersGrid.Columns.Add("SupplierID", "ID");
            suppliersGrid.Columns.Add("SupplierName", "Name");
            suppliersGrid.Columns.Add("Telephone", "Telephone");
            suppliersGrid.Columns.Add("Line", "Line");
            suppliersGrid.Columns.Add("Status", "Status");
            suppliersGrid.Columns.Add("Actions", "Actions");

            // Style specific columns
            suppliersGrid.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Add sample data (replace with database data)
            AddSampleData();

            // Add action buttons to rows
            suppliersGrid.CellPainting += SuppliersGrid_CellPainting;
            suppliersGrid.CellClick += SuppliersGrid_CellClick;

            panel.Controls.Add(suppliersGrid);

            return panel;
        }
    }

        #endregion
    
}