using MedicalApp.WebApp.Attributes;
using MedicalApp.WebApp.Models;
using MedicalApp.WebApp.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Controllers
{
    [TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
    public class AppointmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        public AppointmentController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        public IActionResult Index()
        {
            ViewBag.Clinics = GetClinics();
            ViewBag.Doctors = GetDoctors();
            ViewBag.Patients = GetPatients();

            bool showAddAppointmentButton = true;
            var login = JsonConvert.DeserializeObject<LoginResponseViewModel>(HttpContext.Session.GetString(SessionKeys.Login));
            if (login.IsInRole("Doctor"))
            {
                var apiResponseDoctor = new WebClient().DownloadString(_apiBaseUrl + "/api/Doctor/GetByUserId/" + login.UserId);
                var doctor = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponseDoctor);
                if (!doctor.IsAvailable)
                    showAddAppointmentButton = false;
            }
            ViewBag.ShowAddAppointmentButton = showAddAppointmentButton;

            return View();
        }
        public ActionResult Create()
        {
            AppointmentViewModel model = new AppointmentViewModel();
            model.ListOfClincs = GetClinics();
            model.ListOfDoctors = GetAvailableDoctors("Create");
            model.ListOfPatients = GetPatients();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ClinicId,DoctorId,PatientId,DateTimeStart,DateTimeEnd,Reason,Note")] AppointmentViewModel model)
        {
            model.ListOfClincs = GetClinics();
            model.ListOfDoctors = GetAvailableDoctors();
            model.ListOfPatients = GetPatients();
            if (ModelState.IsValid)
            {
                AppointmentViewModel appointment = new AppointmentViewModel();
                var login = JsonConvert.DeserializeObject<LoginResponseViewModel>(HttpContext.Session.GetString(SessionKeys.Login));
                model.UserId = login.UserId;
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiBaseUrl + "/api/Appointment", contentData))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(apiResponse);
                        }
                        else if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            ModelState.AddModelError(nameof(AppointmentViewModel.DateTimeStart), await response.Content.ReadAsStringAsync());
                            ModelState.AddModelError(nameof(AppointmentViewModel.DateTimeEnd), await response.Content.ReadAsStringAsync());
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
                        }
                    };
                }

                return Json(new { isValid = true, html = "" });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
        }
        public async Task<IActionResult> Edit(int id)
        {
            AppointmentViewModel model = new AppointmentViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Appointment/" + id))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<AppointmentViewModel>(apiResponse);
                    }
                }
            }

            model.ListOfClincs = GetClinics();
            model.ListOfDoctors = GetAvailableDoctors();
            model.ListOfPatients = GetPatients();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,ClinicId,DoctorId,PatientId,DateTimeStart,DateTimeEnd,Reason,Note,Status")] AppointmentViewModel model)
        {
            model.ListOfClincs = GetClinics();
            model.ListOfDoctors = GetAvailableDoctors();
            model.ListOfPatients = GetPatients();
            if (ModelState.IsValid)
            {
                AppointmentViewModel appointment = new AppointmentViewModel();
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync(_apiBaseUrl + "/api/Appointment/" + id, contentData))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            appointment = JsonConvert.DeserializeObject<AppointmentViewModel>(apiResponse);
                        }
                        else if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            ModelState.AddModelError(nameof(AppointmentViewModel.DateTimeStart), await response.Content.ReadAsStringAsync());
                            ModelState.AddModelError(nameof(AppointmentViewModel.DateTimeEnd), await response.Content.ReadAsStringAsync());
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
                        }
                    };
                }

                return Json(new { isValid = true, html = "" });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", model) });
        }
        public async Task<ActionResult> Details(int id)
        {
            AppointmentViewModel model = new AppointmentViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Appointment/" + id + "/GetDetails"))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<AppointmentViewModel>(apiResponse);
                    }
                }
            }
            return View(model);
        }
        public List<SelectListItem> GetClinics()
        {
            var apiResponse = new WebClient().DownloadString(_apiBaseUrl + "/api/Clinic");
            var data = JsonConvert.DeserializeObject<List<ClinicViewModel>>(apiResponse);

            List<SelectListItem> list = data
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }).ToList();
            var selectOption = new SelectListItem()
            {
                Value = "",
                Text = "--- Seleccionar Clínica ---"
            };

            list.Insert(0, selectOption);
            return list;
        }
        public List<SelectListItem> GetDoctors()
        {
            var apiResponse = new WebClient().DownloadString(_apiBaseUrl + "/api/Doctor");
            var data = JsonConvert.DeserializeObject<List<DoctorViewModel>>(apiResponse);
            var login = JsonConvert.DeserializeObject<LoginResponseViewModel>(HttpContext.Session.GetString(SessionKeys.Login));
            List<SelectListItem> list = new List<SelectListItem>();


            if (login.IsInRole("Administrator"))
            {
                list = data
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name + " " + n.LastName
                }).ToList();

                var selectOption = new SelectListItem()
                {
                    Value = "",
                    Text = "--- Seleccionar Doctor ---"
                };

                list.Insert(0, selectOption);
            }
            else
            {
                var apiResponseDoctor = new WebClient().DownloadString(_apiBaseUrl + "/api/Doctor/GetByUserId/" + login.UserId);
                var doctor = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponseDoctor);
                var optionDoctor = data.Where(d => d.Id == doctor.Id).FirstOrDefault();
                list.Insert(0, new SelectListItem { Value = optionDoctor.Id.ToString(), Text = optionDoctor.Name + " " + optionDoctor.LastName });
            }
            return list;
        }
        public List<SelectListItem> GetAvailableDoctors(string action = "")
        {
            var apiResponse = new WebClient().DownloadString(_apiBaseUrl + "/api/Doctor/GetAvailableDoctors");
            var data = JsonConvert.DeserializeObject<List<DoctorViewModel>>(apiResponse);
            var login = JsonConvert.DeserializeObject<LoginResponseViewModel>(HttpContext.Session.GetString(SessionKeys.Login));
            List<SelectListItem> list = new List<SelectListItem>();
            if (action == "Create" && login.IsInRole("Doctor"))
            {
                var apiResponseDoctor = new WebClient().DownloadString(_apiBaseUrl + "/api/Doctor/GetByUserId/" + login.UserId);
                var doctor = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponseDoctor);
                var optionDoctor = data.Where(d => d.Id == doctor.Id).FirstOrDefault();
                list.Insert(0, new SelectListItem { Value = optionDoctor.Id.ToString(), Text = optionDoctor.Name + " " + optionDoctor.LastName });
            }
            else
            {
                list = data
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name + " " + n.LastName
                }).ToList();
                var selectOption = new SelectListItem()
                {
                    Value = "",
                    Text = "--- Seleccionar Doctor ---"
                };

                list.Insert(0, selectOption);
            }
            return list;
        }
        public List<SelectListItem> GetPatients()
        {
            var apiResponse = new WebClient().DownloadString(_apiBaseUrl + "/api/Patient");
            var data = JsonConvert.DeserializeObject<List<PatientViewModel>>(apiResponse);

            List<SelectListItem> list = data
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name + " " + n.LastName
                }).ToList();
            var selectOption = new SelectListItem()
            {
                Value = "",
                Text = "--- Seleccionar Paciente ---"
            };

            list.Insert(0, selectOption);
            return list;
        }
        [HttpGet]
        public async Task<ActionResult> GetAppointments(int clinicId, int doctorId, int patientId)
        {
            List<AppointmentViewModel> appointments = new List<AppointmentViewModel>();
            using (var httpClient = new HttpClient())
            {
                string parameters = string.Empty;
                if (clinicId != 0)
                    parameters = parameters + "clinicId=" + clinicId.ToString() + "&";
                if (doctorId != 0)
                    parameters = parameters + "doctorId=" + doctorId.ToString() + "&";
                if (patientId != 0)
                    parameters = parameters + "patientId=" + patientId.ToString();

                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Appointment?" + parameters))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        appointments = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(apiResponse);
                    }
                }
            }

            return Json(appointments);
        }
        [HttpGet]
        public async Task<ActionResult> GetAvailableAppointments(int clinicId, int doctorId, int patientId)
        {
            List<AppointmentViewModel> appointments = new List<AppointmentViewModel>();
            using (var httpClient = new HttpClient())
            {
                string parameters = string.Empty;
                if (clinicId != 0)
                    parameters = parameters + "clinicId=" + clinicId.ToString() + "&";
                if (doctorId != 0)
                    parameters = parameters + "doctorId=" + doctorId.ToString() + "&";
                if (patientId != 0)
                    parameters = parameters + "patientId=" + patientId.ToString();

                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Appointment/GetAvailableAppointments?" + parameters))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        appointments = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(apiResponse);
                    }
                }
            }

            return Json(appointments);
        }
    }
}
