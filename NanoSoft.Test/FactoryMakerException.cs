using System;

namespace NanoSoft.Test
{
    public class FactoryMakerException : Exception
    {
        public FactoryMakerException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}
