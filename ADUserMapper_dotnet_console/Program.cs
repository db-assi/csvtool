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

            //Console.WriteLine(DateTime.Today.ToFileTime());
            //string date = "132000000000000000";

            //long number = long.Parse(date);

            //if (number < DateTime.Today.ToFileTime())
            //{
            //    Console.WriteLine("this is true" + number + "<" + DateTime.Today.ToFileTime());
            //}
            //else
            //{
            //    Console.WriteLine("this is false" + number + "<" + DateTime.Today.ToFileTime());
            //}

            //Console.WriteLine(long.Parse("0"));

            //var q1 = new Dictionary<string, object>
            //{
            //    ["Field"] = "CanonicalName",
            //    ["Operation"] = "NotContain",
            //    ["Value"] = "NormalUsers",
            //    ["KeepNull"] = false
            //};

            //if (q1["Operation"].ToString().Contains("Not"))
            //{
            //    Console.WriteLine("q1 contains... NotContains " + q1["Operation"]);
            //}
            



            //q1["Operation"] = "IsNull";

            //if (q1["Operation"].ToString().Contains("Null"))
            //{
            //    Console.WriteLine("q1 contains... Null " + q1["Operation"]);
            //}

            //Console.WriteLine("after change " + q1["Operation"]);

            Console.ReadLine();
        }
    }
}
