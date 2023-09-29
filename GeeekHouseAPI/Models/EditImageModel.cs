using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace GeeekHouseAPI.Models
{
    public class EditImageModel
    {
        public int id { get; set; }
        public string src { get; set; }
        public IFormFile? file { get; set; }
    }
}
