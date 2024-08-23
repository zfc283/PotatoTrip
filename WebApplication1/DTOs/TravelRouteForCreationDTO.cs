using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.DTOs
{

    public class TravelRouteForCreationDTO : TravelRouteForManipulationDTO
    {

    }
    /*[TravelRouteTitleMustBeDifferentFromDescription]
    public class TravelRouteForCreationDTO 
    {
        [Required(ErrorMessage = "Title can not be null")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPercent { get; set; }
        *//*public DateTime CreateTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }*//*
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
    }*/
}
