using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using WebApplication1.ValidationAttributes;

namespace WebApplication1.DTOs
{
    /*[TravelRouteTitleMustBeDifferentFromDescription]
    public abstract class TravelRouteForManipulationDTO     // : IValidatableObject
    {
        [Required(ErrorMessage = "Title can not be null")]         // 数据验证
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public virtual string Description { get; set; }
        // 计算方式: Model中的 OriginalPrice * DiscountPercent
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public Nullable<DateTime> DepartureTime { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }

        public ICollection<TravelRoutePictureForCreationDTO> TravelRoutePictures { get; set; }

        public double? Rating { get; set; }
        public string TravelDays { get; set; }
        public string TripType { get; set; }
        public string DepartureCity { get; set; }


        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult(
        //              "Title and Description must be different",     // Error message
        //              new[] { "TravelRouteForCreationDTO" }    // The path through which error originates
        //        );
        //    }
        //}
    }*/

    [TravelRouteTitleMustBeDifferentFromDescription]
    public abstract class TravelRouteForManipulationDTO
    {
        [Required(ErrorMessage = "Title can not be null")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public virtual string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPercent { get; set; }
        /*public DateTime CreateTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }*/
        public string DepartureTime { get; set; }
        [MaxLength]
        public string Features { get; set; }
        [MaxLength]
        public string Fees { get; set; }
        [MaxLength]
        public string Notes { get; set; }


        public ICollection<TravelRoutePictureForCreationDTO> TravelRoutePictures { get; set; }

        public double? Rating { get; set; }
        public string TravelDays { get; set; }
        public string TripType { get; set; }
        public string DepartureCity { get; set; }
    }
}
