using System;

namespace NanoSoft.Wpf.Mvvm
{
    public class ResponseEventArgs : EventArgs
    {
        public ResponseEventArgs(IResponse response)
        {
            Response = response;
        }

        public IResponse Response { get; }
    }
}