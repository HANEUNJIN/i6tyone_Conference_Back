using i6tyone_Conference.Models.Config;
using Microsoft.Extensions.Options;

namespace i6tyone_Conference.Infrastructure.Utils
{
    public class OnSiteGoogleUtil
    {
        private readonly OnSiteGoogleSettings _googleSettings;

        public OnSiteGoogleUtil(IOptions<OnSiteGoogleSettings> googleSettings) => _googleSettings = googleSettings.Value;

        private string Url => _googleSettings.Url;

        private string Id => _googleSettings.Id;

        public string CsvUrl => $"{Url}/{Id}/gviz/tq?tqx=out:csv";
    }
}
