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

namespace GangasiriTeaFactoryBilling.TeaRates
{
    public partial class frmTeaRates : Form
    {
        public frmTeaRates()
        {
            InitializeComponent();
            LoadRates();
        }

        private void LoadRates()
        {
            ratesGrid.Rows.Clear();
            try
            {
                var rates = DataAccess.GetAllTeaRates();
                foreach (var rate in rates)
                {
                    string status = DateTime.Now.Date < rate.ValidFrom ? "⏳ Future" :
                                    (DateTime.Now.Date > rate.ValidTo ? "❌ Expired" : "✅ Active");
                    ratesGrid.Rows.Add(
                        rate.RateID,
                        rate.Rate.ToString("N2"),
                        rate.ValidFrom.ToString("yyyy-MM-dd"),
                        rate.ValidTo.ToString("yyyy-MM-dd"),
                        status,
                        rate.CreatedDate.ToString("yyyy-MM-dd HH:mm")
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Tea Rates: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event Handlers
        private void BtnAddRate_Click(object sender, EventArgs e)
        {
            using (frmAddTeaRates addForm = new frmAddTeaRates())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Add new rate to grid
                    var rateData = addForm.Result;
                    string newId = "RATE-" + (ratesGrid.Rows.Count + 1).ToString("D3");
                    

                    string status = DateTime.Now.Date < rateData.ValidFrom ? "⏳ Future" : "✅ Active";

                    ratesGrid.Rows.Add(
                        newId,
                        rateData.Rate.ToString("N2"),
                        rateData.ValidFrom.ToString("yyyy-MM-dd"),
                        rateData.ValidTo.ToString("yyyy-MM-dd"),
                        status,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                    );

                    MessageBox.Show("Tea rate added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (ratesGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = ratesGrid.SelectedRows[0];
                string rateId = selectedRow.Cells["RateID"].Value.ToString();
                decimal rate = decimal.Parse(selectedRow.Cells["Rate"].Value.ToString());
                DateTime validFrom = DateTime.Parse(selectedRow.Cells["ValidFrom"].Value.ToString());
                DateTime validTo = DateTime.Parse(selectedRow.Cells["ValidTo"].Value.ToString());

                using (frmAddTeaRates editForm = new frmAddTeaRates())
                {
                    editForm.SetEditMode(rateId, rate, validFrom, validTo);

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Update row
                        var rateData = editForm.Result;
                        
                        string status = DateTime.Now.Date < rateData.ValidFrom ? "⏳ Future" : "✅ Active";

                        selectedRow.Cells["Rate"].Value = rateData.Rate.ToString("N2");
                        selectedRow.Cells["ValidFrom"].Value = rateData.ValidFrom.ToString("yyyy-MM-dd");
                        selectedRow.Cells["ValidTo"].Value = rateData.ValidTo.ToString("yyyy-MM-dd");
                        selectedRow.Cells["Status"].Value = status;

                        MessageBox.Show("Tea rate updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a rate to edit", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ratesGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = ratesGrid.SelectedRows[0];
                string rateId = selectedRow.Cells["RateID"].Value.ToString();
                string rate = selectedRow.Cells["Rate"].Value.ToString();

                var result = MessageBox.Show($"Delete rate {rateId}: ₹{rate}/kg?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ratesGrid.Rows.Remove(selectedRow);
                    MessageBox.Show("Tea rate deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a rate to delete", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterStatus.SelectedIndex = 0;
            ratesGrid.Rows.Clear();
            LoadRates();
            MessageBox.Show("Rates list refreshed!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterStatus.SelectedIndex = 0;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            string filterStatus = cmbFilterStatus.SelectedIndex > 0 ? cmbFilterStatus.Text : "";

            foreach (DataGridViewRow row in ratesGrid.Rows)
            {
                bool visible = true;

                // Apply status filter
                if (visible && !string.IsNullOrEmpty(filterStatus))
                {
                    string rowStatus = row.Cells["Status"].Value?.ToString() ?? "";

                    if (filterStatus == "Active Only" && !rowStatus.Contains("Active"))
                        visible = false;
                    else if (filterStatus == "Expired Only" && !rowStatus.Contains("Expired"))
                        visible = false;
                }

                // Apply search filter
                if (visible && !string.IsNullOrEmpty(searchText))
                {
                    bool found = false;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                        {
                            found = true;
                            break;
                        }
                    }
                    visible = found;
                }

                row.Visible = visible;
            }
        }

        public void RefreshData()
        {
            LoadRates();
        }
    }
}
