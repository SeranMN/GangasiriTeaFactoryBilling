using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using GangasiriTeaFactoryBilling.db;

namespace GangasiriTeaFactoryBilling.Audit
{
    public class frmAuditLog : Form
    {
        private DataGridView gridLog;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private TextBox txtSearch;
        private Button btnFilter;
        private Button btnConfig;

        public frmAuditLog()
        {
            InitializeUI();
            LoadLogs();
        }

        private void InitializeUI()
        {
            this.Text = "System Audit Logs";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.White };
            
            Label lblTitle = new Label
            {
                Text = "Audit Logs",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 10),
                AutoSize = true
            };

            // Filters
            Label lblStart = new Label { Text = "From:", Location = new Point(200, 20), AutoSize = true };
            dtpStart = new DateTimePicker { Location = new Point(250, 18), Format = DateTimePickerFormat.Short, Width = 100, Value = DateTime.Today.AddDays(-7) };

            Label lblEnd = new Label { Text = "To:", Location = new Point(370, 20), AutoSize = true };
            dtpEnd = new DateTimePicker { Location = new Point(400, 18), Format = DateTimePickerFormat.Short, Width = 100, Value = DateTime.Today };

            txtSearch = new TextBox { Location = new Point(520, 18), Width = 150, PlaceholderText = "Search Details..." };

            btnFilter = new Button { Text = "Filter", Location = new Point(680, 16), Width = 80, Height = 28 };
            btnFilter.Click += (s, e) => LoadLogs();

            btnConfig = new Button 
            { 
                Text = "⚙ Settings", 
                Location = new Point(780, 16), 
                Width = 90, 
                Height = 28,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnConfig.Click += (s, e) => { new frmAuditConfig().ShowDialog(); };
            
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblStart, dtpStart, lblEnd, dtpEnd, txtSearch, btnFilter, btnConfig });

            gridLog = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false
            };

            this.Controls.Add(gridLog);
            this.Controls.Add(pnlHeader);
        }

        private void LoadLogs()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT LogID, Username, ActionType, TableName, RecordID, Details, Timestamp 
                        FROM AuditLogs 
                        WHERE Timestamp BETWEEN @start AND @end
                        AND (Details LIKE @search OR Username LIKE @search OR TableName LIKE @search)
                        ORDER BY Timestamp DESC";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@start", dtpStart.Value.Date);
                        cmd.Parameters.AddWithValue("@end", dtpEnd.Value.Date.AddDays(1).AddTicks(-1));
                        cmd.Parameters.AddWithValue("@search", $"%{txtSearch.Text}%");

                        DataTable dt = new DataTable();
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                        gridLog.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading logs: {ex.Message}", "Error");
            }
        }
    }
}
