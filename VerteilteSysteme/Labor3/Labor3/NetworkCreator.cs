using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Labor3
{
    public static class NetworkCreator
    {
        public static IPEndPoint InitNetwork()
        {



            // TODO RETURN start node
            return new IPEndPoint(IPAddress.Parse("192.168.178.69"), 1111);
        }

    }
}
