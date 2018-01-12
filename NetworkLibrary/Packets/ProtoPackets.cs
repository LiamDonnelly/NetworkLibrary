using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
using NetworkLibrary.Observer;
using System.ComponentModel;

namespace NetworkLibrary.Packets
{
    [ProtoContract, Serializable]
    public enum PacketTypes
    {
        ActionMesaage,
        ChatMessage,
        Object2D,
        Object3D,
        Connect,
        Disconnect,
        None
    }

    [ProtoContract, Serializable]
    public class Vec2
    {
        [ProtoMember(1)]
        public int x { get; set; }

        [ProtoMember(2)]
        public int y { get; set; }

        public Vec2() { }
        public Vec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    [ProtoContract, Serializable]
    public class Vec3
    {
        [ProtoMember(1)]
        public int x { get; set; }

        [ProtoMember(2)]
        public int y { get; set; }

        [ProtoMember(3)]
        public int z { get; set; }
    }

    [ProtoContract, Serializable]
    [ProtoInclude(1, typeof(ChatMessagePacket))]
    [ProtoInclude(2, typeof(ActionPacket))]
    [ProtoInclude(3, typeof(Object2DPacket))]
    [ProtoInclude(4, typeof(Object3DPacket))]
    [ProtoInclude(5, typeof(ConnectPacket))]
    [ProtoInclude(6, typeof(DisconnectPacket))]
    public class Packet
    {
        [ProtoMember(5), DefaultValue(PacketTypes.None)]
        public PacketTypes PacketType { get; set; }
    }

    [ProtoContract, Serializable]
    public class ChatMessagePacket : Packet
    {
        
        [ProtoMember(2)]
        public int clientID { get; set; }
        [ProtoMember(3)]
        public string message { get; set; }
    }

    [ProtoContract, Serializable]
    public class ActionPacket : Packet
    {
        [ProtoMember(2)]
        public int clientID { get; set; }
        [ProtoMember(3)]
        public string message { get; set; }
    }

    [ProtoContract, Serializable]
    public class Object2DPacket : Packet
    {

        [ProtoMember(2)]
        public int objectID { get; set; }

        [ProtoMember(3)]
        public int angle { get; set; }

        [ProtoMember(4)]
        public Vec2 pos { get; set; }
    }

    [ProtoContract, Serializable]
    public class Object3DPacket : Packet
    {
        [ProtoMember(2)]
        public int objectID { get; set; }

        [ProtoMember(3)]
        public int angle { get; set; }
        
    }

    [ProtoContract, Serializable]
    public class ConnectPacket : Packet
    {
        [ProtoMember(2)]
        public int clientID { get; set; }

    }

    [ProtoContract, Serializable]
    public class DisconnectPacket : Packet
    {
        [ProtoMember(2)]
        public int clientID { get; set; }
        
    }
}
