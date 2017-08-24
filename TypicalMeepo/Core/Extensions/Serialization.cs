using System;
using System.IO;
using System.Linq;
using TypicalMeepo.Core.Repo;
using Meepo.Core.Extensions;
using Newtonsoft.Json;

namespace TypicalMeepo.Core.Extensions
{
    internal static class Serialization
    {
        public static byte[] PackageToBytes<T>(this T package, ITypeRepo repo)
        {
            var type = repo.GetId(typeof(T));

            var typeBytes = BitConverter.GetBytes(type);

            var messageBytes = JsonConvert.SerializeObject(package).Encode();

            var result = new byte[messageBytes.Length + 4];

            typeBytes.CopyTo(result, 0);
            messageBytes.CopyTo(result, 4);

            return result;
        }

        public static (int, object) BytesToPackage(this byte[] bytes, ITypeRepo repo)
        {
            var typeBytes = bytes.Take(4).ToArray();

            var typeValue = BitConverter.ToInt32(typeBytes, 0);

            var message = bytes.Skip(4).ToArray().Decode();

            return (typeValue, JsonConvert.DeserializeObject(message, repo.GetType(typeValue)));
        }
    }
}
