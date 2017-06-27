using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Labor3_Echo;

namespace NodeStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Node started!");
            Random r = new Random((int)DateTime.Now.Ticks);
            uint size = (uint) r.Next(2, 42);
            IPEndPoint address = new IPEndPoint(IPAddress.Parse("192.168.178.69"), 2220);
            Node me = new Node("Node " + 2220, size, address);

            string check = "";
            bool isRunning = true;
            while(isRunning)
            {
                Task receiverTask = Task.Run(() =>
                {                    
                    while (isRunning)
                    {
                        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                        byte[] receivedBytes = me.Socket.Receive(ref sender);
                        Message message = Message.FromByteArray(receivedBytes);
                        me.Receive(sender, message);
                    }
                });
                while (check != "stop")
                    check = Console.ReadLine();
                isRunning = false;
            }            
        }
    }
}
