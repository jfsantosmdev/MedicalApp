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
    public class PatientController : ControllerBase
    {
        IPatientApplication _patient;
        IApplication<Patient> _genericApplication;
        public PatientController(IPatientApplication patient, IApplication<Patient> genericApplication)
        {
            _patient = patient;
            _genericApplication = genericApplication;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _patient.GetAllAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _patient.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save(PatientDTO dto)
        {

            var p = new Patient()
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Gender = (Gender)Enum.Parse(typeof(Gender), dto.Gender),
                Email = dto.Email,
                Telephone = dto.Telephone,
                Address = dto.Address,
                ProfileImage  = dto.ProfileImage ,
                CreatedDate = DateTime.Now
            };

            await _patient.SaveAsync(p);
            return Ok(p);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, PatientDTO dto)
        {

            if (id == 0 || dto == null) return NotFound();

            var tmp = _patient.GetById(id);
            if (tmp != null)
            {
                tmp.Id = id;
                tmp.Name = dto.Name;
                tmp.LastName = dto.LastName;
                tmp.DateOfBirth = dto.DateOfBirth;
                tmp.Gender = (Gender)Enum.Parse(typeof(Gender), dto.Gender);
                tmp.Email = dto.Email;
                tmp.Telephone = dto.Telephone;
                tmp.Address = dto.Address;
                tmp.UpdatedDate = DateTime.Now;
            }

            await _patient.SaveAsync(tmp);
            return Ok(tmp);
        }
    }
}
