using GeeekHouseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public interface IProductRepository
    {
        Task<LandingModel> GetRecentFunkoPops();
        Task<ProductModel> GetProductByName(string id);
        Task<GenericResponse> AddProduct(ProductModel productModel,int cateogory, int subcategoryId, int availability);

        Task<GenericResponse> EditProduct(ProductModel productModel, int category, int subcategoryId, int availability);
        Task<List<ProductModel>> GetProductsByCategory(int category);
        Task<List<ProductModel>> GetRelatedProductsByCategory(int category,int productId);
        Task<SearchModel> GetProductsAdvancedSearch(string category,string searchText, string subcategory, string orderBy,int pageSize,int pageIndex);

        Task<GenericResponse> DisableProduct(int productId);

        Task<List<ProductTableModel>> GetAllProductsAdmin();

        Task<ProductCatalogueModel> GetProductCatalogue();

    }
}