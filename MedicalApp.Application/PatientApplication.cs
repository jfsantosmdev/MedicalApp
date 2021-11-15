using MedicalApp.Entities;
using MedicalApp.Repository;

namespace MedicalApp.Application
{
    public interface IPatientApplication : IApplication<Patient>
    {

    }
    public class PatientApplication : Application<Patient>, IPatientApplication
    {
        IPatientRepository _repo;
        public PatientApplication(IPatientRepository repo) : base(repo)
        {
            _repo = repo;
        }
    }
}
