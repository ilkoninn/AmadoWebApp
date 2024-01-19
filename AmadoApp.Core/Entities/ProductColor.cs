using AmadoApp.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Core.Entities
{
    public class ProductColor : BaseAuditableEntity
    {
        public int ColorId { get; set; }
        public Color? Color { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
