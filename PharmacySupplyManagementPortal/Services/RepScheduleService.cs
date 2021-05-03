using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmacySupplyManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Services
{
    public class RepScheduleService : IRepScheduleService
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RepScheduleService));
        public async Task<IEnumerable<RepSchedule>> GetRepScheduleList(string token,DateTime ScheduleStartDate)
        {
            try
            {
                string startDate = ScheduleStartDate.ToString("MM-dd-yyyy");
                List<RepSchedule> ScheduleList = new List<RepSchedule>();
                string apiResponse;
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
                    var response = await httpClient.GetAsync("https://localhost:44339/api/RepSchedule?ScheduleStartDate=" + startDate);
                    if (response.IsSuccessStatusCode)
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        ScheduleList = JsonConvert.DeserializeObject<List<RepSchedule>>(apiResponse);
                        return ScheduleList;
                    }
                    else if (response.StatusCode==System.Net.HttpStatusCode.BadRequest)
                    {
                        _log.Debug(response.Content.ToString());
                        throw new ArgumentException(response.Content.ToString());
                        
                    }else
                    {
                        _log.Debug("Schedule not found");
                        return null;
                    }
                        
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
