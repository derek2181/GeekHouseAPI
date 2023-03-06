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

        public ICollection<IFormFile?> ImageFiles { get; set; }
        public IFormFile? ImageFile { get; set; }

        public double Price{get;set;}
        public string Description { get; set; }
        public string Path { get; set; }
        public int Stock { get; set; }

        public AvailabilityModel Availability { get; set; }
        public ImageModel Image { get; set; }

        public ICollection<ImageModel> Images { get; set; }
        public ICollection<CategoryModel> Categories { get; set; }
    }
}
