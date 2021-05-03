using PharmacySupplyManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Services
{
    public interface IRepScheduleService
    {
        Task<IEnumerable<RepSchedule>> GetRepScheduleList(string token,DateTime ScheduleStartDate);
    }
}
