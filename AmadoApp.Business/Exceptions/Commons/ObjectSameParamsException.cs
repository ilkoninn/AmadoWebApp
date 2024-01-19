using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Exceptions.Commons
{
    public class ObjectSameParamsException : Exception
    {
        public string ParamName { get; set; }
        public ObjectSameParamsException(string? message, string paramName) : base(message)
        {
            ParamName = paramName ?? string.Empty;
        }
    }
}
