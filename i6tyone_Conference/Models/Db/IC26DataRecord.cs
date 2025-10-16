namespace eGhis_WebService_Core.Models.Db
{
    public record IC26DataRecord
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string churchName { get; set; }
        public string region { get; set; }
        public string denomination { get; set; }
        public string floorSeat { get; set; } = "N";
        public string consentPrivacy { get; set; } = "N";
        public string createdAt { get; set; }
    }
}
