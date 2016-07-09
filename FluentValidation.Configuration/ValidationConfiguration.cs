﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Configuration.Exceptions;
using FluentValidation.Configuration.Internal;
using FluentValidation.Configuration.Resources;

namespace FluentValidation.Configuration
{
    public class ValidationConfiguration : IValidationConfiguration
    {
        protected Dictionary<Type, object> Validators;

        public ValidationConfiguration()
        {
            Validators = new Dictionary<Type, object>();
        }

        public IRulesBuilder<T> RegisterFor<T>()
        {
            if (Validators.ContainsKey(typeof(T)))
            {
                var message = string.Format(MessageResources.RegisterSameTypeForValidation, typeof(T));
                throw new ConfigurationException(message);
            }

            var builder = new RulesBuilder<T>();

            var validator = builder.ComposeValidator();
            Validators.Add(typeof(T), validator);

            return builder;
        }

        public void Clear()
        {
            Validators.Clear();
        }

        public bool ValidatorExistsFor<T>()
        {
            return Validators.ContainsKey(typeof(T));
        }

        public AbstractValidator<T> GetValidator<T>()
        {
            var validatorObj = Validators.FirstOrDefault(x => x.Key == typeof(T));
            var validator = validatorObj.Value as AbstractValidator<T>;

            if (validator == null)
            {
                var message = string.Format(MessageResources.ValidatorNotFound, typeof(T));
                throw new ValidatorNotFoundException(message);
            }

            return validator;
        }
    }
}
