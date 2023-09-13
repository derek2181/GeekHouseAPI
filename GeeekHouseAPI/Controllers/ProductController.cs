using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using GeeekHouseAPI.Models;
using GeeekHouseAPI.Repository;
using GeeekHouseAPI.Services;
using GeeekHouseAPI.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeeekHouseAPI.Controllers
{   [ApiController]
    [Route("products")]
      public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment,
            IStorageService storageService, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
            _storageService=storageService;
            this._configuration = configuration;
        }
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllProductsAdmin()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAdmin();
                if (products == null)
                {
                    return BadRequest("There was en error with the repository");
                }
                return Ok(products);
            }
            catch(Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet("catalogue")]
        public async Task<IActionResult> GetProductCatalogue()
        {
            try
            {
                var products = await _productRepository.GetProductCatalogue();
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

        [HttpGet("recent")]
       public async Task<IActionResult> RecentProducts()
        {
            try
            {
                var products = await _productRepository.GetRecentFunkoPops();
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddNewProduct([FromForm] ProductModel product, [FromForm] int category, [FromForm]  int subcategory,[FromForm] int availability)
        {
            try
            {
                var result = new S3ResponseDTO();
                if(product.ImageFiles != null)
                {
                    product.Images = new Collection<ImageModel>();
                    foreach(IFormFile file in product.ImageFiles)
                    {
                      // ImageModel image = await ImageUploader.Upload("products", file);

                      
                        await using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);

                        var fileExt = Path.GetExtension(file.FileName);
                        var objName = $"{Guid.NewGuid()}{fileExt}";

                        var s3Obj = new S3Object()
                        {
                            BucketName = "geekhouse-bucket",
                            InputStream = memoryStream,
                            Name = objName

                        };

                        var cred = new AWSCredentials()
                        {
                            AwsKey = _configuration["AWSConfig:AWSAccessKey"],
                            AwsSecretKey = _configuration["AWSConfig:AWSSecretKey"]
                        };

                       result = await _storageService.UploadFileAsync(s3Obj, cred);

                        var image = new ImageModel()
                        {
                            Name = file.FileName,
                            Mime = file.ContentType,
                            Path = _configuration["AWSConfig:S3BucketURL"] + objName,
                            objectName= objName
                        };
                        product.Images.Add(image);

                    }
                }


                var id= await _productRepository.AddProduct(product,category, subcategory, availability);
                return Created("images/" + result, "SUCCESS");

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
