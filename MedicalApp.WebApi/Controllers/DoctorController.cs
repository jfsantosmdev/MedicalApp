using MedicalApp.Application;
using MedicalApp.Entities;
using MedicalApp.WebApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        IDoctorApplication _doctor;
        IApplication<Doctor> _genericApplication;
        public DoctorController(IDoctorApplication doctor, IApplication<Doctor> genericApplication)
        {
            _doctor = doctor;
            _genericApplication = genericApplication;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _doctor.GetAllAsync());
        }

        [HttpGet]
        [Route("GetAvailableDoctors")]
        public IActionResult GetAvailableDoctors()
        {
            return Ok(_doctor.SearchFor(d => d.IsAvailable).ToList());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _doctor.GetByIdAsync(id));
        }

        [HttpGet]
        [Route("GetByUserId/{id}")]
        public IActionResult GetByUserId(int id)
        {
            return Ok(_doctor.GetByUserId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save(DoctorDTO dto)
        {

            var d = new Doctor()
            {
                Name = dto.Name,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                Phone = dto.Phone,
                Speciality = dto.Speciality,
                Summary = dto.Summary,
                IsAvailable = true,
                CreatedDate = DateTime.Now
            };

            await _doctor.SaveAsync(d);
            return Ok(d);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, DoctorDTO dto)
        {

            if (id == 0 || dto == null) return NotFound();

            var tmp = _doctor.GetById(id);
            if (tmp != null)
            {
                tmp.Id = id;
                tmp.Name = dto.Name;
                tmp.LastName = dto.LastName;
                tmp.BirthDate = dto.BirthDate;
                tmp.Gender = dto.Gender;
                tmp.Phone = dto.Phone;
                tmp.Speciality = dto.Speciality;
                tmp.Summary = dto.Summary;
                tmp.IsAvailable = dto.IsAvailable;
                tmp.UpdatedDate = DateTime.Now;
            }

            await _doctor.SaveAsync(tmp);
            return Ok(tmp);
        }
        
    }
}
