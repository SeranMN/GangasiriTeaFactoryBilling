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

namespace GangasiriTeaFactoryBilling.LineManagements
{
    public partial class frmLines : Form
    {
        public frmLines()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            LoadLines();
        }

       

        private void LoadLines()
        {
            try
            {
                linesGrid.Rows.Clear();
                var lines = DataAccess.GetAllLines();

                foreach (var line in lines)
                {
                    string statusIcon = line.Status == "Active" ? "✅ Active" : "⚠️ Inactive";

                    linesGrid.Rows.Add(line.LineID, line.LineName, line.Description, line.TransportFee, statusIcon);
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"Error loading Lines: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event Handlers
        private void BtnAddLine_Click(object sender, EventArgs e)
        {
            using (frmAddLines addForm = new frmAddLines())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Add new line to grid
                    var lineData = addForm.Result;
                    string newId = "L-" + (linesGrid.Rows.Count + 1).ToString("D3");

                    linesGrid.Rows.Add(
                        newId,
                        lineData.LineName,
                        lineData.Description,
                        lineData.TransportFee.ToString("N2"),
                        "✅ Active"
                    );

                    MessageBox.Show("Line added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (linesGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = linesGrid.SelectedRows[0];
                string lineId = selectedRow.Cells["LineID"].Value.ToString();
                string lineName = selectedRow.Cells["LineName"].Value.ToString();
                string description = selectedRow.Cells["Description"].Value.ToString();
                decimal transportFee = decimal.Parse(selectedRow.Cells["TransportFee"].Value.ToString());

                using (frmAddLines editForm = new frmAddLines())
                {
                    editForm.SetEditMode(lineId, lineName, description, transportFee);

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Update row
                        var lineData = editForm.Result;
                        selectedRow.Cells["LineName"].Value = lineData.LineName;
                        selectedRow.Cells["Description"].Value = lineData.Description;
                        selectedRow.Cells["TransportFee"].Value = lineData.TransportFee.ToString("N2");

                        MessageBox.Show("Line updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a line to edit", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (linesGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = linesGrid.SelectedRows[0];
                string lineId = selectedRow.Cells["LineID"].Value.ToString();
                string lineName = selectedRow.Cells["LineName"].Value.ToString();

                var result = MessageBox.Show($"Delete line {lineId}: {lineName}?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    linesGrid.Rows.Remove(selectedRow);
                    DataAccess.DeleteLine(lineId);
                    MessageBox.Show("Line deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a line to delete", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            linesGrid.Rows.Clear();
            LoadLines();
            MessageBox.Show("Lines list refreshed!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                foreach (DataGridViewRow row in linesGrid.Rows)
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
                foreach (DataGridViewRow row in linesGrid.Rows)
                {
                    row.Visible = true;
                }
            }
        }

        public void RefreshData()
        {
            LoadLines();
        }
    }
}
