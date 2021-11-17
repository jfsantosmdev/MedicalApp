using MedicalApp.DataAccess;
using MedicalApp.Entities;

namespace MedicalApp.Repository
{
    public interface IDiagnosisRepository : IRepository<Diagnosis>
    {
        Diagnosis GetByAppointmentId(int id);
    }
    public class DiagnosisRepository : Repository<Diagnosis>, IDiagnosisRepository
    {
        IDiagnosisDbContext _ctx;
        public DiagnosisRepository(IDiagnosisDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public Diagnosis GetByAppointmentId(int id)
        {
            return _ctx.GetByAppointmentId(id);
        }
    }
}
