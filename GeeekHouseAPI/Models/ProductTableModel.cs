using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace GeeekHouseAPI.Models
{
    public class ProductTableModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }

        public string Availability { get; set; }
        public ImageModel Image { get; set; }

        public string Category { get; set; }

    }
}
