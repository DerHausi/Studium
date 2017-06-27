using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Labor3
{
    public class NeighborList : List<IPEndPoint>
    {
        public void AddFromString(string s)
        {
            List<string> addresses = s.Split(';').ToList();
            foreach(var addr in addresses)
            {
                string[] adString = addr.Split(':');
                IPEndPoint endPoint = new IPEndPoint(long.Parse(adString[0]), int.Parse(adString[1]));
                Add(endPoint);
            }
        }
        
        override public string ToString()
        {
            string message = "";
            foreach(var v in this)
            {
                message += (v.Address + ":" + v.Port + ";");
            }
            return message;
        }
    }
}
