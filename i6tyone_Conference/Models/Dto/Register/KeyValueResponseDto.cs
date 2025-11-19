using i6tyone_Conference.Infrastructure.Utils;

namespace i6tyone_Conference.Models.Dto.Register
{
    public class KeyValueResponseDto
    {
        public IList<StringKeyValue> list { get; set; } = new List<StringKeyValue>();
    }
}
