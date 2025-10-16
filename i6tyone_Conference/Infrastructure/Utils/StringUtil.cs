using System.Web;

namespace eGhis_WebService_Core.Infrastructure.Utils
{
    public class StringUtil
    {
        #region FIELD AREA ********************************************************************************************
        private static readonly char[] _randomCharArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private static readonly Random _random = new Random();
        #endregion

        #region STATIC METHOD AREA *************************************************************************************
        public static string GetJsonString(string org, bool isUrlEncode = false)
        {
            string rtnValue = org;

            if (isUrlEncode == true)
            {
                if (!string.IsNullOrEmpty(rtnValue))
                {
                    rtnValue = HttpUtility.UrlEncode(rtnValue);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(rtnValue))
                {
                    rtnValue = rtnValue.Replace(System.Environment.NewLine, "<BR />").Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\t", " ").Replace("\n", " ");
                }
            }

            return rtnValue;
        }

        public static string GetSerializeString(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string GenerateRandomString(int length)
        {
            if (length <= 0) return string.Empty;

            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = _randomCharArray[_random.Next(_randomCharArray.Length)];
            }

            return new string(result);
        }
        #endregion
    }
}
