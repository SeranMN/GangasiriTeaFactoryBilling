using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.TeaCollection;
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
using static System.Net.Mime.MediaTypeNames;

namespace GangasiriTeaFactoryBilling.Suppliars
{
    public partial class frmSupplierDetails : Form
    {
        private string supplierId;
        private string supplierName;
        private string TotalWeightForMonth;
        private Supplier supplier;
        private TabControl tabControl;
        private DataGridView dailyCollectionGrid;
        private DataGridView invoiceGrid;
        private DataGridView advancedGrid;
        private DataGridView teaPacketGrid;
        
        // Editable Controls
        private TextBox txtSupplierName;
        private TextBox txtAddress;
        private TextBox txtTelephone;
        private ComboBox cmbLine; // Added Line selection
        private List<LineItem> lineItems = new List<LineItem>(); // Store lines

        
        // Financial Summaries
        private string TotalAdvanceForMonth;
        private string TotalTeaPacketForMonth;
        public frmSupplierDetails(string supplierId, string supplierName)
        {
            this.supplierId = supplierId;
            this.supplierName = supplierName;
            this.Text = $"Supplier Details - {supplierName} ({supplierId})";
            LoadSupplierData();
            LoadLines(); // Load lines for the combobox
            CalculateFinancialSummaries();
            InitializeComponent();
        }

        private void LoadLines()
        {
            try
            {
                var lines = DataAccess.GetAllLines();
                lineItems.Clear();
                foreach (var line in lines)
                {
                    lineItems.Add(new LineItem
                    {
                        LineID = line.LineID,
                        DisplayText = $"{line.LineName} (Transport: LKR{line.TransportFee:F2})"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading lines: {ex.Message}");
            }
        }

        private class LineItem
        {
            public string LineID { get; set; }
            public string DisplayText { get; set; }
            public override string ToString() => DisplayText;
        }


        private void LoadSupplierData()
        {
            try
            {
                 supplier = DataAccess.GetSupplierById(supplierId);

            }
            catch
            {
                MessageBox.Show("Cannot Load Supplier");
            }
        }

        private void LoadDailyCollections()
        {
            dailyCollectionGrid.Rows.Clear();
            try
            {
                var collections = DataAccess.GetSupplierCollections(int.Parse(supplierId));
                foreach (var collection in collections)
                {
                    dailyCollectionGrid.Rows.Add(collection.CollectionDate, collection.CollectionID, collection.Weight, collection.IsTransportAdd, collection.Notes);
                }

                TotalWeightForMonth = collections.Where(c => c.CollectionDate.Year == DateTime.Now.Year && c.CollectionDate.Month == DateTime.Now.Month).Sum(c => c.Weight).ToString();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Collections: {ex.Message}", "Error",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void LoadInvoices()
        {
            invoiceGrid.Rows.Clear();
            try
            {
                var invoices = DataAccess.GetInvoicesBySup(supplierId);
                foreach (var invoice in invoices)
                {
                    invoiceGrid.Rows.Add(invoice.InvoiceID, invoice.InvoiceMonth, invoice.TotalWeight, invoice.NetAmount);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Invoices: {ex.Message}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void LoadAdvances()
        {
            try
            {
                var advances = DataAccess.GetSupplierAdvances(int.Parse(supplierId));
                advancedGrid.Rows.Clear();
                foreach (var advance in advances)
                {
                    advancedGrid.Rows.Add(advance.AdvanceID, advance.AdvanceDate.ToString("yyyy-MM-dd"), advance.Amount.ToString("F2"),advance.Description);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Advances: {ex.Message}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            }

        private void LoadTeaPacketSales()
        {
            teaPacketGrid.Rows.Clear();
            try
            {
                var tea = DataAccess.GetTeaPacketsBySup(supplierId);
                foreach (var item in tea)
                {
                    teaPacketGrid.Rows.Add(item.Date,item.Price,item.Qty,item.Total);
                }
            }
            catch
            {
                throw;
            }
        }

        private void CalculateFinancialSummaries()
        {
            try
            {
                // Calculate Total Advances for current month
                var advances = DataAccess.GetSupplierAdvances(int.Parse(supplierId));
                decimal totalAdv = advances
                    .Where(a => a.AdvanceDate.Year == DateTime.Now.Year && a.AdvanceDate.Month == DateTime.Now.Month)
                    .Sum(a => a.Amount);
                TotalAdvanceForMonth = "LKR " + totalAdv.ToString("N2");

                // Calculate Tea Packet Sales for current month
                var teaPackets = DataAccess.GetTeaPacketsBySup(supplierId);
                decimal totalTea = teaPackets
                    .Where(t => t.Date.Year == DateTime.Now.Year && t.Date.Month == DateTime.Now.Month)
                    .Sum(t => t.Total);
                TotalTeaPacketForMonth = "LKR " + totalTea.ToString("N2");
            }
            catch (Exception ex)
            {
                // Fallback in case of error
                TotalAdvanceForMonth = "LKR 0.00";
                TotalTeaPacketForMonth = "LKR 0.00";
                MessageBox.Show($"Error calculating financials: {ex.Message}");
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
             try
            {
                if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
                {
                    MessageBox.Show("Supplier Name is required.");
                    return;
                }

                // Update supplier object properties
                supplier.SupplierName = txtSupplierName.Text;
                supplier.Address = txtAddress.Text;
                supplier.Telephone = txtTelephone.Text;
                
                if (cmbLine.SelectedItem is LineItem selectedLine)
                {
                    supplier.LineID = selectedLine.LineID;
                }
                
                // Assuming status logic remains same (or we can add status dropdown later if needed)
                // Using existing supplier object which has ID, etc.
                
                bool result = DataAccess.UpdateSupplier(supplier);
                if (result)
                {
                    MessageBox.Show("Supplier details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Update the window title too
                    this.Text = $"Supplier Details - {supplier.SupplierName} ({supplierId})";
                }
                else
                {
                    MessageBox.Show("Failed to update supplier details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BtnAddCollection_Click(object sender, EventArgs e)
        {
            using (AddCollection addForm = new AddCollection(int.Parse(supplierId)))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadDailyCollections();
                    CalculateFinancialSummaries();
                    MessageBox.Show("Daily collection added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void BtnNewInvoice_Click(object sender, EventArgs e)
        {
            using (var form = new GangasiriTeaFactoryBilling.Invoice.SelectInvoicePeriodForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        var result = await GangasiriTeaFactoryBilling.Invoice.InvoiceService.GenerateOrUpdateInvoiceAsync(
                            form.SelectedYear, 
                            form.SelectedMonth, 
                            int.Parse(supplierId));

                        if (result.Success)
                        {
                            MessageBox.Show("Invoice generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadInvoices(); // Refresh grid
                        }
                        else
                        {
                             MessageBox.Show($"Failed to generate invoice: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
        }


        private void InvoiceGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = invoiceGrid.Rows[e.RowIndex];
                var invoiceId = row.Cells["InvoiceNo"].Value?.ToString();

                if (!string.IsNullOrEmpty(invoiceId))
                {
                    using (var detailsForm = new GangasiriTeaFactoryBilling.Invoice.frmInvoiceDetails(invoiceId))
                    {
                        detailsForm.ShowDialog();
                    }
                }
            }
        }

        private void DailyCollectionGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dailyCollectionGrid.Rows[e.RowIndex];
                // Columns: Date, CollectionID, Weight, Transport, Remarks
                
                if (row.Cells["CollectionID"].Value == null) return;
                
                string collectionId = row.Cells["CollectionID"].Value.ToString();
                if (DateTime.TryParse(row.Cells["Date"].Value.ToString(), out DateTime date) &&
                    decimal.TryParse(row.Cells["Weight"].Value.ToString(), out decimal weight))
                {
                    string transport = row.Cells["Transport"].Value?.ToString() ?? "No";
                    string notes = row.Cells["Remarks"].Value?.ToString() ?? "";

                    using (AddCollection editForm = new AddCollection(int.Parse(supplierId)))
                    {
                        editForm.IsEditMode = true;
                        editForm.CollectionId = collectionId;
                        editForm.LoadCollectionData(int.Parse(supplierId), date, weight, transport, notes);
                        
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadDailyCollections();
                            CalculateFinancialSummaries();
                        }
                    }
                }
            }
        }

        private void AdvancedGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = advancedGrid.Rows[e.RowIndex];
                // Columns: AdvanceNo, Date, Amount, Purpose, Status, Balance
                
                if (row.Cells["AdvanceNo"].Value == null) return;

                // AdvanceNo might be like "ADV-001", but update requires ID. 
                // Wait, db uses int ID. Grid shows AdvanceID (from LoadAdvances: advance.AdvanceID). 
                // Let's check LoadAdvances again.
                // LoadAdvances adds: advance.AdvanceID, advance.AdvanceDate... 
                // So column 0 is ID (int/string).
                
                string advanceId = row.Cells["AdvanceNo"].Value.ToString();
                
                if (DateTime.TryParse(row.Cells["Date"].Value.ToString(), out DateTime date) &&
                    decimal.TryParse(row.Cells["Amount"].Value.ToString(), out decimal amount))
                {
                     string description = row.Cells["Purpose"].Value?.ToString() ?? "";

                     using (GangasiriTeaFactoryBilling.Advanced.frmAddAddvanced editForm = new GangasiriTeaFactoryBilling.Advanced.frmAddAddvanced(int.Parse(supplierId)))
                     {
                         editForm.IsEditMode = true;
                         editForm.AdvanceId = advanceId;
                         // Load data
                         editForm.LoadAdvanceData(supplierName, date, amount, description);

                         if (editForm.ShowDialog() == DialogResult.OK)
                         {
                             // Refresh
                             LoadAdvances();
                             CalculateFinancialSummaries();
                         }
                     }
                }
            }
        }

        private void DeleteCollectionItem_Click(object sender, EventArgs e)
        {
            if (dailyCollectionGrid.SelectedRows.Count > 0)
            {
                var row = dailyCollectionGrid.SelectedRows[0];
                string collectionId = row.Cells["CollectionID"].Value?.ToString();

                if (!string.IsNullOrEmpty(collectionId))
                {
                    if (MessageBox.Show("Are you sure you want to delete this collection?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (DataAccess.DeleteDailyCollection(collectionId))
                        {
                            MessageBox.Show("Collection deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDailyCollections();
                            CalculateFinancialSummaries();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete collection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void DeleteAdvanceItem_Click(object sender, EventArgs e)
        {
            if (advancedGrid.SelectedRows.Count > 0)
            {
                var row = advancedGrid.SelectedRows[0];
                string advanceId = row.Cells["AdvanceNo"].Value?.ToString(); // AdvanceNo is likely AdvanceID

                if (!string.IsNullOrEmpty(advanceId) && int.TryParse(advanceId, out int id))
                {
                    if (MessageBox.Show("Are you sure you want to delete this advance?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (DataAccess.DeleteAdvance(id))
                        {
                            MessageBox.Show("Advance deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadAdvances();
                            CalculateFinancialSummaries();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete advance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

    }
}

