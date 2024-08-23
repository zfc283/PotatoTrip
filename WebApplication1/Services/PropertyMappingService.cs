using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services
{

    // PropertyMappingService 包含了整个项目各个类之间的转化关系, 核心成员是 IList<IPropertyMapping> _propertyMappings

    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _travelRoutePropertyMapping =      // 填充数据
               new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
               {
                   {"Id", new PropertyMappingValue(new List<string>(){"Id"}) },
                   {"Title", new PropertyMappingValue(new List<string>(){"Title"}) },
                   {"OriginalPrice", new PropertyMappingValue(new List<string>(){"OriginalPrice"}) },
                   {"DepartureTime", new PropertyMappingValue(new List<string>(){"DepartureTime"}) },
                   {"Rating", new PropertyMappingValue(new List<string>(){"Rating"}) },
                   {"TravelDays", new PropertyMappingValue(new List<string>(){"TravelDays"}) }
               };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();      // 主要成员

        public PropertyMappingService()
        {
            _propertyMappings.Add(
                new PropertyMapping<TravelRouteDTO, TravelRoute>(_travelRoutePropertyMapping)
                );
        }

        // 找到对应类型的转化关系

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool IsPropertyMappingExist<TSource, TDestination>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            var orderByAfterSplit = fields.Split(',');

            foreach (var order in orderByAfterSplit)
            {
                var trimmedOrder = order.Trim();

                var indexOfFirstSpace = trimmedOrder.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrder
                    : trimmedOrder.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }

            return true;
        }

        // Return true if all properties listed in fields exist in T, false otherwise
        public bool PropertiesExist<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');

            foreach (var filed in fieldsAfterSplit)
            {
                // 去掉首尾多余的空格，获得属性名称
                var propertyName = filed.Trim();

                var propertyInfo = typeof(T)
                    .GetProperty(propertyName, BindingFlags.IgnoreCase
                | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
