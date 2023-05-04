using GeeekHouseAPI.Data;
using GeeekHouseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly GeekHouseContext context;

        public ProductRepository(GeekHouseContext context)
        {
            this.context = context;
        }
        public async Task<List<ProductModel>> GetRelatedProductsByCategory(int[] categoriesRelated)
        {

            var data = await context.Product.Where(p => p.Categories.Any(category => categoriesRelated.Contains(category.Id)))
              .Select(p => new ProductModel
              {

                  Id = p.Id,
                  Name = p.Name,
                  Image = p.Image != null ? p.Image.Select(i => new ImageModel
                  {
                      Id = i.Id,
                      Mime = i.Mime,
                      Name = i.Name,
                      Path = i.Path
                  }).FirstOrDefault() : null,
                  Availability = p.Availability != null ? new AvailabilityModel
                  {
                      Description = p.Availability.Description,
                      Id = p.Availability.Id
                  } : null,
                  Price = p.Price,
                  Path = p.Name.Replace(' ', '-'),
                  Description = p.Description,
                  Stock = p.Stock
              }
              ).Take(8).ToListAsync();


            return data;
        }
        public async Task<List<ProductModel>> GetRecentProducts()
        {
            var data = await context.Product.Select(p => new ProductModel()
            {
                Id= p.Id,
                Name=p.Name,
                Image=p.Image != null ?  p.Image.Select(i=> new ImageModel
                {
                    Id=i.Id,
                    Mime=i.Mime,
                    Name=i.Name,
                    Path=i.Path
                }).FirstOrDefault() : null,
                Availability = p.Availability != null ? new AvailabilityModel
                {
                    Description = p.Availability.Description,
                    Id = p.Availability.Id
                } : null,
                Price=p.Price,
                Path= p.Name.Replace(' ','-'),
                Description=p.Description,
                Stock=p.Stock

            }).Take(8).ToListAsync();
            return data;

        }

        public async Task<ProductModel> GetProductByName(string name)
        {
            var product = await context.Product.Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
      
                Availability = p.Availability != null ? new AvailabilityModel
                {
                    Description = p.Availability.Description,
                    Id = p.Availability.Id
                }:null,
                Price=p.Price,
                Description = p.Description,
                Stock = p.Stock,
               
            }).Where(p => p.Name == name).SingleOrDefaultAsync();

            var images = await context.Image.Where(i => i.product.Id == product.Id).Select(i => new ImageModel
            {
                Id=i.Id,
                Mime=i.Mime,
                Name=i.Name,
                Path=i.Path
            }).ToListAsync();

            var categories = await context.Category.Where(c => c.Products.Any(p => p.Id == product.Id)).Select(c=>new CategoryModel
            {
                Id=c.Id,
                Name=c.name
            }).ToListAsync();
            product.Categories = categories;
            product.Images = images;
            return product;
        }

        public async Task<int> AddProduct(ProductModel productModel, List<int> categoryList,int availabilityType)
        {
            var categories = await context.Category.Where(c => categoryList.Contains(c.Id)).ToListAsync();
            var availability = await context.Availability.Where(a => a.Id == availabilityType).FirstOrDefaultAsync();
            var product = new Product()
            {
                Name = productModel.Name,
                Categories = categories,
                Image =productModel.Images != null ? productModel.Images.ToList().Select(i => new Image
                {
                Id=i.Id,
                Mime=i.Mime,
                Name=i.Name,
                Path=i.Path,
                
                }).ToList() : null
                ,
                Description=productModel.Description,
                Availability = availability,
                Price=productModel.Price
            };
            
            context.Product.Add(product);

            await context.SaveChangesAsync();

            return product.Id;

        }
        public async Task<AdvancedSearchModel> GetProductsAdvancedSearch(string productType,string searchText,string title,
            string orderBy,int pageSize,int pageIndex)
        {
            var category = await context.Category.Where(p => p.name.Equals(productType)).Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.name
            }).FirstOrDefaultAsync();

            var categoryFilter= await context.Category.Where(p => p.name.Equals(title)).Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.name
            }).FirstOrDefaultAsync();

            var query = context.Product.AsQueryable();

            if (categoryFilter!=null)
            {
                query = query.Where(p => p.Categories.Any(c => c.Id == categoryFilter.Id) && p.Categories.Any(c => c.Id == category.Id));
            }
            else
            {
                query = query.Where(p => p.Categories.Any(c => c.Id == category.Id));
            }


            if (searchText.Length!=0)
            {
                query = query.Where(p => p.Name.Contains(searchText));
            }

            if (orderBy.Length != 0)
            {
                switch (orderBy)
                {
                    case "A-Z":
                        query = query.OrderBy(p => p.Name);
                        break;

                    case "Z-A":
                 
                        query = query.OrderByDescending(p => p.Name);
                        break;
                    case "Nuevos":
                        query = query.OrderByDescending(p => p.insertDate);
                        break;

                    case "Viejos":
                        query = query.OrderBy(p => p.insertDate);
                        break;
                }
            }

            List<ProductModel> products = await query.Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image != null ? p.Image.Select(i => new ImageModel
                {
                    Id = i.Id,
                    Mime = i.Mime,
                    Name = i.Name,
                    Path = i.Path
                }).FirstOrDefault() : null,
                Availability = p.Availability != null ? new AvailabilityModel
                {
                    Description = p.Availability.Description,
                    Id = p.Availability.Id
                } : null,
                Price = p.Price,
                Path = p.Name.Replace(' ', '-'),
                Description = p.Description,
                Stock = p.Stock
            }).Skip(pageSize*pageIndex).Take(pageSize).ToListAsync();
            
            AdvancedSearchModel advancedSearchModel = new AdvancedSearchModel { products = products, count = query.Count() };

            return advancedSearchModel;
        }
        public async Task<List<ProductModel>> GetProductsByCategory(int category)
        {
            //TODO: Ver como mapear las categorias, ver como hacer que no truene y hacer el query bien

            var data = await context.Product.Where(p => p.Categories.Any(c => c.Id == category)).Select(p=>new ProductModel()
            {
                Id=p.Id,
                Name=p.Name
            }).ToListAsync() ;

        
            //List<ProductModel> data = new List<ProductModel>();

            //foreach(var product in products)
            //{
            //    data.Add(new ProductModel()
            //    {
            //        Id=product.Id,
            //        Name=product.Name
            //    });
            //}
            //var data = await context.Product.Select(p => new ProductModel()
            //{
            //    Id = p.Id,
            //    Name = p.Name
            //}).Where(p=>p.Categories.Any(c=>c.Id==category)).ToListAsync();

            return data;
        }
        
    }
}
