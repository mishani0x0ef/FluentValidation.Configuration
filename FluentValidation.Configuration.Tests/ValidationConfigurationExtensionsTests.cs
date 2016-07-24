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
    }
}
