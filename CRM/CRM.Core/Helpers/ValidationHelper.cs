
using System.ComponentModel.DataAnnotations;

namespace CRM.Core.Helpers;
public static class ValidationHelper
{
    public static void ObjectValidator(object obj)
    {
        ValidationContext context = new ValidationContext(obj);
        List<ValidationResult> results = new List<ValidationResult>();
        bool isValid =  Validator.TryValidateObject(obj, context, results);
        if (!isValid)
        {
            throw new ArgumentException(results.Select(err => err.ErrorMessage).First());
        }
    }
}
