using System.Collections.Generic;
using TypicalMeepo.Core.Attributes;

namespace TypicalMeepo.Tests.Unit.Core
{
    [MeepoPackage]
    internal class TestPackage
    {
        public string Name { get; set; }

        public List<string> AddressLines { get; set; }
    }
}
