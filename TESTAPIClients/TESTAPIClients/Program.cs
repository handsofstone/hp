using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPIClients
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var c in TSNAPIClient.GetPlayerPositions("auston-matthews"))
                System.Console.WriteLine(c);
            System.Console.ReadKey();
        }
    }
}
