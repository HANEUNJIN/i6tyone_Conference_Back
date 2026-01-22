using i6tyone_Conference.Models.Config;
using Microsoft.Extensions.Options;

namespace i6tyone_Conference.Infrastructure.Utils
{
    public class GoogleUtil
    {
        private readonly GoggleSettings _googleSettings;

        public GoogleUtil(IOptions<GoggleSettings> googleSettings) => _googleSettings = googleSettings.Value;

        private string Url => _googleSettings.Url;

        private string Id => _googleSettings.Id;

        private string Gid => _googleSettings.Gid;

        public string CsvUrl => $"{Url}/{Id}/export?format=csv&gid={Gid}";
    }
}
