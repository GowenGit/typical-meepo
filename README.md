# <img src ="https://rawgit.com/GowenGit/typical-meepo/master/Assets/TypicalMeepo%20Logo.svg" height="120px"/>

### [![Build Status](https://travis-ci.org/GowenGit/typical-meepo.svg?branch=master)](https://travis-ci.org/GowenGit/typical-meepo)

### Socket based duplex communication framework for .NET Core

Serialization layer for [Meepo](https://github.com/GowenGit/Meepo).

### Example

Create a type that you would like to transmit:

```cs
public class ChatMessage
{
    public DateTime Date { get; set; }

    public string Message { get; set; }
}
```

You can initialize a new node like this:

```cs
var config = new MeepoConfig
{
    Logger = new ConsoleLogger()
};

// IP Address to expose
var address = new TcpAddress(IPAddress.Loopback, 9201);

// Nodes to connect to
var serverAddresses = new[] { new TcpAddress(IPAddress.Loopback, 9200) };

using (var meepo = new TypicalMeepoNode(address, serverAddresses, config))
{
    meepo.Start();

    meepo.Subscribe<ChatMessage>((id, chatMessage) => System.Console.WriteLine($"Message: {chatMessage.Message}"));

    while (true)
    {
        var text = System.Console.ReadLine();

        meepo.SendAsync(new ChatMessage
        {
            Date = DateTime.Now,
            Message = text
        }).Wait();
    }
}
```

You can pass in a `MeepoConfig` object that lets you change the behavior of the server:

```cs
var config = new MeepoConfig
{
    BufferSizeInBytes = 1000,
    Logger = new ConsoleLogger()
};

...

var meepo = new TypicalMeepo(address, serverAddresses, config);
```

You are able to subscribe to messages of a specific type.

### Installation

* Restore solution: `dotnet restore`
* Run the console app: `dotnet run`

### License

MIT License