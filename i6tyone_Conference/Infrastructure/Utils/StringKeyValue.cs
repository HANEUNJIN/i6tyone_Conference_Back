namespace i6tyone_Conference.Infrastructure.Utils
{
    public struct StringKeyValue
    {
        public static readonly StringKeyValue Empty = new StringKeyValue(string.Empty);

        public string key { get; set; }
        public string value { get; set; }

        public StringKeyValue(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public StringKeyValue(string keyValue)
        {
            this.key = keyValue;
            this.value = keyValue;
        }
    }
}
