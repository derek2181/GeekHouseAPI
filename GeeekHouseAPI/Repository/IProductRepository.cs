using GeeekHouseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetRecentFunkoPops();
        Task<ProductModel> GetProductByName(string id);
        Task<int> AddProduct(ProductModel productModel,int cateogory, int subcategoryId, int availability);

        Task<List<ProductModel>> GetProductsByCategory(int category);
        Task<List<ProductModel>> GetRelatedProductsByCategory(int category,int productId);
        Task<SearchModel> GetProductsAdvancedSearch(string category,string searchText, string subcategory, string orderBy,int pageSize,int pageIndex);

        Task<List<ProductTableModel>> GetAllProductsAdmin();

        Task<ProductCatalogueModel> GetProductCatalogue();

    }
}