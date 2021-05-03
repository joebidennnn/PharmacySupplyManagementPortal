﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacySupplyManagementPortal.Models;
using PharmacySupplyManagementPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Controllers
{
    public class RepScheduleController : Controller
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(RepScheduleController));
        public IRepScheduleService _repScheduleService { get; set; }
        public RepScheduleController(IRepScheduleService repScheduleService)
        {
            _repScheduleService = repScheduleService;
        }
        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetString("token") != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login","User");
                }
                    
            }
            catch(Exception exception)
            {
                _log.Error(exception);
                return RedirectToAction("Index", "User");
            }
        }
        public async Task<IActionResult> Schedule(DateTime ScheduleStartDate)
        {
            try
            {
                if (HttpContext.Session.GetString("token")!= null)
                {
                    string token = HttpContext.Session.GetString("token");
                    IEnumerable<RepSchedule> ScheduleList;
                    ScheduleList= await _repScheduleService.GetRepScheduleList(token, ScheduleStartDate);
                    if (ScheduleList == null)
                    {
                        return Content("not found");
                    }
                    else
                    {
                        return Ok(ScheduleList);
                    }

                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
                    
            }catch(ArgumentException argumentexception)
            {
                _log.Debug(argumentexception.Message);
                return Content("invalid date");
            }
            catch(Exception exception)
            {
                _log.Error(exception);
                return RedirectToAction("Login", "User");
            }            
        }
    }
}