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
            //x => x.Field<string>(field_name).Method(parameter) || x => x.Field<string>(field_name).Method(parameter) && x => x.Field<string>(field_name).Method(parameter) 
            List<Expression> expressions = CreatePredicates(queries, parameter);

            Expression expression = expressions[0];

            for (var i = 1; i < expressions.Count; i++)
            {
                expression = BitWiseOperation(expression, expressions[i], criteria[i - 1]);
            }

            var lambda = Expression.Lambda<Func<DataRow, bool>>(expression, new ParameterExpression[] { parameter }).Compile();

            return lambda;

        }

        private static List<Expression> CreatePredicates(List<Query> queries, ParameterExpression parameter)
        {
            //x => x.Field<string>(field_name).Method(parameter)
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
            //x.Field<string>(field_name)
            MethodInfo method = typeof(DataRowExtensions).GetMethod("Field", new[] { typeof(DataRow), typeof(string) });
            MethodInfo generic = method.MakeGenericMethod(typeof(string));
            return Expression.Call(null, generic, parameter, Expression.Constant(field));
        }

        private static Expression Left(Query query, ParameterExpression parameter)
        {

            Expression field = Field(query.Field, parameter);
            string type = VLookUps.LeftOperation(query.Operation);

            //x.Field<string>(field_name).Method(parameter)
            if (type == "MethodInfo")
            {
                MethodInfo operation = VLookUps.OperationLookUp(query.Operation);
                return Expression.Call(field, operation, Expression.Constant(query.Value));
            }
            //x.Field<string>(field_name).Property
            else if (type == "Property")
            {
                return VLookUps.Property(field, query.Operation);
            }
            //x.Field<string>(field_name)
            else
            {
                return field;
            }
        }

        private static Expression Right(Query query)
        {
            string operation = VLookUps.LeftOperation(query.Operation);
            Type type = query.Value.GetType();

            if (operation == "MethodInfo")
            {
                return null;
            }
            else
            {
                return Expression.Constant(query.Value, type);
            }
        }

        private static Expression Predicate(Query query, ParameterExpression parameter)
        {
            string operation = VLookUps.LeftOperation(query.Operation);
            Expression left = Left(query, parameter);

            if(operation == "MethodInfo")
            {
                return left;
            } 

            Expression right = Right(query);
            return ComparissonOperation(left, right, query.Operation);
        }

        private static Expression ComparissonOperation(Expression e1, Expression e2, string operation)
        {
            switch (operation)
            {
                case "equal":
                    return Expression.Equal(e1, e2);
                case "notequal":
                    return Expression.NotEqual(e1, e2);
                case "greaterthan":
                    return Expression.GreaterThan(e1, e2);
                case "greaterthanorequal":
                    return Expression.GreaterThanOrEqual(e1, e2);
                case "lessthan":
                    return Expression.LessThan(e1, e2);
                case "lessthanorequal":
                    return Expression.LessThanOrEqual(e1, e2);
                default:
                    return e1;
            }
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
