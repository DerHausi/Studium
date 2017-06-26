using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Labor3
{
    public enum MessageType
    {
        Info,
        Echo,
        Logging,
        Neighbors
    }

    [Serializable]
    public class Message : ISerializable
    {
        public MessageType Type { get; set; }
        public uint Number { get; set; }
        public string Data { get; set; }
        private static readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

        public Message(MessageType type, uint number, string data)
        {
            Type = type;
            Number = number;
            Data = data;
        }

        // Constructor to Deserialize
        public Message(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            Type = (MessageType)info.GetValue(nameof(Type), typeof(MessageType));
            Number = (uint)info.GetValue(nameof(Number), typeof(uint));
            Data = (string)info.GetValue(nameof(Data), typeof(string));
        }
        
        // Method so that the serializer knows how to serialize
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Type), Type);
            info.AddValue(nameof(Number), Number);
            info.AddValue(nameof(Data), Data);
        }

        public byte[] ToByteArray()
        {
            return Message.MessageToByteArray(this);
        }

        public static byte[] MessageToByteArray(Message message)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            {
                try
                {
                    _binaryFormatter.Serialize(ms, message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: Error when parsing to byte array: " + e.Message);
                    throw;
                }
                data = ms.ToArray();
            }
            return data;
        }

        public static Message FromByteArray(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                Message message;

                try
                {
                    message = (Message)_binaryFormatter.Deserialize(ms);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: Error when parsing byte array to message: " + e.Message);
                    throw;
                }
                return message;
            }
        }

        public static 


    }
}