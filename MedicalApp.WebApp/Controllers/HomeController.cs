using MedicalApp.WebApp.Attributes;
using MedicalApp.WebApp.Models;
using MedicalApp.WebApp.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Controllers
{
    [TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();
            LoginResponseViewModel login = JsonConvert.DeserializeObject<LoginResponseViewModel>(HttpContext.Session.GetString(SessionKeys.Login));
            if (login.IsInRole("Administrator"))
            {
                var startDate = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                var endDate = DateTime.Now.ToString("yyyy-MM-dd 23:59");
                using (var httpClient = new HttpClient())
                {
                    string parameters = string.Empty;
                    parameters = parameters + "dateTimeStart=" + startDate + "&";
                    parameters = parameters + "dateTimeEnd=" + endDate;

                    using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Appointment?" + parameters))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var appointments = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(apiResponse);
                            model.ScheduledAppointmentsCount = appointments.Where(a => a.Status == AppointmentStatus.Programada).Count();
                            model.ConfirmedAppointmentsCount = appointments.Where(a => a.Status == AppointmentStatus.Confirmada).Count();
                            model.FinishedAppointmentsCount = appointments.Where(a => a.Status == AppointmentStatus.Finalizada).Count();
                            model.CancelledAppointmentsCount = appointments.Where(a => a.Status == AppointmentStatus.Cancelada).Count();
                            model.Appointments = appointments.OrderBy(a => a.DateTimeStart).ToList();
                        }
                    }
                }
            }
            else if (login.IsInRole("Doctor"))
            {
                var startDate = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                var endDate = DateTime.Now.ToString("yyyy-MM-dd 23:59");
                using (var httpClient = new HttpClient())
                {
                    DoctorViewModel doctor = null;
                    using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Doctor/GetByUserId/" + login.UserId))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            doctor = JsonConvert.DeserializeObject<DoctorViewModel>(apiResponse);

                        }
                    }

                    if (doctor != null)
                    {
                        string parameters = string.Empty;
                        parameters = parameters + "doctorId=" + doctor.Id + "&";
                        parameters = parameters + "dateTimeStart=" + startDate + "&";
                        parameters = parameters + "dateTimeEnd=" + endDate;

                        using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Appointment?" + parameters))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                var appointments = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(apiResponse);
                                model.ScheduledAppointmentsCount = appointments.Where(a => a.Status == AppointmentStatus.Programada).Count();
                                model.ConfirmedAppointmentsCount = appointments.Where(a => a.Status == AppointmentStatus.Confirmada).Count();
                                model.FinishedAppointmentsCount = appointments.Where(a => a.Status == AppointmentStatus.Finalizada).Count();
                                model.CancelledAppointmentsCount = appointments.Where(a => a.Status == AppointmentStatus.Cancelada).Count();
                                model.Appointments = appointments.OrderBy(a => a.DateTimeStart).ToList();
                            }
                        }
                    }

                }
            }

            return View(model);
        }
    }
}
