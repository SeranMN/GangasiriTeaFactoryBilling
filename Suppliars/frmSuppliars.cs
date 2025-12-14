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
        public frmSuppliars()
        {
            InitializeComponent();
        }
        private void AddSampleData()
        {
            // Add sample suppliers (replace with actual database data)
            suppliersGrid.Rows.Add("S-001", "Kamal Perera", "077-1234567", "Line 1", "✅ Active");
            suppliersGrid.Rows.Add("S-002", "Sunil Fernando", "071-2345678", "Line 2", "✅ Active", "Edit|Delete");
            suppliersGrid.Rows.Add("S-003", "Anura Silva", "072-3456789", "Line 3", "⚠️ Inactive", "Edit|Delete");
            suppliersGrid.Rows.Add("S-004", "Nimal Rathnayake", "076-4567890", "Line 1", "✅ Active", "Edit|Delete");
            suppliersGrid.Rows.Add("S-005", "Sampath Bandara", "075-5678901", "Line 2", "✅ Active", "Edit|Delete");
            suppliersGrid.Rows.Add("S-006", "Lalith Gunawardena", "078-6789012", "Line 1", "✅ Active", "Edit|Delete");
            suppliersGrid.Rows.Add("S-007", "Chaminda Peris", "070-7890123", "Line 3", "⚠️ Inactive");
        }

        private void SuppliersGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Paint action buttons
            if (e.ColumnIndex == suppliersGrid.Columns["Actions"].Index && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var cellBounds = e.CellBounds;
                var editBounds = new Rectangle(cellBounds.X + 10, cellBounds.Y + 5, 60, cellBounds.Height - 10);
                var deleteBounds = new Rectangle(cellBounds.X + 80, cellBounds.Y + 5, 60, cellBounds.Height - 10);

                // Draw Edit button
                using (Brush brush = new SolidBrush(Color.FromArgb(33, 150, 243)))
                {
                    e.Graphics.FillRectangle(brush, editBounds);
                }
                TextRenderer.DrawText(e.Graphics, "Edit",
                    new Font("Segoe UI", 9, FontStyle.Bold),
                    editBounds, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                // Draw Delete button
                using (Brush brush = new SolidBrush(Color.FromArgb(244, 67, 54)))
                {
                    e.Graphics.FillRectangle(brush, deleteBounds);
                }
                TextRenderer.DrawText(e.Graphics, "Delete",
                    new Font("Segoe UI", 9, FontStyle.Bold),
                    deleteBounds, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        private void SuppliersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == suppliersGrid.Columns["Actions"].Index)
            {
                var cellBounds = suppliersGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int x = e.ColumnIndex;
                int y = e.RowIndex;

                // Check if Edit button was clicked
                if (cellBounds.Contains(suppliersGrid.PointToClient(Cursor.Position)))
                {
                    var point = suppliersGrid.PointToClient(Cursor.Position);
                    var cellRect = suppliersGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                    var editBounds = new Rectangle(cellRect.X + 10, cellRect.Y + 5, 60, cellRect.Height - 10);
                    var deleteBounds = new Rectangle(cellRect.X + 80, cellRect.Y + 5, 60, cellRect.Height - 10);

                    if (editBounds.Contains(point))
                    {
                        // Edit supplier
                        string supplierId = suppliersGrid.Rows[e.RowIndex].Cells["SupplierID"].Value.ToString();
                        MessageBox.Show($"Editing supplier: {supplierId}", "Edit Supplier",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (deleteBounds.Contains(point))
                    {
                        // Delete supplier
                        string supplierId = suppliersGrid.Rows[e.RowIndex].Cells["SupplierID"].Value.ToString();
                        string supplierName = suppliersGrid.Rows[e.RowIndex].Cells["SupplierName"].Value.ToString();

                        var result = MessageBox.Show($"Are you sure you want to delete supplier: {supplierName}?",
                            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            suppliersGrid.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Supplier deleted successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void LoadSuppliers()
        {
            // TODO: Load suppliers from database
            Console.WriteLine("Loading suppliers...");
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
            AddSampleData();
            MessageBox.Show("Suppliers list refreshed!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
