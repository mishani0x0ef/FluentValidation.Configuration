using System;

namespace FluentValidation.Configuration.Exceptions
{
    public class ValidatorNotFoundException : ConfigurationExceptionBase
    {
        protected ValidatorNotFoundException()
        {
        }

        protected ValidatorNotFoundException(string message) : base(message)
        {
        }

        protected ValidatorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
