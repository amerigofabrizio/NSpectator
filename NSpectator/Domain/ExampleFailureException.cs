using System;

namespace NSpectator.Domain
{
    public class ExampleFailureException : Exception
    {
        public ExampleFailureException(string message, Exception innerException)
            : base(message, innerException) {}
    }
}