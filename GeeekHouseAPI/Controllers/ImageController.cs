using GeeekHouseAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImageController : Controller
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this._imageRepository = imageRepository;

          

        }

        /// <summary> Search for a specific image on database and if it exists,
        ///     it access to file using the path in the server file storage to get a byte array and then
        ///     return it.
        /// 
        /// </summary>
        /// <param ><c>id</c>the image id on database</param>
        /// <returns>Buffer with byte array of the specific image with its content type</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage([FromRoute]int id)
        {
            

            var image = await _imageRepository.GetImageById(id);

            byte[] imageData = null;
            if (image != null)
                imageData = System.IO.File.ReadAllBytes(image.Path);
            else
                return BadRequest("The image doesn't exist");

            return File(imageData, image.Mime);
        }
    }
}
