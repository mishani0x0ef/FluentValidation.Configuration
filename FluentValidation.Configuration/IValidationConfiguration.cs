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
        /// Return validator for specific type.
        /// </summary>
        /// <typeparam name="T">Type that will be validated with validator.</typeparam>
        /// <returns>An instance of validator.</returns>
        AbstractValidator<T> GetValidator<T>();
    }
}
