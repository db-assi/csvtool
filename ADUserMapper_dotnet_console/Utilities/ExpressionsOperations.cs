using ADUserMapper_dotnet_console.Logic;
using ADUserMapper_dotnet_console.Models;
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

        public static Func<DataRow, bool> CreateCompoundCriteria(List<Query> queries, List<string> criteria, ParameterExpression parameter)
        {
            List<Expression> expressions = CreatePredicates(queries, parameter);

            Expression expression = expressions[0];

            for (var i = 1; i < expressions.Count; i++)
            {
                expression = BitWiseOperation(expression, expressions[i], criteria[i - 1]);
            }

            var compiled =  Expression.Lambda<Func<DataRow, bool>>(expression, new ParameterExpression[] { parameter }).Compile();

            return compiled;

        }

        private static List<Expression> CreatePredicates(List<Query> queries, ParameterExpression parameter)
        {
            List<Expression> expressions = new List<Expression>();

            foreach (var query in queries)
            {
                Expression predicate = CreatePredicate(query, parameter);
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

        private static Expression CreatePredicate(Query query, ParameterExpression parameter)
        {
            Expression field = Field(query.Field, parameter);
            MethodInfo operation = VLookUps.OperationLookUp(query.Operation);
            return Expression.Call(field, operation, Expression.Constant(query.Value));
        }

        private static Expression BitWiseOperation(Expression e1, Expression e2, string operation)
        {
            if (operation == "and")
            {
                return Expression.And(e1, e2);
            }
            else
            {
                return Expression.Or(e1, e2);
            }
        }
    }
}
