using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GangasiriTeaFactoryBilling.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierNumber { get; set; }
        public string SupplierName { get; set; }
        public string Telephone { get; set; }
        public string LineID { get; set; }
        public string LineName { get; set; } 
        public decimal TransportFee { get; set; } 
        public string Address { get; set; }
        public decimal DueAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
