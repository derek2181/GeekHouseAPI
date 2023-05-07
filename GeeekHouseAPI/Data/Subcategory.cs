using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Data
{
    public class Subcategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
