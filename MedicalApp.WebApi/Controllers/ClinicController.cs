using MedicalApp.Application;
using MedicalApp.Entities;
using MedicalApp.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MedicalApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        IClinicApplication _clinic;
        IApplication<Clinic> _genericApplication;
        public ClinicController(IClinicApplication clinic, IApplication<Clinic> genericApplication)
        {
            _clinic = clinic;
            _genericApplication = genericApplication;
        }

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            return Ok(await _clinic.GetAllAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _clinic.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ClinicDTO dto)
        {
            
            var c = new Clinic()
            {               
                Name = dto.Name,
                Address = dto.Address,
                WebSite = dto.WebSite,
                Email = dto.Email,
                Telephone = dto.Telephone,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.Now
            };

            await _clinic.SaveAsync(c);
            return Ok(c);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, ClinicDTO dto)
        {

            if (id == 0 || dto == null) return NotFound();

            var tmp = _clinic.GetById(id);
            if(tmp != null)
            {
                tmp.Id = id;
                tmp.Name = dto.Name;
                tmp.Address = dto.Address;
                tmp.WebSite = dto.WebSite;
                tmp.Email = dto.Email;
                tmp.Telephone = dto.Telephone;
                tmp.IsActive = dto.IsActive;
                tmp.UpdatedDate = DateTime.Now;
            }

            await _clinic.SaveAsync(tmp);
            return Ok(tmp);
        }
    }
}
