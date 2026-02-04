using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.Invoice
{
    public partial class frmGenerateInvoice : Form
    {
        public frmGenerateInvoice()
        {
            InitializeComponent();
        }

        private async Task GenerateInvoicesAsync(string month, List<SuppliarItem> suppliers, string year)
        {
            int totalSuppliers = suppliers.Count;
            int i = 0;
            int monthNumber = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;
            int SuccessCount = 0;
            int ErrorCount = 0;

            foreach (var sup in suppliers)
            {


                // Simulate work
                await Task.Delay(100);

                int yearInt = int.Parse(year);
                
                var result = await InvoiceService.GenerateOrUpdateInvoiceAsync(yearInt, monthNumber, sup.SuppliarId);

                if (!result.Success)
                {
                    ErrorCount++;
                    MessageBox.Show($"Error generating invoice for {sup.DisplayName}: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // If successfully generated, we might need to handle negative balance logic if it was done outside in previous code.
                    // The service now handles checking net amount < 0 and updating supplier.
                    // Previous code:
                    // if(invoice.NetAmount < 0) DataAccess.UpdateSupplier(...)
                    // This is now inside the service.
                    
                    SuccessCount++; // Track success if needed, though variable wasn't used much before.
                }
                // Update progress
                int progress = (int)((i + 1) / (double)totalSuppliers * 100);
                progressBar.Value = progress;
                lblStatus.Text = $"Generating invoice for {suppliers[i]}... ({i + 1}/{totalSuppliers})";
                i++;
            }

            progressBar.Value = 100;

            if (ErrorCount == 0)
            {
                lblStatus.Text = "All invoices generated successfully!";
            }
            else
            {
                lblStatus.Text = $"{totalSuppliers - ErrorCount} Invoice Generated and {ErrorCount} Invoice generation Failed";
            }
        }
    }
}
