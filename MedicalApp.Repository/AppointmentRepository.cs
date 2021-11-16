using MedicalApp.DataAccess;
using MedicalApp.Entities;
using MedicalApp.Entities.Parameters;
using System.Collections.Generic;

namespace MedicalApp.Repository
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        IList<Appointment> GetAppointments(AppointmentParameters appointmentParameters);
        IList<Appointment> GetAppointmentsByDoctorId(int id);
        IList<Appointment> GetAllByDoctorId(int id);
        Appointment GetAppointmentById(int id);
        IList<Appointment> GetAvailableAppointments(AppointmentParameters appointmentParameters);
    }  
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        IAppointmentDbContext _ctx;

        public AppointmentRepository(IAppointmentDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public IList<Appointment> GetAllByDoctorId(int id)
        {
            return _ctx.GetAllByDoctorId(id);
        }

        public IList<Appointment> GetAppointments(AppointmentParameters appointmentParameters)
        {
            return _ctx.GetAppointments(appointmentParameters);
        }

        public IList<Appointment> GetAppointmentsByDoctorId(int id)
        {
            return _ctx.GetAppointmentsByDoctorId(id);
        }

        public Appointment GetAppointmentById(int id)
        {
            return _ctx.GetAppointmentById(id);
        }

        public IList<Appointment> GetAvailableAppointments(AppointmentParameters appointmentParameters)
        {
            return _ctx.GetAvailableAppointments(appointmentParameters);
        }
    }
}
