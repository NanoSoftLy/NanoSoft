using System;

namespace NanoSoft
{
    public class FactoryMakerException : Exception
    {
        public FactoryMakerException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}
