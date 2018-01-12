using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary.Connection;
using NetworkLibrary.Structs;
using System.Net.Sockets;

namespace NetworkLibrary.Connection
{
    public class ConnectionSetup
    {
        public static IConnection CreateProtocolClient(Protocols? protocol)
        {
            switch (protocol)
            {
                case (Protocols.Tcp):
                    {
                        return new TcpConnection();
                        
                    }
                case (Protocols.Udp):
                    {
                        return new UdpConnection();
                    }
            }

            return null;
        }
        public static IConnection CreateProtocolClientFromSocket(Protocols? protocol, Socket socket)
        {
            switch (protocol)
            {
                case (Protocols.Tcp):
                    {
                        return new TcpConnection();

                    }
                case (Protocols.Udp):
                    {
                        return new UdpConnection();
                    }
            }

            return null;
        }

    }
}
