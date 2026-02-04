using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GangasiriTeaFactoryBilling.Invoice
{
    public static class InvoiceService
    {
        public static async Task<(bool Success, string Message, MonthlyInvoice Invoice)> GenerateOrUpdateInvoiceAsync(int year, int month, int supplierId, string existingInvoiceId = null)
        {
            try
            {
                // Validate Rate
                var rate = DataAccess.GetTeaRateByMonth(year, month);
                if (rate == null)
                {
                    return (false, $"No tea rate found for {month}/{year}.", null);
                }

                // Calculate Values
                var totalWeight = DataAccess.GetSupplierTotalWeightByMonth(supplierId, year, month);
                var totalAdvance = DataAccess.GetSupplierAdvanceTotalByMonth(supplierId, year, month);
                var supplier = DataAccess.GetSupplierById(supplierId.ToString());
                
                if (supplier == null)
                {
                    return (false, "Supplier not found.", null);
                }

                var tranportAddedWeight = DataAccess.GetSupplierTotalWeightWithTransportByMonth(supplierId, year, month);
                var tranportAllowanceWeight = DataAccess.GetSupplierTotalWeightWithoutTransportByMonth(supplierId, year, month);
                
                var totalDeductions = supplier.DueAmount + totalAdvance + (tranportAddedWeight * supplier.TransportFee);
                var totalAmount = totalWeight * rate.Rate + tranportAllowanceWeight * supplier.TransportFee;
                var netAmount = totalAmount - totalDeductions;

                MonthlyInvoice invoice;

                if (string.IsNullOrEmpty(existingInvoiceId))
                {
                    // New Invoice
                    string newInvoiceId = DataAccess.GenerateInvoiceNumber(year);
                    invoice = new MonthlyInvoice
                    {
                        CreatedDate = DateTime.Now.Date,
                        InvoiceID = newInvoiceId,
                        SupplierID = supplierId,
                        RatePerKg = rate.Rate,
                        PreviousBalance = supplier.DueAmount,
                        TotalAdvances = totalAdvance,
                        TotalWeight = totalWeight,
                        InvoiceMonth = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                        Year = year,
                        TransportFee = tranportAddedWeight * supplier.TransportFee,
                        TransportAllowance = tranportAllowanceWeight * supplier.TransportFee,
                        TotalDeductions = totalDeductions,
                        TotalAmount = totalAmount,
                        NetAmount = netAmount,
                        Status = "Pending"
                    };
                    DataAccess.GenerateInvoice(invoice);
                }
                else
                {
                    // Update Existing Invoice
                    invoice = DataAccess.GetInvoicesByInvoiceNumber(existingInvoiceId).FirstOrDefault();
                    if (invoice != null)
                    {
                        invoice.TotalWeight = totalWeight;
                        invoice.RatePerKg = rate.Rate;
                        invoice.TotalAdvances = totalAdvance;
                        invoice.PreviousBalance = supplier.DueAmount;
                        invoice.TransportFee = tranportAddedWeight * supplier.TransportFee;
                        invoice.TransportAllowance = tranportAllowanceWeight * supplier.TransportFee;
                        invoice.TotalDeductions = totalDeductions;
                        invoice.TotalAmount = totalAmount;
                        invoice.NetAmount = netAmount;
                        
                        DataAccess.UpdateInvoice(invoice);
                    }
                    else
                    {
                        return (false, "Invoice to update not found.", null);
                    }
                }

                // Handle Negative Balance (Supplier Debt)
                if (invoice.NetAmount < 0)
                {
                    // Logic to update supplier due amount could be here or separated
                    // For now, mirroring existing logic which updates supplier directly
                    // Logic to update supplier due amount
                    DataAccess.UpdateSupplierDueAmount(supplierId, invoice.NetAmount);
                }

                return (true, "Invoice generated/updated successfully.", invoice);

            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", null);
            }
        }
    }
}
