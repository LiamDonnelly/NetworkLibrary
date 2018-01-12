using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary.Handler.Action;
using NetworkLibrary.Connection;
using NetworkLibrary.Observer;
using NetworkLibrary.Structs;
using NetworkLibrary.Streams;
using NetworkLibrary.Packets;
using NetworkLibrary.Listeners;
using NetworkLibrary.Log;
using System.IO;
using NetworkLibrary.Serializers;

namespace NetworkLibrary.Client
{

    public class Client : IDisposable, IObserver
    {
        public Server.Server server;

        private StreamContainer streamContainer;
        private IConnection connection;
        private IClientListener listener;

        public void AddpendServer(Server.Server server) => this.server = server;

        public void AddStreamController(StreamContainer container) => streamContainer = container;

        public void AddStream(string key, Stream s, ISerializer sr) => streamContainer.Add(key, s, sr);

        public void AddListener(IClientListener listener) => this.listener = listener;

        public void AddAction(Type t, Action<object> Method) => listener.AddAction(t, Method);


        public Client() {
        }

        public Client(IClientListener listener)
        {
            if (listener == null)
            {
                ArgumentNullException ex = new ArgumentNullException();
                Logger.Instance.WriteLog("Failed at client init: " + ex.ToString());
                throw ex;
            }

            this.listener = listener;
        }

        public Client(StreamContainer container)
        {
            if (container == null)
            {
                ArgumentNullException ex = new ArgumentNullException();
                Logger.Instance.WriteLog("Failed at client init: " + ex.ToString());
                throw ex;
            }

            streamContainer = container;
        }

        public Client(IClientListener listener, StreamContainer container)
        {
            if (listener == null || container == null)
            {
                ArgumentNullException ex = new ArgumentNullException();
                Logger.Instance.WriteLog("Failed at client init: " + ex.ToString());
                throw ex;
            }

            this.listener = listener;
            streamContainer = container;
        }

        public void Connect(IConnection connection, string ip, int port, ISerializer serializer)
        {
            if(streamContainer == null)
            {
                ArgumentNullException ex = new ArgumentNullException("StreamContainer was not set.");
                Logger.Instance.WriteLog("Failed at connection: "+ ex.ToString());
                throw ex;
            }
            if(streamContainer.StreamExist("NetworkStream"))
            {
                ArgumentException ex = new ArgumentException("Stream already exist.");
                Logger.Instance.WriteLog("Trying to connect when a stream has already been established.");
                throw ex;
            }
                
            this.connection = connection;

            connection.Connect(ip, port);

            if (connection.fetchStream() == null)
            {
                ArgumentNullException ex = new ArgumentNullException("No stream found when fetching.");
                Logger.Instance.WriteLog("Failed at connection stream retreival: " + ex.ToString());
                throw ex;
            }

            streamContainer.Add("NetworkStream", connection.fetchStream(), serializer);

            connection.AddObserver(listener);
            connection.AddObserver(this);

            listener.Start(streamContainer.Retrieve("NetworkStream"));

        }
        
        public void Disconnect()
        {
            Send(new DisconnectPacket());
            connection.RemoveObserver(listener);
            connection.RemoveObserver(this);
            connection.Disconnect();
            streamContainer.Remove("NetworkStream");
            Logger.Instance.WriteLog("Client Disconnected");
        }
        
        public void Send<T>(T data)
        {
            if (streamContainer.StreamExist("NetworkStream"))
                Write(data, "NetworkStream");
            else
                Logger.Instance.WriteLog("Trying to send to null stream");
        }

        public void Write<T>(T data, string targetStream)
        {
            var tPair = streamContainer.Retrieve(targetStream);
            tPair.Item2.Write(data, tPair.Item1);
        }

        public T Read<T>(string fromStream)
        {
            var tPair = streamContainer.Retrieve(fromStream);
            return tPair.Item2.Read<T>(tPair.Item1);
        }

        public void Update(object obj)
        {
            bool b = Convert.ToBoolean(obj);
            if (!b)
            {
                Disconnect();
            }
        }

        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }
    }
}
