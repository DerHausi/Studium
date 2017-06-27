using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Labor3
{
    class Node
    {
        public UdpClient Socket { get; set; }
        public string Name { get; set; }
        public uint MemorySize { get; set; }
        public uint CumulatedSize { get; set; }

        public IPEndPoint ParentAddress { get; set; }
        public IPEndPoint LoggerAddress { get; set; }

        private NeighborList Neighbors = new NeighborList();
        private bool _isInformed = false;
        private uint _informedNeighbors = 0;

        public Node(string name, uint size, IPEndPoint address)
        {
            Name = name;
            MemorySize = size;
            Socket = new UdpClient(address);
            CumulatedSize = size;
        }

        public void Receive(IPEndPoint recvFrom, Message message)
        {
            switch (message.Type)
            {
                case MessageType.Info:
                    _informedNeighbors++;
                    message.Type = MessageType.Logging;
                    message.Data = recvFrom.ToString();
                    if (!_isInformed)
                    {
                        ParentAddress = recvFrom;
                        _isInformed = true;
                        // send to all neighbors 
                        foreach (var neighbor in Neighbors)
                        {
                            Message mes = new Message(MessageType.Info, 0, null);
                            Socket.Send(mes.ToByteArray(), mes.ToByteArray().Length, neighbor);
                        }
                        // create message to Logger with parent node
                        message.Data += " is Parent Node";
                    }
                    else
                    {
                        // create message to Logger with neighbor node
                        message.Data += " is Neighbor Node";
                    }
                    // send info message to Logger
                    Socket.Send(message.ToByteArray(), message.ToByteArray().Length, LoggerAddress);
                    break;
                case MessageType.Echo:
                    _informedNeighbors++;
                    Message mesLog = new Message(MessageType.Logging, 0, recvFrom.ToString());
                    mesLog.Data = mesLog.Data + " has send Echo with size: " + message.Data;
                    Socket.Send(mesLog.ToByteArray(), mesLog.ToByteArray().Length, LoggerAddress);
                    // sum size
                    CumulatedSize += uint.Parse(message.Data);
                    // check if all neigbors are informed
                    if (_informedNeighbors == Neighbors.Count)
                    {
                        // send to Parent
                        Message mes = new Message(MessageType.Echo, 0, CumulatedSize.ToString());
                        Socket.Send(mes.ToByteArray(), mes.ToByteArray().Length, ParentAddress);
                        mesLog.Data = Name + " echos to his Parent with size: " + CumulatedSize;
                        Socket.Send(mesLog.ToByteArray(), mesLog.ToByteArray().Length, LoggerAddress);
                        // reset sum
                        CumulatedSize = MemorySize;
                    }                    
                    break;
                case MessageType.Logging:
                    Console.WriteLine("Not a Logger!!!");
                    break;
                case MessageType.Neighbors:
                    // TODO save the NeighborList
                    Neighbors.AddFromString(message.Data);
                    break;
                default:
                    break;
            }
        }    
    }
}
