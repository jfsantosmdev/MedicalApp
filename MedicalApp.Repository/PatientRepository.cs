using MedicalApp.DataAccess;
using MedicalApp.Entities;

namespace MedicalApp.Repository
{
    public interface IPatientRepository : IRepository<Patient> { }
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        IPatientDbContext _ctx;
        public PatientRepository(IPatientDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}
