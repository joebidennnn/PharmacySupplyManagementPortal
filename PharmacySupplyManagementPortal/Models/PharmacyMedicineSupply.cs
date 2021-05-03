using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Models
{
    public class PharmacyMedicineSupply
    {
        [DisplayName("Pharmacy")]
        public string PharmacyName { get; set; }
        [DisplayName("Medicine")]
        public string MedicineName { get; set; }
        [DisplayName("Count")]
        public int SupplyCount { get; set; }

    }
}
