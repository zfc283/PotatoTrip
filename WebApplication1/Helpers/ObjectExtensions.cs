using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace WebApplication1.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<T>(this T source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

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

            var dataShapedObject = new ExpandoObject();

            foreach (var propertyInfo in propertyInfoList)
            {
                var propertyValue = propertyInfo.GetValue(source);

                ((IDictionary<string, object>)dataShapedObject)      // ExpandoObject 在底层实现上实际是字符串与对象的字典类型
                    .Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;
        }
    }
}
