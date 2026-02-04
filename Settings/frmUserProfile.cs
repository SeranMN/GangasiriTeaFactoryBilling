using System;
using System.Drawing;
using System.Windows.Forms;
using GangasiriTeaFactoryBilling.db;

namespace GangasiriTeaFactoryBilling.Settings
{
    public partial class frmUserProfile : Form
    {
        private TextBox txtCurrentPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;

        public frmUserProfile()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Padding = new Padding(20);
            this.BackColor = Color.White;

            Label title = new Label
            {
                Text = $"Profile: {Session.Username}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Top,
                Height = 40
            };

            GroupBox grpPassword = new GroupBox
            {
                Text = "Change Password",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 70),
                Size = new Size(440, 250)
            };

            // Current Password
            Label lblCurrent = new Label { Text = "Current Password:", Location = new Point(20, 40), AutoSize = true };
            txtCurrentPassword = new TextBox { Location = new Point(20, 65), Width = 380, PasswordChar = '•' };

            // New Password
            Label lblNew = new Label { Text = "New Password:", Location = new Point(20, 100), AutoSize = true };
            txtNewPassword = new TextBox { Location = new Point(20, 125), Width = 380, PasswordChar = '•' };

            // Confirm Password
            Label lblConfirm = new Label { Text = "Confirm Password:", Location = new Point(20, 160), AutoSize = true };
            txtConfirmPassword = new TextBox { Location = new Point(20, 185), Width = 380, PasswordChar = '•' };

            Button btnSave = new Button
            {
                Text = "Update Password",
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(250, 220), // Relative to main form? No, I'll put it in groupbox or below
                Size = new Size(150, 35),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            btnSave.Location = new Point(270, 200); // Inside GroupBox

            grpPassword.Controls.AddRange(new Control[] { lblCurrent, txtCurrentPassword, lblNew, txtNewPassword, lblConfirm, txtConfirmPassword, btnSave });

            this.Controls.Add(title);
            this.Controls.Add(grpPassword);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text) || 
                string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("New passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verify current password logic
            // Since we don't store plain text in proper apps, we should hash.
            // But current implementation uses plain text (as per existing login code).
            // I'll call DatabaseHelper to check.
            
            if (DatabaseHelper.CheckPassword(Session.UserID, txtCurrentPassword.Text))
            {
                if (DatabaseHelper.UpdatePassword(Session.UserID, txtNewPassword.Text))
                {
                    MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Incorrect current password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
