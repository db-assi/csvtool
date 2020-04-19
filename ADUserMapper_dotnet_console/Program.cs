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

            Console.ReadLine();
        }
    }
}
