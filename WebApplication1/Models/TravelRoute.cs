using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class TravelRoute
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public Nullable<double> DiscountPercent { get; set; }
        public DateTime CreateTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public Nullable<DateTime> DepartureTime { get; set; }
        [MaxLength]
        public string Features { get; set; }
        [MaxLength]
        public string Fees { get; set; }
        [MaxLength]
        public string Notes { get; set; }

        // When we query a TravelRoute entity, by default, only the primary properties are loaded.
        // Navigation properties (like TravelRoutePictures) are not loaded unless explicitly told to do so
        // The Include() function allows us to bring out navigation properties along with the primary properties (eager loading). And it doesn't explicitly require the foreign keys of the navigation properties in order to function (foreign keys are handled internally)
        // In comparison, Join() also achieves eager loading, but requires us to specify the connected keys 
        // Use eager loading in scenarios where we need the navigation properties for every entity retrieved

        public ICollection<TravelRoutePicture> TravelRoutePictures { get; set; }      // Navigation property, 同时也是嵌套子资源
            = new List<TravelRoutePicture>();

        public double? Rating { get; set; }
        public TravelDays? TravelDays { get; set; }
        public TripType? TripType { get; set; }
        public DepartureCity? DepartureCity { get; set; }

    }
}
