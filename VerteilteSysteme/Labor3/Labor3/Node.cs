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
        public IPEndPoint ParentAddress { get; set; }
        public IPEndPoint LoggerAddress { get; set; }

        private List<IPEndPoint> Neighbors = new List<IPEndPoint>();
        private bool _isInformed = false;
        private uint _informedNeighbors = 0;

        public Node(string name, uint size, IPEndPoint address)
        {
            Name = name;
            MemorySize = size;
            Socket = new UdpClient(address);
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
                        // TODO send to all neighbors 
                    }
                    break;
                case MessageType.Echo:
                    _informedNeighbors++;
                    // TODO sum size

                    if (_informedNeighbors == Neighbors.Count)
                    {
                        // TODO send to Parent
                        // TODO reset sum
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
