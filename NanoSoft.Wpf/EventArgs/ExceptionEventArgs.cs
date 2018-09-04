using System;

namespace NanoSoft.Wpf.EventArgs
{
    public class ExceptionEventArgs : System.EventArgs
    {
        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}