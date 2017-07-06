using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Labor3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write \"SUM <IP-Address> <Port>\" to start the algorithm!");
            string input = "";
            string[] inputArray;
            // start the algorithm
            do
            {
                input = Console.ReadLine();
                inputArray = input.Split(' ');
            } while (inputArray[0] != "SUM");
            // TODO erstelle Netzwerk
            NetworkCreator.InitNetwork();

            IPEndPoint initNode = new IPEndPoint(IPAddress.Parse(inputArray[1]), int.Parse(inputArray[2]));
            // TODO erstelle Logger
            Logger logger = new Logger();
            logger.Start(initNode);
        }
    }
}
