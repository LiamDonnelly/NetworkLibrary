using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NetworkLibrary.Log;
using NetworkLibrary.Serializers;

namespace NetworkLibrary.Streams
{
    public class StreamContainer
    {
        IDictionary<string, Tuple<Stream, ISerializer>> streamHolder;

        public StreamContainer()
        {
            streamHolder = new Dictionary<string, Tuple<Stream, ISerializer>>();
            
        }

        public void Add(string key, Stream stream, ISerializer serializer)
        {
            if(!streamHolder.ContainsKey(key))
            {
                Tuple<Stream, ISerializer> tuple = new Tuple<Stream, ISerializer>(stream, serializer);
                streamHolder.Add(key, tuple);
            }
            else
            {
                Logger.Instance.WriteLog("Key already exist.");
            }
        }

        public void Remove(string key)
        {
            try
            {
                streamHolder.Remove(key);
            }
            catch
            {
                Logger.Instance.WriteLog("Cannot remove since key does not exist.");
            }
        }

        public Tuple<Stream, ISerializer> Retrieve(string key)
        {
            return streamHolder.Where(ky => ky.Key == key).First().Value;
        }
        
        public bool StreamExist(string key)
        {
            return streamHolder.ContainsKey(key);
        }
    }
}
