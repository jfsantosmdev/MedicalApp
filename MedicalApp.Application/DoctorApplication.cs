using MedicalApp.Entities;
using MedicalApp.Repository;

namespace MedicalApp.Application
{
    public interface IDoctorApplication : IApplication<Doctor>
    {
        Doctor GetByUserId(int id);
    }
    public class DoctorApplication : Application<Doctor>, IDoctorApplication
    {
        IDoctorRepository _repo;
        public DoctorApplication(IDoctorRepository repo) : base(repo)
        {
            _repo = repo;
        }

        public Doctor GetByUserId(int id)
        {
            return _repo.GetByUserId(id);
        }
    }
}
