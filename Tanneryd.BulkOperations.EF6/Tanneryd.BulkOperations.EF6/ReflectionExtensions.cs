using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tanneryd.BulkOperations.EF6
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo GetPropertyUnambiguous(this Type type, string name, BindingFlags? flags = null)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (name == null) throw new ArgumentNullException(nameof(name));

            if (flags.HasValue)
            {
                flags |= BindingFlags.DeclaredOnly;
            }
            else
            {
                flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
            }

            while (type != null)
            {
                var property = type.GetProperty(name, flags.Value);

                if (property != null)
                {
                    return property;
                }

                type = type.BaseType;
            }

            return null;
        }

        public static PropertyInfo[] GetPropertiesUnambiguous(this Type type, BindingFlags? flags = null)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (flags.HasValue)
            {
                flags |= BindingFlags.DeclaredOnly;
            }
            else
            {
                flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
            }

            List<PropertyInfo> list = new List<PropertyInfo>();

            while (type != null)
            {
                var properties = type.GetProperties(flags.Value);

                foreach (var property in properties)
                {
                    if (property != null && !list.Any(x => x.Name == property.Name))
                    {
                        list.Add(property);
                    }
                }

                type = type.BaseType;
            }

            return list.ToArray();
        }
    }
}
