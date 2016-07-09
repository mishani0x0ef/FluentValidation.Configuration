using FluentValidation.Configuration.Tests.TestResources;
using NUnit.Framework;

namespace FluentValidation.Configuration.Tests
{
    [TestFixture]
    public class ValidationConfigurationTests
    {
        private IValidationConfiguration ValidationConfiguration { get; set; }

        [SetUp]
        public void SetUp()
        {
            InitializeValidationConfiguration();
        }

        private void InitializeValidationConfiguration()
        {
            ValidationConfiguration = new ValidationConfiguration();
        }

        [Test]
        public void GetValidator_SimpleTypeValidatorValidateValidObject_ValidationSuccess()
        {
            ValidationConfiguration.Register<Country>().Build(c => c.Name, e => e.Length(1, 255));
            var country = new Country {Name = new string('a', 10)};

            var validator = ValidationConfiguration.GetValidator<Country>();

            Assert.That(() => validator.ValidateAndThrow(country), Throws.Nothing);
        }

        [Test]
        public void GetValidator_SimpleTypeValidatorValidateValidObject_ValidationFailed()
        {
            ValidationConfiguration.Register<Country>().Build(c => c.Name, e => e.Length(1, 255));
            var country = new Country { Name = new string('a', 256) };

            var validator = ValidationConfiguration.GetValidator<Country>();

            Assert.That(() => validator.ValidateAndThrow(country), Throws.Exception.TypeOf<ValidationException>());
        }
    }
}
