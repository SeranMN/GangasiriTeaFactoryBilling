using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.Invoice
{
    public class SelectInvoicePeriodForm : Form
    {
        public int SelectedYear { get; private set; }
        public int SelectedMonth { get; private set; }

        private ComboBox cmbMonth;
        private TextBox txtYear;
        private Button btnOk;
        private Button btnCancel;

        public SelectInvoicePeriodForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Select Invoice Period";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Label lblYear = new Label
            {
                Text = "Year:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            txtYear = new TextBox
            {
                Location = new Point(100, 18),
                Width = 150,
                Font = new Font("Segoe UI", 10),
                Text = DateTime.Now.Year.ToString()
            };

            Label lblMonth = new Label
            {
                Text = "Month:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            cmbMonth = new ComboBox
            {
                Location = new Point(100, 58),
                Width = 150,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // Populate months
            var months = DateTimeFormatInfo.InvariantInfo.MonthNames;
            foreach (var month in months)
            {
                if (!string.IsNullOrEmpty(month))
                {
                    cmbMonth.Items.Add(month);
                }
            }
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;

            btnOk = new Button
            {
                Text = "Generate",
                Location = new Point(40, 110),
                Width = 90,
                Height = 35,
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += BtnOk_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(150, 110),
                Width = 90,
                Height = 35,
                DialogResult = DialogResult.Cancel,
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            this.Controls.Add(lblYear);
            this.Controls.Add(txtYear);
            this.Controls.Add(lblMonth);
            this.Controls.Add(cmbMonth);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtYear.Text, out int year) && year > 2000 && year < 2100)
            {
                SelectedYear = year;
                SelectedMonth = cmbMonth.SelectedIndex + 1;
            }
            else
            {
                MessageBox.Show("Please enter a valid year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None; // Prevent closing
                return;
            }
        }
    }
}
