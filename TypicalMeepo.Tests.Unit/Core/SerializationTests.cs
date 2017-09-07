using System.Collections.Generic;
using NUnit.Framework;
using TypicalMeepo.Core.Extensions;

namespace TypicalMeepo.Tests.Unit.Core
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void Serialization_WhenCalled_ShouldRoundTripData()
        {
            var package = new TestPackage
            {
                AddressLines = new List<string> { "Line 1", "Line 2" },
                Name = "Meepo"
            };

            var roundTripData = package.PackageToBytes().BytesToPackage<TestPackage>();

            Assert.AreEqual(package.Name, roundTripData.Name);
            Assert.AreEqual(package.AddressLines.Count, roundTripData.AddressLines.Count);

            Assert.AreEqual(package.AddressLines[0], roundTripData.AddressLines[0]);
            Assert.AreEqual(package.AddressLines[1], roundTripData.AddressLines[1]);
        }
    }
}
