using AutoMapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils.Web;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Aligo;
using i6tyone_Conference.Models.Dto.AligoMsg;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace i6tyone_Conference.Service.Aligo
{
    public class AligoMsgService : IAligoMsgService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly ISqlRepository _repo;
        private readonly IMapper _mapper;
        private readonly AligoMsgUtil _aligoMsgUtil;
        private readonly string _contentType = "application/x-www-form-urlencoded";
        private readonly HttpClient _httpClient;

        public AligoMsgService(IDbConnectionFactory connFactory, ISqlRepository repo, IMapper mapper, AligoMsgUtil aligoMsgUtil, HttpClient httpClient)
        {
            _connFactory = connFactory;
            _repo = repo;
            _mapper = mapper;
            _aligoMsgUtil = aligoMsgUtil;
            _httpClient = httpClient;
        }

        ///문자전송(send)
        public async Task<GenericResponse<SendMassResponseDto>> SendSmsAsync(SendMessageApiRequestDto req)
        {
            var res = new GenericResponse<SendMassResponseDto>();

            using var formData = CreateFormData(req);
            using var response = await _httpClient.PostAsync(_aligoMsgUtil.send, formData);

            var content = await response.Content.ReadAsStringAsync();

            var resAnswer = JsonConvert.DeserializeObject<AligoGenericResponse>(content);
            var resData = JsonConvert.DeserializeObject<SendMassResponseDto>(content);
            if (resAnswer?.resultCode != "1")
            {
                res.SetResult(resAnswer.resultCode, resAnswer.message);
                return res;
            }

            res.SetResult(ErrorStatusCode.Success);
            res.Data = resData;
            return res;
        }

        private MultipartFormDataContent CreateFormData(SendMessageApiRequestDto req)
        {
            var formData = new MultipartFormDataContent
            {
                { new StringContent(_aligoMsgUtil.Key), "key" },
                { new StringContent(_aligoMsgUtil.UserId), "user_id" },
                { new StringContent(_aligoMsgUtil.Sender), "sender" },
                { new StringContent(req.receiver), "receiver" },
                { new StringContent(req.msg), "msg" },
                { new StringContent(req.msgType), "msg_type" },
                { new StringContent(req.title), "title" },
                { new StringContent(req.destination), "destination" },
                { new StringContent(req.rDate), "rdate" },
                { new StringContent(req.rTime), "rtime" },
                { new StringContent(req.testModeYn), "testmode_yn" }
            };

            if (req.image1?.Length > 0)
            {
                var imageContent = new StreamContent(new MemoryStream(req.image1));
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                formData.Add(imageContent, "image", "qrcode.png");
            }

            return formData;
        }

        ///문자전송(대량)(send-mult)
        public async Task<GenericResponse<SendMassResponseDto>> SendMultiSmsAsync(SendMassApiRequestDto req)
        {
            var res = new GenericResponse<SendMassResponseDto>();

            var formData = new List<StringKeyValue>
            {
                new StringKeyValue() { key = "key", value = _aligoMsgUtil.Key },
                new StringKeyValue() { key = "user_id", value = _aligoMsgUtil.UserId },
                new StringKeyValue() { key = "sender", value = _aligoMsgUtil.Sender },
                new StringKeyValue() { key = "rec_1", value = req.rec1 },
                new StringKeyValue() { key = "msg_1", value = req.msg1 },
                new StringKeyValue() { key = "cnt", value = req.cnt.ToString() },
                new StringKeyValue() { key = "title", value = req.title },
                new StringKeyValue() { key = "msg_type", value = req.msgType },
                new StringKeyValue() { key = "rdate", value = req.rDate },
                new StringKeyValue() { key = "rtime", value = req.rTime },
                new StringKeyValue() { key = "image1", value = req.image1 },
                new StringKeyValue() { key = "image2", value = req.image2 },
                new StringKeyValue() { key = "image3", value = req.image3 },
                new StringKeyValue() { key = "testmode_yn", value = req.testModeYn },
            };

            var postData = string.Join("&", formData.Select(x => $"{WebUtility.UrlEncode(x.key)}={WebUtility.UrlEncode(x.value)}"));

            var result = await WebRequestUtil.SendPostWebRequestV2Async(
                uri: _aligoMsgUtil.sendMass,
                sendData: postData,
                contentType: _contentType
            );

            var resAnswer = JsonConvert.DeserializeObject<AligoGenericResponse>(result.responseData);
            var resData = JsonConvert.DeserializeObject<SendMassResponseDto>(result.responseData);

            res.Data = resData;
            res.ResultCd = resAnswer.resultCode;
            res.ResultMsg = resAnswer.message;
            return res;
        }

        /// 전송내역조회(history)
        public async Task<GenericResponse<SendResponseDto>> GetSendHistoryAsync(SendApiRequestDto req)
        {
            var res = new GenericResponse<SendResponseDto>();

            var formData = new List<StringKeyValue>
            {
                new StringKeyValue() { key = "key", value = _aligoMsgUtil.Key },
                new StringKeyValue() { key = "user_id", value = _aligoMsgUtil.UserId },
                new StringKeyValue() { key = "page", value = req.page.ToString() },
                new StringKeyValue() { key = "page_size", value = req.pageSize.ToString() },
                new StringKeyValue() { key = "start_date", value = req.startDate },
                new StringKeyValue() { key = "limit_day", value = req.limitDay },
            };

            var postData = string.Join("&", formData.Select(x => $"{WebUtility.UrlEncode(x.key)}={WebUtility.UrlEncode(x.value)}"));

            var result = await WebRequestUtil.SendPostWebRequestV2Async(
                uri: _aligoMsgUtil.list,
                sendData: postData,
                contentType: _contentType
            );

            var resAnswer = JsonConvert.DeserializeObject<AligoGenericResponse>(result.responseData);
            var resData = JsonConvert.DeserializeObject<SendResponseDto>(result.responseData);

            res.Data = resData;
            res.ResultCd = resAnswer.resultCode;
            res.ResultMsg = resAnswer.message;
            return res;
        }

        /// 전송결과조회(상세)(result-detail)
        public async Task<GenericResponse<SmsResponseDto>> GetSendResultDetailAsync(SmsApiRequestDto req)
        {
            var res = new GenericResponse<SmsResponseDto>();

            var formData = new List<StringKeyValue>
            {
                new StringKeyValue() { key = "key", value = _aligoMsgUtil.Key },
                new StringKeyValue() { key = "user_id", value = _aligoMsgUtil.UserId },
                new StringKeyValue() { key = "mid", value = req.mid.ToString() },
                new StringKeyValue() { key = "page", value = req.page.ToString() },
                new StringKeyValue() { key = "page_size", value = req.pageSize.ToString() },
            };

            var postData = string.Join("&", formData.Select(x => $"{WebUtility.UrlEncode(x.key)}={WebUtility.UrlEncode(x.value)}"));

            var result = await WebRequestUtil.SendPostWebRequestV2Async(
                uri: _aligoMsgUtil.smsList,
                sendData: postData,
                contentType: _contentType
            );

            var resAnswer = JsonConvert.DeserializeObject<AligoGenericResponse>(result.responseData);
            var resData = JsonConvert.DeserializeObject<SmsResponseDto>(result.responseData);

            res.Data = resData;
            res.ResultCd = resAnswer.resultCode;
            res.ResultMsg = resAnswer.message;
            return res;
        }

        /// 발송가능건수(balance)
        public async Task<GenericResponse<RemainResponseDto>> GetSmsBalanceAsync()
        {
            var res = new GenericResponse<RemainResponseDto>();

            var formData = new List<StringKeyValue>
            {
                new StringKeyValue() { key = "key", value = _aligoMsgUtil.Key },
                new StringKeyValue() { key = "user_id", value = _aligoMsgUtil.UserId },
            };

            var postData = string.Join("&", formData.Select(x => $"{WebUtility.UrlEncode(x.key)}={WebUtility.UrlEncode(x.value)}"));

            var result = await WebRequestUtil.SendPostWebRequestV2Async(
                uri: _aligoMsgUtil.remain,
                sendData: postData,
                contentType: _contentType
            );

            var resAnswer = JsonConvert.DeserializeObject<AligoGenericResponse>(result.responseData);
            var resData = JsonConvert.DeserializeObject<RemainResponseDto>(result.responseData);

            res.Data = resData;
            res.ResultCd = resAnswer.resultCode;
            res.ResultMsg = resAnswer.message;
            return res;
        }

        /// 예약문자 취소(cancel)
        public async Task<GenericResponse<CancelResponseDto>> CancelSmsAsync(long mid)
        {
            var res = new GenericResponse<CancelResponseDto>();

            var formData = new List<StringKeyValue>
            {
                new StringKeyValue() { key = "key", value = _aligoMsgUtil.Key },
                new StringKeyValue() { key = "user_id", value = _aligoMsgUtil.UserId },
                new StringKeyValue() { key = "mid", value = mid.ToString() },
            };

            var postData = string.Join("&", formData.Select(x => $"{WebUtility.UrlEncode(x.key)}={WebUtility.UrlEncode(x.value)}"));

            var result = await WebRequestUtil.SendPostWebRequestV2Async(
                uri: _aligoMsgUtil.cancel,
                sendData: postData,
                contentType: _contentType
            );

            var resAnswer = JsonConvert.DeserializeObject<AligoGenericResponse>(result.responseData);
            var resData = JsonConvert.DeserializeObject<CancelResponseDto>(result.responseData);

            res.Data = resData;
            res.ResultCd = resAnswer.resultCode;
            res.ResultMsg = resAnswer.message;
            return res;
        }
    }
}
