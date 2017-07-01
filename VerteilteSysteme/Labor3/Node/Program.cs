using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Labor3_Echo;
using System.Net.Sockets;

namespace NodeStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            uint size = (uint)r.Next(1, 11);
            IPEndPoint address = new IPEndPoint(IPAddress.Parse("192.168.178.69"), int.Parse(args[0]));
            Node me = new Node("Node " + int.Parse(args[0]), size, address);
            Console.WriteLine(me.Name + ": " + size);
            string check = "";
            bool isRunning = true;

            Task receiverTask = Task.Run(() =>
            {
                while (isRunning)
                {
                    try
                    {
                        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                        byte[] receivedBytes = me.Socket.Receive(ref sender);
                        Message message = Message.FromByteArray(receivedBytes);
                        me.Receive(sender, message);
                    }
                    catch (SocketException e)
                    {
                        me.Socket.Close();
                        me.Socket = new UdpClient(address);
                    }
                }
            });
            while (check != "stop")
                check = Console.ReadLine();
            isRunning = false;            
        }
    }
}
