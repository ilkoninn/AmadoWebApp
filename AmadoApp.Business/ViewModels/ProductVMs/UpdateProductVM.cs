using AmadoApp.Business.ViewModels.CommonVMs;
using AmadoApp.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.ViewModels.ProductVMs
{
    public class UpdateProductVM : BaseEntityVM
    {
        // Get Section
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsInStock { get; set; }
        public int BrandId { get; set; }
        public ICollection<int>? CategoryIds { get; set; }
        public ICollection<int>? ColorIds { get; set; }
        public ICollection<int>? ProductImageIds { get; set; }   
        public ICollection<IFormFile>? ProductFiles { get; set; }

        // Post Section
        public IQueryable<Brand>? Brands { get; set; }
        public IQueryable<Category>? Categories { get; set; }
        public List<Color>? Colors { get; set; }
        public List<ProductImageVM>? ProductImageVMs { get; set; }
    }

    public class ProductImageVM : BaseEntityVM
    {
        public string ImageUrl { get; set; }
    }
}
