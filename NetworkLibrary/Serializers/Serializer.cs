using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using ProtoBuf;
using NetworkLibrary.Observer;
using NetworkLibrary.Log;

namespace NetworkLibrary.Serializers
{
    public interface ISerializer
    {
        void Write<T>(T input, Stream location);
        T Read<T>(Stream source);
    }

    public class ProtoBufSeralizer : ISerializer
    {
        public T Read<T>(Stream source)
        {
            try
            {
                BinaryReader binaryReader = new BinaryReader(source, Encoding.Default, true);
                int incomingBytes = binaryReader.ReadInt32();
                byte[] bytes = binaryReader.ReadBytes(incomingBytes);
                MemoryStream ms = new MemoryStream(bytes);
                object value = Serializer.Deserialize<object>(ms);

                return (T)value;
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(ex.ToString());
                throw ex;
            }
            return default(T);
        }
        

        public void Write<T>(T input, Stream location)
        {
            try
            {
                BinaryWriter writer = new BinaryWriter(location, Encoding.Default, true);
                MemoryStream ms = new MemoryStream();
                Serializer.Serialize(ms, input);
                var buffer = ms.ToArray();
                writer.Write(buffer.Length);
                writer.Write(buffer);
                writer.Flush();
                
            }
            catch
            {
                Logger.Instance.WriteLog("Protobuf failed to write to stream");
            }
        }
    }
    
    public class BinarySeralizer : ISerializer
    {
        public T Read<T>(Stream source)
        {
            try
            {
                BinaryReader binaryReader = new BinaryReader(source,Encoding.Default,true);

                int incomingBytes = binaryReader.ReadInt32();
                byte[] bytes = binaryReader.ReadBytes(incomingBytes);

                MemoryStream ms = new MemoryStream(bytes);
                BinaryFormatter bf = new BinaryFormatter();

                object a = bf.Deserialize(ms);
                ms.Close();

                if (a is T)
                {
                    return (T)a;
                }
                else
                {
                    try
                    {
                        return (T)Convert.ChangeType(a, typeof(T));
                    }
                    catch (InvalidCastException)
                    {
                        return default(T);
                    }
                }
            }
            catch
            {
                Logger.Instance.WriteLog("BinaryReader failed reading from stream");
            }
            return default(T);
        }

        public void Write<T>(T input, Stream location)
        {
            try
            {
                BinaryWriter writer = new BinaryWriter(location, Encoding.Default, true);
                BinaryFormatter _bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                _bf.Serialize(ms, input);
                byte[] buffer = ms.GetBuffer();
                writer.Write(buffer.Length);
                writer.Write(buffer);
                writer.Flush();
                ms.Close();
                
            }
            catch
            {
                Logger.Instance.WriteLog("BinaryReader failed writing to stream");
            }

        }
    }
}
