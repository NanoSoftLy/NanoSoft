using System.Windows.Controls;

namespace NanoSoft.Wpf.EventArgs
{
    public class NewViewOpenedEventArgs : System.EventArgs
    {
        public NewViewOpenedEventArgs(UserControl userControl)
        {
            UserControl = userControl;
        }

        public UserControl UserControl { get; }
    }
}
