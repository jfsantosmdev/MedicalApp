using MedicalApp.Entities;
using MedicalApp.Repository;

namespace MedicalApp.Application
{
    public interface IClinicApplication : IApplication<Clinic>
    {

    }
    public class ClinicApplication : Application<Clinic>, IClinicApplication
    {
        IClinicRepository _repo;
        public ClinicApplication(IClinicRepository repo) : base(repo)
        {
            _repo = repo;
        }
    }
}
