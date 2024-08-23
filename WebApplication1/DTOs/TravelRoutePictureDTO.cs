using System;
using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class TravelRoutePictureDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid TravelRouteId { get; set; }
    }
}
