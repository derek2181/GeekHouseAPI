using GeeekHouseAPI.Data;
using GeeekHouseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly GeekHouseContext context;


        public CategoryRepository(GeekHouseContext context)
        {
            this.context = context;
        }
        public async Task<List<CategoryModel>> getAllCategories()
        {
            var data = await context.Category.Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

            return data;
        }

        public async Task<List<CategoryModel>> getAllSubcategories(string category)
        {
            var data = await context.Subcategory.Where(c=>c.Category.Name.Equals(category)).Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

            return data;
        }

    }
}
