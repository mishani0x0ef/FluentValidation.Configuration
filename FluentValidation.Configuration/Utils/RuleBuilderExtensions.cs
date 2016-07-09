using System;
using FluentValidation.Configuration.Exceptions;
using FluentValidation.Configuration.Resources;

namespace FluentValidation.Configuration.Utils
{
    public static class RuleBuilderExtensions
    {
        public static void SetFromConfiguration<T, TProperty>(this IRuleBuilder<T, TProperty> builder,
            IValidationConfiguration configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (!configuration.ValidatorExistsFor<TProperty>())
            {
                throw new ConfigurationException(string.Format(MessageResources.ValidatorNotFound, typeof(TProperty)));
            }

            var validator = configuration.GetValidator<TProperty>();
            builder.SetValidator(validator);
        }
    }
}
