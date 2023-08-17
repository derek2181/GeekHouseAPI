using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Data
{
    public class Image
    {
        public int Id { get; set; }
        
        public string Path { get; set; }

        public string Name { get; set; }

        public string Mime { get; set; }
        public Product product { get; set; }

        public string objectName { get; set; }

    }
}
