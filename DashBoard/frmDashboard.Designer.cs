using GangasiriTeaFactoryBilling.Advanced;
using GangasiriTeaFactoryBilling.Suppliars;
using GangasiriTeaFactoryBilling.TeaCollection;

namespace GangasiriTeaFactoryBilling.DashBoard
{
    partial class frmDashboard
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

        private Panel mainContainer;
        private FlowLayoutPanel mainPanel;
        private Panel headerPanel;
        private FlowLayoutPanel cardsPanel;
        private Panel activityPanel;
        private Panel quickActionsPanel;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainContainer = new Panel();
            mainPanel = new FlowLayoutPanel();
            mainContainer.SuspendLayout();
            SuspendLayout();
            // 
            // mainContainer
            // 
            mainContainer.Controls.Add(mainPanel);
            mainContainer.Location = new Point(0, 0);
            mainContainer.Name = "mainContainer";
            mainContainer.Size = new Size(200, 100);
            mainContainer.TabIndex = 0;
            // 
            // mainPanel
            // 
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(200, 100);
            mainPanel.TabIndex = 0;
            // 
            // frmDashboard
            // 
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(2010, 603);
            Controls.Add(mainContainer);
            Name = "frmDashboard";
            Text = "Dashboard";
            Load += FrmDashboard_Load;
            Resize += FrmDashboard_Resize;
            mainContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            CreateDashboardUI();
        }

        private void CreateDashboardUI()
        {
            // Clear existing controls
            mainPanel.Controls.Clear();

            // Calculate available width
            int availableWidth = mainContainer.ClientSize.Width - 40;

            // Welcome Header
            headerPanel = CreateHeaderPanel(availableWidth);
            mainPanel.Controls.Add(headerPanel);

            // Summary Cards
            cardsPanel = CreateSummaryCards(availableWidth);
            mainPanel.Controls.Add(cardsPanel);

            // Recent Activity
            activityPanel = CreateActivityPanel(availableWidth);
            mainPanel.Controls.Add(activityPanel);

            // Quick Actions
            quickActionsPanel = CreateQuickActionsPanel(availableWidth);
            mainPanel.Controls.Add(quickActionsPanel);
        }

        private Panel CreateHeaderPanel(int width)
        {
            Panel panel = new Panel
            {
                Height = 100,
                Width = width,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 20)
            };

            Label welcomeLabel = new Label
            {
                Text = "Welcome to Tea Factory Management",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(0, 0),
                AutoSize = true
            };

            Label dateLabel = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy"),
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                Location = new Point(0, 50),
                AutoSize = true
            };

            panel.Controls.Add(dateLabel);
            panel.Controls.Add(welcomeLabel);

            return panel;
        }

        private FlowLayoutPanel CreateSummaryCards(int width)
        {
            int cardWidth = (width - 80) / 4; // 4 cards with 20px margins on each side

            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Height = 150,
                Width = width,
                Margin = new Padding(0, 0, 0, 30),
                AutoSize = false,
                BackColor = Color.Transparent
            };

            // Create summary cards
            panel.Controls.Add(CreateSummaryCard(
                "Today's Collection",
                "1,250 kg",
                Color.FromArgb(0, 150, 136),
                "🍃",
                cardWidth
            ));

            panel.Controls.Add(CreateSummaryCard(
                "Monthly Total",
                "25,400 kg",
                Color.FromArgb(63, 81, 181),
                "📦",
                cardWidth
            ));

            panel.Controls.Add(CreateSummaryCard(
                "Active Suppliers",
                "42",
                Color.FromArgb(156, 39, 176),
                "👥",
                cardWidth
            ));

            panel.Controls.Add(CreateSummaryCard(
                "Pending Payments",
                "₹ 1,25,000",
                Color.FromArgb(255, 87, 34),
                "💰",
                cardWidth
            ));

            return panel;
        }

        private Panel CreateSummaryCard(string title, string value, Color color, string icon, int width)
        {
            Panel card = new Panel
            {
                Size = new Size(width, 130),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 20, 20),
                Padding = new Padding(15),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            // Add shadow effect
            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid);
            };

            // Icon Label
            Label iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 24),
                Location = new Point(15, 15),
                Size = new Size(50, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Title Label
            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(75, 20),
                AutoSize = false,
                Size = new Size(width - 90, 20)
            };

            // Value Label
            Label valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(75, 45),
                AutoSize = false,
                Size = new Size(width - 90, 40)
            };

            // Bottom line
            Panel bottomLine = new Panel
            {
                BackColor = color,
                Height = 4,
                Dock = DockStyle.Bottom
            };

            // Click event - Add actual functionality
            card.Click += (s, e) =>
            {
                if (title == "Today's Collection")
                {
                    // Open collection form
                    using (AddCollection form = new AddCollection())
                    {
                        form.ShowDialog();
                    }
                }
                else if (title == "Active Suppliers")
                {
                    // Navigate to suppliers
                    var mainForm = this.ParentForm as frmMain;
                    if (mainForm != null)
                    {
                        // You'll need to add a method in frmMain to navigate programmatically
                        MessageBox.Show("Navigating to Suppliers...", "Navigation",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"View details for {title}", "Quick View",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            card.Controls.Add(iconLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(bottomLine);

            return card;
        }

        private Panel CreateActivityPanel(int width)
        {
            Panel panel = new Panel
            {
                Height = 300,
                Width = width,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(0, 0, 0, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Title
            Label titleLabel = new Label
            {
                Text = "📅 Recent Activity",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(5)
            };

            // Activity List
            ListBox activityList = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                ItemHeight = 30,
                Margin = new Padding(5)
            };

            // Add sample activities
            string[] activities = {
                "✅ Supplier #045 - Collected 125kg (Today 10:30 AM)",
                "💵 Supplier #012 - Advanced ₹5,000 (Yesterday)",
                "📊 New Rate Set: ₹85/kg for November (2 days ago)",
                "🧾 Invoice Generated for Supplier #008 (3 days ago)",
                "👥 New Supplier Added: S-046 - Nimal Silva (4 days ago)",
                "📈 Monthly Report Generated for October (5 days ago)"
            };

            foreach (var activity in activities)
            {
                activityList.Items.Add(activity);
            }

            panel.Controls.Add(activityList);
            panel.Controls.Add(titleLabel);

            return panel;
        }

        private Panel CreateQuickActionsPanel(int width)
        {
            Panel panel = new Panel
            {
                Height = 150,
                Width = width,
                BackColor = Color.Transparent,
                Padding = new Padding(5)
            };

            Label titleLabel = new Label
            {
                Text = "Quick Actions",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                ForeColor = Color.FromArgb(64, 64, 64),
                Padding = new Padding(5)
            };

            FlowLayoutPanel buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(5)
            };

            // Calculate button width based on available space
            int buttonWidth = Math.Max((width - 100) / 4, 200); // Minimum 200px width

            // Quick Action Buttons with actual functionality
            Button btnAddCollection = CreateQuickButton("➕ Add Collection", Color.FromArgb(0, 150, 136), buttonWidth);
            btnAddCollection.Click += (s, e) =>
            {
                using (AddCollection form = new AddCollection())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh dashboard after adding collection
                        CreateDashboardUI();
                    }
                }
            };

            Button btnNewSupplier = CreateQuickButton("👥 New Supplier", Color.FromArgb(63, 81, 181), buttonWidth);
            btnNewSupplier.Click += (s, e) =>
            {
                using (frmAddSupplier form = new frmAddSupplier())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh dashboard after adding supplier
                        CreateDashboardUI();
                    }
                }
            };

            Button btnRecordAdvance = CreateQuickButton("💵 Record Advance", Color.FromArgb(255, 152, 0), buttonWidth);
            btnRecordAdvance.Click += (s, e) =>
            {
                using (frmAddAdvanced form = new frmAddAdvanced())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh dashboard after adding advance
                        CreateDashboardUI();
                    }
                }
            };

            Button btnGenerateInvoice = CreateQuickButton("🧾 Generate Invoice", Color.FromArgb(233, 30, 99), buttonWidth);
            btnGenerateInvoice.Click += (s, e) =>
            {
                MessageBox.Show("Invoice generation will be implemented soon!", "Coming Soon",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            buttonsPanel.Controls.Add(btnAddCollection);
            buttonsPanel.Controls.Add(btnNewSupplier);
            buttonsPanel.Controls.Add(btnRecordAdvance);
            buttonsPanel.Controls.Add(btnGenerateInvoice);

            panel.Controls.Add(buttonsPanel);
            panel.Controls.Add(titleLabel);

            return panel;
        }

        private Button CreateQuickButton(string text, Color color, int width)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, 50),
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(10, 5, 10, 5),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Max(color.R - 20, 0),
                Math.Max(color.G - 20, 0),
                Math.Max(color.B - 20, 0)
            );

            return btn;
        }

        // Handle form resize
        private void FrmDashboard_Resize(object sender, EventArgs e)
        {
            RefreshLayout();
        }

        private void RefreshLayout()
        {
            if (mainContainer != null)
            {
                // Recalculate available width
                int availableWidth = mainContainer.ClientSize.Width - 40;

                // Update main panel width
                mainPanel.Width = availableWidth;

                // Recreate UI with new width
                CreateDashboardUI();
            }
        }
    }
    #endregion
}