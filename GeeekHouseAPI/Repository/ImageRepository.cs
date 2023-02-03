using GeeekHouseAPI.Data;
using GeeekHouseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly GeekHouseContext context;

        public ImageRepository(GeekHouseContext context)
        {
            this.context = context;
        }

        public async Task<ImageModel> GetImageById(int id)
        {

            var image = await context.Image.Where(i => i.Id == id).Select(i => new ImageModel()
            {
                Id = i.Id,
                Name = i.Name,
                Mime = i.Mime,
                Path = i.Path

            }).FirstOrDefaultAsync();

            


            return image;
        }
    }
}
