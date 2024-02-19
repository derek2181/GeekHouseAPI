using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeeekHouseAPI.Data;
using GeeekHouseAPI.Models;
using GeeekHouseAPI.Repository;
using GeeekHouseAPI.Services;
using GeeekHouseAPI.Utils;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        public ProductController(IProductRepository productRepository, IImageRepository imageRepository, IWebHostEnvironment webHostEnvironment,
            IStorageService storageService, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _webHostEnvironment = webHostEnvironment;
            _storageService=storageService;
            this._configuration = configuration;
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> EditProduct([FromForm] ProductModel product, [FromForm] ICollection<EditImageModel> imageFiles, [FromForm] int category, [FromForm] int subcategory, [FromForm] int availability)
        {
            try
            {
                var result = new S3ResponseDTO();

                var imagesToAdd = imageFiles.Where(image=>image.file!=null).ToList();

                product.Images=new Collection<ImageModel>();

                    foreach (EditImageModel imageModel in imagesToAdd)
                    {
                        // ImageModel image = await ImageUploader.Upload("products", file);

                        await using var memoryStream = new MemoryStream();
                        await imageModel.file.CopyToAsync(memoryStream);
                    
                        var fileExt = Path.GetExtension(imageModel.file.FileName);
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


                        //SEARCH IMAGE
                        var imageFromDB = await _imageRepository.GetImageById(imageModel.id);
                        
                        await _storageService.DeleteFileIfExists(imageFromDB != null ? imageFromDB.Path : "", cred);
                        result = await _storageService.UploadFileAsync(s3Obj, cred);

                        var image = new ImageModel()
                        {
                            Name = imageModel.file.FileName,
                            Mime = imageModel.file.ContentType,
                            Path = _configuration["AWSConfig:S3BucketURL"] + objName,
                            objectName = objName,
                            Id= imageModel.id
                        };
                        product.Images.Add(image);

                    }
                    var response = await _productRepository.EditProduct(product, category, subcategory, availability);

                if(response.code.Equals(200))
                    return Ok(response);
                if (response.code.Equals(404))
                    return NotFound(response);

                return BadRequest(response);

            }
            catch (Exception e)
            {
                return BadRequest(new { message = "ERROR", code = 400 });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewProduct([FromForm] ProductModel product,[FromForm] ICollection<IFormFile?> imageFiles, [FromForm]  int category, [FromForm]  int subcategory,[FromForm] int availability)
        {
            try
            {
                var response = new GenericResponse<string>();
                var result = new S3ResponseDTO();
                if(imageFiles.Count>0)
                {
                    product.Images = new Collection<ImageModel>();
                    foreach(IFormFile file in imageFiles)
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
                    response = await _productRepository.AddProduct(product, category, subcategory, availability);
                    return Ok(response);
                }

                response.code = 400;
                response.message = "Hubo un error al agregar el producto";
                return BadRequest(response);

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, code = 400 });
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

                var product = await _productRepository.GetProductsAdvancedSearch(category, searchText, subcategory, orderBy, pageSize, pageIndex);
                return Ok(product);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("disable")]
        public async Task<IActionResult> DisableProduct([FromForm] int productId)
        {
            try
            {
                return Ok(await _productRepository.DisableProduct(productId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
