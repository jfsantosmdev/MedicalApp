using MedicalApp.DataAccess;
using MedicalApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalApp.Repository
{
    public interface IDiagnosisFileRepository : IRepository<DiagnosisFile>
    {
    }
    public class DiagnosisFileRepository : Repository<DiagnosisFile>, IDiagnosisFileRepository
    {
        IDiagnosisFileDbContext _ctx;
        public DiagnosisFileRepository(IDiagnosisFileDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}
