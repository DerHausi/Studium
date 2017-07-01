using System;
using System.Net;
using System.Net.Sockets;

namespace Labor3_Echo
{
    public class Node
    {
        public UdpClient Socket { get; set; }
        public string Name { get; set; }
        public uint MemorySize { get; set; }
        public uint CumulatedSize { get; set; }

        public IPEndPoint ParentAddress { get; set; }
        public IPEndPoint LoggerAddress = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 6666);

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
            // simulating the lag of the network betwee 0 an 100 milisec
            Random r = new Random((int)DateTime.Now.Ticks);
            int lagTime = r.Next(0, 100);
            System.Threading.Thread.Sleep(lagTime);

            switch (message.Type)
            {
                case MessageType.Info:
                    _informedNeighbors++;
                    message.Type = MessageType.Logging;
                    message.Data = recvFrom.ToString();
                    if (!_isInformed)
                    {
                        ParentAddress = recvFrom;
                        Console.WriteLine("My Parent Node is: " + ParentAddress.Port);
                        _isInformed = true;
                        // send to all neighbors                                                
                        foreach (var neighbor in Neighbors)
                        {
                            if(!(neighbor.Port == ParentAddress.Port) )
                            {
                                Message mes = new Message(MessageType.Info, 0, null);
                                Socket.Send(mes.ToByteArray(), mes.ToByteArray().Length, neighbor);
                                Console.WriteLine("Send INFO to: " + neighbor.Port);
                            }
                        }                        
                        // create message to Logger with parent node
                        message.Data += (" is Parent Node of " + Name);                                               
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
                    // sum size
                    CumulatedSize += uint.Parse(message.Data);                    
                    Message mesLog = new Message(MessageType.Logging, 0, recvFrom.ToString());
                    mesLog.Data = mesLog.Data + " has send Echo with size: " + message.Data;
                    Socket.Send(mesLog.ToByteArray(), mesLog.ToByteArray().Length, LoggerAddress);
                    break;
                case MessageType.Logging:
                    Console.WriteLine("Not a Logger!!!");
                    break;
                case MessageType.Neighbors:
                    // save the NeighborList
                    Neighbors.AddFromString(message.Data);                    
                    break;
                default:
                    break;
            }
            // check if all neigbors are informed
            if (_informedNeighbors == Neighbors.Count)
            {
                Console.WriteLine("Send ECHO to: " + ParentAddress.Port + " with size: " + CumulatedSize);
                // send to Parent
                Message mesEcho = new Message(MessageType.Echo, 0, CumulatedSize.ToString());
                Socket.Send(mesEcho.ToByteArray(), mesEcho.ToByteArray().Length, ParentAddress);
               
                CumulatedSize = MemorySize;
            }

        }    
    }
}
