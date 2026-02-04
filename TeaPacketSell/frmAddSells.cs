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

namespace GangasiriTeaFactoryBilling.TeaPacketSell
{
    public partial class frmAddSells : Form
    {

        public bool IsEditMode { get; set; } = false;
        public string AdvanceId { get; set; } = string.Empty;

        // Property to get the advance data after saving
        public TeaPacket Result { get; private set; } = null!;
        public frmAddSells()
        {
            InitializeComponent();
            LoadSuppliers();
            
        }

        private void LoadSuppliers()
        {
            try
            {
                var suppliers = DataAccess.GetAllSuppliers();
                foreach (var supplier in suppliers)
                {
                    cmbSupplier.Items.Add(new SuppliarItem
                    {
                        SuppliarId = supplier.SupplierID,
                        DisplayName = $"{supplier.SupplierNumber} - {supplier.SupplierName}"
                    });
                }
            }
            catch
            {
                throw;
            }


            if (cmbSupplier.Items.Count > 0)
                cmbSupplier.SelectedIndex = 0;
        }

        // Method to load data for editing
        public void LoadSellData(string supplier, DateTime date, decimal amount, string description)
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
            //txtDescription.Text = description;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var supplierItem = cmbSupplier.SelectedItem as SuppliarItem;
            CalculateTotal();
            if (ValidateForm())
            {
                Result = new TeaPacket
                {
                   SupllierID = supplierItem.SuppliarId.ToString(),
                   Date = dtpDate.Value,
                   Price = decimal.Parse(txtAmount.Text),
                   Qty = txtQty.Text != string.Empty ? int.Parse( txtQty.Text) : 0,
                   Total = total.Text != string.Empty ? decimal.Parse( total.Text) : 0
                };
                try
                {
                    if (IsEditMode)
                    {
                        Result.SupllierID = AdvanceId;
                        
                    }
                    else
                    {

                        DataAccess.AddTeaPacketSell(Result);
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

        private void CalculateTotal()
        {
            if (decimal.TryParse(txtAmount.Text, out decimal price) && int.TryParse(txtQty.Text, out int qty))
            {
                decimal totalCal = price * qty;
                total.Text = totalCal.ToString("N2");
            }
            else
            {
                total.Text = "0.00";
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
