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

namespace GangasiriTeaFactoryBilling.TeaCollection
{
    public partial class frmTeaCollection : Form
    {
        public frmTeaCollection()
        {
            InitializeComponent();
            LoadCollections();
            LoadFilterSuppliers();
        }


       

        private void LoadFilterSuppliers()
        {
            cmbFilterSupplier.Items.Clear();
            cmbFilterSupplier.Items.Add("All");
            try
            {
                var suppliers = DataAccess.GetAllSuppliers();
                foreach (var supplier in suppliers)
                {
                    cmbFilterSupplier.Items.Add(new SuppliarItem
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
        }

        // Load actual data from database
        private void LoadCollections()
        {
            collectionGrid.Rows.Clear();

            try
            {
                var collections = DataAccess.GetDailyCollections();
                foreach (var collection in collections)
                {
                    collectionGrid.Rows.Add(collection.CollectionID, collection.SupplierName, collection.CollectionDate, collection.Weight,collection.IsTransportAdd, collection.Notes,collection.SupplierID);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error loading Collections: {ex.Message}", "Error",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        collectionData.SupplierName,
                        collectionData.CollectionDate.ToString("yyyy-MM-dd"),
                        collectionData.Weight.ToString("N2"),
                        "85.00", // Current rate
                        collectionData.IsTransportAdd,
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
                int supplierId = int.Parse(selectedRow.Cells["SupplierID"].Value.ToString());
                DateTime Date =DateTime.Parse(selectedRow.Cells["Date"].Value.ToString());
                decimal weight = Convert.ToDecimal(selectedRow.Cells["Weight"].Value);
                string transport = selectedRow.Cells["Transport"].Value.ToString();
                string notes =selectedRow.Cells["Notes"].Value.ToString();

                using (AddCollection editForm = new AddCollection())
                {
                    // Pre-populate form with selected data
                    editForm.Text = "Edit Collection - " + collectionId;
                    editForm.IsEditMode = true;
                    editForm.CollectionId = collectionId;
                    editForm.LoadCollectionData(supplierId, Date,weight, transport, notes);

                    // TODO: Load collection data for editing
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Update row
                        var collectionData = editForm.Result;
                        selectedRow.Cells["Supplier"].Value = collectionData.SupplierName;
                        selectedRow.Cells["Date"].Value = collectionData.CollectionDate.ToString("yyyy-MM-dd");
                        selectedRow.Cells["Weight"].Value = collectionData.Weight.ToString("N2");
                        selectedRow.Cells["Transport"].Value = collectionData.IsTransportAdd;
                        //selectedRow.Cells["Total"].Value = collectionData.TotalAmount.ToString("N2");
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
                    try
                    {
                        DataAccess.DeleteDailyCollection(collectionId);
                        MessageBox.Show("Collection deleted successfully!", "Success",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting collection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                   
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
            LoadFilterSuppliers();
            txtSearch.Clear();
            dtpFilterDate.Value = DateTime.Now;
            cmbFilterSupplier.SelectedIndex = -1;
            collectionGrid.Rows.Clear();
            LoadCollections();
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

        private void ChkFilterDate_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            dtpFilterDate.Value = DateTime.Now;
            chkFilterDate.Checked = true;
            cmbFilterSupplier.Text = "";
            cmbFilterSupplier.SelectedIndex = -1;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            string filterDate = dtpFilterDate.Value.ToString("yyyy-MM-dd");
            string selectedSupplier = cmbFilterSupplier.Text;

            foreach (DataGridViewRow row in collectionGrid.Rows)
            {
                bool visible = true;

                // Apply date filter
                if (chkFilterDate.Checked)
                {
                    string rowDate = row.Cells["Date"].Value?.ToString();
                    if (!string.IsNullOrEmpty(rowDate) && rowDate != filterDate)
                    {
                        visible = false;
                    }
                }

                // Apply supplier filter
                if (visible && cmbFilterSupplier.SelectedItem is SuppliarItem selectedItem)
                {
                    if (row.Cells["SupplierID"].Value != null)
                    {
                        int rowSupplierId = int.Parse(row.Cells["SupplierID"].Value.ToString());
                        if (rowSupplierId != selectedItem.SuppliarId)
                        {
                            visible = false;
                        }
                    }
                }
                else if (visible && !string.IsNullOrEmpty(selectedSupplier) && selectedSupplier != "All")
                {
                     // Fallback for typed text if not selected from list (optional, but good for autocomplete)
                     string rowSupplier = row.Cells["Supplier"].Value?.ToString() ?? "";
                     if (!rowSupplier.ToLower().Contains(selectedSupplier.ToLower()))
                     {
                         // Strict matching might be better, but let's stick to ID logic primarily.
                         // If no item selected (e.g. user typed partial name), maybe we shouldn't filter strict?
                         // For now, if SelectedItem is null, we assume NO supplier filter unless we want text search.
                         // But usually dropdown autocomplete sets SelectedItem.
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

