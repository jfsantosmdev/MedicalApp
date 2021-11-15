using MedicalApp.WebApp.DTOs;
using MedicalApp.WebApp.Models;
using MedicalApp.WebApp.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl; 
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiBaseUrl + "/api/Auth/Login", contentData))
                    {
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var loginResponse = JsonConvert.DeserializeObject<LoginResponseViewModel>(apiResponse);
                            if (loginResponse.Login)
                            {
                                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                                {
                                    return Redirect(model.ReturnUrl);
                                }
                                else
                                {
                                    var serialisedLoginResponse = JsonConvert.SerializeObject(loginResponse);
                                    HttpContext.Session.SetString(SessionKeys.Login, serialisedLoginResponse);
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = loginResponse.Errors.Aggregate((i, j) => i + "\n" + j);
                            }
                            return View(model);
                        }
                        else
                        {
                            ViewBag.FatalMessage = "Ha ocurrido un error no controlado en la aplicación.";
                            return View(model);
                        }                        
                    }
                }
                
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
        public IActionResult RegisterDoctor()
        {            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterDoctor(RegisterDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = new RegisterDoctorRequestDTO()
                {
                    Email = model.Register.Email,
                    Password = model.Register.Password,
                    Name = model.Name,
                    LastName = model.LastName,
                    BirthDate = (DateTime)model.BirthDate,
                    Gender = model.Gender
                };

                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(doctor);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiBaseUrl + "/api/Auth/RegisterDoctor", contentData))
                    {
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var registerResponse = JsonConvert.DeserializeObject<RegisterDoctorResponseViewModel>(apiResponse);
                            if (registerResponse.Register)
                            {
                                return RedirectToAction("Login", "Auth");
                            }
                            else
                            {
                                ViewBag.ErrorMessage = registerResponse.Errors.Aggregate((i, j) => i + "\n" + j);
                            }
                            return View(model);
                        }
                        else
                        {
                            ViewBag.FatalMessage = "Ha ocurrido un error no controlado en la aplicación.";
                            return View(model);
                        }
                    }
                }
            }
            return View(model);
        }
    }
}
