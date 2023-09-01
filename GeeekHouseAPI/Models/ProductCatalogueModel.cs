using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Models
{
    public class ProductCatalogueModel
    {
        public ICollection<CategoryModel> categories { get; set; }

        public ICollection<CategoryModel> subcategories { get; set; }

        public ICollection<AvailabilityModel> availabilities { get; set; }
    }
}
