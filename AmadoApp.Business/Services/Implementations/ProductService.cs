using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Exceptions.Product;
using AmadoApp.Business.Helpers;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.ProductVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _rep;
        private readonly IBrandRepository _repBrand;
        public ProductService(IProductRepository rep, IBrandRepository repBrand)
        {
            _rep = rep;
            _repBrand = repBrand;
        }

        private string[] includes = { 
            "Brand", 
            "ProductCategories", "ProductCategories.Category", 
            "ProductColors", "ProductColors.Color",
            "ProductImages", 
        };

        public async Task<IQueryable<Product>> ReadAsync()
        {
            IQueryable<Product> query = await _rep.ReadAsync(includes: includes);

            return query;
        }

        public async Task<Product> ReadIdAsync(int Id)
        {
            var oldProdct =  await CheckProduct(Id, includes);

            return oldProdct;
        }

        public async Task CreateAsync(CreateProductVM entity, string env)
        {
            var entityIsNull = entity.ColorIds == null || entity.CategoryIds == null || entity.Title is null
                || entity.Description is null || entity.Price == null || entity.ProductFiles is null 
                || entity.BrandId == null;

            if (entityIsNull) throw new ObjectParamsNullException("Object parameters is required", nameof(entity.Title));

            var existsSameTitle = await (await _rep.ReadAsync(expression: x =>  x.Title == entity.Title))
                .FirstOrDefaultAsync() is null;
            var existsSameDescription = await (await _rep.ReadAsync(expression: x => x.Description == entity.Description))
                .FirstOrDefaultAsync() is null;

            if (!existsSameTitle) throw new ObjectSameParamsException("There is same title product in data!", nameof(entity.Title));  
            if (!existsSameDescription) throw new ObjectSameParamsException("There is same description product in data!", nameof(entity.Description));

            Product newProduct = new()
            {
                Title = entity.Title,
                Description = entity.Description,
                Price = entity.Price,
                IsInStock = entity.IsInStock,
                BrandId = entity.BrandId,
                ProductImages = new List<ProductImage>(),
                ProductColors = new List<ProductColor>(),
                ProductCategories = new List<ProductCategory>(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            // Product Image Section
            if(entity.ProductFiles is not null)
            {
                foreach (var item in entity.ProductFiles)
                {
                    if (!item.CheckImage()) throw new ProductImageException("File must be image format and lower than 3MB!", nameof(item));

                    ProductImage newProductImage = new()
                    {
                        Product = newProduct,
                        ImageUrl = item.Upload(env, @"/Upload/ProductImages/")
                    };

                    newProduct.ProductImages.Add(newProductImage);
                }
            }
            else
            {
                throw new ObjectParamsNullException("Object parameters is required!", nameof(entity.ProductFiles));
            }

            // Product Color Section
            if(entity.ColorIds is not null)
            {
                foreach (var item in entity.ColorIds)
                {
                    ProductColor newProductColor = new()
                    {
                        Product = newProduct,
                        ColorId = item,
                    };

                    newProduct.ProductColors.Add(newProductColor);
                }
            }
            else
            {
                throw new ObjectParamsNullException("Object parameters is required!", nameof(entity.ProductFiles));
            }

            // Product Category Section
            if (entity.CategoryIds is not null)
            {
                foreach (var item in entity.CategoryIds)
                {
                    ProductCategory newProductCategory = new()
                    {
                        Product = newProduct,
                        CategoryId = item,
                    };

                    newProduct.ProductCategories.Add(newProductCategory);
                }
            }
            else
            {
                throw new ObjectParamsNullException("Object parameters is required!", nameof(entity.ProductFiles));
            }

            await _rep.CreateAsync(newProduct);
            await _rep.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateProductVM entity, string env)
        {
            var oldProduct = await CheckProduct(entity.Id, includes);

            var entityIsNull = entity.ColorIds == null || entity.CategoryIds == null || entity.Title is null
                || entity.Description is null || entity.Price == null || entity.BrandId == null;

            if (entityIsNull) throw new ObjectParamsNullException("Object parameters is required", nameof(entity.Title));

            var existsSameTitle = await (await _rep.ReadAsync(expression: x => x.Title == entity.Title && x.Id != entity.Id))
               .FirstOrDefaultAsync() is null;
            var existsSameDescription = await (await _rep.ReadAsync(expression: x => x.Description == entity.Description && x.Id != entity.Id))
                .FirstOrDefaultAsync() is null;

            if (!existsSameTitle) throw new ObjectSameParamsException("There is same title product in data!", nameof(entity.Title));
            if (!existsSameDescription) throw new ObjectSameParamsException("There is same description product in data!", nameof(entity.Description));

            oldProduct.Title = entity.Title;
            oldProduct.Description = entity.Description;
            oldProduct.Price = entity.Price;
            oldProduct.IsInStock = entity.IsInStock;
            oldProduct.UpdatedDate = DateTime.Now;
            oldProduct.Brand = await _repBrand.ReadIdAsync(entity.BrandId);

            //oldProduct.BrandId = entity.BrandId; ==> ola da biler, olmaya da
            
            await _rep.UpdateAsync(oldProduct);

            oldProduct.ProductColors.Clear();
            oldProduct.ProductCategories.Clear();   

            // Product Color Section
            if (entity.ColorIds is not null)
            {
                foreach (var item in entity.ColorIds)
                {
                    ProductColor newProductColor = new()
                    {
                        Product = oldProduct,
                        ColorId = item,
                    };

                    oldProduct.ProductColors.Add(newProductColor);
                }
            }

            // Product Category Section
            if (entity.CategoryIds is not null)
            {
                foreach (var item in entity.CategoryIds)
                {
                    ProductCategory newProductCategory = new()
                    {
                        Product = oldProduct,
                        CategoryId = item,
                    };

                    oldProduct.ProductCategories.Add(newProductCategory);
                }
            }

            if(entity.ProductImageIds == null)
            {
                oldProduct.ProductImages.Clear();
            }
            else
            {
                List<ProductImage> removeList = oldProduct.ProductImages.Where(pt => !entity.ProductImageIds.Contains(pt.Id)).ToList();

                if (removeList.Count > 0)
                {
                    foreach (var item in removeList)
                    {
                        oldProduct.ProductImages.Remove(item);
                        FileManager.Delete(item.ImageUrl, env, @"/Upload/ProductImages/");
                    }
                }
            }

            // Product Image Section
            if (entity.ProductFiles != null)
            {
                foreach (var item in entity.ProductFiles)
                {
                    if (!item.CheckImage()) throw new ProductImageException("File must be image format and lower than 3MB!", nameof(item));

                    ProductImage newProductImage = new()
                    {
                        Product = oldProduct,
                        ImageUrl = item.Upload(env, @"/Upload/ProductImages/")
                    };

                    oldProduct.ProductImages.Add(newProductImage);
                }
            }

            await _rep.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            await CheckProduct(Id);
            
            await _rep.DeleteAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task RecoverAsync(int Id)
        {
            await CheckProduct(Id);

            await _rep.RecoverAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task RemoveAsync(int Id)
        {
            await CheckProduct(Id);

            await _rep.RemoveAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task<Product> CheckProduct(int id, params string[] includes)
        {
            if (id <= 0) throw new IdNegativeOrZeroException("Id must be over than and not equal to zero!", nameof(id));
            var oldProduct = await _rep.ReadIdAsync(Id: id, entityIncludes: includes);
            if (oldProduct is null) throw new ObjectNullException("There is no like that object in Data!", nameof(oldProduct));

            return oldProduct;
        }
    }
}
