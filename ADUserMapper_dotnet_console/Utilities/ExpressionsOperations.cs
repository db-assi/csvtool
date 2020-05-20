using ADUserMapper_dotnet_console.Logic;
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
    public static class ExpressionsOperations
    {

        public static Func<DataRow, bool> CreateCriteria(List<Dictionary<string, object>> queries, List<string> criteria, ParameterExpression parameter)
        {
            List<Expression> expressions = CreatePredicates(queries, parameter);
            Expression expression = expressions[0];

            if (criteria.Count != 0)
            {
                for (var i = 1; i < expressions.Count; i++)
                {
                    expression = BitWiseOperation(expression, expressions[i], criteria[i - 1]);
                }
            }

            return Expression.Lambda<Func<DataRow, bool>>(expression, new ParameterExpression[] { parameter }).Compile();
        }

        public static Func<DataRow, bool> CreateCriteria(Dictionary<string, object> query, ParameterExpression parameter)
        {
            Expression expression = ConditionalClause(query, parameter);

            return Expression.Lambda<Func<DataRow, bool>>(expression, new ParameterExpression[] { parameter }).Compile();
        }

        public static Action<DataRow> CreateSetField(Dictionary<string, object> query, ParameterExpression parameter)
        {
            Expression expression = SetField(query, parameter);

            return Expression.Lambda<Action<DataRow>>(expression, new ParameterExpression[] { parameter }).Compile();
        }

        private static Expression Field(string field, ParameterExpression parameter)
        {
            MethodInfo method = typeof(DataRowExtensions).GetMethod("Field", new[] { typeof(DataRow), typeof(string) });
            MethodInfo generic = method.MakeGenericMethod(typeof(string));
            return Expression.Call(null, generic, parameter, Expression.Constant(field));
        }

        private static Expression SetField(Dictionary<string, object> query, ParameterExpression parameter)
        {
            Type T = query["Value"].GetType();

            MethodInfo method = typeof(DataRowExtensions).GetMethod("SetField", new Type[] { typeof(DataRow), typeof(string), typeof(string) });
            MethodInfo generic = method.MakeGenericMethod(typeof(string));
            return Expression.Call(null, generic, parameter, Expression.Constant(query["Field"]), Expression.Constant(query["Value"]));
        }

        private static Expression Right(Dictionary<string, object> query)
        {
            Type type = query["Value"].GetType();

            if (query["Operation"].ToString().Contains("Contain"))
            {
                return null;
            }

            if (query["Operation"].ToString().Contains("Null"))
            {
                return Expression.Constant(0, typeof(int));
            }

            return Expression.Constant(query["Value"], type);
        }

        private static Expression Left(Dictionary<string, object> query, ParameterExpression parameter)
        {
            Expression field = Field(query["Field"].ToString(), parameter);
            Expression left;

            if (query["Operation"].ToString().Contains("Contain"))
            {
                MethodInfo operation = VLookUps.OperationLookUp(query["Operation"].ToString().ToLower());
                left = Expression.Call(field, operation, Expression.Constant(query["Value"]));
            }
            else if (query["Operation"].ToString().Contains("Null") || query["Operation"].ToString().Contains("Length"))
            {
                left = VLookUps.Property(field, query["Operation"].ToString().ToLower());
            }
            else
            {
                MethodInfo method = typeof(Int64).GetMethod("Parse", new[] { typeof(string) });
                left = Expression.Convert(field, typeof(long), method);
            }

            return left;
        }

        private static Expression Predicate(Dictionary<string, object> query, ParameterExpression parameter)
        {
            Expression left = Left(query, parameter);
            Expression right = Right(query);
            Expression predicate;

            if (query["Operation"].ToString().Contains("Contain"))
            {
                predicate = left;
            }

            if (query["Operation"].ToString().Contains("Null"))
            {    
                predicate = ComparissonOperation(left, right, "equal");
            } else
            {
                predicate = ComparissonOperation(left, right, query["Operation"].ToString().ToLower());
            }

            if (query["Operation"].ToString().Contains("Not"))
            {
                return Expression.Not(predicate);
            }

            return predicate;
        }

        private static Expression IsNotNull(Dictionary<string, object> query, ParameterExpression parameter)
        {
            query["Operation"] = "IsNotNull";

            return Predicate(query, parameter);
        }

        private static Expression IsNull(Dictionary<string, object> query, ParameterExpression parameter)
        {
            query["Operation"] = "IsNull";

            return Predicate(query, parameter);
        }

        private static Expression ConditionalClause(Dictionary<string, object> query, ParameterExpression parameter)
        {
            Expression ifTrue = Predicate(query, parameter);

            if(query["Operation"].ToString().Contains("Null"))
            {
                return ifTrue;
            }

            Expression test = IsNotNull(query, parameter);
            Expression ifFalse = Expression.Constant(false);

            if(query["KeepNull"].Equals(true))
            {
                ifFalse = IsNull(query, parameter);
            }

            return Expression.Condition(test, ifTrue, ifFalse);
        }

        private static List<Expression> CreatePredicates(List<Dictionary<string, object>> queries, ParameterExpression parameter)
        {
            List<Expression> expressions = new List<Expression>();

            foreach (var query in queries)
            {
                Expression predicate = ConditionalClause(query, parameter);
                expressions.Add(predicate);
            }

            return expressions;
        }

        private static Expression ComparissonOperation(Expression left, Expression right, string operation)
        {
            switch (operation)
            {
                case string a when a.Contains("equal"):
                    return Expression.Equal(left, right);
                case string a when a.Contains("notequal"):
                    return Expression.NotEqual(left, right);
                case string a when a.Contains("greaterthan"):
                    return Expression.GreaterThan(left, right);
                case string a when a.Contains("greaterthanorequal"):
                    return Expression.GreaterThanOrEqual(left, right);
                case string a when a.Contains("lessthan"):
                    return Expression.LessThan(left, right);
                case string a when a.Contains("lessthanorequal"):
                    return Expression.LessThanOrEqual(left, right);
                default:
                    return left;
            }
        }

        private static Expression BitWiseOperation(Expression e1, Expression e2, string operation)
        {
            if (operation == "and")
            {
                return Expression.And(e1, e2);
            }
 
            return Expression.Or(e1, e2);
        }
    }
}
