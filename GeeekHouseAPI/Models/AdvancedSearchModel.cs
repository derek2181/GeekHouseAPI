using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Models
{
    public class AdvancedSearchModel
    {
        public ICollection<ProductModel> products { get; set; }

        public int count { get; set; }
    }
}
