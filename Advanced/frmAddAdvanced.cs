using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.Advanced
{
    public partial class frmAddAdvanced : Form
    {
        public bool IsEditMode { get; set; } = false;
        public string AdvanceId { get; set; } = string.Empty;

        // Property to get the advance data after saving
        public AdvanceData Result { get; private set; } = null!;

        public class AdvanceData
        {
            public string Supplier { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
        }   
        public frmAddAdvanced()
        {
            InitializeComponent();
        }

        private void LoadSuppliers()
        {
            // TODO: Load suppliers from database
            // Sample data
            cmbSupplier.Items.Add("S-001 - Kamal Perera");
            cmbSupplier.Items.Add("S-002 - Sunil Fernando");
            cmbSupplier.Items.Add("S-003 - Anura Silva");
            cmbSupplier.Items.Add("S-004 - Nimal Rathnayake");
            cmbSupplier.Items.Add("S-005 - Sampath Bandara");

            if (cmbSupplier.Items.Count > 0)
                cmbSupplier.SelectedIndex = 0;
        }

        // Method to load data for editing
        public void LoadAdvanceData(string supplier, DateTime date, decimal amount, string description)
        {
            // Find and select the supplier
            for (int i = 0; i < cmbSupplier.Items.Count; i++)
            {
                if (cmbSupplier.Items[i].ToString() == supplier)
                {
                    cmbSupplier.SelectedIndex = i;
                    break;
                }
            }

            dtpDate.Value = date;
            txtAmount.Text = amount.ToString("N2");
            txtDescription.Text = description;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Result = new AdvanceData
                {
                    Supplier = cmbSupplier.Text,
                    Date = dtpDate.Value,
                    Amount = decimal.Parse(txtAmount.Text),
                    Description = txtDescription.Text
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateForm()
        {
            if (cmbSupplier.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a supplier", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSupplier.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Please enter advance amount", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return false;
            }

            return true;
        }
    }
}

