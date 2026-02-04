namespace GangasiriTeaFactoryBilling.TeaRates
{
    partial class frmTeaRates
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
        private DataGridView ratesGrid;
        private Button btnAddRate;

        private Button btnDelete;
        private Button btnRefresh;
        private TextBox txtSearch;
        private ComboBox cmbFilterStatus;
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Tea Rates Management";
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
                Text = "📊 Tea Rates Management",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(0, 10),
                AutoSize = true
            };

            // Filter panel
            Panel filterPanel = new Panel
            {
                Height = 40,
                Width = 400,
                Location = new Point(0, 50)
            };

            Label lblStatus = new Label
            {
                Text = "Status:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 10),
                AutoSize = true
            };

            cmbFilterStatus = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(50, 5),
                Size = new Size(150, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilterStatus.Items.Add("All Rates");
            cmbFilterStatus.Items.Add("Active Only");
            cmbFilterStatus.Items.Add("Expired Only");
            cmbFilterStatus.SelectedIndex = 0;
            cmbFilterStatus.SelectedIndexChanged += CmbFilterStatus_SelectedIndexChanged;

            Button btnClearFilters = new Button
            {
                Text = "Clear Filters",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 30),
                Location = new Point(210, 5),
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
                PlaceholderText = "Search rates...",
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

            btnAddRate = new Button
            {
                Text = "➕ Add Rate",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(120, 40),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddRate.FlatAppearance.BorderSize = 0;
            btnAddRate.Click += BtnAddRate_Click;



            btnDelete = new Button
            {
                Text = "🗑️ Delete",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 40),
                Location = new Point(130, 0),
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
                Location = new Point(240, 0),
                BackColor = Color.FromArgb(121, 85, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            filterPanel.Controls.Add(lblStatus);
            filterPanel.Controls.Add(cmbFilterStatus);
            filterPanel.Controls.Add(btnClearFilters);

            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);

            actionPanel.Controls.Add(btnAddRate);

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
            ratesGrid = new DataGridView
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
            ratesGrid.EnableHeadersVisualStyles = false;
            ratesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(156, 39, 176);
            ratesGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            ratesGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            ratesGrid.ColumnHeadersHeight = 40;
            ratesGrid.RowTemplate.Height = 35;

            // Add columns
            ratesGrid.Columns.Add("RateID", "Rate ID");
            ratesGrid.Columns.Add("Rate", "Rate (LKR/kg)");
            ratesGrid.Columns.Add("ValidFrom", "Valid From");
            ratesGrid.Columns.Add("ValidTo", "Valid To");
            ratesGrid.Columns.Add("Status", "Status");
            ratesGrid.Columns.Add("CreatedDate", "Created Date");

            // Style columns
            ratesGrid.Columns["RateID"].Width = 50;
            ratesGrid.Columns["Rate"].DefaultCellStyle.Format = "N2";
            ratesGrid.Columns["Rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ratesGrid.Columns["ValidFrom"].Width = 50;
            ratesGrid.Columns["ValidTo"].Width = 50;
            ratesGrid.Columns["Status"].Width = 50;
            ratesGrid.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ratesGrid.Columns["CreatedDate"].Width = 50;

            ratesGrid.CellDoubleClick += BtnEdit_Click;

            panel.Controls.Add(ratesGrid);

            return panel;
        }

        #endregion
    }
}