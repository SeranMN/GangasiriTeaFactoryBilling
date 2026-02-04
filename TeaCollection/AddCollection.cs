using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.Models;
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

        public bool IsEditMode { get; set; }
        public string CollectionId { get; set; }

        public decimal currentRate = DataAccess.GetCurrentTeaRate() == null ? 0 : DataAccess.GetCurrentTeaRate().Rate; // Example rate per kg

        // Property to get the collection data after saving
        public DailyCollection Result { get; private set; }

        public class CollectionData
        {
            public string Supplier { get; set; }
            public DateTime Date { get; set; }
            public decimal Weight { get; set; }
            public decimal Transport { get; set; }
            public string Notes { get; set; }
            public decimal TotalAmount { get; set; }
        }
        public AddCollection(int? supplierId = null)
        {
            InitializeComponent();
            LoadSuppliers(supplierId);
            CalculateTotal();
        }

        private class SuppliarItem
        {
            public int SuppliarId { get; set; }
            public string DisplayName { get; set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private void LoadSuppliers(int? selectedSupplierId = null)
        {
            cmbSupplier.Items.Clear();
            SuppliarItem? itemToSelect = null;
            try
            {
                var supliars = DataAccess.GetActiveSuppliers();

                // Enable Autocomplete
                cmbSupplier.DropDownStyle = ComboBoxStyle.DropDown;
                cmbSupplier.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbSupplier.AutoCompleteSource = AutoCompleteSource.ListItems;

                foreach (var supplier in supliars)
                {
                    var item = new SuppliarItem
                    {
                        SuppliarId = supplier.SupplierID,
                        DisplayName = $"{supplier.SupplierNumber} - {supplier.SupplierName}"
                    };
                    cmbSupplier.Items.Add(item);

                    if (selectedSupplierId.HasValue && item.SuppliarId == selectedSupplierId.Value)
                    {
                        itemToSelect = item;
                    }
                }

                if (itemToSelect != null)
                {
                    cmbSupplier.SelectedItem = itemToSelect;
                }
                else if (cmbSupplier.Items.Count > 0)
                {
                    cmbSupplier.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Method to load data for editing
        // Method to load data for editing
        public void LoadCollectionData(int supplierId, DateTime date, decimal weight, string transport, string notes)
        {
            // Find and select the supplier
            SuppliarItem? itemToSelect = null;
            foreach (var item in cmbSupplier.Items)
            {
                if (item is SuppliarItem supplierItem && supplierItem.SuppliarId == supplierId)
                {
                    itemToSelect = supplierItem;
                    break;
                }
            }

            if (itemToSelect != null)
            {
                cmbSupplier.SelectedItem = itemToSelect;
            }

            dtpDate.Value = date;
            txtWeight.Text = weight.ToString("N2");

            if (transport == "Yes")
            {
                chkTransport.Checked = true;
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
                lblTotalAmount.Text = $"LKR {total:N2}";
            }
            else
            {
                lblTotalAmount.Text = "LKR 0.00";
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
                var selectedSupplier = cmbSupplier.SelectedItem as SuppliarItem;
                Result = new DailyCollection
                {
                    SupplierID = selectedSupplier.SuppliarId,
                    CollectionDate = dtpDate.Value,
                    Weight = weight,
                    IsTransportAdd = chkTransport.Checked ? "Yes" : "No",
                    Notes = txtNotes.Text,
                    TotalAmount = total
                };

                if (IsEditMode)
                {
                    Result.CollectionID = int.Parse(CollectionId);
                    DataAccess.UpdateDailyCollection(Result);
                }
                else
                {
                    DataAccess.AddDailyCollection(Result);
                }

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

