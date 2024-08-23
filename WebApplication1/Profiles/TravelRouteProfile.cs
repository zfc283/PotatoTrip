using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Profiles
{
    public class TravelRouteProfile : Profile
    {
        public TravelRouteProfile()
        {
            // 两个对象相同的字段会被自动映射，找不到匹配的字段会被赋值为默认值或 null, 我们也可以自定义映射关系
            CreateMap<TravelRoute, TravelRouteDTO>()
                .ForMember(
                    dest => dest.Price,
                    src => src.MapFrom(src => src.OriginalPrice * (decimal)(src.DiscountPercent ?? 1))
                )
                .ForMember(
                    dest => dest.TravelDays,
                    opt => opt.MapFrom(src => src.TravelDays.ToString())
                )
                .ForMember(
                    dest => dest.TripType,
                    opt => opt.MapFrom(src => src.TripType.ToString())
                )
                .ForMember(
                    dest => dest.DepartureCity,
                    opt => opt.MapFrom(src => src.DepartureCity.ToString())
                );

            CreateMap<TravelRouteForCreationDTO, TravelRoute>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid())
                )
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => DateTime.Now)
                )
                .ForMember(
                    dest => dest.UpdateTime,
                    opt => opt.MapFrom(src => DateTime.Now)
                )
                .ForMember(
                    dest => dest.DepartureTime,
                    opt => opt.MapFrom<TravelRouteForCreationDTOConverter.DateTimeTypeConverter>()
                )
                .ForMember(
                    dest => dest.TripType,
                    opt => opt.ConvertUsing(new TravelRouteForCreationDTOConverter.StringToTripTypeConverter(), src => src.TripType)
                )
                .ForMember(
                    dest => dest.TravelDays,
                    opt => opt.ConvertUsing(new TravelRouteForCreationDTOConverter.StringToTravelDaysConverter(), src => src.TravelDays)
                )
                .ForMember(
                    dest => dest.DepartureCity,
                    opt => opt.ConvertUsing(new TravelRouteForCreationDTOConverter.StringToDepartureCityConverter(), src => src.DepartureCity)
                );

            CreateMap<TravelRouteForUpdateDTO, TravelRoute>();

            CreateMap<TravelRoute, TravelRouteForUpdateDTO>();
        }
    }
}
