using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Define;

namespace eGhis_WebService_Core.Exceptions
{
    public class BusinessException : Exception
    {
        public string Code { get; }

        // ✅ 기본 string code + message 버전
        public BusinessException(string code, string message)
            : base(message)
        {
            Code = code;
        }

        public BusinessException(string code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        public BusinessException(ErrorStatusCode status)
            : base(EnumUtil.GetDescription(status))
        {
            Code = EnumUtil.GetDisplayName(status);
        }

        public BusinessException(ErrorStatusCode status, Exception innerException)
            : base(EnumUtil.GetDescription(status), innerException)
        {
            Code = EnumUtil.GetDisplayName(status);
        }
    }
}
