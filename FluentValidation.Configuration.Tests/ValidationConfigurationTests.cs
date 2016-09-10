using System;
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
        public void Register_SingleValidator_Success()
        {
            Assert.That(() => ValidationConfiguration.Register<Country>(), Throws.Nothing);
        }

        [Test]
        public void Register_RegisterTwoValidatorForSameType_ThrowConfigurationException()
        {
            ValidationConfiguration.Register<Country>().RuleFor(c => c.Name, e => e.Length(1, 255));

            Assert.That(() => ValidationConfiguration.Register<Country>(),
                Throws.Exception.TypeOf<ConfigurationException>());
        }

        [Test]
        public void SetFromConfiguration_ComplexTypeValidator_Success()
        {
            ValidationConfiguration.Register<Country>().RuleFor(c => c.Name, e => e.Length(1, 255));

            Assert.That(
                () =>
                    ValidationConfiguration.Register<Address>()
                        .RuleFor(a => a.Country, e => e.SetFromConfiguration(ValidationConfiguration)),
                Throws.Nothing);
        }

        [Test]
        public void SetFromConfiguration_WithMissedRegistrationForInnerType_Success()
        {
            ValidationConfiguration.Clear();

            Assert.That(
                () =>
                    ValidationConfiguration.Register<Address>()
                        .RuleFor(a => a.Country, e => e.SetFromConfiguration(ValidationConfiguration)),
                Throws.Exception.TypeOf<ConfigurationException>());
        }

        [Test]
        public void SetFromConfiguration_WithNullValidationConfiguration_ThrowArgumentNullException()
        {
            Assert.That(() => ValidationConfiguration.Register<Address>()
                .RuleFor(a => a.Country, e => e.SetFromConfiguration(null)),
                Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Register_ValidatorInstance_Success()
        {
            AbstractValidator<Country> validator = new InlineValidator<Country>();

            Assert.That(() => ValidationConfiguration.Register(validator), Throws.Nothing);
        }

        [Test]
        public void Register_TwoValidatorInstnacesForSameType_ThrowsException()
        {
            AbstractValidator<Country> firstValidator = new InlineValidator<Country>();
            AbstractValidator<Country> secondValidator = new InlineValidator<Country>();

            ValidationConfiguration.Register(firstValidator);

            Assert.That(() => ValidationConfiguration.Register(secondValidator),
                Throws.Exception.TypeOf<ConfigurationException>());
        }

        [Test]
        public void Register_ConcreteValidator_Success()
        {
            Assert.That(() => ValidationConfiguration.Register(new CountryValidator()), Throws.Nothing);
        }

        [Test]
        public void Register_TwoValidatorForSameTypeWithConcreteValidator_ThrowConfigurationException()
        {
            ValidationConfiguration.Register<Country>()
                .RuleFor(c => c.Name, rule => rule.Length(1, 50));

            Assert.That(() => ValidationConfiguration.Register(new CountryValidator()),
                Throws.Exception.TypeOf<ConfigurationException>());
        }

        [Test]
        public void Register_NullConcreteValidator_ThrowArgumentNullException()
        {
            CountryValidator validator = null;

            Assert.That(() => ValidationConfiguration.Register(validator),
                Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ExistsFor_CheckExistedValidator_True()
        {
            ValidationConfiguration.Register<Country>().RuleFor(c => c.Name, e => e.Length(1, 255));

            Assert.That(ValidationConfiguration.IsExistsFor<Country>(), Is.True);
        }

        [Test]
        public void ExistsFor_CheckMissedValidator_False()
        {
            ValidationConfiguration.Clear();

            Assert.That(ValidationConfiguration.IsExistsFor<Country>(), Is.False);
        }

        [Test]
        public void Clear_NotEmptyConfiguration_ConfigurationCleared()
        {
            ValidationConfiguration.Register<Country>();
            ValidationConfiguration.Clear();

            Assert.That(() => ValidationConfiguration.IsExistsFor<Country>(), Is.False);
        }

        [Test]
        public void Clear_AfterValidatorInstanceRegister_ConfigurationCleared()
        {
            AbstractValidator<Country> validator = new InlineValidator<Country>();
            ValidationConfiguration.Register(validator);

            ValidationConfiguration.Clear();

            Assert.That(() => ValidationConfiguration.IsExistsFor<Country>(), Is.False);
        }

        [Test]
        public void GetValidator_SimpleTypeValidatorValidateValidObject_ValidationSuccess()
        {
            ValidationConfiguration.Register<Country>().RuleFor(c => c.Name, e => e.Length(1, 255));
            var country = new Country {Name = new string('a', 10)};

            var validator = ValidationConfiguration.GetValidator<Country>();

            Assert.That(() => validator.ValidateAndThrow(country), Throws.Nothing);
        }

        [Test]
        public void GetValidator_SimpleTypeValidatorValidateInvalidObject_ValidationFailed()
        {
            ValidationConfiguration.Register<Country>().RuleFor(c => c.Name, e => e.Length(1, 255));
            var country = new Country {Name = new string('a', 256)};

            var validator = ValidationConfiguration.GetValidator<Country>();

            Assert.That(() => validator.ValidateAndThrow(country), Throws.Exception.TypeOf<ValidationException>());
        }

        [Test]
        public void GetValidator_ComplexTypeValidatorValidateValidObject_ValidationSuccess()
        {
            ValidationConfiguration.Register<Country>().RuleFor(c => c.Name, e => e.Length(1, 255));
            ValidationConfiguration.Register<Address>()
                .RuleFor(a => a.Town, e => e.NotNull().NotEmpty())
                .RuleFor(a => a.Country, e => e.SetFromConfiguration(ValidationConfiguration));

            var country = new Country {Name = new string('a', 10)};
            var address = new Address {Country = country, Town = new string('a', 10)};

            var validator = ValidationConfiguration.GetValidator<Address>();

            Assert.That(() => validator.ValidateAndThrow(address), Throws.Nothing);
        }

        [Test]
        public void GetValidator_ComplexTypeValidatorValidateInvalidInnerObject_ValidationFailed()
        {
            ValidationConfiguration.Register<Country>().RuleFor(c => c.Name, e => e.Length(1, 255));
            ValidationConfiguration.Register<Address>()
                .RuleFor(a => a.Town, e => e.NotNull().NotEmpty())
                .RuleFor(a => a.Country, e => e.SetFromConfiguration(ValidationConfiguration));

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

        [Test]
        public void Remove_ExistingValidator_CorrectValidatorRemoved()
        {
            ValidationConfiguration.Register<Country>();
            ValidationConfiguration.Register<Address>();

            ValidationConfiguration.Remove<Country>();

            Assert.That(ValidationConfiguration.IsExistsFor<Country>(), Is.False);
            Assert.That(ValidationConfiguration.IsExistsFor<Address>(), Is.True);
        }

        [Test]
        public void Remove_NotExistingValidator_DoNothing()
        {
            ValidationConfiguration.Register<Address>();

            ValidationConfiguration.Remove<Country>();

            Assert.That(ValidationConfiguration.IsExistsFor<Country>(), Is.False);
            Assert.That(ValidationConfiguration.IsExistsFor<Address>(), Is.True);
        }
    }
}
