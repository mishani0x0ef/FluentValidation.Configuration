using FluentValidation.Configuration.Exceptions;
using FluentValidation.Configuration.Tests.TestResources;
using FluentValidation.Configuration.Utils;
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
            ValidationConfiguration.Clear();
        }

        [Test]
        public void RegisterFor_SingleValidator_Success()
        {
            Assert.That(() => ValidationConfiguration.RegisterFor<Country>(), Throws.Nothing);
        }

        [Test]
        public void RegisterFor_RegisterTwoValidatorForSameType_ThrowConfigurationException()
        {
            ValidationConfiguration.RegisterFor<Country>().Build(c => c.Name, e => e.Length(1, 255));

            Assert.That(() => ValidationConfiguration.RegisterFor<Country>(),
                Throws.Exception.TypeOf<ConfigurationException>());
        }

        [Test]
        public void RegisterFor_ComplexTypeValidator_Success()
        {
            ValidationConfiguration.RegisterFor<Country>().Build(c => c.Name, e => e.Length(1, 255));

            Assert.That(
                () =>
                    ValidationConfiguration.RegisterFor<Address>()
                        .Build(a => a.Country, e => e.SetFromConfiguration(ValidationConfiguration)),
                Throws.Nothing);
        }

        [Test]
        public void RegisterFor_ComplexTypeValidatorWithMissedRegistrationForInnerType_Success()
        {
            ValidationConfiguration.Clear();

            Assert.That(
                () =>
                    ValidationConfiguration.RegisterFor<Address>()
                        .Build(a => a.Country, e => e.SetFromConfiguration(ValidationConfiguration)),
                Throws.Exception.TypeOf<ConfigurationException>());
        }

        [Test]
        public void RegisterFor_ValidatorInstance_Success()
        {
            AbstractValidator<Country> validator = new InlineValidator<Country>();

            Assert.That(() => ValidationConfiguration.RegisterFor(validator), Throws.Nothing);
        }

        [Test]
        public void RegisterFor_TwoValidatorInstnacesForSameType_ThrowsException()
        {
            AbstractValidator<Country> firstValidator = new InlineValidator<Country>();
            AbstractValidator<Country> secondValidator = new InlineValidator<Country>();

            ValidationConfiguration.RegisterFor(firstValidator);

            Assert.That(() => ValidationConfiguration.RegisterFor(secondValidator),
                Throws.Exception.TypeOf<ConfigurationException>());
        }

        [Test]
        public void ExistsFor_CheckExistedValidator_True()
        {
            ValidationConfiguration.RegisterFor<Country>().Build(c => c.Name, e => e.Length(1, 255));

            Assert.That(ValidationConfiguration.ExistsFor<Country>(), Is.True);
        }

        [Test]
        public void ExistsFor_CheckMissedValidator_False()
        {
            ValidationConfiguration.Clear();

            Assert.That(ValidationConfiguration.ExistsFor<Country>(), Is.False);
        }

        [Test]
        public void Clear_NotEmptyConfiguration_ConfigurationCleared()
        {
            ValidationConfiguration.RegisterFor<Country>();
            ValidationConfiguration.Clear();

            Assert.That(() => ValidationConfiguration.ExistsFor<Country>(), Is.False);
        }

        [Test]
        public void Clear_AfterValidatorInstanceRegister_ConfigurationCleared()
        {
            AbstractValidator<Country> validator = new InlineValidator<Country>();
            ValidationConfiguration.RegisterFor(validator);

            ValidationConfiguration.Clear();

            Assert.That(() => ValidationConfiguration.ExistsFor<Country>(), Is.False);
        }

        [Test]
        public void GetValidator_SimpleTypeValidatorValidateValidObject_ValidationSuccess()
        {
            ValidationConfiguration.RegisterFor<Country>().Build(c => c.Name, e => e.Length(1, 255));
            var country = new Country {Name = new string('a', 10)};

            var validator = ValidationConfiguration.GetValidator<Country>();

            Assert.That(() => validator.ValidateAndThrow(country), Throws.Nothing);
        }

        [Test]
        public void GetValidator_SimpleTypeValidatorValidateInvalidObject_ValidationFailed()
        {
            ValidationConfiguration.RegisterFor<Country>().Build(c => c.Name, e => e.Length(1, 255));
            var country = new Country {Name = new string('a', 256)};

            var validator = ValidationConfiguration.GetValidator<Country>();

            Assert.That(() => validator.ValidateAndThrow(country), Throws.Exception.TypeOf<ValidationException>());
        }

        [Test]
        public void GetValidator_ComplexTypeValidatorValidateValidObject_ValidationSuccess()
        {
            ValidationConfiguration.RegisterFor<Country>().Build(c => c.Name, e => e.Length(1, 255));
            ValidationConfiguration.RegisterFor<Address>()
                .Build(a => a.Town, e => e.NotNull().NotEmpty())
                .Build(a => a.Country, e => e.SetFromConfiguration(ValidationConfiguration));

            var country = new Country {Name = new string('a', 10)};
            var address = new Address {Country = country, Town = new string('a', 10)};

            var validator = ValidationConfiguration.GetValidator<Address>();

            Assert.That(() => validator.ValidateAndThrow(address), Throws.Nothing);
        }

        [Test]
        public void GetValidator_ComplexTypeValidatorValidateInvalidInnerObject_ValidationFailed()
        {
            ValidationConfiguration.RegisterFor<Country>().Build(c => c.Name, e => e.Length(1, 255));
            ValidationConfiguration.RegisterFor<Address>()
                .Build(a => a.Town, e => e.NotNull().NotEmpty())
                .Build(a => a.Country, e => e.SetFromConfiguration(ValidationConfiguration));

            var country = new Country {Name = string.Empty};
            var address = new Address {Country = country, Town = new string('a', 10)};

            var validator = ValidationConfiguration.GetValidator<Address>();

            Assert.That(() => validator.ValidateAndThrow(address), Throws.Exception.TypeOf<ValidationException>());
        }

        [Test]
        public void GetValidator_NotExistedValidator_ThowValidatorNotFountException()
        {
            Assert.That(() => ValidationConfiguration.GetValidator<Country>(),
                Throws.Exception.TypeOf<ValidatorNotFoundException>());
        }
    }
}
