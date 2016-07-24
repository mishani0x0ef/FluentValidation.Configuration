using System;
using FluentValidation.Configuration.Exceptions;
using NUnit.Framework;

namespace FluentValidation.Configuration.Tests.Exceptions
{
    [TestFixture]
    public class ValidatorNotFoundExceptionTests
    {
        private const string Message = "Test exception";

        [Test]
        public void Create_WithMessage()
        {
            var exception = new ValidatorNotFoundException(Message);

            Assert.That(exception.Message, Is.EqualTo(Message));
        }

        [Test]
        public void Create_WithMessageAndInner()
        {
            var inner = new ArgumentNullException();

            var exception = new ValidatorNotFoundException(Message, inner);

            Assert.That(exception.InnerException, Is.EqualTo(inner));
        }
    }
}
