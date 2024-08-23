using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Profiles
{
    public class TravelRoutePictureProfile : Profile
    {
        public TravelRoutePictureProfile()
        {
            CreateMap<TravelRoutePicture, TravelRoutePictureDTO>();
            CreateMap<TravelRoutePictureForCreationDTO, TravelRoutePicture>();
            CreateMap<TravelRoutePicture, TravelRoutePictureForCreationDTO>();
        }
    }
}
