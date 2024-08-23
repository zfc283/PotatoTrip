using System.Collections;
using System.Collections.Generic;

namespace WebApplication1.Services
{
    public class PropertyMappingValue
    {
        // 将会被映射的目标的属性
        public IEnumerable<string> DestinationProperties { get; private set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties)
        {
            DestinationProperties = destinationProperties;
        }
    }
}
