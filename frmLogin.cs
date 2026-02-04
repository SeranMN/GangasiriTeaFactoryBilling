using System;
using System.Windows.Forms;
using GangasiriTeaFactoryBilling.db;

namespace GangasiriTeaFactoryBilling
{
    public partial class frmLogin : Form
    {
        public frmLogin()
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
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DatabaseHelper.CheckUser(username, password))
            {
                var user = DatabaseHelper.GetUser(username);
                Session.Login(user.UserID, user.Username, user.Role);

                // Login successful
                this.Hide();
                frmMain mainForm = new frmMain();
                mainForm.FormClosed += (s, args) => this.Close(); 
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}
