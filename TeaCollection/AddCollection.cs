using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.TeaCollection
{
    public partial class AddCollection : Form
    {

        public bool IsEditMode { get; set; } = false;
        public string CollectionId { get; set; }

        public decimal currentRate = 160.00m; // Example rate per kg

        // Property to get the collection data after saving
        public CollectionData Result { get; private set; }

        public class CollectionData
        {
            public string Supplier { get; set; }
            public DateTime Date { get; set; }
            public decimal Weight { get; set; }
            public decimal Transport { get; set; }
            public string Notes { get; set; }
            public decimal TotalAmount { get; set; }
        }
        public AddCollection()
        {
            InitializeComponent();
            LoadSuppliers();
            CalculateTotal();
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
        public void LoadCollectionData(string supplier, DateTime date, decimal weight, decimal transport, string notes)
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
            txtWeight.Text = weight.ToString("N2");

            if (transport > 0)
            {
                chkTransport.Checked = true;
                txtTransport.Text = transport.ToString("N2");
            }

            txtNotes.Text = notes;
            CalculateTotal();
        }

        // Event Handlers
        private void ChkTransport_CheckedChanged(object sender, EventArgs e)
        {
            txtTransport.Enabled = chkTransport.Checked;
            if (!chkTransport.Checked)
            {
                txtTransport.Clear();
            }
            CalculateTotal();
        }

        private void TxtWeight_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void TxtTransport_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            if (decimal.TryParse(txtWeight.Text, out decimal weight))
            {
                decimal transport = chkTransport.Checked && !string.IsNullOrWhiteSpace(txtTransport.Text)
                    ? decimal.Parse(txtTransport.Text)
                    : 0;

                decimal total = (weight * currentRate) + transport;
                lblTotalAmount.Text = $"₹{total:N2}";
            }
            else
            {
                lblTotalAmount.Text = "₹0.00";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                decimal weight = decimal.Parse(txtWeight.Text);
                decimal transport = chkTransport.Checked && !string.IsNullOrWhiteSpace(txtTransport.Text)
                    ? decimal.Parse(txtTransport.Text)
                    : 0;
                decimal total = (weight * currentRate) + transport;

                Result = new CollectionData
                {
                    Supplier = cmbSupplier.Text,
                    Date = dtpDate.Value,
                    Weight = weight,
                    Transport = transport,
                    Notes = txtNotes.Text,
                    TotalAmount = total
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

            if (string.IsNullOrWhiteSpace(txtWeight.Text))
            {
                MessageBox.Show("Please enter tea weight", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWeight.Focus();
                return false;
            }

            if (!decimal.TryParse(txtWeight.Text, out decimal weight) || weight <= 0)
            {
                MessageBox.Show("Please enter a valid weight", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWeight.Focus();
                return false;
            }

            if (chkTransport.Checked && !string.IsNullOrWhiteSpace(txtTransport.Text))
            {
                if (!decimal.TryParse(txtTransport.Text, out decimal transport) || transport < 0)
                {
                    MessageBox.Show("Please enter a valid transport amount", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTransport.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}

