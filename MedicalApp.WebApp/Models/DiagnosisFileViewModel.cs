using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Models
{
    public class DiagnosisFileViewModel
    {
        public int Id { get; set; }
        public int DiagnosisId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
