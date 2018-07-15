using System;
using System.Net;
using System.Threading;
using Meepo.Core.Configs;
using Meepo.Core.Logging;

namespace TypicalMeepo.Console
{
    public class Server
    {
        private static ITypicalMeepoNode meepoNode;

        public static void Main()
        {
            var config = new MeepoConfig
            {
                Logger = new ConsoleLogger(),
                BufferSizeInBytes = 1000
            };

            var address = new TcpAddress(IPAddress.Loopback, 9200);

            using (meepoNode = new TypicalMeepoNode(address, config))
            {
                meepoNode.Start();

                meepoNode.Subscribe<Info>(OnInfoReceived);

                while (true)
                {
                    var text = System.Console.ReadLine();

                    if (text.ToLower() == "q") return;
                }
            }
        }

        public static void OnInfoReceived(Guid id, Info info)
        {
            System.Console.WriteLine($"Client ID: {id}");
            System.Console.WriteLine($"Date: {info.Date}");
            System.Console.WriteLine($"Message: {info.Message}");
        }
    }
}