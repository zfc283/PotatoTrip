using AutoMapper;
using System;
using System.Globalization;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Profiles
{
    public static class TravelRouteForCreationDTOConverter
    {
        public class StringToTripTypeConverter : IValueConverter<string, TripType?>
        {
            public TripType? Convert(string sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(sourceMember))
                {
                    return null;
                }
                if (Enum.TryParse<TripType>(sourceMember, true, out var tripType))
                {
                    return tripType;
                }
                throw new ArgumentException($"Unknown trip type: {sourceMember}");
            }
        }

        public class StringToTravelDaysConverter : IValueConverter<string, TravelDays?>
        {
            public TravelDays? Convert(string sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(sourceMember))
                {
                    return null;
                }
                if (Enum.TryParse<TravelDays>(sourceMember, true, out var travelDays))
                {
                    return travelDays;
                }
                throw new ArgumentException($"Unknown travel days: {sourceMember}");
            }
        }

        public class StringToDepartureCityConverter : IValueConverter<string, DepartureCity?>
        {
            public DepartureCity? Convert(string sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(sourceMember))
                {
                    return null;
                }
                if (Enum.TryParse<DepartureCity>(sourceMember, true, out var departureCity))
                {
                    return departureCity;
                }
                throw new ArgumentException($"Unknown departure city: {sourceMember}");
            }
        }

        /*public static DateTime? StringToNullableDateTime(String source)
        {
            if (!string.IsNullOrEmpty(source) && DateTime.TryParseExact(source, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }

            return null;
        }*/

        public class DateTimeTypeConverter : IValueResolver<TravelRouteForCreationDTO, TravelRoute, DateTime?>
        {
            public DateTime? Resolve(TravelRouteForCreationDTO source, TravelRoute destination, DateTime? destMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source.DepartureTime) && DateTime.TryParse(source.DepartureTime, out DateTime parsedDate))
                {
                    return parsedDate;
                }

                return null; // Or return a default value as necessary
            }
        }
    }
}
