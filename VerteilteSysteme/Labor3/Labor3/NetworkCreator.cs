using Labor3_Echo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Labor3
{
    public static class NetworkCreator
    {
        public static UdpClient Socket = new UdpClient();
        public static IPEndPoint logger = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 6666);
        public static IPEndPoint node0 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2220);
        public static IPEndPoint node1 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2221);
        public static IPEndPoint node2 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2222);
        public static IPEndPoint node3 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2223);
        public static IPEndPoint node4 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2224);
        public static IPEndPoint node5 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2225);
        public static IPEndPoint node6 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2226);
        public static IPEndPoint node7 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2227);
        public static IPEndPoint node8 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2228);
        public static IPEndPoint node9 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2229);
        public static IPEndPoint node10 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2230);
        public static IPEndPoint node11 = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2231);

        public static void InitNetwork()
        {
            // init nodes
            SendNeighbors(node0, node1, node2, node3, logger);
            SendNeighbors(node1, node0, node2, node4, node5);
            SendNeighbors(node2, node0, node1, node3, node5, node6);
            SendNeighbors(node3, node0, node2, node6, node7);
            SendNeighbors(node4, node1, node5, node8);
            SendNeighbors(node5, node1, node2, node4, node6, node8, node9);
            SendNeighbors(node6, node2, node3, node5, node7, node9, node10);
            SendNeighbors(node7, node3, node6, node10);
            SendNeighbors(node8, node4, node5, node9, node11);
            SendNeighbors(node9, node5, node6, node8, node10, node11);
            SendNeighbors(node10, node6, node7, node9, node11);
            SendNeighbors(node11, node8, node9, node10);            
        }

        public static void SendNeighbors(IPEndPoint destination, IPEndPoint ep1, IPEndPoint ep2 = null, IPEndPoint ep3 = null, IPEndPoint ep4 = null, IPEndPoint ep5 = null, IPEndPoint ep6 = null)
        {
            NeighborList list = new NeighborList();
            if (ep1 != null)
                list.Add(ep1);
            if (ep2 != null)
                list.Add(ep2);
            if (ep3 != null)
                list.Add(ep3);
            if (ep4 != null)
                list.Add(ep4);
            if (ep5 != null)
                list.Add(ep5);
            if (ep6 != null)
                list.Add(ep6);

            Message message = new Message(MessageType.Neighbors, 0, list.ToString());
            Socket.Send(message.ToByteArray(), message.ToByteArray().Length, destination);
        }
    }
}
