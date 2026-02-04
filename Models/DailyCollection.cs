using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GangasiriTeaFactoryBilling.Models
{
    public class DailyCollection
    {
        public int CollectionID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; } // For display
        public DateTime CollectionDate { get; set; }
        public decimal Weight { get; set; }
        public int Rate { get; set; }
        public string IsTransportAdd { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
