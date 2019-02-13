using NanoSoft.Wpf.EventArgs;
using System;

namespace NanoSoft.Wpf.Services
{
    public interface INotifyNewViewOpened
    {
        event EventHandler<NewViewOpenedEventArgs> NewViewOpened;
    }
}
