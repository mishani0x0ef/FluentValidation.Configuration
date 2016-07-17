using System;
using System.Linq.Expressions;

namespace FluentValidation.Configuration
{
    /// <summary>
    /// Provide functionality for building validation rules for specific type.
    /// </summary>
    public interface IRulesBuilder<T>
    {
        /// <summary>
        /// Build validation rule for specific property.
        /// </summary>
        /// <typeparam name="TProperty">Type of property for which rules building.</typeparam>
        /// <param name="selector">Property selector expression.</param>
        /// <param name="rules">FluentValidation rules for property.</param>
        /// <returns></returns>
        IRulesBuilder<T> RuleFor<TProperty>(
            Expression<Func<T, TProperty>> selector,
            Action<IRuleBuilder<T, TProperty>> rules);

        /// <summary>
        /// Compose validator based on builded rules.
        /// </summary>
        /// <returns>An instance of validator that use builded rules.</returns>
        IValidator<T> ComposeValidator();
    }
}
