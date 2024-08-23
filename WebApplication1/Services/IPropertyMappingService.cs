using System.Collections.Generic;

namespace WebApplication1.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool IsPropertyMappingExist<TSource, TDestination>(string fields);

        bool PropertiesExist<T>(string fields);
    }
}