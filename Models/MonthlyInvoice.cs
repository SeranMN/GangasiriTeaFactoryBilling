using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GangasiriTeaFactoryBilling.Models
{
    public class MonthlyInvoice
    {
        public string InvoiceID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string InvoiceMonth { get; set; }
        public int Year { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal TransportFee { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAdvances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal NetAmount { get; set; }
        public string Status { get; set; }

        public string LineName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
