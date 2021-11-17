﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApi.DTOs
{
    public class DiagnosisFileDTO
    {
        public int DiagnosisId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
