using MedicalApp.Entities;
using MedicalApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalApp.Application
{
    public interface IDiagnosisFileApplication : IApplication<DiagnosisFile>
    {
    }
    public class DiagnosisFileApplication : Application<DiagnosisFile>, IDiagnosisFileApplication
    {
        IDiagnosisFileRepository _repo;
        public DiagnosisFileApplication(IDiagnosisFileRepository repo) : base(repo)
        {
            _repo = repo;
        }
    }
}
