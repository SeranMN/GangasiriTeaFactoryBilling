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

namespace GangasiriTeaFactoryBilling.Advanced
{
    public partial class frmAddAddvanced : Form
    {
        public bool IsEditMode { get; set; } = false;
        public string AdvanceId { get; set; } = string.Empty;

        // Property to get the advance data after saving
        public Advance Result { get; private set; } = null!;

        public class AdvanceData
        {
            public string Supplier { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
        }   
        public frmAddAddvanced(int? supplierId = null)
        {
            InitializeComponent();
            LoadSuppliers(supplierId);
        }



        private void LoadSuppliers(int? selectedSupplierId = null)
        {
            SuppliarItem? itemToSelect = null;
            try
            {
                var suppliers = DataAccess.GetActiveSuppliers();
                // Enable Autocomplete
                cmbSupplier.DropDownStyle = ComboBoxStyle.DropDown;
                cmbSupplier.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbSupplier.AutoCompleteSource = AutoCompleteSource.ListItems;

                foreach (var supplier in suppliers)
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
            catch
            {
                throw;
            }
        }

        // Method to load data for editing
        public void LoadAdvanceData(string supplier, DateTime date, decimal amount, string description)
        {
            // Find and select the supplier
            for (int i = 0; i < cmbSupplier.Items.Count; i++)
            {
                if (cmbSupplier.Items[i].ToString().Contains(supplier))
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
            var supplierItem = cmbSupplier.SelectedItem as SuppliarItem;
            if (supplierItem == null)
            {
                MessageBox.Show("Please select a valid supplier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ValidateForm())
            {
                Result = new Advance
                {
                    SupplierID = supplierItem.SuppliarId,
                    AdvanceDate = dtpDate.Value,
                    Amount = decimal.Parse(txtAmount.Text),
                    Description = txtDescription.Text,
                    Status = "Active",
                    CreatedDate = DateTime.Now
                };
                try
                {
                    if (IsEditMode)
                    {
                        Result.AdvanceID = int.Parse(AdvanceId);
                        DataAccess.UpdateAdvance(Result);
                    }
                    else
                    {

                        DataAccess.AddAdvance(Result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving advance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

