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
    class Logger
    {
        private UdpClient Socket { get; set; }
        private const string Address = "192.168.178.69";
        private const int Port = 6666;
        private IPEndPoint LoggerAddress = new IPEndPoint(IPAddress.Parse(Address), Port);

        public Logger()
        {
            Socket = new UdpClient(Port);
        }

        public void Start(IPEndPoint initNode)
        {
            Console.WriteLine("Logger Started...");
            string input = "";
            // lauschen
            Task receiverTask = Task.Run(() =>
            {
                bool isRunning = true;
                while (input != "stop" || isRunning)
                {
                    IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

                    byte[] receivedBytes = Socket.Receive(ref sender);

                    Message message = Message.FromByteArray(receivedBytes);

                    if (message.Type == MessageType.Logging)
                        Console.WriteLine(message.Data);
                    else if (message.Type == MessageType.Echo)
                    {
                        Console.WriteLine("Echo: From: 192.168.178.69:" + sender.Port + " To: 192.168.178.69:" + Port + " Value: " + message.Data);
                        Console.WriteLine("The size of the network is: " + message.Data );
                        Console.WriteLine("Algorithm STOPPED!");
                        isRunning = false;
                    }
                }
            });          
            
            Message mes = new Message(MessageType.Info, 0, null);
            Socket.Send(mes.ToByteArray(), mes.ToByteArray().Length, initNode);
            // stop the algorithm
            do
            {
                input = Console.ReadLine();
            } while (input != "stop");
        }
    }
}
