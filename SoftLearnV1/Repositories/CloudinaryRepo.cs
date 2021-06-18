using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.Services.Cloudinary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SoftLearnV1.Repositories
{

    public class CloudinaryRepo : ICloudinaryRepo
    {
        private readonly CloudinaryConfig _cloudinaryConfig;
        private IHostingEnvironment _hostingEnvironment;
        private readonly AppDbContext _context;

        public CloudinaryRepo(AppDbContext context, CloudinaryConfig cloudinaryConfig, IHostingEnvironment environment)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _hostingEnvironment = environment;
            _context = context;
        }

        //Course Document Upload
        public async Task<RawUploadResult> DocumentUpload(IFormFile file)
        {
            try
            {
                Account account = new Account(_cloudinaryConfig.Cloud, _cloudinaryConfig.ApiKey, _cloudinaryConfig.ApiSecret);
                Cloudinary cloudinary = new Cloudinary(account);


                //var path = Path.Combine(Directory.GetCurrentDirectory(), "TempFileUpload", file.FileName);
                var path = Path.Combine(_hostingEnvironment.WebRootPath, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //Uploads the Course Images to cloudinary
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(path),
                    PublicId = "Softlearn/course_materials/" + file.FileName + "",
                };
                var uploadResult = cloudinary.Upload(uploadParams);

                //deletes the file from the "TempFileUplaod" directory if the status of upload result is okay
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.IO.File.Delete(path);
                }

                return uploadResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //For Course Images Upload
        public async Task<ImageUploadResult> ImagesUpload(IFormFile file)
        {
            try
            {
                Account account = new Account(_cloudinaryConfig.Cloud, _cloudinaryConfig.ApiKey, _cloudinaryConfig.ApiSecret);
                Cloudinary cloudinary = new Cloudinary(account);

                // var path = Path.Combine(Directory.GetCurrentDirectory(), "TempFileUpload", file.FileName);
                var path = Path.Combine(_hostingEnvironment.WebRootPath, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //Uploads the Course Images to cloudinary
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path),
                    Transformation = new Transformation().Height(500).Width(500).Crop("scale"),
                    PublicId = "Softlearn/course_Images/" + file.FileName + "",
                };
                var uploadResult = cloudinary.Upload(uploadParams);

                //deletes the file from the "TempFileUplaod" directory if the status of upload result is okay
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.IO.File.Delete(path);
                }

                return uploadResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //For Category Images Upload
        public async Task<ImageUploadResult> CategoryImagesUpload(IFormFile file)
        {
            try
            {
                Account account = new Account(_cloudinaryConfig.Cloud, _cloudinaryConfig.ApiKey, _cloudinaryConfig.ApiSecret);
                Cloudinary cloudinary = new Cloudinary(account);

                // var path = Path.Combine(Directory.GetCurrentDirectory(), "TempFileUpload", file.FileName);
                var path = Path.Combine(_hostingEnvironment.WebRootPath, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //Uploads the Course Images to cloudinary
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path),
                    Transformation = new Transformation().Height(500).Width(500).Crop("scale"),
                    PublicId = "Softlearn/course_category_Images/" + file.FileName + "",
                };
                var uploadResult = cloudinary.Upload(uploadParams);

                //deletes the file from the "TempFileUplaod" directory if the status of upload result is okay
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.IO.File.Delete(path);
                }

                return uploadResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Course Videos Upload
        public async Task<VideoUploadResult> VideosUpload(IFormFile file)
        {
            try
            {
                Account account = new Account(_cloudinaryConfig.Cloud, _cloudinaryConfig.ApiKey, _cloudinaryConfig.ApiSecret);
                Cloudinary cloudinary = new Cloudinary(account);

                //var path = Path.Combine(Directory.GetCurrentDirectory(), "TempFileUpload", file.FileName);
                var path = Path.Combine(_hostingEnvironment.WebRootPath, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //Uploads the Course Images to cloudinary
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(path),
                    Transformation = new Transformation().Height(500).Width(500).Crop("scale"),
                    PublicId = "Softlearn/course_videos/" + file.FileName + "",
                };
                var uploadResult = cloudinary.Upload(uploadParams);

                //deletes the file from the "TempFileUplaod" directory if the status of upload result is okay
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.IO.File.Delete(path);
                }

                return uploadResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Video Uploads
        public async Task<DeletionResult> VideosDelete(string fileName)
        {
            try
            {
                Account account = new Account(_cloudinaryConfig.Cloud, _cloudinaryConfig.ApiKey, _cloudinaryConfig.ApiSecret);
                Cloudinary cloudinary = new Cloudinary(account);

                var deletionParams = new DeletionParams("Softlearn/course_videos/" + fileName + "")
                {
                    ResourceType = ResourceType.Video
                };
                var deletionResult =  cloudinary.Destroy(deletionParams);

                return  deletionResult;
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();

                return new DeletionResult { StatusCode = System.Net.HttpStatusCode.InternalServerError, Result = "An Error Occurred"};
            }
        }

        //Delete Video Uploads
        public async Task<DeletionResult> DocumentDelete(string fileName)
        {
            try
            {
                Account account = new Account(_cloudinaryConfig.Cloud, _cloudinaryConfig.ApiKey, _cloudinaryConfig.ApiSecret);
                Cloudinary cloudinary = new Cloudinary(account);

                var deletionParams = new DeletionParams("Softlearn/course_materials/" + fileName + "")
                {
                    ResourceType = ResourceType.Raw
                };
                var deletionResult = cloudinary.Destroy(deletionParams);

                return deletionResult;
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();

                return new DeletionResult { StatusCode = System.Net.HttpStatusCode.InternalServerError, Result = "An Error Occurred" };
            }
        }
    }
}
