namespace FluentValidation.Configuration.Tests.TestResources
{
    public class Address : IAddress
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public Country Country { get; set; }
        public int Id { get; set; }
    }
}
