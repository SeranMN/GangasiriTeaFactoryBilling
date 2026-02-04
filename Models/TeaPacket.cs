using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GangasiriTeaFactoryBilling.Models
{
    public class TeaPacket
    {
        public string Id { get; set; }
        public string SupllierID { get; set; }
        public string SupplierName { get; set; }
        public DateTime Date {  get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal Total {  get; set; }
    }
}
