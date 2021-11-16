using MedicalApp.Application;
using MedicalApp.Entities;
using MedicalApp.Entities.Parameters;
using MedicalApp.Services;
using MedicalApp.WebApi.Configuration;
using MedicalApp.WebApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        IAppointmentApplication _appointment;
        IPatientApplication _patient;
        IClinicApplication _clinic;
        IApplication<Appointment> _genericApplication;
        IMailService _mailService;
        public AppointmentController(IAppointmentApplication appointment, IApplication<Appointment> genericApplication, IPatientApplication patient, IClinicApplication clinic, IMailService mailService)
        {
            _appointment = appointment;
            _genericApplication = genericApplication;
            _patient = patient;
            _clinic = clinic;
            _mailService = mailService;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] AppointmentParameters appointmentParameters)
        {
            return Ok(_appointment.GetAppointments(appointmentParameters));
        }

        [HttpGet]
        [Route("GetAvailableAppointments")]
        public IActionResult GetAvailableAppointments([FromQuery] AppointmentParameters appointmentParameters)
        {
            return Ok(_appointment.GetAvailableAppointments(appointmentParameters));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _appointment.GetByIdAsync(id));
        }
        [HttpGet]
        [Route("{id:int}/GetDetails")]
        public IActionResult GetDetails(int id)
        {
            return Ok(_appointment.GetAppointmentById(id));
        }

        [HttpGet]
        [Route("GetAppointmentsByDoctorId/{id}")]
        public IActionResult GetAppointmentsByDoctorId(int id)
        {
            return Ok(_appointment.GetAppointmentsByDoctorId(id));
        }

        [HttpGet]
        [Route("GetAllByDoctorId/{id}")]
        public IActionResult GetAllByDoctorId(int id)
        {
            return Ok(_appointment.GetAllByDoctorId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save(AppointmentDTO dto)
        {
            var hasOverlap = _appointment.SearchFor(a => a.ClinicId == dto.ClinicId 
                                                    && a.DoctorId == dto.DoctorId 
                                                    && ((dto.DateTimeStart >= a.DateTimeStart && dto.DateTimeStart < a.DateTimeEnd)
                                                    || (dto.DateTimeEnd > a.DateTimeStart && dto.DateTimeEnd <= a.DateTimeEnd))
                                                    && (a.Status != AppointmentStatus.Cancelada)).Count();

            if (hasOverlap > 0)
                return BadRequest("El horario se traslapa con otras citas");

            var a = new Appointment()
            {
                ClinicId = dto.ClinicId,
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                DateTimeStart = dto.DateTimeStart,
                DateTimeEnd = dto.DateTimeEnd,
                Reason = dto.Reason,
                Note = dto.Note,
                UserId = dto.UserId,
                CreatedDate = DateTime.Now,
                Status = AppointmentStatus.Programada
            };

            await _appointment.SaveAsync(a);

            var patient = _patient.GetById(a.PatientId);
            var clinic = _clinic.GetById(a.ClinicId);

            string filePath = Directory.GetCurrentDirectory() + "\\Templates\\AddOrEditAppointment.html";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(filePath))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string emailTemplateText = string.Format(builder.HtmlBody, "Nueva cita programada", patient.Name + " " + patient.LastName, a.DateTimeStart.ToString("dd/MM/yyyy HH:mm"), a.Reason, clinic.Name + " (" + clinic.Address + ")");

            await _mailService.SendEmailAsync(new MailParameters
            {
                ToEmail = patient.Email,
                Subject = "Nueva cita programada",
                Body = emailTemplateText
            });
            return Ok(a);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, AppointmentDTO dto)
        {

            if (id == 0 || dto == null) return NotFound();

            var hasOverlap = _appointment.SearchFor(a => a.ClinicId == dto.ClinicId
                                                    && a.DoctorId == dto.DoctorId
                                                    && ((dto.DateTimeStart >= a.DateTimeStart && dto.DateTimeStart < a.DateTimeEnd)
                                                    || (dto.DateTimeEnd > a.DateTimeStart && dto.DateTimeEnd <= a.DateTimeEnd))
                                                    && (a.Status != AppointmentStatus.Cancelada)
                                                    && a.Id != id).Count();

            if (hasOverlap > 0)
                return BadRequest("El horario se traslapa con otras citas");

            var tmp = _appointment.GetById(id);
            if (tmp != null)
            {
                tmp.Id = id;
                tmp.ClinicId = dto.ClinicId;
                tmp.DoctorId = dto.DoctorId;
                tmp.PatientId = dto.PatientId;
                tmp.DateTimeStart = dto.DateTimeStart;
                tmp.DateTimeEnd = dto.DateTimeEnd;
                tmp.Reason = dto.Reason;
                tmp.Note = dto.Note;
                tmp.UserId = null;
                tmp.Status = dto.Status;
                tmp.UpdatedDate = DateTime.Now;
            }

            await _appointment.SaveAsync(tmp);

            var patient = _patient.GetById(tmp.PatientId);
            var clinic = _clinic.GetById(tmp.ClinicId);

            string filePath = Directory.GetCurrentDirectory() + "\\Templates\\AddOrEditAppointment.html";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(filePath))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string emailTemplateText = string.Format(builder.HtmlBody, "Cita reprogramada", patient.Name + " " + patient.LastName, tmp.DateTimeStart.ToString("dd/MM/yyyy HH:mm"), tmp.Reason, clinic.Name + " (" + clinic.Address + ")");

            await _mailService.SendEmailAsync(new MailParameters
            {
                ToEmail = patient.Email,
                Subject = "Cita reprogramada",
                Body = emailTemplateText
            });
            return Ok(tmp);
        }
    }
}
