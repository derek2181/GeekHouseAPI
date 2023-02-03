using GeeekHouseAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IFormFile ImageFile { get; set; }
       
        public ImageModel Image { get; set; }
        public ICollection<CategoryModel> Categories { get; set; }
    }
}
