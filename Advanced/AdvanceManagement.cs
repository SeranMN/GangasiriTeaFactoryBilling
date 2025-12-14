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
    public partial class AdvanceManagement : Form
    {
        public AdvanceManagement()
        {
            InitializeComponent();
            LoadSampleData();
            LoadAdvances();
        }

        private void LoadSampleData()
        {
            advancesGrid.Rows.Clear();
            advancesGrid.Rows.Add("ADV-001", "S-001 - Kamal Perera", "2024-11-25", "5000.00", "Monthly advance", "✅ Active");
            advancesGrid.Rows.Add("ADV-002", "S-002 - Sunil Fernando", "2024-11-24", "3000.00", "Emergency advance", "✅ Active");
            advancesGrid.Rows.Add("ADV-003", "S-004 - Nimal Rathnayake", "2024-11-20", "7500.00", "Festival advance", "⚠️ Deducted");
            advancesGrid.Rows.Add("ADV-004", "S-001 - Kamal Perera", "2024-11-18", "2500.00", "Weekly advance", "✅ Active");
            advancesGrid.Rows.Add("ADV-005", "S-005 - Sampath Bandara", "2024-11-15", "4000.00", "Medical advance", "✅ Active");
        }

        private void LoadAdvances()
        {
            // TODO: Load advances from database
            // Sample data
            advancesGrid.Rows.Clear();
            advancesGrid.Rows.Add(false, "ADV-001", "S-001 - Kamal Perera", "2024-11-25", "5000.00", "Monthly advance", "Active");
            advancesGrid.Rows.Add(false, "ADV-002", "S-002 - Sunil Fernando", "2024-11-24", "3000.00", "Emergency advance", "Active");
            advancesGrid.Rows.Add(false, "ADV-003", "S-004 - Nimal Rathnayake", "2024-11-20", "7500.00", "Festival advance", "Deducted");
        }

        // Event Handlers
        private void BtnAddAdvance_Click(object sender, EventArgs e)
        {
            using (frmAddAdvanced addForm = new frmAddAdvanced())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Add new advance to grid
                    var advanceData = addForm.Result;
                    string newId = "ADV-" + (advancesGrid.Rows.Count + 1).ToString("D3");

                    advancesGrid.Rows.Add(
                        newId,
                        advanceData.Supplier,
                        advanceData.Date.ToString("yyyy-MM-dd"),
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

                using (frmAddAdvanced editForm = new frmAddAdvanced())
                {
                    // Pre-populate form with selected data
                    editForm.Text = "Edit Advance - " + advanceId;

                    // TODO: Load advance data for editing
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Update row
                        var advanceData = editForm.Result;
                        selectedRow.Cells["Supplier"].Value = advanceData.Supplier;
                        selectedRow.Cells["Date"].Value = advanceData.Date.ToString("yyyy-MM-dd");
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
            LoadSampleData();
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

