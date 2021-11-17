using MedicalApp.Abstractions;
using MedicalApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalApp.DataAccess
{
    public interface IDiagnosisDbContext : IDbContext<Diagnosis>
    {
        Diagnosis GetByAppointmentId(int id);
    }
    public class DiagnosisDbContext : DbContext<Diagnosis>, IDiagnosisDbContext
    {
        public DiagnosisDbContext(ApiDbContext ctx) : base(ctx) { }
        public Diagnosis GetByAppointmentId(int id)
        {
            return SearchFor(a => a.AppointmentId == id).Include(a => a.Appointment).Include(a => a.Files).FirstOrDefault();
        }
    }
}
