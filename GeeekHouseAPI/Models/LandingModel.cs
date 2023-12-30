using System.Collections.Generic;

namespace GeeekHouseAPI.Models
{
    public class LandingModel
    {
        public List<ProductModel> newStock{ get; set; }
        public List<ProductModel> newPreorders { get; set; }

        public List<ProductModel> newDiscount {  get; set; }
    }
}
