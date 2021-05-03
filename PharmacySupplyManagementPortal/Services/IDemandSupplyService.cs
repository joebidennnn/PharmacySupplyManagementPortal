using PharmacySupplyManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Services
{
    public interface IDemandSupplyService
    {
        public Task<HttpResponseMessage> GetStock();
        public Task<HttpResponseMessage> GetSupply(List<MedicineDemand> demands, string token);
        public List<MedicineDemand> GetDemand(List<MedicineStock> stocks);
        Task<List<MedicineDemand>> GetMedicine();
    }
}
