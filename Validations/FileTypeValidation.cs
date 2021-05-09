using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MoviesApi.Validations
{
    public class FileTypeValidation: ValidationAttribute
    {
        private readonly string[] validTypes;

        public FileTypeValidation(string[] validTypes)
        {
            this.validTypes = validTypes;
        }

        public FileTypeValidation(FileType fileType)
        {
            if(fileType == FileType.Image)
            {
                validTypes = new string[] { "image/png", "image/jpg", "image/jpeg", "image/gif" };
            }
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

            if (!validTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult($"Solo se aceotan los tipos de arhivos {string.Join(", ", validTypes)}");
            }

            return ValidationResult.Success;
        }
    }
}
