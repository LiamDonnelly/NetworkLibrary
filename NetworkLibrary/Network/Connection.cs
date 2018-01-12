using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using NetworkLibrary.Observer;
using NetworkLibrary.Log;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using NetworkLibrary.Streams;

namespace NetworkLibrary.Connection
{
    public interface IConnection : ISubject
    {
        void Connect(string ip, int port);
        void Disconnect();

        Stream fetchStream();

    }

    public class TcpConnection : Subject, IConnection
    {
        TcpClient _tcpClient;
        Thread _timeout;

        public TcpConnection()
        {
            _tcpClient = new TcpClient();
            _timeout = new Thread(Timeout);
        }

        public TcpConnection(Socket socket)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Client = socket;
        }

        public void Connect(string ip, int port)
        {
            try
            {
                _tcpClient.Connect(ip, port);
                Update(true);
                _timeout.Start();
            }
            catch(SocketException ex)
            {
                Logger.Instance.WriteLog("Cannot connect to Socket : " + ex.ToString());
                throw ex;
            }
        }

        public void Disconnect()
        {
            if (_tcpClient.Connected)
            {
                _tcpClient.GetStream().Close();
                Update(false);
            }
        }

        public Stream fetchStream()
        {
            Stream stream = null;
            try
            {
                stream = _tcpClient.GetStream();
            }
            catch(InvalidOperationException ex)
            {
                Logger.Instance.WriteLog("Cannot fetch stream : " + ex.ToString());
                throw ex;
            }
            return stream;
        }

        private void Timeout()
        {
            while(_tcpClient.Connected)
            {
                Thread.Sleep(1000);
            }
            Update(false);
        }
    }

    public class UdpConnection : Subject, IConnection
    {
        UdpClient _udpClient;
        UdpNetworkStream stream; //fake stream
        Thread _timeout;

        public UdpConnection()
        {
            
            _timeout = new Thread(Timeout);
        }

        public UdpConnection(Socket socket)
        {
            _udpClient.Client = socket;
        }

        public void Connect(string ip, int port)
        {
            IPEndPoint serveAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            _udpClient = new UdpClient();

            byte[] data = Encoding.ASCII.GetBytes("Connect");
            _udpClient.Send(data, data.Length, serveAddress);

            byte[] mes = _udpClient.Receive(ref serveAddress);

            MemoryStream ms = new MemoryStream(mes);
            BinaryFormatter bf = new BinaryFormatter();

            object ipEndPoint = bf.Deserialize(ms);
            IPEndPoint ed = ipEndPoint as IPEndPoint;
            _udpClient.Connect(ed);

            if (_udpClient.Client.Connected)
            {
                stream = new UdpNetworkStream(_udpClient.Client);
                Update(true);
                _timeout.Start();
            }
        }

        public void Disconnect()
        {
            if(stream != null)
            {
                stream.Dispose();

            }
            Update(false);

        }

        public Stream fetchStream()
        {
            if(stream == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Udp stream is null.");
                Logger.Instance.WriteLog("Cannot fetch stream : " + ex.ToString());
                throw ex;
            }

            return stream;
        }

        private void Timeout()
        {
            byte[] data = Encoding.ASCII.GetBytes("TimeoutCheck");
            
            while (_udpClient.Client.Connected)
            {
                Thread.Sleep(1000);
                _udpClient.Client.Send(data);
            }
            Update(false);
        }
    }
}
