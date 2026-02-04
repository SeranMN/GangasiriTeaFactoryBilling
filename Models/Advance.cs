using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GangasiriTeaFactoryBilling.Models
{
    public class Advance
    {
        public int AdvanceID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; } // For display
        public DateTime AdvanceDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
