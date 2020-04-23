using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADUserMapper_dotnet_console.Utilities
{
    public static class DtOperations
    {
        public static DataTable RemoveColumns(DataTable dt, string[] columns)
        {

            for (int i = 0; columns.Length > i; i++)
            {
                dt.Columns.Remove(columns[i]);
            }

            return dt;

        }

        public static DataTable Contains(DataTable dt, string field, string filter)
        {
            var parameterExpression = Expression.Parameter(Type.GetType("DataColumn"), "x");
            var constant = Expression.Constant(filter);
            var property = Expression.Property(parameterExpression, field);
            var expression = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<DataRow, bool>>(expression, parameterExpression);
            var compiledLambda = lambda.Compile();

            var query = from a in dt.AsEnumerable()
                        .Where  (
                                    //x => x.Field<string>(field).Contains(filter)
                                    compiledLambda
                                )
                        select a;

            return query.CopyToDataTable();

        }

        public static DataTable Excludes(DataTable dt, string field, string filter)
        {
            var query = from a in dt.AsEnumerable()
            .Where(x =>
                      !x.Field<string>(field).Contains(filter)
                    )
                        select a;

            return query.CopyToDataTable();
        }

        public static DataTable IsNull(DataTable dt, string field)
        {
            var query = from a in dt.AsEnumerable()
                        where a.Field<string>(field).Length != 0
                        select a;

            return query.CopyToDataTable();
        }

        public static DataTable ConditionalColumns(DataTable dt, string newCol, string lookUpCol, Func<string, string> VLooupMethod)
        {
            dt = AddColumnDummyData(dt, newCol);

            int lookUpColIndex = dt.Columns.IndexOf(lookUpCol);
            int newColIndex = dt.Columns.IndexOf(newCol);

            DataRow[] rows = dt.Select();

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                rows[i][newColIndex] = VLooupMethod(rows[i][lookUpColIndex].ToString());

            }

            return dt;
        }

        public static DataTable AddColumnDummyData(DataTable dt, string[] colNames)
        {
            DataColumn col;

            for (int i = 0; i < colNames.Length; i++)
            {
                col = new DataColumn
                {
                    DataType = Type.GetType("System.String"),
                    ColumnName = colNames[i],
                    DefaultValue = "NULL"

                };
                dt.Columns.Add(col);
            }

            return dt;
        }

        public static DataTable AddColumnDummyData(DataTable dt, string colName)
        {
            DataColumn col;

            col = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = colName,
                DefaultValue = "NULL"

            };
            dt.Columns.Add(col);

            return dt;
        }

        public static DataTable ChangeColumnName(DataTable dt, string[] oldNames, string [] newNames)
        {

            for(int i = 0; i < oldNames.Length; i++)
            {
                dt.Columns[oldNames[i]].ColumnName = newNames[i];
            }


            return dt;
        }

        public static DataTable AddColumnsDefaultValue(DataTable dt, string colName, string value)
        {
            DataColumn col;

            col = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = colName,
                DefaultValue = "GOSH"

            };
            dt.Columns.Add(col);

            return dt;
        }



    }
}

