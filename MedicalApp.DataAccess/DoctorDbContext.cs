using MedicalApp.Abstractions;
using MedicalApp.Entities;
using System.Linq;

namespace MedicalApp.DataAccess
{
    public interface IDoctorDbContext : IDbContext<Doctor>
    {
        Doctor GetByUserId(int id);
    }
    public class DoctorDbContext : DbContext<Doctor>, IDoctorDbContext
    {
        public DoctorDbContext(ApiDbContext ctx) : base(ctx)
        {
        }

        public Doctor GetByUserId(int id)
        {
            return GetAll().FirstOrDefault(x => x.UserId == id);
        }
    }
}
