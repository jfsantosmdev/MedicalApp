using MedicalApp.WebApp.Attributes;
using MedicalApp.WebApp.Models;
using MedicalApp.WebApp.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class SearchController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        public SearchController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        public async Task<IActionResult> Index(SearchAppointmentViewModel model)
        {

            if (model.DateStart == null || model.DateEnd == null)
            {
                //var today = DateTime.Now;
                //var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                //model.DateStart = firstDayOfMonth;
                //model.DateEnd = lastDayOfMonth;
                model.DateStart = DateTime.Now;
                model.DateEnd = DateTime.Now;
                ModelState.Clear();
            }

            model.ListOfClinics = GetClinics();
            model.ListOfDoctors = GetDoctors();
            model.ListOfPatients = GetPatients();

            model.Appointments = new List<AppointmentViewModel>();
            using (var httpClient = new HttpClient())
            {
                string parameters = string.Empty;
                if (model.ClincId != null)
                    parameters = parameters + "clinicId=" + model.ClincId.ToString() + "&";
                if (model.DoctorId != null)
                {
                    parameters = parameters + "doctorId=" + model.DoctorId.ToString() + "&";
                }
                else
                {
                    parameters = parameters + "doctorId=" + model.ListOfDoctors.FirstOrDefault().Value + "&";
                }

                if (model.Status != null)
                    parameters = parameters + "status=" + (int)model.Status + "&";

                if (model.PatientId != null)
                    parameters = parameters + "patientId=" + model.PatientId.ToString() + "&";

                var dateStart = (DateTime)model.DateStart;
                var dateEnd = (DateTime)model.DateEnd;
                parameters = parameters + "dateTimeStart=" + dateStart.ToString("yyyy-MM-dd 00:00") + "&";
                parameters = parameters + "dateTimeEnd=" + dateEnd.ToString("yyyy-MM-dd 23:59");

                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Appointment?" + parameters))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model.Appointments = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(apiResponse);
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

        //public List<SelectListItem> GetDoctors()
        //{
        //    var apiResponse = new WebClient().DownloadString(_apiBaseUrl + "/api/Doctor");
        //    var data = JsonConvert.DeserializeObject<List<DoctorViewModel>>(apiResponse);

        //    List<SelectListItem> list = data
        //        .OrderBy(n => n.Name)
        //        .Select(n => new SelectListItem
        //        {
        //            Value = n.Id.ToString(),
        //            Text = n.Name + " " + n.LastName
        //        }).ToList();
        //    var selectOption = new SelectListItem()
        //    {
        //        Value = "",
        //        Text = "--- Seleccionar Doctor ---"
        //    };

        //    list.Insert(0, selectOption);
        //    return list;
        //}
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
    }
}
