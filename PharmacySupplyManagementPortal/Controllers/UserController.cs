using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmacySupplyManagementPortal.Models;
using PharmacySupplyManagementPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PharmacySupplyManagementPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserController));
        private readonly IUserService _userService;

        public UserController(IUserService userProvider)
        {
            _userService = userProvider;
        }

        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetString("token") == null)
                {
                    _log.Info("token not found");
                    return RedirectToAction("Login");
                }
                else
                {
                    _log.Info("Displaying Home Page");
                    ViewBag.UserName = HttpContext.Session.GetString("userName");
                    return View();
                }
            }
            catch (Exception e)
            {
                _log.Error("Error in UserController - " + e.Message);
                return Content("Server didn't respond");
            }
        }
        public IActionResult Login()
        {
            try
            {
                if (HttpContext.Session.GetString("token") != null)
                {
                    _log.Info("Already logged in");
                    return RedirectToAction("Index");
                }
                else
                {
                    _log.Info("Displaying Login Page");
                    return View();
                }
            }
            catch (Exception e)
            {
                _log.Error("Error in UserController while displaying login page - " + e.Message);
                return Content("Server didn't respond");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User credentials)
        {
            //return Content(credentials.UserName);
            try
            {
                HttpResponseMessage response = await _userService.Login(credentials);
                if (response.IsSuccessStatusCode)
                {
                    _log.Info("success response received");
                    var result = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<Models.JsonToken>(result);
                    HttpContext.Session.SetString("token", token.Token);
                    HttpContext.Session.SetString("userName", credentials.UserName);
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _log.Info("invalid username or password for user : " + credentials.UserName);
                    return Content("Invalid username/password");
                }
                else
                {
                    _log.Error("Error in Micro-service called for Authentication");
                    return View("Home","Index");
                }
            }
            catch (Exception e)
            {
                _log.Error("Error in UserController while logging in for user : " + credentials.UserName + " - " + e.Message);
                return Content("Server didn't respond");
            }
        }
        public IActionResult Logout()
        {
            try
            {
                _log.Info("Logging out user : " + HttpContext.Session.GetString("userName"));
                HttpContext.Session.Remove("token");
                HttpContext.Session.Remove("userName");
                return View();
            }
            catch (Exception e)
            {
                _log.Error("Error in UserController while logging out for user : " + HttpContext.Session.GetString("userName") + " - " + e.Message);
                return View("Error");
            }
        }
    }
}
