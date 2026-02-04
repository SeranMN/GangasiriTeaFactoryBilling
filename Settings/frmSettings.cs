using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using GangasiriTeaFactoryBilling.db;
using System.IO;
using GangasiriTeaFactoryBilling.Audit;

namespace GangasiriTeaFactoryBilling.Settings
{
    public partial class frmSettings : Form
    {
        private Color PrimaryColor = Color.FromArgb(52, 152, 219);
        private CheckBox chkAutoBackup;
        private ComboBox cmbFrequency;
        private TextBox txtBackupPath;
        private Label lblLastBackup;
        private Button btnManualBackup;
        private Button btnRestore;
        private Button btnBrowse;

        public frmSettings()
        {
            InitializeComponent();
            InitializeSettingsUI();
            LoadSettings();
        }

        private void InitializeSettingsUI()
        {
            this.Padding = new Padding(20);
            
            // Container Panel for Header (to ensure safe docking)
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70
            };

            // Access Control: Add Audit Log Button to Header (Right Aligned) - Must be added BEFORE Fill content regarding Dock order
            if (Session.IsAdmin())
            {
                Button btnAudit = new Button
                {
                    Text = "View Audit Logs",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    BackColor = Color.FromArgb(142, 68, 173),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(150, 35),
                    Cursor = Cursors.Hand,
                    Dock = DockStyle.Right, // Dock to right
                    Margin = new Padding(10)
                };
                // Add padding/margin workaround for Docking if needed, or use a panel. 
                // For simple button, Dock.Right works but fills vertical height. 
                // To keep it looking like a button, put it in a container or use Anchor.
                // Let's use a container panel for the button to control its size/margin nicely.
                Panel pnlBtnContainer = new Panel
                {
                    Dock = DockStyle.Right,
                    Width = 170,
                    Padding = new Padding(10, 15, 10, 15) // Center vertically roughly
                };
                btnAudit.Dock = DockStyle.Fill;
                btnAudit.Click += (s, e) => { new frmAuditLog().Show(); };
                
                pnlBtnContainer.Controls.Add(btnAudit);
                pnlHeader.Controls.Add(pnlBtnContainer);
            }

            Label title = new Label
            {
                Text = "Settings",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(title);

            // Container Panel for TabControl (to fill remaining space)
            Panel pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0) // Small gap below header
            };

            // TabControl
            TabControl tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                ItemSize = new Size(120, 30),
                SizeMode = TabSizeMode.Fixed
            };
            
            // Database Tab
            TabPage dbTab = new TabPage("Database");
            dbTab.Padding = new Padding(20);
            dbTab.BackColor = Color.White;
            dbTab.AutoScroll = true; // Ensure scrolling if window small

            // Main Layout for Tab (FlowLayoutPanel to stack groups)
            FlowLayoutPanel mainFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Width = 800
            };

            // 1. Auto Backup Group
            GroupBox grpAuto = new GroupBox
            {
                Text = "Automatic Backup Configuration",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(800, 280),
                Margin = new Padding(10, 0, 0, 20)
            };

            // Layout inside GroupBox
            Panel pnlAutoContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            chkAutoBackup = new CheckBox
            {
                Text = "Enable Automatic Backups",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 20),
                AutoSize = true
            };
            chkAutoBackup.CheckedChanged += (s, e) => cmbFrequency.Enabled = chkAutoBackup.Checked;

            Label lblFreq = new Label
            {
                Text = "Backup Frequency:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(20, 60),
                AutoSize = true
            };

            cmbFrequency = new ComboBox
            {
                Location = new Point(200, 55),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbFrequency.Items.AddRange(new object[] { "Daily", "Weekly", "Monthly" });
            cmbFrequency.SelectedIndex = 0;

            // Backup Location
            Label lblPath = new Label
            {
                Text = "Backup Location:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(20, 100),
                AutoSize = true
            };

            txtBackupPath = new TextBox
            {
                Location = new Point(200, 95),
                Width = 400,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10)
            };

            btnBrowse = new Button
            {
                Text = "Browse...",
                Location = new Point(610, 94),
                Size = new Size(100, 29),
                Font = new Font("Segoe UI", 9)
            };
            btnBrowse.Click += BtnBrowse_Click;

            Label lblLastBackupInfo = new Label
            {
                Text = "Last Successful Backup:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(20, 150),
                AutoSize = true
            };

            lblLastBackup = new Label
            {
                Text = "Never",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(200, 150),
                AutoSize = true
            };

            Button btnSave = new Button
            {
                Text = "Save Settings",
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(200, 190),
                Size = new Size(150, 40),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            pnlAutoContent.Controls.Add(chkAutoBackup);
            pnlAutoContent.Controls.Add(lblFreq);
            pnlAutoContent.Controls.Add(cmbFrequency);
            pnlAutoContent.Controls.Add(lblPath);
            pnlAutoContent.Controls.Add(txtBackupPath);
            pnlAutoContent.Controls.Add(btnBrowse);
            pnlAutoContent.Controls.Add(lblLastBackupInfo);
            pnlAutoContent.Controls.Add(lblLastBackup);
            pnlAutoContent.Controls.Add(btnSave);
            grpAuto.Controls.Add(pnlAutoContent);


            // 2. Manual Actions Group
            GroupBox grpManual = new GroupBox
            {
                Text = "Manual Operations",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(800, 180),
                Margin = new Padding(0, 0, 0, 20)
            };
            
            Panel pnlManualContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

            btnManualBackup = new Button
            {
                Text = "Backup Now",
                BackColor = Color.FromArgb(46, 204, 113), // Green
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 30),
                Size = new Size(200, 40),
                Cursor = Cursors.Hand
            };
            btnManualBackup.FlatAppearance.BorderSize = 0;
            btnManualBackup.Click += BtnManualBackup_Click;

            btnRestore = new Button
            {
                Text = "Restore Database...",
                BackColor = Color.FromArgb(231, 76, 60), // Red
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(250, 30),
                Size = new Size(200, 40),
                Cursor = Cursors.Hand
            };
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.Click += BtnRestore_Click;
            
            Label lblWarn = new Label
            {
                Text = "⚠️ Restoring will overwrite all current data. Proceed with caution.",
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9),
                Location = new Point(20, 90),
                AutoSize = true
            };

            pnlManualContent.Controls.Add(btnManualBackup);
            pnlManualContent.Controls.Add(btnRestore);
            pnlManualContent.Controls.Add(lblWarn);
            grpManual.Controls.Add(pnlManualContent);

            mainFlow.Controls.Add(grpAuto);
            mainFlow.Controls.Add(grpManual);

            dbTab.Controls.Add(mainFlow);
            tabControl.TabPages.Add(dbTab);

            
            // Add main structure to form
            pnlBody.Controls.Add(tabControl);
            this.Controls.Add(pnlBody);   // Fill
            this.Controls.Add(pnlHeader); // Top






            // Access Control: Only show User Management for Admins
            if (Session.IsAdmin())
            {
                AddUserManagementTab(tabControl);
            }
        }

        private void AddUserManagementTab(TabControl tabControl)
        {
            try
            {
                // User Management Tab
                TabPage usersTab = new TabPage("User Management");
                usersTab.Padding = new Padding(20);
                usersTab.BackColor = Color.White;

                GroupBox grpAddUser = new GroupBox
                {
                    Text = "Add New User",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Location = new Point(20, 20),
                    Size = new Size(400, 250)
                };

                Label lblU = new Label { Text = "Username:", Location = new Point(20, 40), Font = new Font("Segoe UI", 10, FontStyle.Regular), AutoSize = true };
                TextBox txtNewUser = new TextBox { Location = new Point(20, 65), Width = 350, Font = new Font("Segoe UI", 10) };
                
                Label lblP = new Label { Text = "Password:", Location = new Point(20, 100), Font = new Font("Segoe UI", 10, FontStyle.Regular), AutoSize = true };
                TextBox txtNewPass = new TextBox { Location = new Point(20, 125), Width = 350, PasswordChar = '•', Font = new Font("Segoe UI", 10) };

                Label lblR = new Label { Text = "Role:", Location = new Point(20, 160), Font = new Font("Segoe UI", 10, FontStyle.Regular), AutoSize = true };
                ComboBox cmbRole = new ComboBox { Location = new Point(20, 185), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
                cmbRole.Items.AddRange(new object[] { "User", "Admin" });
                cmbRole.SelectedIndex = 0;

                Button btnAddUser = new Button
                {
                    Text = "Create User",
                    BackColor = PrimaryColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(250, 210),
                    Size = new Size(120, 35)
                };
                
                // User List Grid
                Label lblList = new Label { Text = "Existing Users", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(450, 20), AutoSize = true };
                DataGridView gridUsers = new DataGridView
                {
                    Location = new Point(450, 50),
                    Size = new Size(400, 500),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    Font = new Font("Segoe UI", 9)
                };
                
                // Logic
                 btnAddUser.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNewUser.Text) || string.IsNullOrWhiteSpace(txtNewPass.Text))
                    {
                        MessageBox.Show("Username and Password are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (DatabaseHelper.AddUser(txtNewUser.Text, txtNewPass.Text, cmbRole.SelectedItem.ToString()))
                    {
                        MessageBox.Show($"User '{txtNewUser.Text}' created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNewUser.Clear();
                        txtNewPass.Clear();
                         try { gridUsers.DataSource = DatabaseHelper.GetUsers(); } catch { }
                    }
                };

                // Load Users
                try { gridUsers.DataSource = DatabaseHelper.GetUsers(); } catch { }

                grpAddUser.Controls.AddRange(new Control[] { lblU, txtNewUser, lblP, txtNewPass, lblR, cmbRole, btnAddUser });

                usersTab.Controls.Add(grpAddUser);
                usersTab.Controls.Add(lblList);
                usersTab.Controls.Add(gridUsers);

                tabControl.TabPages.Add(usersTab);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding User Management tab: {ex.Message}", "UI Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadSettings()
        {
            string autoBackup = DatabaseHelper.GetSetting("AutoBackup");
            chkAutoBackup.Checked = autoBackup == "True";

            string frequency = DatabaseHelper.GetSetting("BackupFrequency");
            if (!string.IsNullOrEmpty(frequency) && cmbFrequency.Items.Contains(frequency))
                cmbFrequency.SelectedItem = frequency;
            
            string path = DatabaseHelper.GetSetting("BackupFolderPath");
            if (string.IsNullOrEmpty(path))
            {
                txtBackupPath.Text = Path.Combine(Application.StartupPath, "Backups");
            }
            else
            {
                txtBackupPath.Text = path;
            }
                
            string lastBackup = DatabaseHelper.GetSetting("LastBackupDate");
            if (!string.IsNullOrEmpty(lastBackup))
                lblLastBackup.Text = lastBackup;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select a folder to store database backups";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtBackupPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DatabaseHelper.SaveSetting("AutoBackup", chkAutoBackup.Checked.ToString());
            DatabaseHelper.SaveSetting("BackupFrequency", cmbFrequency.SelectedItem.ToString());
            DatabaseHelper.SaveSetting("BackupFolderPath", txtBackupPath.Text);
            
            MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnManualBackup_Click(object sender, EventArgs e)
        {
            // First save the current path setting if implied
            if (!string.IsNullOrEmpty(txtBackupPath.Text))
            {
                DatabaseHelper.SaveSetting("BackupFolderPath", txtBackupPath.Text);
            }
            
            DatabaseHelper.BackupDatabase();
            DatabaseHelper.SaveSetting("LastBackupDate", DateTime.Now.ToString());
            lblLastBackup.Text = DateTime.Now.ToString();
            
            string path = DatabaseHelper.GetSetting("BackupFolderPath");
            if(string.IsNullOrEmpty(path)) path = Path.Combine(Application.StartupPath, "Backups");
            
            MessageBox.Show($"Backup created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRestore_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SQLite Database|*.sqlite|All Files|*.*";
                ofd.Title = "Select Backup File to Restore";
                
                // Try to start in backup directory
                string path = DatabaseHelper.GetSetting("BackupFolderPath");
                if(!string.IsNullOrEmpty(path) && Directory.Exists(path)) ofd.InitialDirectory = path;
                else ofd.InitialDirectory = Path.Combine(Application.StartupPath, "Backups");

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("Are you sure you want to overwrite the current database with this backup? This cannot be undone.", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        DatabaseHelper.RestoreDatabase(ofd.FileName);
                    }
                }
            }
        }
    }
}
