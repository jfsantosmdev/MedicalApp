using MedicalApp.Entities;
using MedicalApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalApp.Application
{
    public interface IDiagnosisApplication : IApplication<Diagnosis>
    {
        Diagnosis GetByAppointmentId(int id);
    }
    public class DiagnosisApplication : Application<Diagnosis>, IDiagnosisApplication
    {
        IDiagnosisRepository _repo;
        public DiagnosisApplication(IDiagnosisRepository repo) : base(repo)
        {
            _repo = repo;
        }
        public Diagnosis GetByAppointmentId(int id)
        {
            return _repo.GetByAppointmentId(id);
        }
    }
}
