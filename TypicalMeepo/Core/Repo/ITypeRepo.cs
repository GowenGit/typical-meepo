using System;

namespace TypicalMeepo.Core.Repo
{
    internal interface ITypeRepo
    {
        Type GetType(int packageType);

        int GetId(Type type);
    }
}