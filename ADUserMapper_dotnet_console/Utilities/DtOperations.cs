using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public static DataTable Contains(DataTable dt, string col_name, string condition)
        {
            // x => x.Field<string>(field).Contains(filter)

            // x=>
            ParameterExpression parameter = Expression.Parameter(typeof(DataRow), "x");

            // Field<string>
            MethodInfo field = typeof(DataRowExtensions).GetMethod("Field", new[] { typeof(DataRow), typeof(string) });
            MethodInfo generic = field.MakeGenericMethod(typeof(string));

            // x.Field<string>(field)
            Expression _field = Expression.Call(null, generic, parameter, Expression.Constant(col_name));

            // Contains
            MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            // x.Field<string>(field).Contains(filter)
            Expression predicate = Expression.Call(_field, contains, Expression.Constant(condition));


            //Expression left = Expression.Property(_field, typeof(string).GetProperty("Length"));
            //Expression right = Expression.Constant(0, typeof(int));
            //Expression predicate = Expression.NotEqual(left, right);

            var lambda = Expression.Lambda<Func<DataRow, bool>>(predicate, new ParameterExpression[] { parameter });
            var compiled = lambda.Compile();

            var query = from a in dt.AsEnumerable()
                        .Where  (
                                   compiled
                                )
                        select a;

            return query.CopyToDataTable();

        }

        private static MethodCallExpression GetFieldCallExpression(ParameterExpression expRow, MethodInfo methodFieldGeneric, Type type, string columnName)
        {
            ConstantExpression expColumnName = Expression.Constant(columnName, typeof(string));

            MethodInfo methodFieldTyped = methodFieldGeneric.MakeGenericMethod(type);

            MethodCallExpression expCall = Expression.Call(null, methodFieldTyped, expRow, expColumnName);
            return expCall;
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

        public static DataTable IsNullD(DataTable dt, string col_name)
        {
            //x => x.Field<string>(field).Length != 0

            ParameterExpression parameter = Expression.Parameter(typeof(DataRow), "x");
            MethodInfo method = typeof(DataRowExtensions).GetMethod("Field", new[] { typeof(DataRow), typeof(string) });
            MethodInfo generic = method.MakeGenericMethod(typeof(string));
            Expression field = Expression.Call(null, generic, parameter, Expression.Constant(col_name));
            Expression left = Expression.Property(field, typeof(string).GetProperty("Length"));
            Expression right = Expression.Constant(0, typeof(int));
            Expression predicate = Expression.NotEqual(left, right);

            var lambda = Expression.Lambda<Func<DataRow, bool>>(predicate, new ParameterExpression[] { parameter });
            var compiled = lambda.Compile();

            var query = from a in dt.AsEnumerable()
                        .Where  (
                                    compiled
                                )
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

