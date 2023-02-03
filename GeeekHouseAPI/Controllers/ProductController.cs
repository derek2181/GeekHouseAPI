using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeeekHouseAPI.Models;
using GeeekHouseAPI.Repository;
using GeeekHouseAPI.Utils;
using Microsoft.AspNetCore.Hosting;
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

        [HttpPost]
        public async Task<IActionResult> AddNewProduct([FromForm] ProductModel product, [FromForm] List<int> categories)
        {
            try
            {
               
                if(product.ImageFile != null)
                {

                    ImageModel image = await ImageUploader.Upload("products", product.ImageFile);

                    product.Image = image;

                }


                var id= await _productRepository.AddBook(product, categories);
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
        [HttpGet("{id}")]
        public async Task<IActionResult> ProductById([FromRoute]int id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);
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
    }
}
