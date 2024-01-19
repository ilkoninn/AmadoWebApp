using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Exceptions.Product
{
    public class ProductImageException : Exception
    {
        public string ParamName { get; set; }
        public ProductImageException(string? message, string paramName) : base(message)
        {
            ParamName = paramName ?? string.Empty;
        }
    }
}
