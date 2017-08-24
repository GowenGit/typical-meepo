# <img src ="https://rawgit.com/GowenGit/TypicalMeepo/master/Assets/TypicalMeepo%20Logo.svg" height="120px"/>

### Socket based duplex communication framework for .NET Core

Serialization layer for [Meepo](https://github.com/GowenGit/Meepo).

## Notes

* Default constructor will look for `MeepoPackage` attributes in the entry assembly. Please pass in a list of assemblies where your POCOs are defined if this is not the case.
* All nodes need to have identical POCOs defined.

### Example

Create a type that you would like to transmit:

```
[MeepoPackage]
public class ChatMessage
{
    public DateTime Date { get; set; }

    public string Message { get; set; }
}
```

`MeepoPackage` is used to map all transmittable types to unique IDs. This is done by traversing the assemblies 
that are passed in and giving them IDs based on where they appear in an ordered list.

You can initialize a new node like this:

```
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
        System.Console.ReadLine();

        meepo.SendAsync(new ChatMessage
        {
            Date = DateTime.Now,
            Message = "Hello there!"
        }).Wait();
    }
}
```

You can pass in a `MeepoConfig` object that lets you change the behavior of the server:

```
var config = new MeepoConfig
{
    BufferSizeInBytes = 1000,
    Logger = new ConsoleLogger()
};

...

var meepo = new TypicalMeepo(address, serverAddresses, config);
```

`Assembly.GetEntryAssembly()` is used to specify where to look for types that have `MeepoPackage` attribute if no assemblies are passed in.

You are able to subscribe to messages of a specific type.

### Installation

* Restore solution: `dotnet restore`
* Run the console app: `dotnet run`

### License

MIT License