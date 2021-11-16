using MedicalApp.Entities;
using MedicalApp.Entities.Parameters;
using MedicalApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalApp.Application
{
    public interface IAppointmentApplication : IApplication<Appointment>
    {
        IList<Appointment> GetAppointments(AppointmentParameters appointmentParameters);
        IList<Appointment> GetAppointmentsByDoctorId(int id);
        IList<Appointment> GetAllByDoctorId(int id);
        Appointment GetAppointmentById(int id);
        IList<Appointment> GetAvailableAppointments(AppointmentParameters appointmentParameters);
    }
    public class AppointmentApplication : Application<Appointment>, IAppointmentApplication
    {
        IAppointmentRepository _repo;
        public AppointmentApplication(IAppointmentRepository repo) : base(repo)
        {
            _repo = repo;
        }

        public IList<Appointment> GetAllByDoctorId(int id)
        {
            return _repo.GetAllByDoctorId(id);
        }

        public Appointment GetAppointmentById(int id)
        {
            return _repo.GetAppointmentById(id);
        }

        public IList<Appointment> GetAppointments(AppointmentParameters appointmentParameters)
        {
            return _repo.GetAppointments(appointmentParameters);
        }

        public IList<Appointment> GetAppointmentsByDoctorId(int id)
        {
            return _repo.GetAppointmentsByDoctorId(id);
        }

        public IList<Appointment> GetAvailableAppointments(AppointmentParameters appointmentParameters)
        {
            return _repo.GetAvailableAppointments(appointmentParameters);
        }
    }
}
