using GangasiriTeaFactoryBilling.db;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.Invoice
{
    public partial class frmInvoices : Form
    {
        private ImageList _actionIcons;

        public frmInvoices()
        {
            InitializeComponent();
            InitializeActionIcons();
        }

        private void InitializeActionIcons()
        {
            _actionIcons = new ImageList();
            _actionIcons.ImageSize = new Size(16, 16); // Set icon size
            _actionIcons.ColorDepth = ColorDepth.Depth32Bit;

            // Load icons from the img/icons directory
            string iconsPath = Path.Combine(Application.StartupPath, "img", "icons");

            try
            {
                _actionIcons.Images.Add("Edit", Image.FromFile(Path.Combine(iconsPath, "Edit.png")));
                _actionIcons.Images.Add("Print", Image.FromFile(Path.Combine(iconsPath, "print.png")));
                // Delete button removed as per requirement
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading icons: {ex.Message}", "Icon Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            // Implement search functionality
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Filter grid based on search term
            }
        }

        private void BtnGenerateInvoice_Click(object sender, EventArgs e)
        {
            // Open Generate Invoice popup
            frmGenerateInvoice generateForm = new frmGenerateInvoice();
            if (generateForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh grid after generation
                LoadInvoices();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // Implement export to Excel/PDF
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx|PDF Files|*.pdf",
                Title = "Export Invoices"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Export logic here
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadInvoices();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilterMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilterSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void InvoicesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get invoice number from selected row
                string invoiceNo = invoicesGrid.Rows[e.RowIndex].Cells["InvoiceNo"].Value?.ToString();
                if (!string.IsNullOrEmpty(invoiceNo))
                {
                    // Open invoice details screen (not popup)
                    frmInvoiceDetails detailsForm = new frmInvoiceDetails(invoiceNo);
                    detailsForm.Show();
                }
            }
        }

        private void InvoicesGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Paint action buttons (View, Print, Delete)
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 &&
                invoicesGrid.Columns[e.ColumnIndex].Name == "Actions")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                // Draw action buttons
                int buttonWidth = 40;
                int padding = 5;
                int x = e.CellBounds.Left + padding;

                // Edit button
                Rectangle viewRect = new Rectangle(x, e.CellBounds.Top + 5, buttonWidth, 25);
                using (Brush brush = new SolidBrush(Color.FromArgb(33, 150, 243)))
                {
                    e.Graphics.FillRectangle(brush, viewRect);
                }
                if (_actionIcons.Images.ContainsKey("Edit"))
                {
                    Image viewIcon = _actionIcons.Images["Edit"];
                    e.Graphics.DrawImage(viewIcon, viewRect.Left + (viewRect.Width - viewIcon.Width) / 2, viewRect.Top + (viewRect.Height - viewIcon.Height) / 2, viewIcon.Width, viewIcon.Height);
                }

                // Print button
                x += buttonWidth + padding;
                Rectangle printRect = new Rectangle(x, e.CellBounds.Top + 5, buttonWidth, 25);
                using (Brush brush = new SolidBrush(Color.FromArgb(76, 175, 80)))
                {
                    e.Graphics.FillRectangle(brush, printRect);
                }
                if (_actionIcons.Images.ContainsKey("Print"))
                {
                    Image printIcon = _actionIcons.Images["Print"];
                    e.Graphics.DrawImage(printIcon, printRect.Left + (printRect.Width - printIcon.Width) / 2, printRect.Top + (printRect.Height - printIcon.Height) / 2, printIcon.Width, printIcon.Height);
                }


                e.Handled = true;
            }
        }

        private void InvoicesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                invoicesGrid.Columns[e.ColumnIndex].Name == "Actions")
            {
                // Calculate which button was clicked
                int cellWidth = invoicesGrid.Columns["Actions"].Width;
                int clickX = invoicesGrid.PointToClient(Cursor.Position).X -
                             invoicesGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Left;

                int buttonWidth = 40;
                int padding = 5;

                string invoiceNo = invoicesGrid.Rows[e.RowIndex].Cells["InvoiceNo"].Value?.ToString();
                string yearStr = invoicesGrid.Rows[e.RowIndex].Cells["Year"].Value?.ToString();
                string monthName = invoicesGrid.Rows[e.RowIndex].Cells["Month"].Value?.ToString();
                
                // Assuming SupplierName is just display name and we need ID, but grid might not have it hidden.
                // We should probably get the full invoice object or ensure SupplierID is in the grid (hidden or otherwise).
                // Let's check LoadInvoices first. It only adds 5 columns. We need SupplierID for regeneration.
                // Since we don't have SupplierID in grid readily available as a column I will fetch the invoice first using InvoiceNo to get details.

                if (clickX >= padding && clickX <= padding + buttonWidth)
                {
                    // Edit (Regenerate) button clicked
                    if (MessageBox.Show("Do you want to regenerate this invoice with latest data?", "Confirm Regeneration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        RegenerateInvoice(invoiceNo);
                    }
                }
                else if (clickX >= padding * 2 + buttonWidth && clickX <= padding * 2 + buttonWidth * 2)
                {
                    // Print button clicked
                    PrintInvoice(invoiceNo);
                }
            }
        }

        #region Helper Methods

        private void LoadInvoices()
        {
            // Clear existing rows
            invoicesGrid.Rows.Clear();

            var repits = DataAccess.GetAllInvoices();

            foreach (var recpit in repits)
            {
                invoicesGrid.Rows.Add(
                    recpit.InvoiceID,
                    recpit.SupplierName,
                    recpit.InvoiceMonth,
                    recpit.Year,
                    recpit.NetAmount
                    
                    );
            }


            
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in invoicesGrid.Rows)
            {
                if (row.Cells["Amount"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["Amount"].Value);
                }
            }
            if (lblTotalAmount != null) lblTotalAmount.Text = $"Total: Rs. {total:N2}";
        }

        private void ApplyFilters()
        {
            string month = cmbFilterMonth.SelectedIndex > 0 && cmbFilterMonth.SelectedItem != null ? cmbFilterMonth.SelectedItem.ToString() : null;
            string supplier = cmbFilterSupplier.SelectedIndex > 0 && cmbFilterSupplier.SelectedItem != null ? cmbFilterSupplier.SelectedItem.ToString() : null;

            // Apply filtering logic
            foreach (DataGridViewRow row in invoicesGrid.Rows)
            {
                bool showRow = true;

                if (month != null && row.Cells["Month"].Value?.ToString() != month)
                    showRow = false;

                if (supplier != null && row.Cells["SupplierName"].Value?.ToString() != supplier)
                    showRow = false;

                row.Visible = showRow;
            }

            CalculateTotalAmount();
        }

        private async void RegenerateInvoice(string invoiceNo)
        {
            try
            {
                var invoices = DataAccess.GetInvoicesByInvoiceNumber(invoiceNo);
                if (invoices == null || invoices.Count == 0)
                {
                    MessageBox.Show("Invoice not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var invoice = invoices[0];
                int monthNum = DateTime.ParseExact(invoice.InvoiceMonth, "MMMM", CultureInfo.InvariantCulture).Month;

                var result = await InvoiceService.GenerateOrUpdateInvoiceAsync(invoice.Year, monthNum, invoice.SupplierID, invoiceNo);

                if (result.Success)
                {
                    MessageBox.Show("Invoice regenerated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInvoices(); // Refresh grid
                }
                else
                {
                    MessageBox.Show($"Regeneration failed: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error regenerating invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintInvoice(string invoiceNo)
        {
            // Print invoice logic
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // Print implementation
            }
        }

        #endregion
    }
}

