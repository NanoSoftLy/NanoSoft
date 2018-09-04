using NanoSoft.Wpf.EventArgs;
using System;
using System.Threading.Tasks;

namespace NanoSoft.Wpf.Services
{
    public interface IAppServices
    {
        void Alert(string message);
        Task<bool> ConfirmAsync(string message);
        void Logout();
        void HandleException(Exception e);
        event EventHandler<ExceptionEventArgs> ExceptionThrown;
    }

    public interface IAppServices<TApp> : IAppServices
    {
        Task<TApp> InitializeAsync();
    }
}
