using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService: IMailService
    {
        private string _mailFrom { get; } = Startup.Configuration["mailSettings:mailFromAddress"];
        private string _mailTo { get; } = Startup.Configuration["mailSettings:mailToAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo} from LocalMailService");
            Debug.WriteLine($"{subject}");
            Debug.WriteLine($"{message}");
        }
    }
}
