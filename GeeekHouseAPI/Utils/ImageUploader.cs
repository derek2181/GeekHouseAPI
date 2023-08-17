using GeeekHouseAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Utils
{
    public static class ImageUploader
    {
        /// <summary>
        /// This method uploads a file image into the specificied folder on server path 
        /// "uploads/<paramref name="entityFolderName">entityFolderName</paramref>/images"
        /// if the folder doesn't exists it  will create a new one
        /// </summary>
        /// <param name="entityFolderName">the folder's name that is going to be located on uploads folder on server</param>
        /// <param name="file">the image file to be uploaded</param>
        /// <returns>The image model filled with all file specs. if it fails,this method returns null</returns>
        /// 


        public static async Task<ImageModel> Upload(string entityFolderName, IFormFile file)
        {

            
            string root = Directory.GetCurrentDirectory();
            string folder = $"uploads/{entityFolderName}/images/";
            string storageFolder = Path.Combine(root, folder);

            //If folder doesn't exist, we create a new one
            if (!Directory.Exists(storageFolder))
            {
                Directory.CreateDirectory(storageFolder);
            }

            // Path with file name embebed
            folder += Guid.NewGuid().ToString() + file.FileName;

            try
            {
                FileStream filestream=new FileStream((Path.Combine(root, folder)), FileMode.Create);
               
               await file.CopyToAsync(filestream);

               filestream.Close();

            } catch( Exception e)
            {
                return null;
            }

            var image = new ImageModel()
            {
                Name = file.FileName,
                Mime = file.ContentType,
                Path = folder
            };


            return image;
        }


    }
}
