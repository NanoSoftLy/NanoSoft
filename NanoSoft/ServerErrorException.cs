using System;

namespace NanoSoft
{
    public class ServerErrorException : Exception
    {
        public ServerErrorException(string message) : base(message)
        {
        }
    }
}
