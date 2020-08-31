using System;

namespace Ajka.BL.Exceptions
{
    [Serializable]
    internal class AjkaBusinessException : Exception
    {
        public AjkaBusinessException()
        {
        }

        public AjkaBusinessException(string message) : base(message)
        {
        }

        public AjkaBusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}