using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserMapper_dotnet_console.Models
{
    public class Query
    {
        public string Field { get; set; }
        public string Operation { get; set; }
        public object Value { get; set; }

        public Query(string _field, string _operation, object _value)
        {
            Field = _field;
            Operation = _operation;
            Value = _value;
        }
    }
}
