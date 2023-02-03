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
                Name=p.Name

            }).Take(8).ToListAsync();
            return data;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var data = await context.Product.Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name
            }).Where(p => p.Id == id).SingleOrDefaultAsync();

            return data;
        }

        public async Task<int> AddBook(ProductModel productModel, List<int> categoryList)
        {
            var categories = await context.Category.Where(c => categoryList.Contains(c.Id)).ToListAsync();

            var product = new Product()
            {
                Name = productModel.Name,
                Categories= categories,
                Image = productModel.Image != null ? new Image() { Path=productModel.Image.Path, Name = productModel.Image.Name, Mime = productModel.Image.Mime} : null
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
