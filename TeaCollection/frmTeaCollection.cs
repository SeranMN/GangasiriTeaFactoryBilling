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
    public partial class frmTeaCollection : Form
    {
        public frmTeaCollection()
        {
            InitializeComponent();
        }

        private void LoadSampleData()
        {
            collectionGrid.Rows.Clear();
            collectionGrid.Rows.Add("COL-001", "S-001 - Kamal Perera", "2024-11-25", "125.50", "85.00", "150.00", "10,817.50", "Morning collection");
            collectionGrid.Rows.Add("COL-002", "S-002 - Sunil Fernando", "2024-11-25", "98.75", "85.00", "0.00", "8,393.75", "");
            collectionGrid.Rows.Add("COL-003", "S-004 - Nimal Rathnayake", "2024-11-25", "156.25", "85.00", "200.00", "13,481.25", "With transport");
            collectionGrid.Rows.Add("COL-004", "S-001 - Kamal Perera", "2024-11-24", "115.00", "85.00", "0.00", "9,775.00", "Evening");
            collectionGrid.Rows.Add("COL-005", "S-005 - Sampath Bandara", "2024-11-24", "87.50", "85.00", "100.00", "7,537.50", "");
        }

        private void LoadFilterSuppliers()
        {
            cmbFilterSupplier.Items.Add("All Suppliers");
            cmbFilterSupplier.Items.Add("S-001 - Kamal Perera");
            cmbFilterSupplier.Items.Add("S-002 - Sunil Fernando");
            cmbFilterSupplier.Items.Add("S-003 - Anura Silva");
            cmbFilterSupplier.Items.Add("S-004 - Nimal Rathnayake");
            cmbFilterSupplier.Items.Add("S-005 - Sampath Bandara");
            cmbFilterSupplier.SelectedIndex = 0;
        }

        // Load actual data from database
        private void LoadCollections()
        {
            // TODO: Load collections from database
            Console.WriteLine("Loading collections...");
        }

        // Event Handlers
        private void BtnAddCollection_Click(object sender, EventArgs e)
        {
            using (AddCollection addForm = new AddCollection())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Add new collection to grid
                    var collectionData = addForm.Result;
                    string newId = "COL-" + (collectionGrid.Rows.Count + 1).ToString("D3");

                    collectionGrid.Rows.Add(
                        newId,
                        collectionData.Supplier,
                        collectionData.Date.ToString("yyyy-MM-dd"),
                        collectionData.Weight.ToString("N2"),
                        "85.00", // Current rate
                        collectionData.Transport.ToString("N2"),
                        collectionData.TotalAmount.ToString("N2"),
                        collectionData.Notes
                    );

                    MessageBox.Show("Collection added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (collectionGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = collectionGrid.SelectedRows[0];
                string collectionId = selectedRow.Cells["CollectionID"].Value.ToString();

                using (AddCollection editForm = new AddCollection())
                {
                    // Pre-populate form with selected data
                    editForm.Text = "Edit Collection - " + collectionId;

                    // TODO: Load collection data for editing
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Update row
                        var collectionData = editForm.Result;
                        selectedRow.Cells["Supplier"].Value = collectionData.Supplier;
                        selectedRow.Cells["Date"].Value = collectionData.Date.ToString("yyyy-MM-dd");
                        selectedRow.Cells["Weight"].Value = collectionData.Weight.ToString("N2");
                        selectedRow.Cells["Transport"].Value = collectionData.Transport.ToString("N2");
                        selectedRow.Cells["Total"].Value = collectionData.TotalAmount.ToString("N2");
                        selectedRow.Cells["Notes"].Value = collectionData.Notes;

                        MessageBox.Show("Collection updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a collection to edit", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (collectionGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = collectionGrid.SelectedRows[0];
                string collectionId = selectedRow.Cells["CollectionID"].Value.ToString();
                string supplier = selectedRow.Cells["Supplier"].Value.ToString();

                var result = MessageBox.Show($"Delete collection {collectionId} for {supplier}?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    collectionGrid.Rows.Remove(selectedRow);
                    MessageBox.Show("Collection deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a collection to delete", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            dtpFilterDate.Value = DateTime.Now;
            cmbFilterSupplier.SelectedIndex = 0;
            collectionGrid.Rows.Clear();
            LoadSampleData();
            MessageBox.Show("Collections list refreshed!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void DtpFilterDate_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilterSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            dtpFilterDate.Value = DateTime.Now;
            cmbFilterSupplier.SelectedIndex = 0;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            string filterDate = dtpFilterDate.Value.ToString("yyyy-MM-dd");
            string selectedSupplier = cmbFilterSupplier.SelectedIndex > 0 ? cmbFilterSupplier.Text : "";

            foreach (DataGridViewRow row in collectionGrid.Rows)
            {
                bool visible = true;

                // Apply date filter
                string rowDate = row.Cells["Date"].Value?.ToString();
                if (!string.IsNullOrEmpty(rowDate) && rowDate != filterDate)
                {
                    visible = false;
                }

                // Apply supplier filter
                if (visible && !string.IsNullOrEmpty(selectedSupplier))
                {
                    string rowSupplier = row.Cells["Supplier"].Value?.ToString();
                    if (rowSupplier != selectedSupplier)
                    {
                        visible = false;
                    }
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

        // Public method to refresh data
        public void RefreshData()
        {
            LoadCollections();
        }
    }
}

