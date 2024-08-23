using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class TravelRouteDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // 计算方式: Model中的 OriginalPrice * DiscountPercent
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public Nullable<double> DiscountPercent { get; set; }
        public DateTime CreateTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public Nullable<DateTime> DepartureTime { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }

        public double? Rating { get; set; }
        public string TravelDays { get; set; }       
        public string TripType { get; set; }
        public string DepartureCity { get; set; }

        public ICollection<TravelRoutePictureDTO> TravelRoutePictures { get; set; }

        //public TravelRouteDTO(TravelRoute travelRoute)
        //{
        //    Id = travelRoute.Id;
        //    Title = travelRoute.Title;
        //    Description = travelRoute.Description;
        //    Price = travelRoute.OriginalPrice * (decimal)(travelRoute.DiscountPercent ?? 1);
        //    CreateTime = travelRoute.CreateTime;
        //    UpdateTime = travelRoute.UpdateTime;
        //    Features = travelRoute.Features;
        //    Fees = travelRoute.Fees;
        //    Notes = travelRoute.Notes;
        //    Rating = travelRoute.Rating;
        //    TravelDays = travelRoute.TravelDays.ToString();
        //    TripType = travelRoute.TripType.ToString();
        //    DepartureCity = travelRoute.DepartureCity.ToString();
        //}
    }
}
