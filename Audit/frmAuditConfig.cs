using System;
using System.Drawing;
using System.Windows.Forms;
using GangasiriTeaFactoryBilling.Audit;

namespace GangasiriTeaFactoryBilling.Audit
{
    public class frmAuditConfig : Form
    {
        private CheckedListBox clbTables;
        private Button btnSave;
        private string[] availableTables = { "Users", "Suppliers", "TeaRates", "DailyCollection", "Advances", "MonthlyInvoices" };

        public frmAuditConfig()
        {
            InitializeUI();
            LoadSettings();
        }

        private void InitializeUI()
        {
            this.Text = "Audit Configuration";
            this.Size = new Size(300, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblTitle = new Label
            {
                Text = "Select Tables to Audit",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };

            clbTables = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                Font = new Font("Segoe UI", 10),
                Padding = new Padding(10)
            };
            clbTables.Items.AddRange(availableTables);

            btnSave = new Button
            {
                Text = "Save",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(clbTables);
            this.Controls.Add(lblTitle);
            this.Controls.Add(btnSave);
        }

        private void LoadSettings()
        {
            for (int i = 0; i < clbTables.Items.Count; i++)
            {
                string tableName = clbTables.Items[i].ToString();
                clbTables.SetItemChecked(i, AuditManager.IsAuditEnabled(tableName));
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbTables.Items.Count; i++)
            {
                string tableName = clbTables.Items[i].ToString();
                bool isChecked = clbTables.GetItemChecked(i);
                AuditManager.SetAuditEnabled(tableName, isChecked);
            }
            MessageBox.Show("Audit settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
