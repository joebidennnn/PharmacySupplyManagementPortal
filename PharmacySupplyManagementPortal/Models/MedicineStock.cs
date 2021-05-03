using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Models
{
    public class MedicineStock
    {
        [Key]
        public string Name { get; set; }
        public string ChemicalComposition { get; set; }
        public string TargetAilment { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public int NumberOfTabletsInStock { get; set; }

    }
}
