# <img src ="./icon.png" width="23px" /> TypicalMeepo

### Socket based duplex communication framework for .NET Core

Serialization layer for [Meepo](https://github.com/GowenGit/Meepo). Will be added as a nuget package.

### Example

Create a type that you would like to transmit:

```
[MeepoPackage]
public class Info
{
    public DateTime Date { get; set; }

    public string Message { get; set; }
}
```

`MeepoPackage` is used to map all transmittable types to unique IDs 


You can initialize a new node like this:

```
// IP Address to expose
var address = new TcpAddress(IPAddress.Loopback, 9201);

// Nodes to connect to
var serverAddresses = new[] { new TcpAddress(IPAddress.Loopback, 9200) };

using (var meepo = new TypicalMeepo(address, serverAddresses, new[] { Assembly.GetEntryAssembly() }))
{
    meepo.Start();

    meepo.Subscribe<Info>((id, info) => System.Console.WriteLine($"Message: {info.Message}"));

    while (true)
    {
        var text = System.Console.ReadLine();

        meepo.Send(new Info
        {
            Date = DateTime.Now,
            Message = "Hello there!"
        }).Wait();
    }
}
``` 

`Assembly.GetEntryAssembly()` is used to specify where to look for types that have `MeepoPackage` attribute.

You are able to subscribe to messages of a specific type.

### Run on Windows or Linux

* Restore solution: `dotnet restore`
* Run the console app: `dotnet run`