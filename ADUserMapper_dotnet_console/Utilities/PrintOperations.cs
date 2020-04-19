using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserMapper_dotnet_console.Utilities
{
    public static class PrintOperations
    {
        public static void PrintDataTable(DataTable dataTable)
        {
            //Headers
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                Console.Write(dataTable.Columns[i]);

                if (i < dataTable.Columns.Count - 1)
                {
                    Console.Write(",");
                }
                else
                {
                    Console.Write("\n");
                }
            }

            //Rows
            foreach(DataRow dataRow in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dataRow[i]))
                    {
                        Console.Write(dataRow[i]);

                        if (i < dataTable.Columns.Count - 1)
                        {
                            Console.Write(",");
                        }
                        else
                        {
                            Console.Write("\n");
                        }
                    }
                }
            }






        }
    }
}
