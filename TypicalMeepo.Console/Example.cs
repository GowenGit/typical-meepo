using System;
using System.Net;
using Meepo.Core.Configs;
using TypicalMeepo.Core.Attributes;

namespace TypicalMeepo.Console
{
    public class Example
    {
        public void ExampleMethod()
        {
            // IP Address to expose
            var address = new TcpAddress(IPAddress.Loopback, 9201);

            // Nodes to connect to
            var serverAddresses = new[] { new TcpAddress(IPAddress.Loopback, 9200) };

            using (var meepo = new TypicalMeepo(address, serverAddresses))
            {
                meepo.Start();

                meepo.Subscribe<ChatMessage>((id, chatMessage) => System.Console.WriteLine($"Message: {chatMessage.Message}"));

                while (true)
                {
                    System.Console.ReadLine();

                    meepo.SendAsync(new ChatMessage
                    {
                        Date = DateTime.Now,
                        Message = "Hello there!"
                    }).Wait();
                }
            }
        }
    }

    [MeepoPackage]
    public class ChatMessage
    {
        public DateTime Date { get; set; }

        public string Message { get; set; }
    }
}
