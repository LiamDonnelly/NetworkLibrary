using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary.Client;
using NetworkLibrary.Structs;
using NetworkLibrary;
using NetworkLibrary.Connection;
using NetworkLibrary.Packets;
using NetworkLibrary.Listeners;
using System.IO;
using NetworkLibrary.Serializers;
using NetworkLibrary.Streams;

namespace TestGame
{
    class Program
    {
        static public Client c;

        static public void DoSomethingWithString(object a)
        {
            Console.WriteLine(a as string);
        }
        static void Main(string[] args)
        {

            c = new Client(new ClientListener(), new StreamContainer());
            c.AddAction(typeof(string), DoSomethingWithString);

            c.Connect(ConnectionSetup.CreateProtocolClient(Protocols.Tcp), "127.0.0.1", 4444, new BinaryReader());
            
            while (true)
            {
                int k;
                Int32.TryParse(Console.ReadLine(), out k);

                switch (k)
                {
                    case (0):
                        {
                            c.Connect(ConnectionSetup.CreateProtocolClient(Protocols.Tcp), "127.0.0.1", 4444, new BinaryReader());
                            break;
                        }
                    case (1):
                        {
                            c.Send("Data");
                            break;
                        }
                }
            }
        }        
    }
}