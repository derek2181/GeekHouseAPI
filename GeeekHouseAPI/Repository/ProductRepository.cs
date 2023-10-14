using GeeekHouseAPI.Data;
using GeeekHouseAPI.Models;
using GeeekHouseAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GeeekHouseAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly GeekHouseContext context;
        private readonly IStorageService _storageService;

        public ProductRepository(GeekHouseContext context, IStorageService storageService)
        {
            this.context = context;
            this._storageService = storageService;
        }
        public async Task<List<ProductModel>> GetRelatedProductsByCategory(int category,int productId)
        {

            var data = await context.Product.Where(p => p.Category.Id==category && p.Id!= productId && p.isActive)
              .Select(p => new ProductModel
              {

                  Id = p.Id,
                  Name = p.Name,
                  Image = p.Image != null ? p.Image.Select(i => new ImageModel
                  {
                      Id = i.Id,
                      Mime = i.Mime,
                      Name = i.Name,
                      Path = i.Path
                  }).FirstOrDefault() : null,
                  Availability = p.Availability != null ? new AvailabilityModel
                  {
                      Description = p.Availability.Description,
                      Id = p.Availability.Id
                  } : null,
                  Price = p.Price,
                  Path = p.Name.Replace(' ', '-'),
                  Description = p.Description,
                  Stock = p.Stock
              }
              ).Take(8).ToListAsync();


            return data;
        }
        public async Task<LandingModel> GetRecentFunkoPops()
        {
            var response = new LandingModel();
            var newFunkoStock = await context.Product.Where(p=>p.Availability.Id==1 && p.Category.Id==1 && p.isActive).Select(p => new ProductModel()
            {
                Id= p.Id,
                Name=p.Name,
                Image=p.Image != null ?  p.Image.Select(i=> new ImageModel
                {
                    Id=i.Id,
                    Mime=i.Mime,
                    Name=i.Name,
                    Path=i.Path
                }).FirstOrDefault() : null,
                Availability = p.Availability != null ? new AvailabilityModel
                {
                    Description = p.Availability.Description,
                    Id = p.Availability.Id
                } : null,
                Price=p.Price,
                Path= p.Name.Replace(' ','-'),
                Description=p.Description,
                Stock=p.Stock,
                Category=p.Category !=null ? new CategoryModel
                {
                    Id = p.Category.Id,
                    Name=p.Category.Name,
                }: null,
                Subcategory = p.Category != null ? new CategoryModel
                {
                    Id = p.Subcategory.Id,
                    Name = p.Subcategory.Name,
                } : null

            }).Take(8).ToListAsync();

            var newFunkoPreorder = await context.Product.Where(p => p.Availability.Id ==2  && p.Category.Id == 1 && p.isActive).Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image != null ? p.Image.Select(i => new ImageModel
                {
                    Id = i.Id,
                    Mime = i.Mime,
                    Name = i.Name,
                    Path = i.Path
                }).FirstOrDefault() : null,
                Availability = p.Availability != null ? new AvailabilityModel
                {
                    Description = p.Availability.Description,
                    Id = p.Availability.Id
                } : null,
                Price = p.Price,
                Path = p.Name.Replace(' ', '-'),
                Description = p.Description,
                Stock = p.Stock,
                Category = p.Category != null ? new CategoryModel
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                } : null,
                Subcategory = p.Category != null ? new CategoryModel
                {
                    Id = p.Subcategory.Id,
                    Name = p.Subcategory.Name,
                } : null

            }).Take(8).ToListAsync();


            response.newPreorders = newFunkoPreorder;
            response.newStock = newFunkoStock;
            return response;

        }

        public async Task<ProductModel> GetProductByName(string name)
        {
            var product = await context.Product.Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
      
                Availability = p.Availability != null ? new AvailabilityModel
                {
                    Description = p.Availability.Description,
                    Id = p.Availability.Id
                }:null,
                Category = new CategoryModel { Id = p.Category.Id, Name = p.Category.Name },
                Price =p.Price,
                Description = p.Description,
                Stock = p.Stock,
                Subcategory=new CategoryModel {Id=p.Subcategory.Id,Name=p.Subcategory.Name }
            }).Where(p => p.Name == name).SingleOrDefaultAsync();

            var images = await context.Image.Where(i => i.product.Id == product.Id).Select(i => new ImageModel
            {
                Id=i.Id,
                Mime=i.Mime,
                Name=i.Name,
                Path=i.Path
            }).ToListAsync();

          
            product.Images = images;
            return product;
        }
        
        public async Task<GenericResponse<string>> AddProduct(ProductModel productModel,int categoryId,int subcategoryId, int availabilityType)
        {
            var response = new GenericResponse<string>();
            var category = await context.Category.Where(c => c.Id==categoryId).FirstOrDefaultAsync();
            var subcategory = await context.Subcategory.Where(c => c.Id== subcategoryId).FirstOrDefaultAsync();
            var availability = await context.Availability.Where(a => a.Id == availabilityType).FirstOrDefaultAsync();
            var product = new Product()
            {
                Name = productModel.Name,
                Category = category,
                Subcategory= subcategory,
                Image =productModel.Images != null ? productModel.Images.ToList().Select(i => new Image
                {
                Mime=i.Mime,
                Name=i.Name,
                Path=i.Path,
                objectName=i.objectName
                }).ToList() : null
                ,
                Description=productModel.Description,
                Availability = availability,
                Price=productModel.Price,
                Stock=productModel.Stock,
                insertDate=DateTime.Now,
            };
            try
            {
                context.Product.Add(product);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.code = 400;
                response.message = "Hubo un problema al agregar el producto";
                return response;
            }

            response.code = 200;
            response.message = "Se ha añadido el producto con éxito";


            return response;

        }
        public async Task<SearchModel> GetProductsAdvancedSearch(string category,string searchText,string subcategory,
            string orderBy,int pageSize,int pageIndex)
        {
            var categoryType = await context.Category.Where(p => p.Name.Equals(category)).Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.Name
            }).FirstOrDefaultAsync();

            var categoryFilter= await context.Subcategory.Where(p => p.Name.Equals(subcategory)).Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.Name
            }).FirstOrDefaultAsync();

            var query = context.Product.AsQueryable();
            query = query.Where(p => p.isActive);
            if (categoryFilter!=null)
            {
                query = query.Where(p => p.Category.Id == categoryType.Id && p.Subcategory.Id == categoryFilter.Id);
            }
            else if(categoryType!=null)
            {
                query = query.Where(p => p.Category.Id == categoryType.Id);
            }


            if (searchText.Length!=0)
            {
                query = query.Where(p => p.Name.Contains(searchText));
            }

            if (orderBy.Length != 0)
            {
                switch (orderBy)
                {
                    case "A-Z":
                        query = query.OrderBy(p => p.Name);
                        break;

                    case "Z-A":
                 
                        query = query.OrderByDescending(p => p.Name);
                        break;
                    case "Nuevos":
                        query = query.OrderByDescending(p => p.insertDate);
                        break;

                    case "Viejos":
                        query = query.OrderBy(p => p.insertDate);
                        break;

                    case "Disponible":
                        query = query.OrderByDescending(p => p.Availability.Id==1);
                        break;

                    case "Preorden":
                        query = query.OrderByDescending(p => p.Availability.Id == 2);
                        break;
                }
            }

            List<ProductModel> products = await query.Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image != null ? p.Image.Select(i => new ImageModel
                {
                    Id = i.Id,
                    Mime = i.Mime,
                    Name = i.Name,
                    Path = i.Path
                }).FirstOrDefault() : null,
                Availability = p.Availability != null ? new AvailabilityModel
                {
                    Description = p.Availability.Description,
                    Id = p.Availability.Id
                } : null,
                Price = p.Price,
                Path = p.Name.Replace(' ', '-'),
                Description = p.Description,
                Stock = p.Stock
            }).Skip(pageSize*pageIndex).Take(pageSize).ToListAsync();

            SearchModel advancedSearchModel = new SearchModel { products = products, count = query.Count() };

            return advancedSearchModel;
        }
        public async Task<ProductCatalogueModel> GetProductCatalogue()
        {
            var productCatalogueModel = new ProductCatalogueModel();

            var categories =await context.Category.Select(c => new CategoryModel()
            {
                Id=c.Id,
                Name=c.Name
            }).ToListAsync();

            var subcategories = await context.Subcategory.Select(c => new CategoryModel()
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

            var availabilities = await context.Availability.Select(c => new AvailabilityModel()
            {
             Description=c.Description,
             Id=c.Id
            }).ToListAsync();

            productCatalogueModel.categories = categories;
            productCatalogueModel.availabilities = availabilities;
            productCatalogueModel.subcategories = subcategories;

            return productCatalogueModel;
        }
        public async Task<List<ProductModel>> GetProductsByCategory(int category)
        {
            //TODO: Ver como mapear las categorias, ver como hacer que no truene y hacer el query bien

            var data = await context.Product.Where(p => p.Subcategory.Id==category).Select(p=>new ProductModel()
            {
                Id=p.Id,
                Name=p.Name
            }).ToListAsync() ;

        
            //List<ProductModel> data = new List<ProductModel>();

            //foreach(var product in products)
            //{
            //    data.Add(new ProductModel()
            //    {
            //        Id=product.Id,
            //        Name=product.Name
            //    });
            //}
            //var data = await context.Product.Select(p => new ProductModel()
            //{
            //    Id = p.Id,
            //    Name = p.Name
            //}).Where(p=>p.Categories.Any(c=>c.Id==category)).ToListAsync();

            return data;
        }

        public async Task<List<ProductTableModel>> GetAllProductsAdmin()
        {
            List<ProductTableModel> products = await context.Product.Select(p => new ProductTableModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock,
                Category=p.Category!=null ? p.Category.Name : "SIN CATEGORIA",
                Availability=p.Availability!=null ? p.Availability.Description : "SIN VALOR",
                Image = p.Image != null ? p.Image.Select(i => new ImageModel
                {
                    Id = i.Id,
                    Mime = i.Mime,
                    Name = i.Name,
                    Path = i.Path
                }).FirstOrDefault() : null,
                isActive=p.isActive
            }).ToListAsync();

            return products;
        }

        public async Task<GenericResponse<string>> EditProduct(ProductModel productModel, int category, int subcategoryId, int availability)
        {
            var productEntity=await context.Product.Where(product=>product.Id==productModel.Id).FirstOrDefaultAsync();
            var response = new GenericResponse<string>();
            if (productEntity!=null)
            {
                var productName = await context.Product.Where(p => p.Name.Equals(productModel.Name) && !p.Id.Equals(productModel.Id)).FirstOrDefaultAsync();

                if (productName != null)
                {
                    response.code = 400;
                    response.message = "No se puede crear un producto con nombre ya existente: " + productModel.Name;
                
                    return response;//"El producto con el nombre" + productModel.Name + " ya existe.";
                }
                productEntity.Name = productModel.Name;
                productEntity.Price = productModel.Price;
                productEntity.Description = productModel.Description;
                productEntity.Stock= productModel.Stock;  
                productEntity.Availability= await context.Availability.Where(av=>av.Id==availability).FirstOrDefaultAsync(); 
                productEntity.Subcategory=await context.Subcategory.Where(s=>s.Id.Equals(subcategoryId)).FirstOrDefaultAsync();
                productEntity.Category=await context.Category.Where(c=>c.Id.Equals(category)).FirstOrDefaultAsync();

                if(productModel.Images.Count>0)
                {
                    foreach(var imageModel in productModel.Images)
                    {
                        Image imageEntity = null;
                        if (imageModel.Id != 0)
                        {
                            imageEntity=await context.Image.Where(image=>image.Id==imageModel.Id).FirstOrDefaultAsync();

                            imageEntity.objectName= imageModel.objectName;
                            imageEntity.Name= imageModel.Name;
                            imageEntity.Path= imageModel.Path;
                            imageEntity.Mime= imageModel.Mime;

                        }
                        else
                        {
                            imageEntity = new Image
                            {
                                Mime = imageModel.Mime,
                                Name = imageModel.Name,
                                Path = imageModel.Path,
                                objectName = imageModel.objectName,
                                product=productEntity
                            };
                            context.Image.Add(imageEntity);
                        }
                       
                    }
                }
                await context.SaveChangesAsync();
                response.code = 200;
                response.message = "Se ha actualizado el producto con exito";
                return response;
            }
            response.code = 404;
            response.message = "No se encontró el producto con nombre: " + productModel.Name;
            return response;
        }

        public async Task<GenericResponse<string>> DisableProduct(int productId)
        {
            var response = new GenericResponse<string>();

            var product = await context.Product.Where(p => p.Id.Equals(productId)).FirstOrDefaultAsync();
            if(product != null)
            {
                product.isActive = !product.isActive;
                await context.SaveChangesAsync();
                response.code = 200;
                response.message = "Se ha modificado el producto";
                return response;
            }
            response.code = 404;
            response.message = "No se pudo encontrar el producto";
            return response;
        }
    }
}
