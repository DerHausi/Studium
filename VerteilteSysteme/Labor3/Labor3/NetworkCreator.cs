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

        
        public static IPEndPoint InitNetwork()
        {
            // init nodes
            SendNeighbors(node0, node1, node3, logger);
            SendNeighbors(node1, node0, node2, node4);
            SendNeighbors(node2, node1, node3, node5);
            SendNeighbors(node3, node0, node2, node6);
            SendNeighbors(node4, node1, node5, node7);
            SendNeighbors(node5, node2, node4, node6, node7);
            SendNeighbors(node6, node3, node5, node7);
            SendNeighbors(node7, node4, node5, node6);

            // RETURN start node
            return node0;
        }

        public static void SendNeighbors(IPEndPoint destination, IPEndPoint ep1, IPEndPoint ep2 = null, IPEndPoint ep3 = null, IPEndPoint ep4 = null)
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

            Message message = new Message(MessageType.Neighbors, 0, list.ToString());
            Socket.Send(message.ToByteArray(), message.ToByteArray().Length, destination);
        }
    }
}
