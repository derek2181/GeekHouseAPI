using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GeeekHouseAPI.Models;
using GeeekHouseAPI.Repository;
using GeeekHouseAPI.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace GeeekHouseAPI.Controllers
{   [ApiController]
    [Route("products")]
      public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("recent")]
       public async Task<IActionResult> RecentProducts()
        {
            try
            {
                var products = await _productRepository.GetRecentProducts();
                if (products==null)
                {
                    return BadRequest("There was en error with the repository");
                }
                return Ok(products);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
           
        }
        [HttpGet("related")]
        public async Task<IActionResult> RelatedProducts([FromQuery] int category,[FromQuery] int productId)
        {
            try
            {
                var products = await _productRepository.GetRelatedProductsByCategory(category, productId);
                if (products == null)
                {
                    return BadRequest("There was en error with the repository");
                }
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddNewProduct([FromForm] ProductModel product, [FromForm] int category,int[] subcategories,[FromForm] int availability)
        {
            try
            {
               
                if(product.ImageFiles != null)
                {
                    product.Images = new Collection<ImageModel>();
                    foreach(IFormFile file in product.ImageFiles)
                    {
                    ImageModel image = await ImageUploader.Upload("products", file);

                    product.Images.Add(image);
                    }
                }


                var id= await _productRepository.AddProduct(product,category, subcategories,availability);
                return Created("products/" + id, "SUCCESS");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> ProductByCategory([FromRoute] int category)
        {
            try
            {
                var data = await _productRepository.GetProductsByCategory(category);

                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> ProductByName([FromRoute]string name)
        {
            try
            {
                var actualName = name.Replace('-',' ');
                var product = await _productRepository.GetProductByName(actualName);
                if (product == null)
                {
                  return  NotFound();
                }

                return Ok(product);
            }
            catch(Exception e)
            {
               return  BadRequest(e.Message);
            }

           
        }
        [HttpGet("advanced-search")]
        public async Task<IActionResult> AdvancedSearch([FromQuery ]string category = "",[FromQuery]string searchText = "", [FromQuery]string subcategory = "",
            [FromQuery] string orderBy = "",[FromQuery] int pageSize=1,[FromQuery] int pageIndex=0)
        {
            try
            {
                
                return Ok(await _productRepository.GetProductsAdvancedSearch(category, searchText, subcategory, orderBy,pageSize,pageIndex));
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
