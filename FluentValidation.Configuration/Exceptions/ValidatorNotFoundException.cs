using System;

namespace FluentValidation.Configuration.Exceptions
{
    public class ValidatorNotFoundException : ConfigurationExceptionBase
    {
        public ValidatorNotFoundException()
        {
        }

        public ValidatorNotFoundException(string message) : base(message)
        {
        }

        public ValidatorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
