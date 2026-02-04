using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using GangasiriTeaFactoryBilling.db;

namespace GangasiriTeaFactoryBilling.DashBoard
{
    public partial class frmDashboard : Form
    {
        // Colors
        private readonly Color PrimaryColor = Color.FromArgb(52, 152, 219); // Blue
        private readonly Color SuccessColor = Color.FromArgb(46, 204, 113); // Green
        private readonly Color WarningColor = Color.FromArgb(243, 156, 18); // Orange
        private readonly Color DangerColor = Color.FromArgb(231, 76, 60);  // Red
        private readonly Color TextColor = Color.FromArgb(44, 62, 80);     // Dark Gray
        private readonly Color BgColor = Color.WhiteSmoke;

        public frmDashboard()
        {
            InitializeComponent();
            try
            {
                using (var bitmap = new System.Drawing.Bitmap(@"D:\GangaSiri Tea Factory Billing\GangasiriTeaFactoryBilling\img\Gemini_Generated_Image_f2q39df2q39df2q3.png"))
                {
                    this.Icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
                }
            }
            catch { /* If icon fails, ignore */ }
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            this.BackColor = BgColor;
            this.Padding = new Padding(20);
            this.AutoScroll = true;

            // Main Layout
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));  // Header
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F)); // Stats Cards
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Recent Activity

            this.Controls.Add(mainLayout);

            // 1. Header Section
            Panel headerPanel = CreateHeader();
            mainLayout.Controls.Add(headerPanel, 0, 0);

            // 2. Stats Section
            FlowLayoutPanel statsPanel = CreateStatsPanel();
            mainLayout.Controls.Add(statsPanel, 0, 1);

            // 3. Recent Activity Section
            Panel activityPanel = CreateActivityPanel();
            mainLayout.Controls.Add(activityPanel, 0, 2);
        }

        private Panel CreateHeader()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 20)
            };

            PictureBox picLogo = new PictureBox
            {
                Size = new Size(50, 50),
                Location = new Point(10, 15),
                SizeMode = PictureBoxSizeMode.Zoom,
                ImageLocation = @"D:\GangaSiri Tea Factory Billing\GangasiriTeaFactoryBilling\img\Gemini_Generated_Image_f2q39df2q39df2q3.png"
            };

            Label title = new Label
            {
                Text = "Dashboard Overview",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(70, 10)
            };

            Label date = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy"),
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(75, 55)
            };
            
            Button btnRefresh = new Button
            {
                Text = "Refresh Data",
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                Size = new Size(120, 35),
                Location = new Point(0, 10), // Will anchor right manually or using Anchor styles
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            // Hack to right align without complex layout in this simple panel
            btnRefresh.Location = new Point(1000, 10); // Approximation, relies on dock/anchor if parent resizes? 
            // Better: use a sub-panel or just Dock Right.
            // Let's us Dock Right for the button wrapper.
            
            Panel btnPanel = new Panel { Dock = DockStyle.Right, Width = 150, Height = 80 };
            btnRefresh.Location = new Point(10, 20);
            btnPanel.Controls.Add(btnRefresh);
            
            btnRefresh.Click += (s, e) => {
                this.Controls.Clear();
                InitializeDashboard();
            };

            panel.Controls.Add(picLogo);
            panel.Controls.Add(title);
            panel.Controls.Add(date);
            panel.Controls.Add(btnPanel);

            return panel;
        }

        private FlowLayoutPanel CreateStatsPanel()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false, // Horizontal scroll if needed
                AutoScroll = true
            };

            // Fetch Data
            double todayCollection = GetScalarDouble("SELECT SUM(Weight) FROM DailyCollection WHERE date(CollectionDate) = date('now')");
            long activeSuppliers = GetScalarLong("SELECT COUNT(*) FROM Suppliers WHERE Status = 'Active'");
            double pendingAdvances = GetScalarDouble("SELECT SUM(Amount) FROM Advances WHERE Status = 'Active'");
            // Mock Rate for now or fetch
            double currentRate = GetScalarDouble("SELECT Rate FROM TeaRates WHERE Status = 'Active' ORDER BY CreatedDate DESC LIMIT 1");

            panel.Controls.Add(CreateCard("Daily Collection", $"{todayCollection:N2} kg", SuccessColor));
            panel.Controls.Add(CreateCard("Active Suppliers", $"{activeSuppliers}", PrimaryColor));
            panel.Controls.Add(CreateCard("Pending Advances", $"Rs. {pendingAdvances:N2}", WarningColor));
            panel.Controls.Add(CreateCard("Current Rate", $"Rs. {currentRate:N2}", DangerColor));

            return panel;
        }

        private Panel CreateCard(string title, string value, Color accentColor)
        {
            Panel card = new Panel
            {
                Size = new Size(250, 140),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 20, 0),
                Padding = new Padding(15)
            };

            // Accent strip
            Panel strip = new Panel
            {
                Width = 5,
                Dock = DockStyle.Left,
                BackColor = accentColor
            };

            Label lblTitle = new Label
            {
                Text = title.ToUpper(),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Gray,
                Location = new Point(20, 20),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(20, 50),
                AutoSize = true
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            card.Controls.Add(strip);

            return card;
        }

        private Panel CreateActivityPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label title = new Label
            {
                Text = "Recent Collections",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 40
            };

            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            grid.DefaultCellStyle.SelectionBackColor = Color.WhiteSmoke;
            grid.DefaultCellStyle.SelectionForeColor = TextColor;
            grid.DefaultCellStyle.Padding = new Padding(5);
            grid.RowTemplate.Height = 40;
            grid.EnableHeadersVisualStyles = false;

            // Load Data
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        SELECT d.CollectionDate, s.SupplierName, d.Weight, d.TotalAmount 
                        FROM DailyCollection d
                        JOIN Suppliers s ON d.SupplierID = s.SupplierID
                        ORDER BY d.CreatedDate DESC LIMIT 10";
                    
                    using (var cmd = new SqliteCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var dt = new System.Data.DataTable();
                            dt.Load(reader);
                            grid.DataSource = dt;
                        }
                    }
                }
            }
            catch { /* Ignore for UI demo if table empty */ }

            panel.Controls.Add(grid);
            panel.Controls.Add(title);

            return panel;
        }

        private double GetScalarDouble(string sql)
        {
            try
            {
                var result = DatabaseHelper.ExecuteScalar(sql);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDouble(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching data: " + ex.Message);
            }
            return 0.0;
        }

        private long GetScalarLong(string sql)
        {
            try
            {
                var result = DatabaseHelper.ExecuteScalar(sql);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt64(result);
                }
            }
            catch { }
            return 0;
        }
    }
}
