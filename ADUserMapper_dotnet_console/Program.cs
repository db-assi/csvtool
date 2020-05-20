using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

            //var methods = typeof(DataRowExtensions).GetMethods();

            //foreach (var m in methods)
            //{
            //    //Console.WriteLine(m);

            //    foreach (var pars in m.GetParameters())
            //    {
            //        Console.WriteLine(m + "Par Type: " + pars.ParameterType);
            //    }
            //}

            //var method = typeof(DataRowExtensions).GetMethod("Field", new[] { typeof(DataRow), typeof(DataColumn), typeof(string) });

            //Console.WriteLine("Returned method: " + method);

            Console.ReadLine();
        }
    }
}
