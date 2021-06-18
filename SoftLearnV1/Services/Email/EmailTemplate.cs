using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Services.Email
{
    public class EmailTemplate
    {
        private readonly IHostingEnvironment _env;

        public EmailTemplate(IHostingEnvironment env)
        {
            this._env = env;
        }
        public string EmailContent(string name, string codeGenerated)
        {
            // string body;  

            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/email-confirmation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "email-confirmation.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{0}", name);
            body = body.Replace("{1}", codeGenerated);

            return body;
        }
        public string SchoolUserPasswordReset(string name, string link)
        {
            // string body;  

            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/email-confirmation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "newuser-passwordreset.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{0}", name);
            body = body.Replace("{1}", link);

            return body;
        }
        public string ResetPasswordEmailContent(string name, string codeGenerated)
        {
            // string body;  

            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/email-confirmation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "password-reset.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{0}", name);
            body = body.Replace("{1}", codeGenerated);

            return body;
        }
        public string PaymentDisbursementEmailContent(string name, string mailBody)
        {
            // string body;  

            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/email-confirmation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "password-reset.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{0}", name);
            body = body.Replace("{1}", mailBody);

            return body;
        }
    }
}
