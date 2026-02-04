using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GangasiriTeaFactoryBilling.Models
{
    public class Line
    {
        public string LineID { get; set; }
        public string LineName { get; set; }
        public string Description { get; set; }
        public decimal TransportFee { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
