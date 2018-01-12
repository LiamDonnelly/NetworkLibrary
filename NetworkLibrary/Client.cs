using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary.Connection;
using NetworkLibrary.Structs;
using NetworkLibrary.IO;
using System.Net.Sockets;
using NetworkLibrary.Packets;
namespace NetworkLibrary.Client
{
    public class Client
    {
        
        public ClientOptions clientOptions { get; }

        public IConnection connection;
        public IWriter writer;
        public IReader reader;

        private Streams.StreamController streams;


        public Client() {
            
        }

        public Client(ClientOptions options)
        {
            streams = new Streams.StreamController();
            clientOptions = options;
            connection = ConnectionSetup.CreateProtocolClient(clientOptions.protocol);
            writer = SerializerContructor.CreateWriterSerializer(clientOptions.seralizer);
            reader = SerializerContructor.CreateReaderSerializer(clientOptions.seralizer);
        }

        public void Connect(string ip, int port)
        {
            streams.Add("NetworkConnection", connection.Connect(ip, port));

        }

        public void Disconnect()
        {
            bool connected = connection.Disconnect();
        }

        public void Send<T>(T data)
        {
            writer.Write(data, streams.Retrieve("NetworkConnection"));
        }
    }
}
