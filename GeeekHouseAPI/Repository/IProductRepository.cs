using GeeekHouseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetRecentProducts();
        Task<ProductModel> GetProductByName(string id);
        Task<int> AddProduct(ProductModel productModel,int cateogory, int[] categories,int availability);

        Task<List<ProductModel>> GetProductsByCategory(int category);
        Task<List<ProductModel>> GetRelatedProductsByCategory(int category,int productId);
        Task<AdvancedSearchModel> GetProductsAdvancedSearch(string category,string searchText, string subcategory, string orderBy,int pageSize,int pageIndex);

    }
}