using System;
using System.Linq.Expressions;

namespace FluentValidation.Configuration.Internal
{
    /// <summary>
    /// Concrete base RulesBuilder.
    /// </summary>
    /// <typeparam name="T">Type that should apply validation rules.</typeparam>
    internal class RulesBuilder<T> : IRulesBuilder<T>
    {
        /// <summary>
        /// Validator that was configured by current RulesBuilder.
        /// </summary>
        protected AbstractValidator<T> Validator { get; }

        public RulesBuilder()
        {
            Validator = new InlineValidator<T>();
        }

        public IRulesBuilder<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> selector, Action<IRuleBuilder<T, TProperty>> rules)
        {
            rules(Validator.RuleFor(selector));
            return this;
        }

        public IValidator<T> ComposeValidator()
        {
            return Validator;
        }
    }
}
