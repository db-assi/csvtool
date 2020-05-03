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

            if(criteria.Count != 0)
            {
                for (var i = 1; i < expressions.Count; i++)
                {
                    expression = BitWiseOperation(expression, expressions[i], criteria[i - 1]);
                }
            }

            return Expression.Lambda<Func<DataRow, bool>>(expression, new ParameterExpression[] { parameter }).Compile();
        }

        private static List<Expression> CreatePredicates(List<Dictionary<string, object>> queries, ParameterExpression parameter)
        {
            List<Expression> expressions = new List<Expression>();

            foreach (var query in queries)
            {
                Expression predicate = Predicate(query, parameter);
                expressions.Add(predicate);
            }

            return expressions;
        }

        private static Expression Field(string field, ParameterExpression parameter)
        {
            MethodInfo method = typeof(DataRowExtensions).GetMethod("Field", new[] { typeof(DataRow), typeof(string) });
            MethodInfo generic = method.MakeGenericMethod(typeof(string));
            return Expression.Call(null, generic, parameter, Expression.Constant(field));
        }

        private static Expression Left(Dictionary<string, object> query, ParameterExpression parameter)
        {
            Expression field = Field(query["Field"].ToString(), parameter);
            Expression left;

            if (query.ContainsKey("Method"))
            {
                MethodInfo operation = VLookUps.OperationLookUp(query["Method"].ToString().ToLower());
                left = Expression.Call(field, operation, Expression.Constant(query["Value"]));
            }
            else if (query.ContainsKey("Property"))
            {
                left = VLookUps.Property(field, query["Property"].ToString().ToLower());
            }
            else
            {
                MethodInfo method = typeof(long).GetMethod("Parse", new[] { typeof(string) });
                left = Expression.Convert(field, typeof(long), method);
            }

            if (query.ContainsKey("Unary"))
            {
                return Expression.Not(left);
            }

            return left;
        }

        private static Expression Right(Dictionary<string, object> query)
        {
            Type type = query["Value"].GetType();

            if (query.ContainsKey("Method"))
            {
                return null;
            }

            return Expression.Constant(query["Value"], type);
        }

        private static Expression Predicate(Dictionary<string, object> query, ParameterExpression parameter)
        {
            Expression left = Left(query, parameter);

            if(query.ContainsKey("Method"))
            {
                return left;
            }

            Expression right = Right(query);
            return ComparissonOperation(left, right, query["Comparisson"].ToString().ToLower());
        }

        private static Expression ComparissonOperation(Expression left, Expression right, string operation)
        {
            switch (operation)
            {
                case "equal":
                    return Expression.Equal(left, right);
                case "notequal":
                    return Expression.NotEqual(left, right);
                case "greaterthan":
                    return Expression.GreaterThan(left, right);
                case "greaterthanorequal":
                    return Expression.GreaterThanOrEqual(left, right);
                case "lessthan":
                    return Expression.LessThan(left, right);
                case "lessthanorequal":
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
