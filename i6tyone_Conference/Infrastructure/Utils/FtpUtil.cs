using i6tyone_Conference.Models.Config;
using Microsoft.Extensions.Options;
using System.Net;
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

        /// <summary>
        /// // 폴더 존재 확인 후 생성
        /// </summary>
        public async Task CreateFtpDirectoryRecursiveAsync(string ftpFolderUrl)
        {
            // 끝에 슬래시 보정
            if (!ftpFolderUrl.EndsWith("/"))
                ftpFolderUrl += "/";

            // URI 분해
            var uri = new Uri(ftpFolderUrl);
            var segments = uri.AbsolutePath.Trim('/').Split('/');
            var baseUrl = $"{uri.Scheme}://{uri.Host}";
            if (!uri.IsDefaultPort)
                baseUrl += $":{uri.Port}";

            // 상위 폴더부터 차근차근 생성
            string currentPath = baseUrl;
            foreach (var segment in segments)
            {
                currentPath += "/" + segment;
                await CreateFtpDirectoryIfNotExistsAsync(currentPath + "/");
            }
        }

        public async Task CreateFtpDirectoryIfNotExistsAsync(string ftpFolderUrl)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpFolderUrl);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(_ftpSettings.User, _ftpSettings.Password);

            try
            {
                using var response = (FtpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException ex)
            {
                var resp = (FtpWebResponse)ex.Response;

                if (resp.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                    throw;
            }
        }

        /// <summary>
        /// // FTP 업로드
        /// </summary>
        public async Task<bool> UploadFileToFtpAsync(byte[] fileBytes, string ftpFolderUrl, string fileName)
        {
            try
            {
                string ftpFileUrl = $"{ftpFolderUrl}/{Uri.EscapeDataString(fileName)}";
                var request = WebRequest.Create(ftpFileUrl) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_ftpSettings.User, _ftpSettings.Password);

                using var requestStream = await request.GetRequestStreamAsync();
                await requestStream.WriteAsync(fileBytes, 0, fileBytes.Length);

                using var response = await request.GetResponseAsync() as FtpWebResponse;
                Console.WriteLine($"Upload Complete: {response.StatusDescription}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FTP Upload Failed: {ex.Message}");
                return false;
            }
        }
    }
}
