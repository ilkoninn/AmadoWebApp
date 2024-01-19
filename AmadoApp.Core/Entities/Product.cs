using AmadoApp.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Core.Entities
{
    public class Product : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsInStock { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
    }
}
