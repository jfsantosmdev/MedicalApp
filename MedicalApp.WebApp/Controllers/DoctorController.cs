using MedicalApp.WebApp.Attributes;
using MedicalApp.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Controllers
{
    [TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
    public class DoctorController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        public async Task<ActionResult> Index()
        {
            List<DoctorViewModel> doctors = new List<DoctorViewModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Doctor"))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        doctors = JsonConvert.DeserializeObject<List<DoctorViewModel>>(apiResponse);
                    }
               }
            }
            return View(doctors);
        }

        // GET: DoctorController/Create
        public ActionResult Create()
        {
            DoctorViewModel model = new DoctorViewModel
            {
                IsAvailable = true
            };
            return View(model);
        }

        // POST: DoctorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name,LastName,BirthDate,Gender,Phone,Speciality,Summary,IsAvailable")] DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                DoctorViewModel doctor = new DoctorViewModel();
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:5001/api/Doctor", contentData))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            doctor = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponse);
                        }
                        else
                        {
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
                        }
                    };
                }
                List<DoctorViewModel> doctors = new List<DoctorViewModel>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:5001/api/Doctor"))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            doctors = JsonConvert.DeserializeObject<List<DoctorViewModel>>(apiResponse);
                        }
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", doctors) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
        }

        // GET: DoctorController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            DoctorViewModel model = new DoctorViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Doctor/" + id))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponse);
                    }
                }

            }
            return View(model);
        }

        // POST: DoctorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id, Name,LastName,BirthDate,Gender,Phone,Speciality,Summary,IsAvailable")] DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                DoctorViewModel doctor = new DoctorViewModel();
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync(_apiBaseUrl + "/api/Doctor/" + id, contentData))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            doctor = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponse);
                        }
                        else
                        {
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", model) });
                        }
                    };
                }

                List<DoctorViewModel> doctors = new List<DoctorViewModel>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Doctor"))
                    {
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            doctors = JsonConvert.DeserializeObject<List<DoctorViewModel>>(apiResponse);
                        }
                    }
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", doctors) });

            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", model) });
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            DoctorViewModel model = new DoctorViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Doctor/" + id))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponse);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Profile(int id)
        {
            DoctorViewModel model = new DoctorViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Doctor/GetByUserId/" + id))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponse);
                    }
                }

            }
            return View("Edit", model);
        }
    }
}
