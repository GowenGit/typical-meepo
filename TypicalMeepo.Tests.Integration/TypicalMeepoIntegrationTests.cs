using System;
using System.Threading.Tasks;
using Meepo.Core.Exceptions;
using Meepo.Core.Logging;
using Moq;
using NUnit.Framework;

namespace TypicalMeepo.Tests.Integration
{
    [TestFixture]
    [NonParallelizable]
    public class TypicalMeepoIntegrationTests
    {
        private Mock<ILogger> clientLogger;
        private Mock<ILogger> serverLogger;

        private IntegrationPackage package;

        private readonly TypicalMeepoNode server;
        private readonly TypicalMeepoNode client;

        public TypicalMeepoIntegrationTests()
        {
            InitializeLoggers();

            server = IntegrationTestHelpers.StartServer<IntegrationPackage>(9313, serverLogger.Object, OnMessageReceived);
            client = IntegrationTestHelpers.StartClient<IntegrationPackage>(9314, 9313, clientLogger.Object, OnMessageReceived, 10000);

            Task.Delay(IntegrationTestHelpers.WaitTime).Wait();
        }

        private void InitializeLoggers()
        {
            clientLogger = new Mock<ILogger>(MockBehavior.Strict);

            clientLogger.Setup(x => x.Message(It.IsAny<string>()));

            serverLogger = new Mock<ILogger>(MockBehavior.Strict);

            serverLogger.Setup(x => x.Message(It.IsAny<string>()));
        }

        [SetUp]
        public void Initialize()
        {
            package = null;
        }

        [Test]
        public void TypicalMeepo_WhenServerStarted_ClientShouldBeAbleToConnect()
        {
            serverLogger.Verify(x => x.Message("Connection accepted"), Times.Once);

            clientLogger.Verify(x => x.Message("Connection accepted from 127.0.0.1:9313"), Times.Once);
        }

        private void OnMessageReceived(Guid id, IntegrationPackage data)
        {
            package = data;
        }

        [Test]
        public void SendAsync_WhenCalled_ServerShouldGetTheMessage()
        {
            var message = new IntegrationPackage
            {
                Name = "Typical",
                Surname = "Meepo",
                Age = 12
            };

            client.SendAsync(message).Wait();

            Task.Delay(IntegrationTestHelpers.WaitTime).Wait();

            Assert.AreEqual(message.Name, package.Name);
            Assert.AreEqual(message.Surname, package.Surname);
            Assert.AreEqual(message.Age, package.Age);
        }

        [Test]
        public void SendAsync_WhenCalled_ClientShouldGetTheMessage()
        {
            var message = new IntegrationPackage
            {
                Name = "James",
                Surname = "Bond",
                Age = 37
            };

            server.SendAsync(message).Wait();

            Task.Delay(IntegrationTestHelpers.WaitTime).Wait();

            Assert.AreEqual(message.Name, package.Name);
            Assert.AreEqual(message.Surname, package.Surname);
            Assert.AreEqual(message.Age, package.Age);
        }

        [Test]
        public void SendAsync_WhenCalledWithInvalidId_ShouldThrow()
        {
            Assert.Throws<MeepoException>(() => server.SendAsync(Guid.NewGuid(), "").GetAwaiter().GetResult());
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            server.Stop();
            client.Stop();
        }
    }
}
