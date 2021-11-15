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
    public class PatientController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        // GET: PatientController
        public async Task<ActionResult> Index()
        {
            List<PatientViewModel> patients = new List<PatientViewModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Patient"))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        patients = JsonConvert.DeserializeObject<List<PatientViewModel>>(apiResponse);
                    }
                }
            }
            return View(patients);
        }

        // GET: PatientController/Create
        public ActionResult Create()
        {
            PatientViewModel model = new PatientViewModel();
            return View(model);
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name,LastName,DateOfBirth,Gender,Email,Telephone,Address")]PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                PatientViewModel patient = new PatientViewModel();
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiBaseUrl + "/api/Patient", contentData))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            patient = JsonConvert.DeserializeObject<PatientViewModel>(apiResponse);
                        }
                    };
                }

                List<PatientViewModel> patients = new List<PatientViewModel>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Patient"))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            patients = JsonConvert.DeserializeObject<List<PatientViewModel>>(apiResponse);
                        }
                    }
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", patients) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
        }

        // GET: PatientController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            PatientViewModel model = new PatientViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Patient/" + id))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<PatientViewModel>(apiResponse);
                    }
                }
            }
            return View(model);
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,LastName,DateOfBirth,Gender,Email,Telephone,Address")] PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                PatientViewModel patient = new PatientViewModel();
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync(_apiBaseUrl + "/api/Patient/" + id, contentData))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            patient = JsonConvert.DeserializeObject<PatientViewModel>(apiResponse);
                        }
                    };
                }

                List<PatientViewModel> patients = new List<PatientViewModel>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Patient"))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            patients = JsonConvert.DeserializeObject<List<PatientViewModel>>(apiResponse);
                        }
                    }
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", patients) });

            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", model) });
        }

        public async Task<ActionResult> Details(int id)
        {
            PatientViewModel model = new PatientViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Patient/" + id))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<PatientViewModel>(apiResponse);
                    }
                }
            }
            return View(model);
        }
    }
}
