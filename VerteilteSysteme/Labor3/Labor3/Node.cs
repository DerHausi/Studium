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
                    }
                    break;
                case MessageType.Echo:
                    _informedNeighbors++;
                    // sum size
                    CumulatedSize += uint.Parse(message.Data);
                    // check if all neigbors are informed
                    if (_informedNeighbors == Neighbors.Count)
                    {
                        // send to Parent
                        Message mes = new Message(MessageType.Echo, 0, CumulatedSize.ToString());
                        Socket.Send(mes.ToByteArray(), mes.ToByteArray().Length, ParentAddress);
                        // reset sum
                        CumulatedSize = MemorySize;
                    }
                    break;
                case MessageType.Logging:
                    Console.WriteLine("Not a Logger!!!");
                    break;
                case MessageType.Neighbors:
                    
                    break;
                default:
                    break;
            }
        }    
    }
}
