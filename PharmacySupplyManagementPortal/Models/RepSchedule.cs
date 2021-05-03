using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Models
{
    public class RepSchedule
    {
        public string Name { get; set; }
        public string DocterName { get; set; }
        public string TreatmentAilment { get; set; }
        public string Medicine { get; set; }
        public string MettingSlot { get; set; }
        public DateTime DateOfMetting { get; set; }
        public string DocterContactNumber { get; set; }
    }
}
