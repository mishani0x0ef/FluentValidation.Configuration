namespace FluentValidation.Configuration.Tests.TestResources
{
    public interface IAddress
    {
        string Line1 { get; set; }
        string Line2 { get; set; }
        string Town { get; set; }
        string County { get; set; }
        string Postcode { get; set; }
        Country Country { get; set; }
    }
}
