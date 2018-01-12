using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary.Structs;
using NetworkLibrary.Connection;
using NetworkLibrary.Listeners;
using System.Net.Sockets;
using NetworkLibrary.Server.Client;
using NetworkLibrary.Log;
using NetworkLibrary.Serializers;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetworkLibrary.Observer;

namespace NetworkLibrary.Server
{
    public class Server
    {
        public ServerOptions serverSettings;

        private IServerListener listener;
        private ISerializer serializer;
        private IList<IServerClient> clientList;
        private IList<IObserver> observerList;
        private bool _running;

        public void AddListerner(IServerListener listener) => this.listener = listener;

        public void AddSerializer(ISerializer serializer) => this.serializer = serializer;

        public Server(ServerOptions serveroptions)
        {
            serverSettings = serveroptions;
            clientList = new List<IServerClient>(serverSettings.MAXCLIENTS);
            observerList = new List<IObserver>();
        }

        public void Start()
        {
            _running = true;

            Task t = new Task(ReceiveClients);
            t.Start();
        }

        public void Stop()
        {
            _running = false;
        }

        public void Send<T>(T data, IServerClient client)
        {
            client.Send(data);
        }

        public void SendAll<T>(T data)
        {
            foreach(IServerClient sc in clientList)
            {
                Send(data, sc);
            } 
        }

        public void AddObserver(IObserver ob)
        {
            observerList.Add(ob);
        }

        public void ReceiveClients()
        {
            while (_running)
            {
                if(listener == null || serializer == null || clientList == null)
                {
                    ArgumentNullException ex = new ArgumentNullException();
                   Logger.Instance.WriteLog("Failed at Server recieve: " + ex.ToString());
                    throw ex;
                }
                
                IServerClient client = ServerClientCreator.CreateClient(listener, serverSettings.protocol);
                clientList.Add(client);

                foreach(IObserver o in observerList)
                {
                    client.AddObserver(o);
                }
                client.AddSerializer(serializer);
                client.Run();
                Console.WriteLine("Client Connected to Server");
                client.Send("Conencted");
            }
        }

        public void Close()
        {
            _running = false;
            clientList = null;
            listener = null;
            serializer = null;
            serverSettings = new ServerOptions();
        }
    }
}
