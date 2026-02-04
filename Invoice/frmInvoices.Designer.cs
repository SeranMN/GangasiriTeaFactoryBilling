namespace GangasiriTeaFactoryBilling.Invoice
{
    partial class frmInvoices
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DataGridView invoicesGrid;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnGenerateInvoice;
        private Button btnExport;
        private Button btnRefresh;
        private Button btnFilter;
        private ComboBox cmbFilterMonth;
        private ComboBox cmbFilterSupplier;
        private Label lblTotalAmount;

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
            this.Text = "Invoice Management";
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Padding = new Padding(20);
            this.Size = new Size(1200, 700);

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

            // Filter panel
            Panel filterPanel = CreateFilterPanel();
            filterPanel.Location = new Point(0, headerPanel.Height + 10);
            mainPanel.Controls.Add(filterPanel);

            // Data grid panel
            Panel gridPanel = CreateGridPanel();
            gridPanel.Location = new Point(0, headerPanel.Height + filterPanel.Height + 20);
            gridPanel.Size = new Size(mainPanel.Width, mainPanel.Height - headerPanel.Height - filterPanel.Height - 40);
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
                Text = "Invoice Management",
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
                PlaceholderText = "Search by invoice no, supplier...",
                Font = new Font("Segoe UI", 10),
                Size = new Size(300, 36),
                Location = new Point(0, 0),
                Padding = new Padding(10)
            };

            btnSearch = new Button
            {
                Text = "Search",
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
                Width = 500,
                Location = new Point(450, 50)
            };

            btnGenerateInvoice = new Button
            {
                Text = "Generate Invoice",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGenerateInvoice.FlatAppearance.BorderSize = 0;
            btnGenerateInvoice.Click += BtnGenerateInvoice_Click;

            btnExport = new Button
            {
                Text = "Export",
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
                Text = "Refresh",
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

            // Total amount label
            lblTotalAmount = new Label
            {
                Text = "Total: Rs. 0.00",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 125, 50),
                Location = new Point(800, 55),
                AutoSize = true
            };

            actionPanel.Controls.Add(btnGenerateInvoice);
            actionPanel.Controls.Add(btnExport);
            actionPanel.Controls.Add(btnRefresh);

            header.Controls.Add(titleLabel);
            header.Controls.Add(searchPanel);
            header.Controls.Add(actionPanel);
            header.Controls.Add(lblTotalAmount);

            return header;
        }

        private Panel CreateFilterPanel()
        {
            Panel filterPanel = new Panel
            {
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(248, 248, 248),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            Label lblMonth = new Label
            {
                Text = "Month:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 13),
                AutoSize = true
            };

            cmbFilterMonth = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(150, 28),
                Location = new Point(80, 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilterMonth.Items.Add("All Months");
            for (int i = 1; i <= 12; i++)
            {
                cmbFilterMonth.Items.Add(new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM"));
            }
            cmbFilterMonth.SelectedIndex = 0;
            cmbFilterMonth.SelectedIndexChanged += CmbFilterMonth_SelectedIndexChanged;

            Label lblSupplier = new Label
            {
                Text = "Supplier:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(250, 13),
                AutoSize = true
            };

            cmbFilterSupplier = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(200, 28),
                Location = new Point(320, 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilterSupplier.Items.Add("All Suppliers");
            // Load suppliers from database here
            cmbFilterSupplier.SelectedIndex = 0;
            cmbFilterSupplier.SelectedIndexChanged += CmbFilterSupplier_SelectedIndexChanged;

            btnFilter = new Button
            {
                Text = "Apply Filters",
                Font = new Font("Segoe UI", 10),
                Size = new Size(120, 30),
                Location = new Point(550, 10),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.Click += BtnFilter_Click;

            filterPanel.Controls.Add(lblMonth);
            filterPanel.Controls.Add(cmbFilterMonth);
            filterPanel.Controls.Add(lblSupplier);
            filterPanel.Controls.Add(cmbFilterSupplier);
            filterPanel.Controls.Add(btnFilter);

            return filterPanel;
        }

        private Panel CreateGridPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10,150,10,10)
            };

            // Create DataGridView
            invoicesGrid = new DataGridView
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
                Font = new Font("Segoe UI", 10),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };

            // Style the grid
            invoicesGrid.EnableHeadersVisualStyles = false;
            invoicesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            invoicesGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            invoicesGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            invoicesGrid.ColumnHeadersHeight = 40;
            invoicesGrid.RowTemplate.Height = 35;
            invoicesGrid.RowTemplate.DefaultCellStyle.Padding = new Padding(5);

            // Alternating row colors
            invoicesGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            // Add columns
            invoicesGrid.Columns.Add("InvoiceNo", "Invoice No");
            invoicesGrid.Columns.Add("SupplierName", "Supplier");
            invoicesGrid.Columns.Add("Month", "Month");
            invoicesGrid.Columns.Add("Year", "Year");          
            invoicesGrid.Columns.Add("Amount", "Amount (Rs.)");
            invoicesGrid.Columns.Add("Actions", "Actions");

            // Style specific columns
            invoicesGrid.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            invoicesGrid.Columns["Amount"].DefaultCellStyle.Format = "N2";
            invoicesGrid.Columns["Year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            invoicesGrid.Columns["Actions"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set column widths
            invoicesGrid.Columns["InvoiceNo"].Width = 100;
            invoicesGrid.Columns["Month"].Width = 50;
            invoicesGrid.Columns["Amount"].Width = 50;
            invoicesGrid.Columns["Year"].Width = 50;
            invoicesGrid.Columns["Actions"].Width = 50;

            // Add sample data
            LoadInvoices();

            // Add double-click event to view details
            invoicesGrid.CellDoubleClick += InvoicesGrid_CellDoubleClick;
            invoicesGrid.CellPainting += InvoicesGrid_CellPainting;
            invoicesGrid.CellClick += InvoicesGrid_CellClick;

            panel.Controls.Add(invoicesGrid);

            return panel;
        }

        #endregion
    }
}