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

        private void LoadSampleData()
        {
            linesGrid.Rows.Clear();
            linesGrid.Rows.Add("L-001", "Line 1", "Main collection line for northern area", "150.00", "✅ Active");
            linesGrid.Rows.Add("L-002", "Line 2", "Secondary line for southern region", "200.00", "✅ Active");
            linesGrid.Rows.Add("L-003", "Line 3", "Premium line for high-quality leaves", "250.00", "✅ Active");
            linesGrid.Rows.Add("L-004", "Line 4", "New experimental line", "100.00", "⚠️ Inactive");
        }

        private void LoadLines()
        {
            // TODO: Load lines from database
            Console.WriteLine("Loading lines...");
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
                    editForm.Text = "Edit Line - " + lineId;
                    editForm.IsEditMode = true;
                    editForm.LineId = lineId;
                    editForm.LoadLineData(lineName, description, transportFee);

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
            LoadSampleData();
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
