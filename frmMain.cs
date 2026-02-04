using GangasiriTeaFactoryBilling.Advanced;
using GangasiriTeaFactoryBilling.DashBoard;
using GangasiriTeaFactoryBilling.Invoice;
using GangasiriTeaFactoryBilling.LineManagements;
using GangasiriTeaFactoryBilling.Suppliars;
using GangasiriTeaFactoryBilling.TeaCollection;
using GangasiriTeaFactoryBilling.TeaPacketSell;
using GangasiriTeaFactoryBilling.TeaRates;
using GangasiriTeaFactoryBilling.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling
{
    public partial class frmMain : Form
    {
        private Panel sidePanel;
        private Panel contentPanel;
        private Button currentButton;
        private Form activeForm;

        public frmMain()
        {
            InitializeComponent();
            this.Load += FrmMain_Load;
        }
        private async void FrmMain_Load(object sender, EventArgs e)
        {
            InitializeUI();
            OpenChildForm(new frmDashboard(), "dashboardBtn");
            
            // Check for updates
            await GangasiriTeaFactoryBilling.Updater.GitUpdateManager.CheckForUpdates();
        }
        private void InitializeUI()
        {
            // Header Panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                
                BackColor = Color.FromArgb(0, 122, 204)
            };

            Label titleLabel = new Label
            {
                Text = "GTF Tea Collection",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                Width = 500,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 10, 10, 0)
            };

            Label userLabel = new Label
            {
                Text = $"👤 {Session.Username} ({Session.Role})",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 20, 0),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            userLabel.Click += (s, e) => new Settings.frmUserProfile().ShowDialog();

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(userLabel);

            // Side Navigation Panel
            sidePanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            // Content Panel
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Add navigation buttons
            InitializeNavigationButtons();

            // Add panels to form
            this.Controls.Add(contentPanel);
            this.Controls.Add(sidePanel);
            this.Controls.Add(headerPanel);
        }
        private void InitializeNavigationButtons()
        {
            // First clear any existing controls
            sidePanel.Controls.Clear();

            int buttonHeight = 50;
            int buttonY = 20;

            // Create buttons array
            (string text, string tag, string name)[] buttons = new[]
            {
                    ("📊 Dashboard", "dashboard", "dashboardBtn"),
                    ("👥 Suppliers", "suppliers", "suppliersBtn"),
                    ("🍃 Daily Collection", "collection", "collectionBtn"),
                    ("💵 Advances", "advances", "advancesBtn"),
                   // ("$ Tea Packet Sell", "tea", "teaBtn"),
                    ("🧾 Invoices", "invoices", "invoicesBtn"),
                   // ("📈 Reports", "reports", "reportsBtn"),
                    ("🏭 Lines", "lines", "linesBtn"), 
                    ("📊 Tea Rates", "rates", "ratesBtn"), 
                    ("⚙️ Settings", "settings", "settingsBtn")
            };

            foreach (var (text, tag, name) in buttons)
            {
                // Access Control: Hide Settings for non-admins
                if (tag == "settings" && !Session.IsAdmin()) continue;

                Button btn = CreateNavButton(text, tag, name);
                btn.Location = new Point(0, buttonY);
                buttonY += buttonHeight + 10;
                sidePanel.Controls.Add(btn);
            }

            // Exit button at bottom
            Button exitBtn = new Button
            {
                Text = "🚪 Exit",
                Tag = "exit",
                Name = "exitBtn",
                Size = new Size(220, 50),
                Location = new Point(0, sidePanel.Height - 70),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(37, 37, 38),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            exitBtn.FlatAppearance.BorderSize = 0;
            exitBtn.Click += (s, e) => Application.Exit();
            exitBtn.MouseEnter += (s, e) => exitBtn.BackColor = Color.FromArgb(62, 62, 64);
            exitBtn.MouseLeave += (s, e) => exitBtn.BackColor = Color.FromArgb(37, 37, 38);

            sidePanel.Controls.Add(exitBtn);
        }

        private Button CreateNavButton(string text, string tag, string name)
        {
            Button btn = new Button
            {
                Text = text,
                Tag = tag,
                Name = name,
                Size = new Size(220, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Gainsboro,
                BackColor = Color.FromArgb(37, 37, 38),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            // Hover effects
            btn.MouseEnter += (s, e) =>
            {
                if (btn != currentButton)
                    btn.BackColor = Color.FromArgb(62, 62, 64);
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn != currentButton)
                    btn.BackColor = Color.FromArgb(37, 37, 38);
            };

            // Click event
            btn.Click += (s, e) =>
            {
                ActivateButton(btn);
                OpenFormByTag(tag);
            };

            return btn;
        }

        private void ActivateButton(object sender)
        {
            Button btn = sender as Button;

            if (btn != null)
            {
                // Reset previous button
                if (currentButton != null)
                {
                    currentButton.BackColor = Color.FromArgb(37, 37, 38);
                    currentButton.ForeColor = Color.Gainsboro;
                    currentButton.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }

                // Activate current button
                btn.BackColor = Color.FromArgb(0, 122, 204);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                currentButton = btn;
            }
        }

        private void OpenFormByTag(string tag)
        {
            Form formToOpen = null;

            switch (tag)
            {
                case "dashboard":
                    formToOpen = new frmDashboard();
                    break;
                case "suppliers":
                    formToOpen = new frmSuppliars();
                    break;
                case "collection":
                    formToOpen = new frmTeaCollection();
                   break;
                case "invoices":
                    formToOpen = new frmInvoices();
                    break;
                case "advances":
                    formToOpen = new frmAdvanceManagement();
                    break;

                case "lines":
                    formToOpen = new frmLines(); 
                    break;
                case "rates":
                    formToOpen = new frmTeaRates(); 
                    break;
                case "tea":
                    formToOpen = new frmTeaPacketSellManagement();
                    break;
                    //case "reports":
                    //    formToOpen = new frmReports();
                    //    break;
                    // Settings
                    case "settings":
                        formToOpen = new frmSettings();
                        break;
            }

            if (formToOpen != null)
            {
                OpenChildForm(formToOpen, tag + "Btn");
            }
        }

        private void OpenChildForm(Form childForm, string buttonName)
        {
            // Close active form
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm.Dispose();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Clear content panel
            contentPanel.Controls.Clear();

            // Add form to content panel
            contentPanel.Controls.Add(childForm);
            childForm.Show();

            // Activate corresponding button
            ActivateNavigationButton(buttonName);
        }

        private void ActivateNavigationButton(string buttonName)
        {
            foreach (Control ctrl in sidePanel.Controls)
            {
                if (ctrl is Button btn && btn.Name == buttonName)
                {
                    ActivateButton(btn);
                    break;
                }
            }
        }
    }
}
