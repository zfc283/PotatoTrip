using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class LineItemDTO
    {
        public int Id { get; set; }
        public Guid TravelRouteId { get; set; }
        public TravelRouteDTO TravelRoute { get; set; }
        public Guid? ShoppingCartId { get; set; }
        public Guid? OrderId { get; set; }
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPercent { get; set; }
    }
}
