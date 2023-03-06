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
                Description=p.Name,
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
                Description = p.Name,
                Stock = p.Stock
            }).Where(p => p.Name == name).SingleOrDefaultAsync();

            var images = await context.Image.Where(i => i.product.Id == product.Id).Select(i => new ImageModel
            {
                Id=i.Id,
                Mime=i.Mime,
                Name=i.Name,
                Path=i.Path
            }).ToListAsync();


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
                Path=i.Path
                }).ToList() : null
                ,
                Availability = availability,
                Price=productModel.Price
            };
            
            context.Product.Add(product);

            await context.SaveChangesAsync();

            return product.Id;

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
