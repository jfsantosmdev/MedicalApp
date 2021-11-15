using MedicalApp.WebApp.Attributes;
using MedicalApp.WebApp.Models;
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
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel model = new DashboardViewModel();

            List<AppointmentViewModel> appointments = new List<AppointmentViewModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Appointment"))
                {
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        appointments = JsonConvert.DeserializeObject<List<AppointmentViewModel>>(apiResponse);
                        //appointments = appointments.Where(x => x.DateTimeStart.ToShortDateString() == DateTime.Now.ToShortDateString()).OrderByDescending(x => x.DateTimeStart).ToList();
                        if(appointments != null)
                        {
                            model.AppointmentsCount = appointments.Count();
                            model.Appointments = appointments;
                        }
                    }
                }
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
                        if(patients != null)
                        {
                            model.PatientsCount = patients.Count();
                        }
                    }
                }
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
                        if(doctors != null)
                            model.DoctorsCount = doctors.Count();
                    }
                }
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
                        if(clinics != null)
                            model.ClinicsCount = clinics.Count();
                    }
                }
            }

            return View(model);
        }
    }
}
