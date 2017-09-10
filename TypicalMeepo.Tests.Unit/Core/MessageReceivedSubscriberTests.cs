using System;
using System.Collections.Generic;
using Meepo.Core.Extensions;
using NUnit.Framework;
using TypicalMeepo.Core.Events;
using TypicalMeepo.Core.Exceptions;
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

        [Test]
        public void OnMessageReceived_WhenCalledWithAssemblyNameTwice_ShouldThrow()
        {
            var subscriber = new MessageReceivedSubscriber<TestPackage>((id, data) => { });

            var clientId = Guid.NewGuid();

            var type = typeof(TestPackage);

            subscriber.OnMessageReceived(new MessageReceivedEventArgs(clientId, type.AssemblyQualifiedName.Encode()));

            Assert.Throws<TypicalMeepoException>(() => subscriber.OnMessageReceived(new MessageReceivedEventArgs(clientId, type.AssemblyQualifiedName.Encode())));
        }

        [Test]
        public void OnMessageReceived_WhenCalledWithoutAssemblyName_ShouldIgnoreCall()
        {
            var result = "";

            const string expectedResult = "Meepo";

            var subscriber = new MessageReceivedSubscriber<string>((id, data) => { result = data; });

            var clientId = Guid.NewGuid();

            subscriber.OnMessageReceived(new MessageReceivedEventArgs(clientId, "".Encode()));

            subscriber.OnMessageReceived(new MessageReceivedEventArgs(clientId, typeof(string).AssemblyQualifiedName.Encode()));

            subscriber.OnMessageReceived(new MessageReceivedEventArgs(clientId, expectedResult.PackageToBytes()));

            Assert.AreEqual(expectedResult, result);
        }
    }
}
