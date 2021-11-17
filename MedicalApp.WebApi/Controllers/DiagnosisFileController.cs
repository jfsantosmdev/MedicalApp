using MedicalApp.Application;
using MedicalApp.Entities;
using MedicalApp.WebApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisFileController : ControllerBase
    {
        IDiagnosisFileApplication _diagnosisFile;
        //IApplication<DiagnosisFile> _application;
        public DiagnosisFileController(IDiagnosisFileApplication diagnosisFile)
        {
            _diagnosisFile = diagnosisFile;
        }

        [HttpPost]
        public async Task<IActionResult> Save(DiagnosisFileDTO dto)
        {
            var f = new DiagnosisFile
            {
                DiagnosisId = dto.DiagnosisId,
                Name = dto.Name,
                Description = dto.Description,
                Type = dto.Type
            };

            await _diagnosisFile.SaveAsync(f);
            return Ok(f);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_diagnosisFile.GetById(id));
        }
    }
}
