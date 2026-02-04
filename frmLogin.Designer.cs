using System.Drawing;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private Label lblTitle;
        private PictureBox picLogo;
        private Panel mainPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            try
            {
                this.Icon = new Icon(@"D:\GangaSiri Tea Factory Billing\GangasiriTeaFactoryBilling\img\app_icon.ico");
            }
            catch { }

            // Main Panel
            mainPanel = new Panel();
            mainPanel.Size = new Size(360, 460);
            mainPanel.Location = new Point(20, 20);
            mainPanel.BackColor = Color.White;
            mainPanel.BorderStyle = BorderStyle.FixedSingle; // Light border
            this.Controls.Add(mainPanel);

            // Logo
            picLogo = new PictureBox();
            picLogo.Size = new Size(80, 80);
            picLogo.Location = new Point(140, 10);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.ImageLocation = @"D:\GangaSiri Tea Factory Billing\GangasiriTeaFactoryBilling\img\Gemini_Generated_Image_f2q39df2q39df2q3.png";
            mainPanel.Controls.Add(picLogo);

            // Title
            lblTitle = new Label();
            lblTitle.Text = "GTF Billing Login";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 122, 204); // Blue theme
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(80, 95);
            mainPanel.Controls.Add(lblTitle);

            // Username Label
            Label lblUser = new Label();
            lblUser.Text = "Username";
            lblUser.Font = new Font("Segoe UI", 10);
            lblUser.ForeColor = Color.Gray;
            lblUser.Location = new Point(40, 150);
            mainPanel.Controls.Add(lblUser);

            // Username TextBox
            txtUsername = new TextBox();
            txtUsername.Font = new Font("Segoe UI", 12);
            txtUsername.Size = new Size(280, 30);
            txtUsername.Location = new Point(40, 175);
            mainPanel.Controls.Add(txtUsername);

            // Password Label
            Label lblPass = new Label();
            lblPass.Text = "Password";
            lblPass.Font = new Font("Segoe UI", 10);
            lblPass.ForeColor = Color.Gray;
            lblPass.Location = new Point(40, 230);
            mainPanel.Controls.Add(lblPass);

            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 12);
            txtPassword.Size = new Size(280, 30);
            txtPassword.Location = new Point(40, 255);
            txtPassword.PasswordChar = '•';
            mainPanel.Controls.Add(txtPassword);

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Size = new Size(280, 45);
            btnLogin.Location = new Point(40, 320);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            mainPanel.Controls.Add(btnLogin);

            // Exit Button
            btnExit = new Button();
            btnExit.Text = "Exit";
            btnExit.Font = new Font("Segoe UI", 10);
            btnExit.ForeColor = Color.Gray;
            btnExit.BackColor = Color.Transparent;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Size = new Size(280, 35);
            btnExit.Location = new Point(40, 380);
            btnExit.Cursor = Cursors.Hand;
            btnExit.Click += (s, e) => Application.Exit();
            mainPanel.Controls.Add(btnExit);

            // Focus on load
            this.Load += (s, e) => txtUsername.Focus();
        }
    }
}
