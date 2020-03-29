using System;

namespace GameProducer.Interfaces.Error
{
    [Serializable()]
    public class GenericApiException : Exception
    {
        public string Reason { get; set; }

        public GenericApiException()
        {
        }
        public GenericApiException(string message) : base(message)
        {
        }
        public GenericApiException(string message, string reason) : base(message)
        {
            Reason = reason;
        }

    }
    public enum ExceptionType
    {
        INTEGRATION_ERROR,
        CLIENT_ERROR,
        VALIDATION_ERROR
    }
}
