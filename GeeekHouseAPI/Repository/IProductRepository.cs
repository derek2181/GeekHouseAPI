using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public interface IProductRepository
    {
      Task<List<ProductModel>> getAllProducts();
    }
}