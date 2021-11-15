using MedicalApp.Abstractions;
using MedicalApp.Entities;

namespace MedicalApp.DataAccess
{
    public interface IClinicDbContext : IDbContext<Clinic>
    {

    }
    public class ClinicDbContext : DbContext<Clinic>, IClinicDbContext
    {
        public ClinicDbContext(ApiDbContext ctx) : base(ctx)
        {

        }
    }
}
