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
                new StringKeyValue() {key = "1", value = "Day1"},
                new StringKeyValue() {key = "2", value = "Day2"},
                new StringKeyValue() {key = "3", value = "Day3"},
                new StringKeyValue() {key = "4", value = "ALL Day"},
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
                new StringKeyValue() {key = "3", value = "얼리 3차"},
                new StringKeyValue() {key = "4", value = "일반"},
                new StringKeyValue() {key = "5", value = "원데이"},
                new StringKeyValue() {key = "6", value = "공식"},
                new StringKeyValue() {key = "7", value = "이벤트"},
                new StringKeyValue() {key = "8", value = "현장등록"},
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
                new StringKeyValue() {key = "A", value = "구역 A"},
                new StringKeyValue() {key = "B", value = "구역 B"},
                new StringKeyValue() {key = "C", value = "구역 C"},
                new StringKeyValue() {key = "D", value = "구역 D"},
                new StringKeyValue() {key = "E", value = "구역 E"},
                new StringKeyValue() {key = "F", value = "구역 F"},
                new StringKeyValue() {key = "G", value = "구역 G"},
                new StringKeyValue() {key = "H", value = "구역 H"},
                new StringKeyValue() {key = "I", value = "구역 I"},
                new StringKeyValue() {key = "J", value = "구역 J"},
            };

            var result = new KeyValueResponseDto() { list = list };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }
    }
}
