using MedicalApp.Abstractions;
using MedicalApp.Entities;

namespace MedicalApp.DataAccess
{
    public interface IPatientDbContext : IDbContext<Patient>
    {

    }
    public class PatientDbContext : DbContext<Patient>, IPatientDbContext
    {
        public PatientDbContext(ApiDbContext ctx) : base(ctx)
        {

        }
    }
}
