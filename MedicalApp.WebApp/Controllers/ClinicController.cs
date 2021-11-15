using MedicalApp.WebApp.Attributes;
using MedicalApp.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Controllers
{
    [TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
    public class ClinicController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        public ClinicController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        // GET: ClinicController
        public async Task<ActionResult> Index()
        {
            List<ClinicViewModel> clinics = new List<ClinicViewModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Clinic"))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        clinics = JsonConvert.DeserializeObject<List<ClinicViewModel>>(apiResponse);
                    }
                }
            }
            return View(clinics);
        }

        // GET: ClinicController/Create
        public ActionResult Create()
        {
            ClinicViewModel model = new ClinicViewModel
            {
                IsActive = true
            };
            return View(model);
        }

        // POST: ClinicController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Name,Address,WebSite,Email,Telephone,IsActive")] ClinicViewModel model)
        {
            if (ModelState.IsValid)
            {
                ClinicViewModel clinic = new ClinicViewModel();
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiBaseUrl + "/api/Clinic", contentData))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            clinic = JsonConvert.DeserializeObject<ClinicViewModel>(apiResponse);
                        }
                    };
                }

                List<ClinicViewModel> clinics = new List<ClinicViewModel>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Clinic"))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            clinics = JsonConvert.DeserializeObject<List<ClinicViewModel>>(apiResponse);
                        }
                    }
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", clinics) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
        }

        // GET: ClinicController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ClinicViewModel model = new ClinicViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Clinic/" + id))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<ClinicViewModel>(apiResponse);
                    }
                }
            }
            return View(model);
        }

        // POST: ClinicController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,Address,WebSite,Email,Telephone,IsActive")] ClinicViewModel model)
        {
            if (ModelState.IsValid)
            {
                ClinicViewModel clinic = new ClinicViewModel();
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8,"application/json");
                    using (var response = await httpClient.PutAsync(_apiBaseUrl + "/api/Clinic/" + id, contentData)) {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            clinic = JsonConvert.DeserializeObject<ClinicViewModel>(apiResponse);
                        }
                    };
                }

                List<ClinicViewModel> clinics = new List<ClinicViewModel>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Clinic"))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            clinics = JsonConvert.DeserializeObject<List<ClinicViewModel>>(apiResponse);
                        }
                    }
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", clinics) });
                    
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", model) });
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            ClinicViewModel model = new ClinicViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Clinic/" + id))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<ClinicViewModel>(apiResponse);
                    }
                }
            }
            return View(model);
        }
    }
}
