using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Models
{
    public class S3ResponseDTO
    {
        public int StatusCode { get; set; } = 200;

        public string Message { get; set; } = "";
    }
}
