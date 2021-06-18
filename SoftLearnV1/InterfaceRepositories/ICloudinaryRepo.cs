using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICloudinaryRepo
    {
        //----------------- UPLOADS ------------------------------------
        Task<ImageUploadResult> ImagesUpload(IFormFile file);
        Task<ImageUploadResult> CategoryImagesUpload(IFormFile file);
        Task<RawUploadResult> DocumentUpload(IFormFile file);
        Task<VideoUploadResult> VideosUpload(IFormFile file);

        //---------------- DELETEION -----------------------------------
        Task<DeletionResult> VideosDelete(string fileName);
        Task<DeletionResult> DocumentDelete(string fileName);
    }
}
