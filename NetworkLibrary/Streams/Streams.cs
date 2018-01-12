using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace NetworkLibrary.Streams
{
    public class UdpNetworkStream : Stream
    {
        private Socket _streamSocket;

        public UdpNetworkStream(Socket socket)
        {
            _streamSocket = socket ?? throw new ArgumentNullException("socket");
        }
        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => 0;

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
        }

        public new void Dispose()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _streamSocket.Receive(buffer);
        }

        public byte[] ReadBytes()
        {
            byte[] buff = new byte[1028];
            _streamSocket.Receive(buff);
            return buff;
        }

        public virtual int ReadInt32()
        {
            MemoryStream ms = new MemoryStream();
            byte[] buff = new byte[1028];
            return _streamSocket.Receive(buff);
        }


        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _streamSocket.Connect(_streamSocket.RemoteEndPoint);
            _streamSocket.Send(buffer);
        }

    }
}
