using MedicalApp.Abstractions;
using MedicalApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalApp.DataAccess
{
    public interface IDiagnosisFileDbContext : IDbContext<DiagnosisFile>
    {
    }
    public class DiagnosisFileDbContext : DbContext<DiagnosisFile>, IDiagnosisFileDbContext
    {
        public DiagnosisFileDbContext(ApiDbContext ctx) : base(ctx) { }
 
    }
}
