using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLibrary.Server.Client;
using NetworkLibrary.Listeners;
using NetworkLibrary.Structs;
namespace NetworkLibrary
{
    class ServerClientCreator
    {
        public static IServerClient CreateClient(IServerListener listeners, Protocols? proto)
        {
            IServerClient sc = null;
            switch (proto)
            {
                case (Protocols.Tcp):
                    {
                        sc = new TcpServerClient(listeners.GetClientSocket());
                        break;
                    }
                case (Protocols.Udp):
                    {
                        sc = new UdpServerClient(listeners.GetClientSocket());
                        break;
                    }
            }

            return sc;
        }
    }
}
