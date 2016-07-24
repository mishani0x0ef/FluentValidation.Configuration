using System;

namespace FluentValidation.Configuration
{
    public static class ValidationConfigurationExtensions
    {
        /// <summary>
        /// Create and register validator of specific type.
        /// </summary>
        /// <typeparam name="T">Type that apply validation rules.</typeparam>
        /// <typeparam name="TValidator">Type of validator to register.</typeparam>
        /// <param name="config">Existed validation configuration.</param>
        public static void Register<T, TValidator>(this IValidationConfiguration config)
            where TValidator : IValidator<T>, new()
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var validator = new TValidator();
            config.Register(validator);
        }
    }
}
