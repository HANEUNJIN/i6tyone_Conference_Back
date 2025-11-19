using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Register;

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
                new StringKeyValue() {key = "4", value = "ALL"},
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
            };

            var result = new KeyValueResponseDto() { list = list };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }
    }
}
