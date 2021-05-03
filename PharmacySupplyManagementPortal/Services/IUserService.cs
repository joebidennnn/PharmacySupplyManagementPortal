using PharmacySupplyManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Services
{
    public interface IUserService
    {
        public Task<HttpResponseMessage> Login(User credentials);
    }
}
