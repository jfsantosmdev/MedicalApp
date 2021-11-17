using MedicalApp.Application;
using MedicalApp.Entities;
using MedicalApp.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        IDiagnosisApplication _diagnosis;
        //IApplication<DiagnosisFile> _application;
        public DiagnosisController(IDiagnosisApplication diagnosis)
        {
            _diagnosis = diagnosis;
            //_application = application;
        }
        // GET: api/<DiagnosisController>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetByAppointmentId(int id)
        {
            return Ok(_diagnosis.GetByAppointmentId(id));
        }

        // POST api/<DiagnosisController>
        [HttpPost]
        public async Task<IActionResult> Save(DiagnosisDTO dto)
        {

            if (dto == null) return BadRequest();

            if(dto.Id == 0)
            {
                var d = new Diagnosis
                {
                    Id = dto.Id,
                    AppointmentId = dto.AppointmentId,
                    Comments = dto.Comments
                };

                await _diagnosis.SaveAsync(d);
                return Ok(d.Id);
            }
            else
            {
                var tmp = _diagnosis.GetById(dto.Id);
                if (tmp != null)
                {
                    tmp.Id = dto.Id;
                    tmp.AppointmentId = dto.AppointmentId;
                    tmp.Comments = dto.Comments;
                    tmp.UpdatedDate = DateTime.Now;

                    await _diagnosis.SaveAsync(tmp);
                    return Ok(tmp.Id);
                }
            }

            return Ok(0);
        }

    }
}
