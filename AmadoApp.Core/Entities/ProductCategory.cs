using AmadoApp.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Core.Entities
{
    public class ProductCategory : BaseAuditableEntity
    {
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
