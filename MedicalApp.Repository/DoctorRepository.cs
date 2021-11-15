using MedicalApp.DataAccess;
using MedicalApp.Entities;
using System.Collections.Generic;

namespace MedicalApp.Repository
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        public Doctor GetByUserId(int id);
        
    }
    class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly IDoctorDbContext _ctx;
        public DoctorRepository(IDoctorDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public Doctor GetByUserId(int id)
        {
            return _ctx.GetByUserId(id);
        }
    }
}
