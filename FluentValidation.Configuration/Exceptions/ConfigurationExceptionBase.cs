using System;

namespace FluentValidation.Configuration.Exceptions
{
    public abstract class ConfigurationExceptionBase : Exception
    {
        protected ConfigurationExceptionBase()
        {
        }

        protected ConfigurationExceptionBase(string message) : base(message)
        {
        }

        protected ConfigurationExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
