using GeeekHouseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public interface ICategoryRepository
    {
        Task<List<CategoryModel>> getAllCategories();
    }
}
