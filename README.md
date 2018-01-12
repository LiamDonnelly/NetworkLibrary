# NetworkLibrary

##Server
Starting Server

```c#
//Initiate Client
ServerOptions serverOptions = new ServerOptions();
serverOptions.ip = "127.0.0.1";
serverOptions.port = 4444;
serverOptions.protocol = Protocols.Tcp;

Server server = new Server(serverOptions);

server.AddListerner(new TCPListener(serverOptions.ip, serverOptions.port));
server.AddSerializer(new BinarySeralizer());

server.Start();
```

Handling Incoming Data Packets
```c#

public class stringPrinter : IObserver
{
    public void Update(object obj)
    {
        Console.WriteLine(obj as string);
    }
}

stringPrinter print = new stringPrinter();
server.AddObserver(print);
```

## Client
Starting Client

```c#
//Initiate Client
Client client = new Client();

//Add a Client Listener
client.AddListener(new ClientListener());

//Add a StreamController
client.AddStreamController(new StreamContainer());

//Connect to Server
client.Connect(ConnectionSetup.CreateProtocolClient(Protocols.Tcp), "127.0.0.1", 4444, new BinarySeralizer());
```

To Send Data
```c#
//Client has to be connected to Server
//Any object can be Sent
Client.Send("Some Data");
```
To Process Incoming Packets

```c#
//Define your action
static public void PrintStringMessage(object obj)
{
    Console.WriteLine(obj as string);
}

//Add your action into Client
client.AddAction(typeof(string), PrintStringMessage);

```
Starting Client Server

The same method of starting a standalone server but using client's server member.
```c#
client.server.//Rest as if normal server
```


