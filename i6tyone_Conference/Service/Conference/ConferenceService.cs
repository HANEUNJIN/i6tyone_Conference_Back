using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Conference;

namespace i6tyone_Conference.Service.Conference
{
    public class ConferenceService : IConferenceService
    {
        public async Task<GenericResponse<KeyValueResponseDto>> GetDayListAsync()
        {
            var res = new GenericResponse<KeyValueResponseDto>();

            var list = new List<StringKeyValue>
            {
                new StringKeyValue() {key = "1", value = "화"},
                new StringKeyValue() {key = "2", value = "수"},
                new StringKeyValue() {key = "3", value = "목"},
                new StringKeyValue() {key = "4", value = "3-day"},
            };

            var result = new KeyValueResponseDto() { list = list };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<KeyValueResponseDto>> GetOptionListAsync()
        {
            var res = new GenericResponse<KeyValueResponseDto>();

            var list = new List<StringKeyValue>
            {
                new StringKeyValue() {key = "1", value = "슈퍼얼리"},
                new StringKeyValue() {key = "2", value = "얼리 1차"},
                new StringKeyValue() {key = "3", value = "얼리 2차"},
                new StringKeyValue() {key = "4", value = "공식"},
                new StringKeyValue() {key = "5", value = "이벤트"},
                new StringKeyValue() {key = "6", value = "현장구매"},
                new StringKeyValue() {key = "7", value = "VIP"},
                new StringKeyValue() {key = "8", value = "새신자"},
            };

            var result = new KeyValueResponseDto() { list = list };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<KeyValueResponseDto>> GetAreaListAsync()
        {
            var res = new GenericResponse<KeyValueResponseDto>();

            var list = new List<StringKeyValue>
            {
                new StringKeyValue() {key = "A1", value = "A1"},
                new StringKeyValue() {key = "B1", value = "B1"},
                new StringKeyValue() {key = "C1", value = "C1"},
                new StringKeyValue() {key = "D1", value = "D1"},
                new StringKeyValue() {key = "A2-1", value = "A2-1"},
                new StringKeyValue() {key = "A2-2", value = "A2-2"},
                new StringKeyValue() {key = "B2-1", value = "B2-1"},
                new StringKeyValue() {key = "B2-2", value = "B2-2"},
                new StringKeyValue() {key = "C2-1", value = "C2-1"},
                new StringKeyValue() {key = "C2-2", value = "C2-2"},
                new StringKeyValue() {key = "D2-1", value = "D2-1"},
                new StringKeyValue() {key = "D2-2", value = "D2-2"},
                new StringKeyValue() {key = "E2-1", value = "E2-1"},
                new StringKeyValue() {key = "E2-2", value = "E2-2"},
                new StringKeyValue() {key = "F2-1", value = "F2-1"},
                new StringKeyValue() {key = "F2-2", value = "F2-2"},
                new StringKeyValue() {key = "G2-1", value = "G2-1"},
                new StringKeyValue() {key = "G2-2", value = "G2-2"},
                new StringKeyValue() {key = "유아&장애", value = "유아&장애"},
                new StringKeyValue() {key = "STAFF", value = "STAFF"},
                new StringKeyValue() {key = "", value = "미지정"},
            };

            var result = new KeyValueResponseDto() { list = list };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }
    }
}
