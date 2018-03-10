using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Manager.Struct.DTO.Validations;

namespace Manager.Struct.DTO
{
     public class UserDto : IValidatableObject
     {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public string Profession { get; set; }
        public int SchedulesCreated { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new UserDtoValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => 
                new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
