using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class TravelRoutePicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Url { get; set; }
        [ForeignKey("TravelRouteId")]
        public Guid TravelRouteId { get; set; }            // Foreign key
        public TravelRoute TravelRoute { get; set; }        // Navigation property


    }
}
