using JetBrains.Annotations;
using NanoSoft.Wpf.Services;
using System;
using System.Threading.Tasks;

namespace NanoSoft.Wpf.Mvvm
{
    [PublicAPI]
    public abstract class NanoSoftBindableBase<TApp> : ValidatableBindableBase
    {
        private readonly IAppServices<TApp> _services;
        protected NanoSoftBindableBase(IAppServices<TApp> services)
        {
            _services = services;
        }

        protected virtual Task<TApp> InitializeAsync() => _services.InitializeAsync();
        protected virtual void Alert(string message) => _services.Alert(message);
        protected virtual Task<bool> ConfirmAsync(string message) => _services.ConfirmAsync(message);

        protected virtual void Read(IResponse response)
        {
            switch (response.State)
            {
                case ResponseState.Valid:
                    ClearErrors();
                    break;

                case ResponseState.BadRequest:
                case ResponseState.Unacceptable:
                    AddErrors(response.Errors);
                    break;

                case ResponseState.Unauthorized:
                    _services.Logout();
                    break;

                case ResponseState.NotFound:
                case ResponseState.Forbidden:
                case ResponseState.Unavailable:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(response.State));
            }

            _services.Alert(response.Message);
        }
    }
}