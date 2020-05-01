using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADUserMapper_dotnet_console.Logic
{
    public static class VLookUps
    {
        public static string StateLookUp(string abbreviation)
        {
            switch (abbreviation)
            {
                case "AL":
                    return "Alaska";
                case "CA":
                    return "California";
                case "CO":
                    return "Colorado";
                case "FL":
                    return "Florida";
                case "MI":
                    return "Michigan";
                default:
                    return "Not Know";
            }
        }

        public static MethodInfo OperationLookUp(string operation)
        {
            switch(operation)
            {
                case "contains":
                    return typeof(string).GetMethod("Contains", new[] { typeof(string) });
                default:
                    return null;
            }
        }

        public static string LeftOperation(string operation)
        {
            switch (operation)
            {
                case "contains":
                    return "MethodInfo";
                case "length":
                    return "Property";
                default:
                    return "Binary";
            }
        }

        public static Expression Property(Expression e, string property)
        {
            switch (property)
            {
                case "lenght":
                    return Expression.Property(e, typeof(string).GetProperty("Length"));
                default:
                    return null;
            }
        }









    }
}
