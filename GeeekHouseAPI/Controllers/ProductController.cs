using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeeekHouseAPI.Repository;
using Microsoft.AspNetCore.Mvc;
namespace GeeekHouseAPI.Controllers
{   [ApiController]
    [Route("products")]
      public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
       public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.getAllProducts();
            return Ok(products);
        }
    }
}
