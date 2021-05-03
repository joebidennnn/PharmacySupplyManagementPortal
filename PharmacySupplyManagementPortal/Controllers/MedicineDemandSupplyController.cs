using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacySupplyManagementPortal.Services;
using PharmacySupplyManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;


namespace PharmacySupplyManagementPortal.Controllers
{
    public class MedicineDemandSupplyController:Controller
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(MedicineDemandSupplyController));
        private readonly IDemandSupplyService _demandService;
        private HttpResponseMessage _response;
        private string _token;

        public MedicineDemandSupplyController(IDemandSupplyService demandService)
        {
            _demandService = demandService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.Session.GetString("token") == null)
                {
                    _log.Info("token not found");
                    return Content("Un Authorized");
                }
                else
                {
                    _log.Info("Displaying Schedule input page");
                    var stocks =await _demandService.GetMedicine();
                    if(stocks!=null&& stocks.Count > 0)
                    return Ok(stocks);
                    return Content("Not found");
                }
            }
            catch (Exception e)
            {
                _log.Info("Error in DemandController while displaying input page for user : " + HttpContext.Session.GetString("userName") + " - " + e.Message);
                return Content("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(IEnumerable<MedicineDemand> demands)
        {
            try
            {   
                if (HttpContext.Session.GetString("token") == null)
                {
                    _log.Info("token not found");
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    _token = HttpContext.Session.GetString("token");
                    //changed below
                    _response = await _demandService.GetSupply(demands.ToList(), _token);
                    if (_response.IsSuccessStatusCode)
                    {
                        _log.Info("Supply received");
                        var result = await _response.Content.ReadAsStringAsync();
                        TempData["supply"] = result;
                        return RedirectToAction("DisplaySupply");
                    }
                    else if (_response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _log.Error("error while getting supply");
                        return Content("NoSupply");
                    }
                    else if (_response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return Content("Unauthorized");
                    }
                    else
                    {
                        return Content("Error");
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Error while getting demand list in DemandController for user : " + HttpContext.Session.GetString("userName") + " - " + e.Message);
                return View("Error");
            }
        }

        public IActionResult DisplaySupply()
        {
            try
            {
                if (HttpContext.Session.GetString("token") == null)
                {
                    _log.Info("token not found");
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    _log.Info("Displaying Supply List");
                    List<PharmacyMedicineSupply> supplies = JsonConvert.DeserializeObject<List<PharmacyMedicineSupply>>(TempData["supply"].ToString());
                    return Ok(supplies.OrderBy(s => s.PharmacyName));
                }
            }
            catch (Exception e)
            {
                _log.Error("Error in Demand Controller while displaying Supply for user : " + HttpContext.Session.GetString("userName") + " - " + e.Message);
                return Content("Error");
            }
        }
    }
}
