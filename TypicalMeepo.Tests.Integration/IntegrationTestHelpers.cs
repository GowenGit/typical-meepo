using System.Net;
using Meepo.Core.Configs;
using Meepo.Core.Logging;
using TypicalMeepo.Core.Events;

namespace TypicalMeepo.Tests.Integration
{
    internal static class IntegrationTestHelpers
    {
        public const int WaitTime = 1000;

        public static TypicalMeepoNode StartServer<T>(int port, ILogger logger, MessageReceivedHandler<T> messageReceived, int buffer = 1000)
        {
            var address = new TcpAddress(IPAddress.Loopback, port);

            var config = new MeepoConfig
            {
                Logger = logger,
                BufferSizeInBytes = buffer
            };

            var tmp = new TypicalMeepoNode(address, config);

            tmp.Subscribe(messageReceived);

            tmp.Start();

            return tmp;
        }

        public static TypicalMeepoNode StartClient<T>(int port, int serverPort, ILogger logger, MessageReceivedHandler<T> messageReceived, int buffer = 1000)
        {
            var address = new TcpAddress(IPAddress.Loopback, port);

            var serverAddress = new[] { new TcpAddress(IPAddress.Loopback, serverPort) };

            var config = new MeepoConfig
            {
                Logger = logger,
                BufferSizeInBytes = buffer
            };

            var tmp = new TypicalMeepoNode(address, serverAddress, config);

            tmp.Subscribe(messageReceived);

            tmp.Start();

            return tmp;
        }
    }
}
