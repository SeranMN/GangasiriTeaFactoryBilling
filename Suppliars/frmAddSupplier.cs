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
        public frmAddSupplier()
        {
            InitializeComponent();
        }
        private void BtnAutoGenerate_Click(object sender, EventArgs e)
        {
            // Generate supplier number like S-001, S-002, etc.
            // In real app, get next number from database
            Random rnd = new Random();
            int nextNum = rnd.Next(1, 999);
            txtSupplierNumber.Text = $"S-{nextNum:D3}";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // Save to database
                MessageBox.Show("Supplier saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                
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
