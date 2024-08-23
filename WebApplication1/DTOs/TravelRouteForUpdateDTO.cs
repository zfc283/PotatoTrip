using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebApplication1.DTOs
{
    public class TravelRouteForUpdateDTO : TravelRouteForManipulationDTO
    {
        [Required(ErrorMessage ="Description is required for update")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
