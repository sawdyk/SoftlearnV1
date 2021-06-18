﻿using ByteSizeLib;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace SoftLearnV1.Repositories
{
    public class FileUploadRepo : IFileUploadRepo
    {
        private readonly AppDbContext _context;
        private readonly ServerPath _serverPath;
        private readonly IHostingEnvironment _env;

        public FileUploadRepo(AppDbContext context, ServerPath serverPath, IHostingEnvironment env)
        {
            _context = context;
            _serverPath = serverPath;
            _env = env;
        }

        //public Double Duration(String file)
        //{
        //    var wmp = new WindowsMediaPlayer();
        //    IWMPMedia mediainfo = wmp.newMedia(file);
        //    return mediainfo.duration;
        //}


        public IList<string> allFileExtensionsAsync()
        {
            string[] fileExt = new[]{ ".xls", ".xlsx", ".doc", ".docx", ".mp4", ".mp3", ".mpeg", ".avi", ".jpeg", ".jpg", ".png", ".gif", ".txt", ".pdf" };

            return fileExt;
        }

        public string getFileTypeAsync(string fileExtension)
        {
            try
            {
                string fileType = string.Empty;

                if (fileExtension.Equals(".xlsx") || fileExtension.Equals(".xls"))
                {
                    fileType = "Excel Document";
                }
                else if (fileExtension.Equals(".mp4") || fileExtension.Equals(".mp3") || fileExtension.Equals(".mpeg") || fileExtension.Equals(".avi"))
                {
                    fileType = "Video";
                }
                else if (fileExtension.Equals(".doc") || fileExtension.Equals(".docx"))
                {
                    fileType = "Word Document";
                }
                else if (fileExtension.Equals(".jpeg") || fileExtension.Equals(".png") || fileExtension.Equals(".jpg") || fileExtension.Equals(".gif"))
                {
                    fileType = "Image";
                }
                else if (fileExtension.Equals(".txt"))
                {
                    fileType = "Text Document";
                }
                else if (fileExtension.Equals(".pdf"))
                {
                    fileType = "PDF Document";
                }
                else
                {
                    fileType = "";
                }

                return fileType;

            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        public async Task<FileUploadResponseModel> uploadFilesAsync(FileUploadRequestModel obj)
        {
            try
            {
                FileData fileData = new FileData();

                //get the folderType
                var folder = _context.FolderTypes.Where(c => c.Id == Convert.ToInt64(obj.FolderTypeId)).FirstOrDefault();
                //get the appType
                var app = _context.AppTypes.Where(c => c.Id == Convert.ToInt64(obj.AppId)).FirstOrDefault();

                //get the appType
                var checkAppFolder = _context.FolderTypes.Where(c => c.Id == Convert.ToInt64(obj.FolderTypeId) && c.AppId == Convert.ToInt64(obj.AppId)).FirstOrDefault();

                //the list of file extensions
                IList<string> fileExtensions = allFileExtensionsAsync();

                if (app == null)
                {
                    return new FileUploadResponseModel { StatusCode = 409, StatusMessage = "No App With the Specified ID"};
                }
                if (folder == null)
                {
                    return new FileUploadResponseModel { StatusCode = 409, StatusMessage = "No Folder With the Specified ID"};
                }
                if (checkAppFolder == null)
                {
                    return new FileUploadResponseModel { StatusCode = 409, StatusMessage = "Folder does not exists for the App and Folder with the Specified ID"};
                }
                if (obj.File == null || obj.File.Length == 0)
                {
                    return new FileUploadResponseModel { StatusCode = 409, StatusMessage = "Select a file to Upload"};
                }
                if (!fileExtensions.Contains(Path.GetExtension(obj.File.FileName.ToLower())))
                {
                    return new FileUploadResponseModel { StatusCode = 409, StatusMessage = "UnSupported Media/File Type"};
                }
                else
                {
                    //get the subfoldername
                    string subFolderName = folder.FolderName;

                    //the sub folder to save all application files based on the appId i.e. SchoolApp or CourseApp 
                    //(this should be modified only if the folder name on the server was changed/edited) 
                    string folderName = string.Empty;

                    if (app.Id == (int)EnumUtility.AppName.CourseApp)
                    {
                        folderName = "CourseDocuments";
                    }
                    else if (app.Id == (int)EnumUtility.AppName.SchoolApp)
                    {
                        folderName = "SchoolDocuments";
                    }
                    else
                    {
                        folderName = "";
                    }
                   
                    //get the defined filepath (e.g. @"C:\inetpub\wwwroot\SoftlearnMedia")
                    string serverPath = _serverPath.ServerFolderPath(Convert.ToInt64(obj.AppId), subFolderName);

                    //the main folder to save all application files and the BaseURL
                    string serverMainFolderName = ServerPath.ServerMainFolderName();
                    string serverBaseURL = _serverPath.ServerBaseURL();

                    //get the orignal filename
                    string originalFileName = obj.File.FileName.ToLower();

                    //get a unique file name for the file uploaded
                    string randomUniqueFileName = RandomNumberGenerator.UniqueFileName();

                    //gets the extension of the file Uploaded
                    string fileExtension = Path.GetExtension(obj.File.FileName.ToLower());

                    //concantenate the randomString and the file extension of the file uploaded
                    string uniqueFileName = randomUniqueFileName + fileExtension;

                    //the file path to save the file
                    var FilePath = Path.Combine(serverPath, uniqueFileName);

                    //get the file type(e.g. PDF, Word, Excel etc)
                    string fileType = getFileTypeAsync(fileExtension);

                    //get the file size
                    long fileLenght = obj.File.Length;
                    string fileSize = string.Empty;

                    //converts the file size from bytes to mb
                    var maxFileSize = ByteSize.FromBytes(fileLenght);
                    fileSize = maxFileSize.KiloBytes.ToString();
                    //maxFileSize.GigaBytes;
                   
                    //if the file Type is an image, Reduce the file to a new dimension and write to the path
                    if (fileType == "Image")
                    {
                        var image = Image.Load(obj.File.OpenReadStream());
                        //240: height
                        //240: width
                        image.Mutate(x => x.Resize(240, 240));
                        image.Save(FilePath);
                    }
                    else
                    {
                        //copy the file to the stream and read from the file
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            await obj.File.CopyToAsync(stream);
                        }
                    }

                    //the file URL generated  after concantenation (the server IP Address shuould be prepended to get the full path of the file on the server)
                    string fileUrl = string.Empty;

                    if (_env.IsDevelopment()) //for development environment (IIS)
                    {
                        //e.g.http://173.212.213.205/SoftlearnFilesRepository/CourseDocuments/CourseImages/S7xF5EmzZNnXC@iE60g94tIeWFz.jpg
                        fileUrl = serverBaseURL + "/" + serverMainFolderName + "/" + folderName + "/" + subFolderName + "/" + uniqueFileName;
                    }
                    if (_env.IsProduction()) //for production environment(Docker Containerization)
                    {
                        //e.g.https://expertplat.com:9090/CourseDocuments/CourseImages/S7xF5EmzZNnXC@iE60g94tIeWFz.jpg
                        fileUrl = serverBaseURL + "/" + folderName + "/" + subFolderName + "/" + uniqueFileName;
                    }

                    //the result of file data uploaded
                    fileData.OriginalFileName = originalFileName;
                    fileData.FileType = fileType;
                    fileData.FileExtension = fileExtension;
                    fileData.FileUrl = fileUrl;
                    fileData.FileSize = fileSize;
                    fileData.UniqueFileName = uniqueFileName;

                    return new FileUploadResponseModel { StatusCode = 200, StatusMessage = "Successful", FileData = fileData };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new FileUploadResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllAppTypesAsync()
        {
            try
            {
                //get the apptypes
                var result = from ap in _context.AppTypes select ap;

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAppTypesByIdAsync(long appId)
        {
            try
            {
                //get the apptypes
                var result = from ap in _context.AppTypes where ap.Id == appId select ap;

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericResponseModel> getAllSupportedFileExtensionsAsync()
        {
            try
            {
                //the list of file extensions
                IList<string> result = allFileExtensionsAsync();

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllFolderTypesByAppIdAsync(long appId)
        {
            try
            {
                //get the folderTypes
                var result = from fd in _context.FolderTypes where fd.AppId == appId
                            select new
                            {
                                fd.Id,
                                fd.AppId,
                                fd.AppTypes.AppName,
                                fd.FolderName,
                            };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getFolderTypeByIdAsync(long folderId)
        {
            try
            {
                //get the folderTypes
                var result = from fd in _context.FolderTypes
                             where fd.Id == folderId
                             select new
                             {
                                 fd.Id,
                                 fd.AppId,
                                 fd.AppTypes.AppName,
                                 fd.FolderName,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

    }
}
