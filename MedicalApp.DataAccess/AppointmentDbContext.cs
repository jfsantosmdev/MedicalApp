using MedicalApp.Abstractions;
using MedicalApp.Entities;
using MedicalApp.Entities.Parameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalApp.DataAccess
{
    public interface IAppointmentDbContext : IDbContext<Appointment>
    {
        IList<Appointment> GetAppointments(AppointmentParameters appointmentParameters);
        IList<Appointment> GetAppointmentsByDoctorId(int id);
        IList<Appointment> GetAllByDoctorId(int id);
        Appointment GetAppointmentById(int id);
        IList<Appointment> GetAvailableAppointments(AppointmentParameters appointmentParameters);
    }
    public class AppointmentDbContext : DbContext<Appointment>, IAppointmentDbContext
    {
        public AppointmentDbContext(ApiDbContext ctx) : base(ctx)
        {
        }
        
        public IList<Appointment> GetAllByDoctorId(int id)
        {
            return SearchFor(a => a.DoctorId == id).Include(a => a.Clinic).Include(a => a.Doctor).Include(a => a.Patient).ToList();
        }

        public IList<Appointment> GetAppointments(AppointmentParameters appointmentParameters)
        {
            var appointments = GetAll()
                .Include(a => a.Clinic)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a =>
                (appointmentParameters.ClinicId != null ? a.ClinicId == appointmentParameters.ClinicId : a.ClinicId == a.ClinicId) &&
                (appointmentParameters.DoctorId != null ? a.DoctorId == appointmentParameters.DoctorId : a.DoctorId == a.DoctorId) &&
                (appointmentParameters.PatientId != null ? a.PatientId == appointmentParameters.PatientId : a.PatientId == a.PatientId));

            if (appointmentParameters.DateTimeStart != null && appointmentParameters.DateTimeEnd != null)
                appointments = appointments.Where(a => a.DateTimeStart >= appointmentParameters.DateTimeStart && a.DateTimeEnd <= appointmentParameters.DateTimeEnd);

            if (appointmentParameters.Status != null)
                appointments = appointments.Where(a => a.Status == appointmentParameters.Status);

            return appointments.OrderByDescending(a => a.DateTimeStart).Skip((appointmentParameters.PageNumber - 1) * appointmentParameters.PageSize).Take(appointmentParameters.PageSize).ToList();

        }

        public IList<Appointment> GetAppointmentsByDoctorId(int id)
        {
            var appointments = GetAll()
                .Include(a => a.Clinic)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == id).ToList();

            return appointments;
        }

        public Appointment GetAppointmentById(int id)
        {
            return GetAll().Include(a => a.Clinic).Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.Id == id).FirstOrDefault();
        }

        public IList<Appointment> GetAvailableAppointments(AppointmentParameters appointmentParameters)
        {
            var appointments = GetAll()
                .Include(a => a.Clinic)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a =>
                (appointmentParameters.ClinicId != null ? a.ClinicId == appointmentParameters.ClinicId : a.ClinicId == a.ClinicId) &&
                (appointmentParameters.DoctorId != null ? a.DoctorId == appointmentParameters.DoctorId : a.DoctorId == a.DoctorId) &&
                (appointmentParameters.PatientId != null ? a.PatientId == appointmentParameters.PatientId : a.PatientId == a.PatientId));

            if (appointmentParameters.DateTimeStart != null && appointmentParameters.DateTimeEnd != null)
                appointments = appointments.Where(a => a.DateTimeStart >= appointmentParameters.DateTimeStart && a.DateTimeEnd <= appointmentParameters.DateTimeEnd);

            appointments = appointments.Where(a => a.Status == AppointmentStatus.Programada || a.Status == AppointmentStatus.Confirmada);
            return appointments.OrderByDescending(a => a.DateTimeStart).Skip((appointmentParameters.PageNumber - 1) * appointmentParameters.PageSize).Take(appointmentParameters.PageSize).ToList();

        }
    }
}
