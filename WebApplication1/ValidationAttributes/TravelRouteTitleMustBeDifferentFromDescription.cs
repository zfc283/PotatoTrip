using System.ComponentModel.DataAnnotations;
using WebApplication1.DTOs;

namespace WebApplication1.ValidationAttributes
{
    public class TravelRouteTitleMustBeDifferentFromDescription : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value,                              // This parameter represents the value of the property on which the attribute is applied. It is used to perform the validation check specific to that property's value
            ValidationContext validationContext          // This parameter provides context about the model object being validated. It includes metadata about the model, such as the entire object instance, which allows for more complex validations involving multiple properties of the model
        )
        {
            var travelRouteForManipulationDTO = (TravelRouteForManipulationDTO)validationContext.ObjectInstance;

            if (travelRouteForManipulationDTO.Title == travelRouteForManipulationDTO.Description)
            {
                return new ValidationResult(
                      "Title and Description must be different",     // Error message
                      new[] { "TravelRouteForManipulationDTO" }    // The path through which error originates
                );
            }

            return ValidationResult.Success;
        }
    }
}
