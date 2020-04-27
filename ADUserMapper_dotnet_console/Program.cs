using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADUserMapper_dotnet_console.Logic;
using ADUserMapper_dotnet_console.Utilities;

namespace ADUserMapper_dotnet_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.processAD();

            //DateTime now = DateTime.Now;

            //Console.WriteLine(now.ToFileTime());

            //if(now.ToFileTime()> 9220000000000000000)
            //{
            //    Console.WriteLine("true");
            //} else
            //{
            //    Console.WriteLine("false");
            //}

            Console.ReadLine();
        }
    }
}
