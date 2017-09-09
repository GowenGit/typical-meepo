using System;
using System.Net;
using Meepo.Core.Configs;
using Meepo.Core.Logging;

namespace TypicalMeepo.Console
{
    public class Example
    {
        public void ExampleMethod()
        {
            var config = new MeepoConfig
            {
                Logger = new ConsoleLogger()
            };

            // IP Address to expose
            var address = new TcpAddress(IPAddress.Loopback, 9201);

            // Nodes to connect to
            var serverAddresses = new[] { new TcpAddress(IPAddress.Loopback, 9200) };

            using (var meepo = new TypicalMeepo(address, serverAddresses, config))
            {
                meepo.Start();

                meepo.Subscribe<ChatMessage>((id, chatMessage) => System.Console.WriteLine($"Message: {chatMessage.Message}"));

                while (true)
                {
                    var text = System.Console.ReadLine();

                    if (text.ToLower() == "q") break;

                    meepo.SendAsync(new ChatMessage
                    {
                        Date = DateTime.Now,
                        Message = "Hello there!"
                    }).Wait();
                }
            }
        }
    }

    public class ChatMessage
    {
        public DateTime Date { get; set; }

        public string Message { get; set; }
    }
}
