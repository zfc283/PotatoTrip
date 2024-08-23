using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace WebApplication1.Helpers
{
    public static class IEnumerableExtensions
    {

        // 这种数据塑形方法非常消耗资源。当数据量大时，这种方法非常耗时
        // 在处理大量数据的塑形时，一般会使用 elasticsearch 

        public static IEnumerable<ExpandoObject> ShapeData<T>(this IEnumerable<T> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObjectList = new List<ExpandoObject>();

            // 获取 PropertyInfo，需要用到反射。为了避免在列表中遍历数据，创建一个属性信息列表
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // 希望返回 T 类的所有属性
                var propertyInfos = typeof(T)
                    .GetProperties(BindingFlags.IgnoreCase
                    | BindingFlags.Public | BindingFlags.Instance);       // BindingFlags.Instance => non-static

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                //逗号来分隔字段字符串
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
                        throw new Exception($"属性 {propertyName} 找不到" +
                            $" {typeof(T)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }


            foreach (T sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject)
                        .Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);
            }

            return expandoObjectList;
        }
    }
}
