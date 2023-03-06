using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public string Mime { get; set; }

        public ProductModel productModel { get; set; }
    }
}
