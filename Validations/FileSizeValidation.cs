using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Validations
{
    public class FileSizeValidation : ValidationAttribute
    {
        private readonly int _maxSize;

        public FileSizeValidation(int maxSize)
        {
            _maxSize = maxSize;

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if(formFile == null)
            {
                return ValidationResult.Success;
            }

            if(formFile.Length > _maxSize * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no debe exceder los {_maxSize}MB");
            }

            return ValidationResult.Success;
        }
    }
}
