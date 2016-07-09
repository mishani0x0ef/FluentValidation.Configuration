using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Configuration.Internal;

namespace FluentValidation.Configuration
{
    public class ValidationConfiguration : IValidationConfiguration
    {
        protected Dictionary<Type, object> Validators;

        public ValidationConfiguration()
        {
            Validators = new Dictionary<Type, object>();
        }

        // todo: provide handling of registration same type twice. MR
        public IRulesBuilder<T> Register<T>()
        {
            var builder = new RulesBuilder<T>();

            var validator = builder.ComposeValidator();
            Validators.Add(typeof(T), validator);

            return builder;
        }

        // todo: provide appropriate behavior in case no validators registered for type. MR
        public AbstractValidator<T> GetValidator<T>()
        {
            var validator = Validators.FirstOrDefault(x => x.Key == typeof(T));
            return validator.Value as AbstractValidator<T>;
        }
    }
}
