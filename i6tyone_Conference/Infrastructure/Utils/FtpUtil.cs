using i6tyone_Conference.Models.Config;
using Microsoft.Extensions.Options;
using System.Web;

namespace i6tyone_Conference.Infrastructure.Utils
{
    public class FtpUtil
    {
        private readonly FtpSettings _ftpSettings;

        public FtpUtil(IOptions<FtpSettings> ftpSettings) => _ftpSettings = ftpSettings.Value;

        public string Url => $"{_ftpSettings.Url}:{_ftpSettings.Port}";
        public string User => _ftpSettings.User;
        public string Password => _ftpSettings.Password;
        public string UrlEncode => HttpUtility.UrlEncode(Url);
    }
}
