using System;
using System.Net;
using Meepo.Core.Configs;
using Meepo.Core.Logging;
using TypicalMeepo.Core.Attributes;

namespace TypicalMeepo.Console
{
    public class Program
    {
        private static ITypicalMeepo meepo;

        public static void Main()
        {
            var config = new MeepoConfig
            {
                Logger = new ConsoleLogger(),
                BufferSizeInBytes = 1000000
            };

            var address = new TcpAddress(IPAddress.Loopback, 9201);
            var serverAddresses = new[] { new TcpAddress(IPAddress.Loopback, 9200) };

            using (meepo = new TypicalMeepo(address, serverAddresses, config))
            {
                meepo.Start();

                meepo.Subscribe<Info>(OnInfoReceived);

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

                    meepo.SendAsync(info).Wait();
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

    [MeepoPackage]
    public class Info
    {
        public DateTime Date { get; set; }

        public string Message { get; set; }
    }
}