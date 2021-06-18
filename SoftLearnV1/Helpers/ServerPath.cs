using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Helpers
{
    public class ServerPath
    {
        private readonly IHostingEnvironment _env;

        public ServerPath(IHostingEnvironment env)
        {
            _env = env;
        }

        public string ServerFolderPath(long appId, string folderName)
        {
            string path = string.Empty;

            //the root path of the server to upload files
            string serverRootPath = @"C:\inetpub\wwwroot\";

            if (appId == (int)EnumUtility.AppName.CourseApp)
            {
                if (_env.IsDevelopment()) //for development environment (IIS)
                {
                    path = serverRootPath + @"SoftlearnFilesRepository\CourseDocuments\" + folderName;
                }
                else if (_env.IsProduction()) //for production environment(Docker Containerization)
                {
                    path = "wwwroot/CourseDocuments/" + folderName;
                }
                else //local development test (Create this folder or determine folder path)
                {
                    path = @"C:\ASPNETApplications\uploads\CourseDocuments\" + folderName;
                }
                //path = @"C:\ASPNETApplications\uploads\CourseDocuments\" + folderName;
            }
            if (appId == (int)EnumUtility.AppName.SchoolApp)
            {
                if (_env.IsDevelopment()) //for development environment (IIS)
                {
                    path = serverRootPath + @"SoftlearnFilesRepository\SchoolDocuments\" + folderName;
                }
                else if (_env.IsProduction()) //for production environment(Docker Containerization)
                {
                    path = "wwwroot/SchoolDocuments/" + folderName;
                }
                else //local development test (Create this folder or determine folder path)
                {
                    path = @"C:\ASPNETApplications\uploads\SchoolDocuments\" + folderName;
                }
            }
            
            //string path = @"C:\inetpub\wwwroot\SoftlearnMedia\" + folderName;

            return path;
        }

        public static string ServerMainFolderName()
        {
            //the main folder to save all application files 
            //(this should be modified only if the folder name on the server was changed/edited)
            const string mainFolderName = "SoftlearnFilesRepository";

            return mainFolderName;
        }

        public string ServerBaseURL()
        {
            string baseUrl = string.Empty;

            if (_env.IsDevelopment()) //for development environment (IIS)
            {
               baseUrl = "http://173.212.213.205";
            }
            else if (_env.IsProduction()) //for production environment(Docker Containerization)
            {
               baseUrl = "https://expertplat.com:9090";
            }

            //the main folder to save all application files 
            //(this should be modified only if the server or port was changed/edited) 
            
            return baseUrl;
        }
    }
}
