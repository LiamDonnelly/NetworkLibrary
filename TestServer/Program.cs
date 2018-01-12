using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NetworkLibrary.Listeners;
using NetworkLibrary.Structs;
using NetworkLibrary.Packets;
using NetworkLibrary.Server;
using NetworkLibrary.Serializers;
using NetworkLibrary.Observer;

namespace TestServer
{
    class Program
    {

        public class stringPrinter : IObserver
        {
            public void Update(object obj)
            {
                string s = obj.ToString();
                Console.WriteLine(s);
            }
        }

        static void Main(string[] args)
        {

            ServerOptions serverOptions = new ServerOptions();
            serverOptions.ip = "127.0.0.1";
            serverOptions.port = 4444;
            serverOptions.protocol = Protocols.Tcp;

            Server server = new Server(serverOptions);
            IServerListener listener = new TCPListener(serverOptions.ip, serverOptions.port);

            server.AddListerner(listener);
            server.AddSerializer(new BinaryReader());

            stringPrinter print = new stringPrinter();
            server.AddObserver(print);

            server.Start();
            
            bool running = true;

            while (running)
            {
                int k;
                Int32.TryParse(Console.ReadLine(), out k);

                switch (k)
                {
                    case (0):
                        {
                            Object2DPacket p = new Object2DPacket();
                            p.objectID = 23;
                            p.angle = 360;
                            p.pos = new Vec2(30, 40);
                            p.PacketType = PacketTypes.Object2D;

                            server.SendAll(p);
                            break;
                        }
                    case (1):
                        {
                            server.SendAll("HELLO EVERYONE");
                            break;
                        }
                    case (2):
                        {
                            server.SendAll(54);
                            break;
                        }
                    case (3):
                        {
                            Object2DPacket p = new Object2DPacket();
                            p.objectID = 23;
                            p.angle = 360;
                            p.pos = new Vec2(30, 40);
                            p.PacketType = PacketTypes.Object2D;

                            server.SendAll(p);
                            break;
                        }
                    case (9):
                        {
                            running = false;
                            Console.WriteLine("Shut Down");
                            break;
                        }
                }
            }
        }
    }

    
}



//IPEndPoint endPoint = new IPEndPoint(IPAddress.Any,11000);

//UdpClient serverListener = new UdpClient(endPoint);

//IPEndPoint clientSender = new IPEndPoint(IPAddress.Any,0);

//UdpClient serverclient;

//Thread GetClient = new Thread(() => {

//    while (true)
//    {
//        byte[] buff = serverListener.Receive(ref clientSender);

//        Console.WriteLine("Message From Client");


//        serverclient = new UdpClient();
//        serverclient.Connect(clientSender);

//        MemoryStream ms = new MemoryStream();
//        BinaryFormatter bf = new BinaryFormatter();
//        bf.Serialize(ms, serverclient.Client.LocalEndPoint);

//        serverListener.Send(ms.ToArray(), ms.ToArray().Length, clientSender);

//        byte[] buffffer = new byte[1028];
//        serverclient.Client.Receive(buffffer);
//        Console.WriteLine("Yes!");

//        serverclient.Client.Send(buffffer);
//        Console.WriteLine("Yes!");

//    }

//});
//GetClient.Start();



//while (true)
//{
//    byte[] data = Encoding.ASCII.GetBytes("Greertings");

//    string va = Console.ReadLine();

//    if (va == "s")
//    {
//        IPEndPoint serveAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);

//        UdpClient client = new UdpClient();
//        UdpClient client2 = new UdpClient();
//        client2.Client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 51222));

//        client.Send(data, data.Length, serveAddress);

//        //Recieven new Address to send too
//        byte[] mes = client.Receive(ref serveAddress);

//        MemoryStream ms = new MemoryStream(mes);
//        BinaryFormatter bf = new BinaryFormatter();

//        object sEP = bf.Deserialize(ms);

//        IPEndPoint ed = sEP as IPEndPoint;
//        client.Connect(ed);
//        Console.WriteLine("Message From Server");

//        client.Client.Send(data);
//        Console.WriteLine("Send message to server");

//        byte[] dataaa = new byte[1028];
//        client.Client.Receive(dataaa);
//        Console.WriteLine("Message Retrieved");
//    }
//}