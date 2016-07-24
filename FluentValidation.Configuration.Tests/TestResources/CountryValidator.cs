namespace FluentValidation.Configuration.Tests.TestResources
{
    public class CountryValidator : AbstractValidator<Country>
    {
        public CountryValidator()
        {
            RuleFor(c => c.Name).Length(1, 50);
        }
    }
}
