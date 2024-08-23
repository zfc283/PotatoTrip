using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class TravelRoutePictureForCreationDTO
    {
        [Required]
        [MaxLength(100)]
        public string Url { get; set; }
    }
}
