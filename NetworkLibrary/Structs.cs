using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace NetworkLibrary.Structs
{

    public enum Protocols
    {
        Tcp,Udp
    }

    public enum Seralizers
    {
        Protobuf, Binary
    }

    public enum ClientType
    {
        Peer2Peer, Peer2Server
    }

    public struct ServerOptions
    {
        public string ip;
        public int port;
        public Protocols? protocol;
        public int MAXCLIENTS;

    }
}
