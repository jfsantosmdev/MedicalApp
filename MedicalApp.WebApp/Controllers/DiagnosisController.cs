using MedicalApp.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Controllers
{

    public class DiagnosisController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        private IHostingEnvironment _env;
        public DiagnosisController(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
            _env = env;
        }
        public async Task<IActionResult> Index(int appointmentId)
        {
            DiagnosisViewModel model = new DiagnosisViewModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Diagnosis/" + appointmentId))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<DiagnosisViewModel>(apiResponse);
                    }
                }
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DiagnosisViewModel model, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                int diagnosisId = 0;
                using (var httpClient = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiBaseUrl + "/api/Diagnosis", contentData))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            diagnosisId = Convert.ToInt32(apiResponse);
                        }
                    }
                }

                try
                {
                    if(diagnosisId != 0)
                    {
                        foreach (var f in files)
                        {
                            if (f.Length > 0)
                            {
                                DiagnosisFileViewModel file = new DiagnosisFileViewModel();
                                file.DiagnosisId = diagnosisId;
                                file.Name = Path.GetFileName(f.FileName);
                                file.Type = Path.GetExtension(f.FileName);
                                int diagnosisFileId = 0;
                                using (var httpClient = new HttpClient())
                                {
                                    string stringData = JsonConvert.SerializeObject(file);
                                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                                    using (var response = await httpClient.PostAsync(_apiBaseUrl + "/api/DiagnosisFile", contentData))
                                    {
                                        if (response.StatusCode == HttpStatusCode.OK)
                                        {
                                            string apiResponse = await response.Content.ReadAsStringAsync();
                                            var modelResponse = JsonConvert.DeserializeObject<DiagnosisFileViewModel>(apiResponse);
                                            diagnosisFileId = modelResponse.Id;
                                        }
                                    }
                                }

                                if(diagnosisFileId != 0)
                                {
                                    var rootPath = Path.Combine(_env.WebRootPath, "Files");
                                    if (!Directory.Exists(rootPath))
                                        Directory.CreateDirectory(rootPath);
                                    var name = diagnosisFileId.ToString() + file.Type;
                                    var filePath = Path.Combine(rootPath, name);
                                    using (FileStream fs = System.IO.File.Create(filePath))
                                    {
                                        f.CopyTo(fs);
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {

                }


                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/Diagnosis/" + model.AppointmentId))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            model = JsonConvert.DeserializeObject<DiagnosisViewModel>(apiResponse);
                        }
                    }
                }

                
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Index", model) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Index", model) });
        }

        public async Task<FileResult> DownloadFile(int id) {

            DiagnosisFileViewModel model = new DiagnosisFileViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiBaseUrl + "/api/DiagnosisFile/" + id))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<DiagnosisFileViewModel>(apiResponse);
                    }
                }
            }

            string path = Path.Combine(_env.WebRootPath, "Files/") + model.Id.ToString() + model.Type;

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", model.Name);
        }
    }
}
