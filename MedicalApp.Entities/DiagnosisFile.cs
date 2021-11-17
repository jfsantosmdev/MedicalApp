namespace MedicalApp.Entities
{
    public class DiagnosisFile : Entity
    {
        public int DiagnosisId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Diagnosis Diagnosis { get; set; }
    }
}
