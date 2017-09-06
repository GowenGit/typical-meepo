using System;
using System.Reflection;
using Meepo.Core.Extensions;
using Newtonsoft.Json;

namespace TypicalMeepo.Core.Extensions
{
    internal static class Serialization
    {
        public static byte[] PackageToBytes<T>(this T package)
        {
            var messageBytes = JsonConvert.SerializeObject(package).Encode();

            return messageBytes;
        }

        public static T BytesToPackage<T>(this byte[] bytes)
        {
            return (T) JsonConvert.DeserializeObject(bytes.Decode(), typeof(T));
        }

        public static string GetAssemblyName(this Type type)
        {
            return type.GetTypeInfo().AssemblyQualifiedName;
        }
    }
}
