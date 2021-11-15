using MedicalApp.DataAccess;
using MedicalApp.Entities;

namespace MedicalApp.Repository
{
    public interface IClinicRepository : IRepository<Clinic>
    {

    }
    public class ClinicRepository : Repository<Clinic>, IClinicRepository
    {
        IClinicDbContext _ctx;

        public ClinicRepository(IClinicDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}
