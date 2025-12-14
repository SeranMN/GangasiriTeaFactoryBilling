namespace GangasiriTeaFactoryBilling.TeaCollection
{
    partial class frmTeaCollection
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
        private DataGridView collectionGrid;
        private Button btnAddCollection;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private TextBox txtSearch;
        private DateTimePicker dtpFilterDate;
        private ComboBox cmbFilterSupplier;
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Daily Tea Collection";
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
                Height = 150,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            // Title
            Label titleLabel = new Label
            {
                Text = "🍃 Daily Tea Collection",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(0, 10),
                AutoSize = true
            };

            // Filter panel
            Panel filterPanel = new Panel
            {
                Height = 40,
                Width = 600,
                Location = new Point(0, 50)
            };

            Label lblDate = new Label
            {
                Text = "Date:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 10),
                AutoSize = true
            };

            dtpFilterDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(40, 5),
                Size = new Size(120, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };
            dtpFilterDate.ValueChanged += DtpFilterDate_ValueChanged;

            Label lblSupplier = new Label
            {
                Text = "Supplier:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 10),
                AutoSize = true
            };

            cmbFilterSupplier = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(250, 5),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilterSupplier.SelectedIndexChanged += CmbFilterSupplier_SelectedIndexChanged;

            Button btnClearFilters = new Button
            {
                Text = "Clear Filters",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 30),
                Location = new Point(460, 5),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.Click += BtnClearFilters_Click;

            // Search panel
            Panel searchPanel = new Panel
            {
                Height = 40,
                Width = 400,
                Location = new Point(0, 100)
            };

            txtSearch = new TextBox
            {
                PlaceholderText = "Search collections...",
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
                Location = new Point(450, 100)
            };

            btnAddCollection = new Button
            {
                Text = "➕ Add Collection",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(150, 40),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddCollection.FlatAppearance.BorderSize = 0;
            btnAddCollection.Click += BtnAddCollection_Click;

            btnEdit = new Button
            {
                Text = "✏️ Edit",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 40),
                Location = new Point(160, 0),
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
                Location = new Point(270, 0),
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
                Location = new Point(380, 0),
                BackColor = Color.FromArgb(121, 85, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            filterPanel.Controls.Add(lblDate);
            filterPanel.Controls.Add(dtpFilterDate);
            filterPanel.Controls.Add(lblSupplier);
            filterPanel.Controls.Add(cmbFilterSupplier);
            filterPanel.Controls.Add(btnClearFilters);

            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);

            actionPanel.Controls.Add(btnAddCollection);
            actionPanel.Controls.Add(btnEdit);
            actionPanel.Controls.Add(btnDelete);
            actionPanel.Controls.Add(btnRefresh);

            header.Controls.Add(titleLabel);
            header.Controls.Add(filterPanel);
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
                Padding = new Padding(5,150,5,5)
            };

            // Create DataGridView
            collectionGrid = new DataGridView
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
            collectionGrid.EnableHeadersVisualStyles = false;
            collectionGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 150, 136);
            collectionGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            collectionGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            collectionGrid.ColumnHeadersHeight = 40;
            collectionGrid.RowTemplate.Height = 35;

            // Add columns
            collectionGrid.Columns.Add("CollectionID", "Collection ID");
            collectionGrid.Columns.Add("Supplier", "Supplier");
            collectionGrid.Columns.Add("Date", "Date");
            collectionGrid.Columns.Add("Weight", "Weight (kg)");
            collectionGrid.Columns.Add("Rate", "Rate (₹)");
            collectionGrid.Columns.Add("Transport", "Transport (₹)");
            collectionGrid.Columns.Add("Total", "Total (₹)");
            collectionGrid.Columns.Add("Notes", "Notes");

            // Style columns
            collectionGrid.Columns["CollectionID"].Width = 120;
            collectionGrid.Columns["Date"].Width = 100;
            collectionGrid.Columns["Weight"].DefaultCellStyle.Format = "N2";
            collectionGrid.Columns["Weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            collectionGrid.Columns["Rate"].DefaultCellStyle.Format = "N2";
            collectionGrid.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            collectionGrid.Columns["Transport"].DefaultCellStyle.Format = "N2";
            collectionGrid.Columns["Transport"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            collectionGrid.Columns["Total"].DefaultCellStyle.Format = "N2";
            collectionGrid.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            collectionGrid.Columns["Total"].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            collectionGrid.Columns["Total"].DefaultCellStyle.ForeColor = Color.FromArgb(0, 150, 136);

            // Add sample data
            LoadSampleData();

            panel.Controls.Add(collectionGrid);

            return panel;
        }

        #endregion
    }
}