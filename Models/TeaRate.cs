using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GangasiriTeaFactoryBilling.Models
{
    public class TeaRate
    {
        public int RateID { get; set; }
        public decimal Rate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
