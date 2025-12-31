using Microsoft.AspNetCore.Mvc;

namespace i6tyone_Conference.Models.Dto.Register
{
    public class ExcelResponseDto
    {
        public byte[] FileBytes { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
