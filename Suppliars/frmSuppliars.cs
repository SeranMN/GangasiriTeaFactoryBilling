using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.Models;
using GangasiriTeaFactoryBilling.Suppliars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling
{
        
    public partial class frmSuppliars : Form
    {
        private DataGridView suppliersGrid;
        private Button btnAddSupplier;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnExport;
        private Button btnRefresh;
        private ComboBox cmbStatusFilter;
        public frmSuppliars()
        {
            InitializeComponent();
            InitializeStatusFilter();
        }

        private void InitializeStatusFilter()
        {
            cmbStatusFilter.Items.Add("Active Suppliers");
            cmbStatusFilter.Items.Add("Inactive Suppliers");
            cmbStatusFilter.Items.Add("All Suppliers");
            cmbStatusFilter.SelectedIndex = 0; // Default to Active
            cmbStatusFilter.SelectedIndexChanged += (s, e) => LoadSuppliers();
        }
        

        private void SuppliersGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Paint action buttons
            if (e.ColumnIndex == suppliersGrid.Columns["Actions"].Index && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var cellBounds = e.CellBounds;
                var buttonBounds = new Rectangle(cellBounds.X + (cellBounds.Width - 80) / 2, cellBounds.Y + 5, 80, cellBounds.Height - 10);

                // Determine Status
                string status = suppliersGrid.Rows[e.RowIndex].Cells["Status"].Value?.ToString() ?? "";
                bool isActive = status.Contains("Active");
                
                string btnText = isActive ? "Delete" : "Restore";
                Color btnColor = isActive ? Color.FromArgb(244, 67, 54) : Color.FromArgb(76, 175, 80); // Red or Green

                using (Brush brush = new SolidBrush(btnColor))
                {
                    e.Graphics.FillRectangle(brush, buttonBounds);
                }
                TextRenderer.DrawText(e.Graphics, btnText,
                    new Font("Segoe UI", 9, FontStyle.Bold),
                    buttonBounds, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        private void SuppliersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == suppliersGrid.Columns["Actions"].Index)
            {
                var cellBounds = suppliersGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                var point = suppliersGrid.PointToClient(Cursor.Position);
                // Re-calculate bounds to match painting
                var buttonBounds = new Rectangle(cellBounds.X + (cellBounds.Width - 80) / 2, cellBounds.Y + 5, 80, cellBounds.Height - 10);

                if (buttonBounds.Contains(point))
                {
                    string supplierIdStr = suppliersGrid.Rows[e.RowIndex].Cells["SupplierID"].Value.ToString();
                    int supplierId = int.Parse(supplierIdStr);
                    string supplierName = suppliersGrid.Rows[e.RowIndex].Cells["SupplierName"].Value.ToString();
                    
                    string status = suppliersGrid.Rows[e.RowIndex].Cells["Status"].Value?.ToString() ?? "";
                    bool isActive = status.Contains("Active");

                    if (isActive)
                    {
                        var result = MessageBox.Show($"Are you sure you want to deactivate (soft delete) supplier: {supplierName}? They will be moved to 'Inactive Suppliers'.",
                            "Confirm Deactivate", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            DataAccess.DeleteSupplier(supplierId); // This is now Soft Delete
                            MessageBox.Show("Supplier deactivated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSuppliers(); // Refresh
                        }
                    }
                    else
                    {
                         var result = MessageBox.Show($"Activate supplier: {supplierName}?",
                            "Confirm Activate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            DataAccess.ActivateSupplier(supplierId);
                            MessageBox.Show("Supplier activated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSuppliers(); // Refresh
                        }
                    }
                }
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                suppliersGrid.Rows.Clear();
                var supliars = DataAccess.GetAllSuppliers();
                
                // Get filter, default to Active if null
                string filter = cmbStatusFilter.SelectedItem?.ToString() ?? "Active Suppliers";

                foreach (var supplier in supliars)
                {
                    // Normalize status
                    string status = string.IsNullOrEmpty(supplier.Status) ? "Active" : supplier.Status;
                    bool isActive = status.Equals("Active", StringComparison.OrdinalIgnoreCase);

                    bool show = false;
                    if (filter == "All Suppliers") 
                    {
                        show = true;
                    }
                    else if (filter == "Active Suppliers") 
                    {
                        show = isActive;
                    }
                    else if (filter == "Inactive Suppliers") 
                    {
                        show = !isActive;
                    }

                    if (show)
                    {
                        string statusIcon = isActive ? "✅ Active" : "❌ Inactive";
                        suppliersGrid.Rows.Add(supplier.SupplierID, supplier.SupplierNumber, supplier.SupplierName, supplier.Telephone, supplier.LineName, statusIcon, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event Handlers
        private void BtnAddSupplier_Click(object sender, EventArgs e)
        {
            using (frmAddSupplier addForm = new frmAddSupplier())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh grid
                    LoadSuppliers();
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                // TODO: Implement search logic
                MessageBox.Show($"Searching for: {searchText}", "Search",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx|CSV Files|*.csv",
                Title = "Export Suppliers",
                FileName = $"Suppliers_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"Exported to: {saveFileDialog.FileName}", "Export Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            suppliersGrid.Rows.Clear();
            LoadSuppliers();
            MessageBox.Show("Suppliers list refreshed!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private frmSupplierDetails currentDetailsForm;

        private void SuppliersGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string supplierId = suppliersGrid.Rows[e.RowIndex].Cells["SupplierID"].Value.ToString();
                string supplierName = suppliersGrid.Rows[e.RowIndex].Cells["SupplierName"].Value.ToString();

                ShowSupplierDetailsInSameWindow(supplierId, supplierName);
            }
        }

        private void ShowSupplierDetailsInSameWindow(string supplierId, string supplierName)
        {
            // Hide the current content
            suppliersGrid.Visible = false;

            // Create and show the details form inside this window
            currentDetailsForm = new frmSupplierDetails(supplierId, supplierName);
            currentDetailsForm.TopLevel = false; // Important: This makes it not a top-level window
            currentDetailsForm.FormBorderStyle = FormBorderStyle.None;
            currentDetailsForm.Dock = DockStyle.Fill;

            // Add form to current window
            this.Controls.Add(currentDetailsForm);
            currentDetailsForm.BringToFront();
            currentDetailsForm.Show();

            // Add back button to details form or handle in details form
            AddBackButtonToDetailsForm();
        }

        private void AddBackButtonToDetailsForm()
        {
            if (currentDetailsForm != null)
            {
                Button btnBack = new Button
                {
                    Text = "← Back to Suppliers",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Size = new Size(160, 35),
                    Location = new Point(50, 750),
                    BackColor = Color.FromArgb(0, 122, 204),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnBack.FlatAppearance.BorderSize = 0;
                btnBack.Click += BtnBackFromDetails_Click;

                // Add to the details form
                currentDetailsForm.Controls.Add(btnBack);
                btnBack.BringToFront();
            }
        }

        private void BtnBackFromDetails_Click(object sender, EventArgs e)
        {
            // Remove the details form
            if (currentDetailsForm != null)
            {
                this.Controls.Remove(currentDetailsForm);
                currentDetailsForm.Dispose();
                currentDetailsForm = null;
            }

            // Show the suppliers grid again
            suppliersGrid.Visible = true;
            suppliersGrid.BringToFront();
        }

        // Optional: Handle form closing
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (currentDetailsForm != null)
            {
                currentDetailsForm.Dispose();
            }
            base.OnFormClosing(e);
        }

    }
}
