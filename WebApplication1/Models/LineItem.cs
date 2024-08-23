using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebApplication1.Models
{
    public class LineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("TravelRouteId")]
        public Guid TravelRouteId { get; set; }
        public TravelRoute TravelRoute { get; set; }
        public Guid? ShoppingCartId { get; set; }
        public Guid? OrderId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPercent { get; set; }
    }
}
