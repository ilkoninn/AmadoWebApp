using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Exceptions.Commons
{
    public class ObjectParamsNullException : Exception
    {
        public string ParamName { get; set; }
        public ObjectParamsNullException(string? message, string paramName) : base(message)
        {
            ParamName = paramName ?? string.Empty;
        }
    }
}
