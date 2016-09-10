using System;
using FluentValidation.Configuration.Tests.TestResources;
using NUnit.Framework;

namespace FluentValidation.Configuration.Tests
{
    [TestFixture]
    public class ValidationConfigurationExtensionsTests
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
        public void Register_NewValidatorByType_Success()
        {
            ValidationConfiguration.Register<Country, CountryValidator>();

            Assert.That(ValidationConfiguration.IsExistsFor<Country>(), Is.True);
        }

        [Test]
        public void Register_WithNullConfiguration_ThrowArgumentNullException()
        {
            ValidationConfiguration = null;

            Assert.That(() => ValidationConfiguration.Register<Country, CountryValidator>(),
                Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void OverrideWithRuleBuilder_WithNullConfiguration_ThrowArgumentNullException()
        {
            ValidationConfiguration = null;

            Assert.That(() => ValidationConfiguration.Override<Country, CountryValidator>(),
                Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void OverrideWithRuleBuilder_OverideRules_ValidatorChanged()
        {
            ValidationConfiguration.Register<Country>()
                .RuleFor(c => c.Name, e => e.Length(0, 3));

            ValidationConfiguration.Override<Country>()
                .RuleFor(c => c.Name, e => e.Length(0, 1));

            var validator = ValidationConfiguration.GetValidator<Country>();
            var country = new Country { Name = new string('a', 2) };

            Assert.That(validator.Validate(country).IsValid, Is.False);
        }

        [Test]
        public void OverrideWithInstance_WithNullConfiguration_ThrowArgumentNullException()
        {
            ValidationConfiguration = null;

            var validator = new CountryValidator();

            Assert.That(() => ValidationConfiguration.Override(validator), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void OverrideWithInstance_OverideValidator_ValidatorChanged()
        {
            ValidationConfiguration.Register<Country>()
                .RuleFor(c => c.Name, e => e.Length(0, 52));

            ValidationConfiguration.Override(new CountryValidator());

            var validator = ValidationConfiguration.GetValidator<Country>();
            var country = new Country { Name = new string('a', 51) };

            Assert.That(validator.Validate(country).IsValid, Is.False);
        }

        [Test]
        public void OverrideWithValidatorType_WithNullConfiguration_ThrowArgumentNullException()
        {
            ValidationConfiguration = null;

            Assert.That(() => ValidationConfiguration.Override<Country, CountryValidator>(), 
                Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void OverrideWithValidatorType_OverideValidator_ValidatorChanged()
        {
            ValidationConfiguration.Register<Country>()
                .RuleFor(c => c.Name, e => e.Length(0, 52));

            ValidationConfiguration.Override<Country, CountryValidator>();

            var validator = ValidationConfiguration.GetValidator<Country>();
            var country = new Country { Name = new string('a', 51) };

            Assert.That(validator.Validate(country).IsValid, Is.False);
        }
    }
}
