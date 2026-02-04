using Microsoft.VisualBasic;

namespace GangasiriTeaFactoryBilling.Advanced
{
    partial class frmAdvanceManagement
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
        private DataGridView advancesGrid;
        private Button btnAddAdvance;
        
        private Button btnDelete;
        private Button btnRefresh;
        private TextBox txtSearch;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Advance Management";
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
                Text = "💵 Advance Management",
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
                PlaceholderText = "Search advances...",
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

            btnAddAdvance = new Button
            {
                Text = "➕ Add Advance",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(150, 40),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddAdvance.FlatAppearance.BorderSize = 0;
            btnAddAdvance.Click += BtnAddAdvance_Click;

            btnAddAdvance.FlatAppearance.BorderSize = 0;
            btnAddAdvance.Click += BtnAddAdvance_Click;

            btnDelete = new Button
            {
                Text = "🗑️ Delete",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 40),
                Location = new Point(160, 0),
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
                Location = new Point(270, 0),
                BackColor = Color.FromArgb(121, 85, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);

            actionPanel.Controls.Add(btnAddAdvance);
            
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
            advancesGrid = new DataGridView
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
            advancesGrid.EnableHeadersVisualStyles = false;
            advancesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
            advancesGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            advancesGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            advancesGrid.ColumnHeadersHeight = 40;
            advancesGrid.RowTemplate.Height = 35;

            // Add columns
            advancesGrid.Columns.Add("AdvanceID", "Advance ID");
            advancesGrid.Columns.Add("Supplier", "Supplier");
            advancesGrid.Columns.Add("Date", "Date");
            advancesGrid.Columns.Add("Amount", "Amount (LKR)");
            advancesGrid.Columns.Add("Description", "Description");
            advancesGrid.Columns.Add("Status", "Status");

            // Ensure important columns stay readable
            advancesGrid.Columns["AdvanceID"].MinimumWidth = 110;
            advancesGrid.Columns["Supplier"].MinimumWidth = 180;
            advancesGrid.Columns["Date"].MinimumWidth = 100;
            advancesGrid.Columns["Amount"].MinimumWidth = 120;
            advancesGrid.Columns["Status"].MinimumWidth = 90;

            advancesGrid.Columns["Amount"].DefaultCellStyle.Format = "N2";
            advancesGrid.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            advancesGrid.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            advancesGrid.CellDoubleClick += BtnEdit_Click;

            // Context Menu
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");
            deleteItem.Click += BtnDelete_Click;
            contextMenu.Items.Add(deleteItem);
            advancesGrid.ContextMenuStrip = contextMenu;

            panel.Controls.Add(advancesGrid);

            return panel;
        }
    }

        #endregion
    }