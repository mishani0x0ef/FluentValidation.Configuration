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
            Validator = new GenericValidator<T>();
        }

        public IRulesBuilder<T> Build<TProperty>(Expression<Func<T, TProperty>> selector, Action<IRuleBuilder<T, TProperty>> rules)
        {
            rules(Validator.RuleFor(selector));
            return this;
        }

        public AbstractValidator<T> ComposeValidator()
        {
            //todo: it's better to return clone instead of original validator. MR
            return Validator;
        }
    }
}
