﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Data
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; }
    }
}
