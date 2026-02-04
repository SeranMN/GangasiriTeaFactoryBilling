using GangasiriTeaFactoryBilling.db;
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
    public partial class frmAdvanceManagement : Form
    {
        public frmAdvanceManagement()
        {
            InitializeComponent();
            
            LoadAdvances();
        }

       

        private void LoadAdvances()
        {
            // TODO: Load advances from database
            // Sample data
            advancesGrid.Rows.Clear();
            try
            {
                var Advances = DataAccess.GetAllAdvances();
                foreach (var item in Advances)
                {
                    advancesGrid.Rows.Add(item.AdvanceID, item.SupplierName, item.AdvanceDate, item.Amount, item.Description, item.Status);
                }
            }
            catch
            {
                MessageBox.Show("Error In Loading Advance Data");
            }
        }

        // Event Handlers
        private void BtnAddAdvance_Click(object sender, EventArgs e)
        {
            using (frmAddAddvanced addForm = new frmAddAddvanced())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Add new advance to grid
                    var advanceData = addForm.Result;
                    string newId = "ADV-" + (advancesGrid.Rows.Count + 1).ToString("D3");

                    advancesGrid.Rows.Add(
                        newId,
                        advanceData.SupplierName,
                        advanceData.AdvanceDate.ToString("yyyy-MM-dd"),
                        advanceData.Amount.ToString("N2"),
                        advanceData.Description,
                        "✅ Active"
                    );

                    MessageBox.Show("Advance added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (advancesGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = advancesGrid.SelectedRows[0];
                string advanceId = selectedRow.Cells["AdvanceID"].Value.ToString();
                string supplier = selectedRow.Cells["Supplier"].Value.ToString();
                DateTime date = Convert.ToDateTime(selectedRow.Cells["Date"].Value.ToString());
                decimal amount = Convert.ToDecimal (selectedRow.Cells["Amount"].Value.ToString());
                string description = selectedRow.Cells["Description"].Value.ToString();

                using (frmAddAddvanced editForm = new frmAddAddvanced())
                {
                    // Pre-populate form with selected data
                    editForm.Text = "Edit Advance - " + advanceId;
                    editForm.LoadAdvanceData(supplier, date, amount, description);

                    // TODO: Load advance data for editing
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Update row
                        var advanceData = editForm.Result;
                        selectedRow.Cells["Supplier"].Value = advanceData.SupplierName;
                        selectedRow.Cells["Date"].Value = advanceData.AdvanceDate.ToString("yyyy-MM-dd");
                        selectedRow.Cells["Amount"].Value = advanceData.Amount.ToString("N2");
                        selectedRow.Cells["Description"].Value = advanceData.Description;

                        MessageBox.Show("Advance updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an advance to edit", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (advancesGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = advancesGrid.SelectedRows[0];
                string advanceId = selectedRow.Cells["AdvanceID"].Value.ToString();
                string supplier = selectedRow.Cells["Supplier"].Value.ToString();

                var result = MessageBox.Show($"Delete advance {advanceId} for {supplier}?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    advancesGrid.Rows.Remove(selectedRow);
                    MessageBox.Show("Advance deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select an advance to delete", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            advancesGrid.Rows.Clear();
            LoadAdvances();
            MessageBox.Show("Advances list refreshed!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                foreach (DataGridViewRow row in advancesGrid.Rows)
                {
                    bool visible = false;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                        {
                            visible = true;
                            break;
                        }
                    }
                    row.Visible = visible;
                }
            }
            else
            {
                foreach (DataGridViewRow row in advancesGrid.Rows)
                {
                    row.Visible = true;
                }
            }
        }

        // Public method to refresh data
        public void RefreshData()
        {
            LoadAdvances();
        }
    }
}

