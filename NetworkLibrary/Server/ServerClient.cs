using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using NetworkLibrary.Structs;
using NetworkLibrary.Connection;
using NetworkLibrary.Observer;
using NetworkLibrary.Streams;
using NetworkLibrary.Log;
using NetworkLibrary.Serializers;
using System.Threading;
using NetworkLibrary.Packets;

namespace NetworkLibrary.Server.Client
{
    public interface IServerClient : ISubject
    {
        void Send<T>(T data);
        void Run();
        void AddSerializer(ISerializer serializer);
    }

    public class UdpServerClient : Subject, IServerClient
    {
        Socket socket;
        UdpNetworkStream stream;
        ISerializer serializer;
        Thread clientService;
        bool active;

        public UdpServerClient(Socket socket)
        {
            if(socket == null)
            {
                ArgumentNullException ex = new ArgumentNullException("socket is null.");
                Logger.Instance.WriteLog("UDP stream creation: " + ex.ToString());
                throw ex;
            }
            
            this.socket = socket;
            stream = new UdpNetworkStream(socket);
        }

        public void AddSerializer(ISerializer serializer)
        {
            if(serializer == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Seralizer is null.");
                Logger.Instance.WriteLog("UDP Add Seralizer: " + ex.ToString());
                throw ex;
            }
            this.serializer = serializer;
        }

        public void Run()
        {
            

            active = true;
            clientService = new Thread(new ThreadStart(ClientSocketMethod));
            clientService.Start();
        }

        public void Stop()
        {
            active = false;
            stream.Dispose();
        }

        public void Send<T>(T data)
        {
            serializer.Write(data, stream);
        }

        public void ClientSocketMethod()
        {
            while (active)
            {
                object data = serializer.Read<object>(stream);

                if (data == null)
                {
                    break;
                }
                if (data.GetType() == typeof(DisconnectPacket))
                {
                    Stop();
                    break;
                }
                if(data.GetType() == typeof(string))
                {
                    Console.WriteLine(data.ToString());
                }
                Update(this);
            }
        }
    }

    public class TcpServerClient : Subject, IServerClient
    {
        NetworkStream stream;
        ISerializer serializer;
        Thread clientService;
        bool active;

        public TcpServerClient(Socket socket)
        {
            if (socket == null)
            {
                ArgumentNullException ex = new ArgumentNullException("socket is null.");
                Logger.Instance.WriteLog("UDP stream creation: " + ex.ToString());
                throw ex;
            }

            stream = new NetworkStream(socket);
        }

        public void AddSerializer(ISerializer serializer)
        {
            if (serializer == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Seralizer is null.");
                Logger.Instance.WriteLog("UDP Add Seralizer: " + ex.ToString());
                throw ex;
            }
            this.serializer = serializer;
        }

        public void Run()
        {
            active = true;
            clientService = new Thread(new ThreadStart(ClientSocketMethod));
            clientService.Start();

        }

        public void Send<T>(T data)
        {
            serializer.Write(data, stream);
        }

        public void ClientSocketMethod()
        {
            while (active)
            {
                object data = serializer.Read<object>(stream);
                Console.WriteLine("Data recieved from Client");
                Update(this);
            }
        }
    }

}
