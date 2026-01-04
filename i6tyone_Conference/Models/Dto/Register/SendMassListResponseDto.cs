using i6tyone_Conference.Models.Dto.Aligo;

namespace i6tyone_Conference.Models.Dto.Register
{
    public class SendMassListResponseDto
    {
        public IList<SendMassResponseDto> list { get; set; } = new List<SendMassResponseDto>();
    }
}
