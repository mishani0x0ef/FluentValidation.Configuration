namespace FluentValidation.Configuration
{
    /// <summary>
    /// Represent general configuration mechanism for types validations.
    /// </summary>
    public interface IValidationConfiguration
    {
        /// <summary>
        /// Create and register validatior for specific type.
        /// </summary>
        /// <typeparam name="T">Type that apply validation rules.</typeparam>
        /// <returns>RulesBuilder that could be used to configure validation for specific properties.</returns>
        IRulesBuilder<T> Register<T>();

        /// <summary>
        /// Register concrete validator for specific type.
        /// </summary>
        /// <typeparam name="T">Type that apply validation rules.</typeparam>
        /// <param name="validator">Validator instance.</param>
        void Register<T>(IValidator<T> validator);

        /// <summary>
        /// Clear all registered validators.
        /// </summary>
        void Clear();

        /// <summary>
        /// Check is validator for type T registered.
        /// </summary>
        /// <typeparam name="T">Type that apply validation rules.</typeparam>
        /// <returns>True if validator already registered.</returns>
        bool IsExistsFor<T>();

        /// <summary>
        /// Return validator for specific type.
        /// </summary>
        /// <typeparam name="T">Type that will be validated with validator.</typeparam>
        /// <returns>An instance of validator.</returns>
        IValidator<T> GetValidator<T>();
    }
}
