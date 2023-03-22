using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetRecentProducts();
        Task<ProductModel> GetProductByName(string id);
        Task<int> AddProduct(ProductModel productModel, List<int> categories,int availability);

        Task<List<ProductModel>> GetProductsByCategory(int category);
        Task<List<ProductModel>> GetRelatedProductsByCategory(int[] category);
    }
}