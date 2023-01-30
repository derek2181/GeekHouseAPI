using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetRecentProducts();
        Task<ProductModel> GetProductById(int id);
        Task<int> AddBook(ProductModel productModel, List<int> categories);

        Task<List<ProductModel>> GetProductsByCategory(int category);

    }
}