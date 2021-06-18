using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IFileUploadRepo
    {
        Task<FileUploadResponseModel> uploadFilesAsync(FileUploadRequestModel obj);
        Task<GenericResponseModel> getAllSupportedFileExtensionsAsync();
        Task<GenericResponseModel> getAllAppTypesAsync();
        Task<GenericResponseModel> getAppTypesByIdAsync(long appId);
        Task<GenericResponseModel> getAllFolderTypesByAppIdAsync(long appId);
        Task<GenericResponseModel> getFolderTypeByIdAsync(long folderId);

        //Reusables
        string getFileTypeAsync(string fileExtension);
        IList<string> allFileExtensionsAsync();
    }
}
