using System;
using System.Reflection;

namespace TypicalMeepo.Core.Attributes
{
    internal static class AttributeExtensions
    {
        public static MeepoPackageAttribute GetMeepoPackageAttribute(this Type type)
        {
            return type.GetTypeInfo().GetCustomAttribute<MeepoPackageAttribute>();
        }
    }
}
