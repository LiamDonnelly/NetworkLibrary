using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary.Structs;
using NetworkLibrary.Observer;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetworkLibrary.Handler.Action;
using System.Threading;
using NetworkLibrary.Log;
using NetworkLibrary.Serializers;

namespace NetworkLibrary.Listeners
{

    public interface IClientListener : IObserver
    {
        void Start(Tuple<Stream, ISerializer> stream);
        void AddAction(Type t, Action<object> method);
        void Stop();
    }

    public class ClientListener : IClientListener
    {

        public ActionHandler<object> actions { get; }
        private ISerializer serializer;
        private Stream stream;

        private bool _connected;
        Thread task;

        public void AddAction(Type t, Action<object> method) => actions.AddFunction(t, method);


        public ClientListener()
        {
            _connected = false;
            actions = new ActionHandler<object>();
            task = new Thread(Recieve<object>);
        }
        public ClientListener(ISerializer serializer)
        {
            this.serializer = serializer;
            _connected = false;
            actions = new ActionHandler<object>();

        }

        public void Recieve<T>()
        {
            while (_connected)
            {
                var value = serializer.Read<T>(stream);

                if (value != null)
                {
                    actions.InvokeFunction(value.GetType(), value);
                }
                else
                {
                    Logger.Instance.WriteLog("Null value recieved");
                }
            }
        }

        public void Start(Tuple<Stream, ISerializer> stream)
        {
            this.stream = stream.Item1;
            this.serializer = stream.Item2;
            _connected = true;
            task = new Thread(Recieve<object>);
            task.Start();
        }
        private void Start()
        {
            _connected = true;
            task.Start();
        }

        public void Stop()
        {
            _connected = false;
        }

        public void Update(object obj)
        {
            _connected = Convert.ToBoolean(obj);

            if (_connected)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

    }

    public interface IServerListener 
    {
        Socket GetClientSocket();
    }


        public class TCPListener : Subject, IServerListener
    {
        TcpListener listener;
        IPEndPoint endPoint;

        public TCPListener(string ip, int port)
        {
            endPoint = new IPEndPoint(IPAddress.Parse(ip),port);
            listener = new TcpListener(endPoint);
            listener.Start();
        }

        public Socket GetClientSocket()
        {
            return listener.AcceptSocket();
        }
    }

    public class UDPListener : Subject, IServerListener
    {
        UdpClient serverListener;

        public UDPListener(string ip, int port)
        {
            if(serverListener == null)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                serverListener = new UdpClient(endPoint);
            }
        }

        public Socket GetClientSocket()
        {
            IPEndPoint clientSender = new IPEndPoint(IPAddress.Any, 0);
            byte[] buff = serverListener.Receive(ref clientSender);

            UdpClient serverclient = new UdpClient();
            serverclient.Connect(clientSender);

            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, serverclient.Client.LocalEndPoint);

            serverListener.Send(ms.ToArray(), ms.ToArray().Length, clientSender);

            return serverclient.Client;

        }
    }
}
