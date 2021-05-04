using Newtonsoft.Json;
using PharmacySupplyManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Services
{
    public class DemandSupplyService: IDemandSupplyService
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(DemandSupplyService));

        readonly HttpClient httpClient = new HttpClient();
        public async Task<HttpResponseMessage> GetSupply(List<MedicineDemand> demands, string token)
        {
            try
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                StringContent content = new StringContent(JsonConvert.SerializeObject(demands), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://pharmacysupplymicroservice.azurewebsites.net/api/pharmacysupply", content);
                _log.Info("response received");
                return response;
            }
            catch (Exception e)
            {
                _log.Error("Error in DemandProvider while getting supply - " + e.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetStock()
        {
            try
            {
                var response = await httpClient.GetAsync("https://medicinestock.azurewebsites.net/api/MedicineStockInformation");
                _log.Info("response received");
                return response;
            }
            catch (Exception e)
            {
                _log.Error("Error in DemandProvider while getting stock - " + e.Message);
                throw;
            }
        }

        public List<MedicineDemand> GetDemand(List<MedicineStock> stocks)
        {
            try
            {
                List<MedicineDemand> demands = new List<MedicineDemand>();
                foreach (var stock in stocks)
                {
                    MedicineDemand demand = new MedicineDemand() { Medicine = stock.Name, DemandCount = 0 };
                    demands.Add(demand);
                }
                _log.Info("stock converted to demand");
                return demands;
            }
            catch (Exception e)
            {
                _log.Error("Error while converting Stock to Demand in DemandProvider" + e.Message);
                throw;
            }
        }
        public async Task<List<MedicineDemand>> GetMedicine()
        {
            try
            {
                var response = await httpClient.GetAsync("https://medicinestock.azurewebsites.net/api/MedicineStockInformation");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    List<MedicineStock> stocks = JsonConvert.DeserializeObject<List<MedicineStock>>(result);
                    var demand = GetDemand(stocks);
                    return demand;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception exception)
            {
                _log.Error(exception);
                throw;
            }
        } 
    }
}
