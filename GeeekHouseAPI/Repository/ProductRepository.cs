using GeeekHouseAPI.Data;
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

        public async Task<List<ProductModel>> getAllProducts()
        {
            var data = await context.Product.Select(p => new ProductModel()
            {
                Id= p.Id,
                Name=p.Name

            }).ToListAsync();
            return data;
        }
    }
}
