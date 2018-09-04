namespace NanoSoft.Wpf.EventArgs
{
    public class ResponseEventArgs : System.EventArgs
    {
        public ResponseEventArgs(IResponse response)
        {
            Response = response;
        }

        public IResponse Response { get; }
    }
}