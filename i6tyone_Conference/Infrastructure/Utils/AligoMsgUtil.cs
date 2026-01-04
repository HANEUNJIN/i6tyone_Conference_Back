using i6tyone_Conference.Models.Config;
using Microsoft.Extensions.Options;

namespace i6tyone_Conference.Infrastructure.Utils
{
    public class AligoMsgUtil
    {
        private readonly AligoMsgSettings _aligoMsgSettings;

        public AligoMsgUtil(IOptions<AligoMsgSettings> aligoMsgSettings) => _aligoMsgSettings = aligoMsgSettings.Value;

        /// <summary>
        /// Url
        /// </summary>
        public string Url => _aligoMsgSettings.Url;

        /// <summary>
        /// 인증용 API Key
        /// </summary>
        public string Key => _aligoMsgSettings.Key;

        /// <summary>
        /// 사용자 ID
        /// </summary>
        public string UserId => _aligoMsgSettings.UserId;

        /// <summary>
        /// 발신자 전화번호 (최대 16bytes)
        /// </summary>
        public string Sender => _aligoMsgSettings.Sender;

        /// <summary>
        /// 문자전송 API
        /// </summary>
        public string send => $"{Url}/send/";

        /// <summary>
        /// 문자전송(대량) API
        /// </summary>
        public string sendMass => $"{Url}/send_mass/";

        /// <summary>
        /// 전송내역조회
        /// </summary>
        public string list => $"{Url}/list/";

        /// <summary>
        /// 전송결과조회(상세)
        /// </summary>
        public string smsList => $"{Url}/sms_list/";

        /// <summary>
        /// 발송가능건수
        /// </summary>
        public string remain => $"{Url}/remain/";

        /// <summary>
        /// 예약문자 취소
        /// </summary>
        public string cancel => $"{Url}/cancel/";
    }
}
