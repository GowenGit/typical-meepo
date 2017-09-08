using System;
using System.Collections.Generic;
using Meepo.Core.Extensions;
using NUnit.Framework;
using TypicalMeepo.Core.Events;
using TypicalMeepo.Core.Extensions;

namespace TypicalMeepo.Tests.Unit.Core
{
    [TestFixture]
    public class MessageReceivedSubscriberTests
    {
        [Test]
        public void OnMessageReceived_WhenCalledInTheRightOrder_ShouldDeserialize()
        {
            TestPackage result = null;

            var package = new TestPackage
            {
                AddressLines = new List<string> { "Line 1", "Line 2" },
                Name = "Meepo"
            };

            var subscriber = new MessageReceivedSubscriber<TestPackage>((id, data) => { result = data; });

            var clientId = Guid.NewGuid();

            var type = typeof(TestPackage);

            subscriber.OnMessageReceived(new MessageReceivedEventArgs(clientId, type.AssemblyQualifiedName.Encode()));

            subscriber.OnMessageReceived(new MessageReceivedEventArgs(clientId, package.PackageToBytes()));

            Assert.AreEqual(package.Name, result.Name);
            Assert.AreEqual(package.AddressLines.Count, result.AddressLines.Count);

            Assert.AreEqual(package.AddressLines[0], result.AddressLines[0]);
            Assert.AreEqual(package.AddressLines[1], result.AddressLines[1]);
        }
    }
}
