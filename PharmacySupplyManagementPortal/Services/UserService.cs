using Newtonsoft.Json;
using PharmacySupplyManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Services
{
    public class UserService : IUserService
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserService));
        public async Task<HttpResponseMessage> Login(User credentials)
        {
            try
            {
                string url = "http://52.143.18.227/api/Authentication";
                var httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                return response;
            }
            catch (Exception e)
            {
                _log.Info("Error in UserProvider while logging in for user : " + credentials.UserName + " - " + e.Message);
                throw;
            }
        }
    }
}
