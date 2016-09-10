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

        /// <summary>
        /// Override existing validation for specific type if exists.
        /// Otherwise simply create new validator.
        /// </summary>
        /// <typeparam name="T">Type that apply validation rules.</typeparam>
        /// <param name="config">Existed validation configuration.</param>
        /// <returns>RulesBuilder that could be used to configure validation for specific properties.</returns>
        public static IRulesBuilder<T> Override<T>(this IValidationConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Remove<T>();
            return config.Register<T>();
        }

        /// <summary>
        /// Override existing validation for specific type if exists.
        /// Otherwise simply create new validator.
        /// </summary>
        /// <typeparam name="T">Type that apply validation rules.</typeparam>
        /// <param name="config">Existed validation configuration.</param>
        /// <param name="validator">Validator instance.</param>
        public static void Override<T>(this IValidationConfiguration config, IValidator<T> validator)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Remove<T>();
            config.Register(validator);
        }

        /// <summary>
        /// Override existing validation for specific type if exists.
        /// Otherwise simply create new validator.
        /// </summary>
        /// <typeparam name="T">Type that apply validation rules.</typeparam>
        /// <typeparam name="TValidator">Type of validator that should override existing.</typeparam>
        /// <param name="config">Existed validation configuration.</param>
        public static void Override<T, TValidator>(this IValidationConfiguration config)
            where TValidator : IValidator<T>, new()
        {
            var validator = new TValidator();
            Override(config, validator);
        }
    }
}
