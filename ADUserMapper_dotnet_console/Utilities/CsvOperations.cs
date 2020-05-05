using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserMapper_dotnet_console.Utilities
{
    public static class CsvOperations
    {
        public static DataTable CsvToDataTable(string path)
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        using (var data = new CsvDataReader(csv))
                        {
                            var dt = new DataTable();
                            dt.Load(data);

                            return dt;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static void DataTableToCsv(DataTable dt, string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sw.Write(dt.Columns[i]);
                        if (i < dt.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);

                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                string value = dr[i].ToString();
                                if (value.Contains(','))
                                {
                                    value = String.Format("\"{0}\"", value);
                                    sw.Write(value);
                                }
                                else
                                {
                                    sw.Write(dr[i].ToString());
                                }
                            }
                            if (i < dt.Columns.Count - 1)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private static DataTable ReplaceEmptyString(DataTable dt)
        {
            foreach(DataColumn col in dt.Columns)
            {
                col.ReadOnly = false;
            }

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (row[i].ToString().Length == 0)
                    {
                        row[i] = "0";
                    }
                }
            }

            return dt;
        }

    }
}
