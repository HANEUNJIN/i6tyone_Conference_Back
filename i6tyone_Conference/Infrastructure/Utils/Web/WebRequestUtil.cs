using System.Net;
using System.Text;
using eGhis_WebService_Core.Models.Common;

namespace eGhis_WebService_Core.Infrastructure.Utils.Web
{
    public class WebRequestUtil
    {
        #region FIELD AREA *********************************************************************
        #endregion

        #region GENERAL STATIC METHOD AREA ***************************************************************
        public static SendWebRequestResult SendPostWebRequestV2(string uri,
                                                                string sendData,
                                                                string authHeader = "",
                                                                string contentType = "application/json",
                                                                string rcvEncodeType = "",
                                                                int timeout = 8000)
        {
            var res = new SendWebRequestResult();
            var request = WebRequest.Create(uri);
            request.Method = "POST";
            request.Timeout = timeout;

            if (!string.IsNullOrEmpty(authHeader))
            {
                request.Headers.Add("Authorization", authHeader);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(sendData);
            request.ContentType = contentType;
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            try
            {
                using var response = request.GetResponse();
                res.headerDate = response.Headers.GetValues("Date")?[0];
                res.statusCode = (int)((HttpWebResponse)response).StatusCode;

                var encoding = !string.IsNullOrEmpty(rcvEncodeType)
                    ? Encoding.GetEncoding(rcvEncodeType)
                    : Encoding.Default;

                using var dataStream = response.GetResponseStream();
                using var reader = new StreamReader(dataStream, encoding);
                res.responseData = reader.ReadToEnd();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    res.statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                    using var reader = new StreamReader(ex.Response.GetResponseStream());
                    res.responseData = reader.ReadToEnd();
                }
                else
                {
                    throw;
                }
            }

            return res;
        }

        public static async Task<SendWebRequestResult> SendPostWebRequestV2Async(string uri,
                                                                                 string sendData,
                                                                                 string authHeader = "",
                                                                                 string contentType = "application/json",
                                                                                 string rcvEncodeType = "",
                                                                                 int timeout = 8000)
        {
            var result = new SendWebRequestResult();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = contentType;
                request.Timeout = timeout;

                if (!string.IsNullOrEmpty(authHeader))
                {
                    request.Headers.Add("Authorization", authHeader);
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(sendData);
                using (var dataStream = await request.GetRequestStreamAsync())
                {
                    await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                }

                using var response = (HttpWebResponse)await request.GetResponseAsync();
                result.statusCode = (int)response.StatusCode;
                result.headerDate = response.Headers["Date"];

                using var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result.responseData = await reader.ReadToEndAsync();
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    result.statusCode = (int)errorResponse.StatusCode;
                    using var reader = new StreamReader(errorResponse.GetResponseStream());
                    result.responseData = await reader.ReadToEndAsync();
                }
                else
                {
                    result.statusCode = 0;
                    result.responseData = ex.Message;
                }
            }

            return result;
        }
        #endregion
    }
}
