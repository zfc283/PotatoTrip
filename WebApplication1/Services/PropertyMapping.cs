using System.Collections.Generic;

namespace WebApplication1.Services
{

    // 其实 PropertyMapping 就是 Dictionary<string, PropertyMappingValue>, 只不过为了加上泛型 (为了限制转化的类型) 才创造的这个 PropertyMapping 类

    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> _mappingDictionary { get; set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }
    }
}
