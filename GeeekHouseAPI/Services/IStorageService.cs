using GeeekHouseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Services
{
    public interface IStorageService
    {
        Task<S3ResponseDTO> UploadFileAsync(S3Object s3Object,AWSCredentials credentials);
    }
}
