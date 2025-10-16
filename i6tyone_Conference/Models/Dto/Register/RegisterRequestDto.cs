namespace eGhis_WebService_Core.Models.Dto.Auth
{
    public class RegisterRequestDto
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string churchName { get; set; }
        public string region { get; set; }
        public string denomination { get; set; }
        public string floorSeat { get; set; }
        public string consentPrivacy { get; set; }
    }
}
