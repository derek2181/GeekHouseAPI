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

            var data = await context.Product.Where(p => p.Category.Id==category && p.Id!= productId)
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
        public async Task<List<ProductModel>> GetRecentFunkoPops()
        {
            var data = await context.Product.Where(p=>p.Availability.Id==1 && p.Category.Id==1).Select(p => new ProductModel()
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
                Stock=p.Stock

            }).Take(8).ToListAsync();
            return data;

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

        public async Task<int> AddProduct(ProductModel productModel,int categoryId,int[] subcategoriesList,int availabilityType)
        {
            var category = await context.Category.Where(c => c.Id==categoryId).FirstOrDefaultAsync();
            var subcategories = await context.Subcategory.Where(c => subcategoriesList.Contains(c.Id)).ToListAsync();
            var availability = await context.Availability.Where(a => a.Id == availabilityType).FirstOrDefaultAsync();
            var product = new Product()
            {
                Name = productModel.Name,
                Category = category,
                Subcategories=subcategories,
                Image =productModel.Images != null ? productModel.Images.ToList().Select(i => new Image
                {
                Id=i.Id,
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
                return -1;
            }


            
            return product.Id;

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

            if (categoryFilter!=null)
            {
                query = query.Where(p => p.Category.Id == categoryType.Id && p.Subcategories.Any(c => c.Id== categoryFilter.Id));
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
        public async Task<List<ProductModel>> GetProductsByCategory(int category)
        {
            //TODO: Ver como mapear las categorias, ver como hacer que no truene y hacer el query bien

            var data = await context.Product.Where(p => p.Subcategories.Any(c => c.Id == category)).Select(p=>new ProductModel()
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
                }).FirstOrDefault() : null
            }).ToListAsync();

            return products;
        }
    }
}
