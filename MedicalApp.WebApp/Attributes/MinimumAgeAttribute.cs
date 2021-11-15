using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.WebApp.Attributes
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
            ErrorMessage = "{0} debe ser alguien de al menos {1} años de edad";
        }

        public override bool IsValid(object value)
        {
            DateTime date;
            if (value == null)
                return false;

            if (DateTime.TryParse(value.ToString(), out date))
                return date.AddYears(_minimumAge) < DateTime.Now;

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, _minimumAge);
        }
    }
}
