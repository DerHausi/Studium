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
        public UdpClient Socket { get; set; }


        private IPEndPoint LoggerAddress = new IPEndPoint(IPAddress.Parse("192.168.2.10"), 6666);

        public Logger()
        {
          
        }


    }
}
