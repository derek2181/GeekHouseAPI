using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Data
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public ICollection<Image> Image { get; set; }
        public Availability Availability { get; set; }
        public Category Category { get; set; }

        public ICollection<Subcategory>  Subcategories { get; set;}
        public DateTime insertDate { get; set; } = DateTime.Now;
    }

}
