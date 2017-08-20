using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypicalMeepo.Core.Attributes;
using TypicalMeepo.Core.Exceptions;

namespace TypicalMeepo.Core.Repo
{
    internal class TypeRepo : ITypeRepo
    {
        private readonly Dictionary<int, Type> mappedTypes = new Dictionary<int, Type>();

        public TypeRepo(IEnumerable<Assembly> assemblies)
        {
            MapTypes(assemblies);
        }

        private void MapTypes(IEnumerable<Assembly> assemblies)
        {
            var count = 0;

            var validTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attribute = type.GetMeepoPackageAttribute();

                    if (attribute == null) continue;

                    validTypes.Add(type);
                }
            }

            var orderedTypes = validTypes.OrderBy(x => x.AssemblyQualifiedName);

            foreach (var orderedType in orderedTypes)
            {
                mappedTypes[count] = orderedType;
                count++;
            }
        }

        public Type GetType(int packageType)
        {
            if (!mappedTypes.ContainsKey(packageType)) throw new TypicalMeepoException($"No type was found for package id: {packageType}!");

            return mappedTypes[packageType];
        }

        public int GetId(Type type)
        {
            if (!mappedTypes.ContainsValue(type)) throw new TypicalMeepoException($"No type was found for package type: {type}!");

            return mappedTypes.First(x => x.Value == type).Key;
        }
    }
}
