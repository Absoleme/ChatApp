using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace SharedLibrary
{
        public class Net
        {
            public static void sendMsg(Stream s, Message msg)
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(s, msg);
            }

            public static Message rcvMsg(Stream s)
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (Message)bf.Deserialize(s);
            }
        }
    
}
