using MimeKit;
using SoftLearnV1.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IEmailRepo
    {
        void SendEmail(EmailMessage message);
    }
}
