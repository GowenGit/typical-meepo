using System;
using System.Net;
using Meepo.Core.Configs;
using Meepo.Core.Logging;

namespace TypicalMeepo.Console
{
    public class Program
    {
        private static ITypicalMeepoNode meepoNode;

        public static void Main()
        {
            var config = new MeepoConfig
            {
                Logger = new ConsoleLogger(),
                BufferSizeInBytes = 1000000
            };

            var address = new TcpAddress(IPAddress.Loopback, 9200);
            var serverAddresses = new[] { new TcpAddress(IPAddress.Loopback, 9201) };

            using (meepoNode = new TypicalMeepoNode(address, serverAddresses, config))
            {
                meepoNode.Start();

                meepoNode.Subscribe<Info>(OnInfoReceived);

                var message = "";

                for (var i = 0; i < 10000; i++)
                {
                    message += $"{i} Hello there! ";
                }

                var info = new Info
                {
                    Date = DateTime.Now,
                    Message = message
                };

                while (true)
                {
                    var text = System.Console.ReadLine();

                    if (text.ToLower() == "q") return;

                    meepoNode.SendAsync(info).Wait();
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

    public class Info
    {
        public DateTime Date { get; set; }

        public string Message { get; set; }
    }
}