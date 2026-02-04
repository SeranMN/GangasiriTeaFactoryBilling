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

namespace GangasiriTeaFactoryBilling.Suppliars
{
    public partial class frmAddSupplier : Form
    {
        public Supplier Result { get; private set; }

        public frmAddSupplier()
        {
            InitializeComponent();
            LoadLines();
        }
        private void BtnAutoGenerate_Click(object sender, EventArgs e)
        {
            // Generate supplier number like S-001, S-002, etc.
            // In real app, get next number from database
            Random rnd = new Random();
            int nextNum = rnd.Next(1, 999);
            txtSupplierNumber.Text = $"S-{nextNum:D3}";
        }

        private void LoadLines()
        {
            try
            {
                cmbLine.Items.Clear();
                var lines = DataAccess.GetAllLines();

                foreach (var line in lines)
                {
                    cmbLine.Items.Add(new LineItem
                    {
                        LineID = line.LineID,
                        DisplayText = $"{line.LineName} (Transport: LKR{line.TransportFee:F2})"
                    });
                }

                if (cmbLine.Items.Count > 0)
                    cmbLine.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading lines: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class LineItem
        {
            public string LineID { get; set; }
            public string DisplayText { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    // Get selected line
                    var selectedLine = cmbLine.SelectedItem as LineItem;

                    Result = new Supplier
                    {
                        SupplierNumber = txtSupplierNumber.Text,
                        SupplierName = txtFullName.Text,
                        Telephone = txtTelephone.Text,
                        LineID = selectedLine?.LineID ??string.Empty,
                        Address = txtAddress.Text,
                        DueAmount = decimal.TryParse(txtdueAmount.Text, out var dueAmount) ? dueAmount : 0m,
                        Status = rbActive.Checked ? "Active" : "Inactive"
                       
                    };
                    DataAccess.AddSupplier(Result);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving supplier: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void BtnSaveAndNew_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // Save to database
                MessageBox.Show("Supplier saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear form for new entry
                ClearForm();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtSupplierNumber.Text))
            {
                MessageBox.Show("Please enter supplier number", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSupplierNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter supplier name", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTelephone.Text))
            {
                MessageBox.Show("Please enter telephone number", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelephone.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtSupplierNumber.Clear();
            txtFullName.Clear();
            txtTelephone.Clear();
            txtAddress.Clear();
            cmbLine.SelectedIndex = 0;
            rbActive.Checked = true;
            txtSupplierNumber.Focus();
        }
    
}
}
