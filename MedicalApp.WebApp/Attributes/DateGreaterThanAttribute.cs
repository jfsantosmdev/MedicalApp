using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Attributes
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        public DateGreaterThanAttribute(string dateToCompareToFieldName)
        {
            DateToCompareToFieldName = dateToCompareToFieldName;
        }

        private string DateToCompareToFieldName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if(value == null)
                return new ValidationResult(string.Empty);

            DateTime earlierDate = (DateTime)value;

            var aux = validationContext.ObjectType.GetProperty(DateToCompareToFieldName).GetValue(validationContext.ObjectInstance, null);
            if(aux == null)
                return ValidationResult.Success;

            DateTime laterDate = (DateTime)aux;

            if (laterDate > earlierDate)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("La Fecha/Hora Inicio no puede ser mayor a la Fecha/Hora Fin");
            }
        }
    }
}
