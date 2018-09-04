using JetBrains.Annotations;
using NanoSoft.Wpf.EventArgs;
using NanoSoft.Wpf.Services;
using System;

namespace NanoSoft.Wpf.Mvvm
{
    [PublicAPI]
    public abstract class NanoSoftBindableBase<TApp> : ValidatableBindableBase
    {
        public IAppServices<TApp> Services { get; }

        protected NanoSoftBindableBase(IAppServices<TApp> services)
        {
            Services = services;
        }

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
                    Services.Logout();
                    break;

                case ResponseState.NotFound:
                case ResponseState.Forbidden:
                case ResponseState.Unavailable:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(response.State));
            }

            Services.Alert(response.Message);

            ResponseRead(this, new ResponseEventArgs(response));
        }

        public event EventHandler<ResponseEventArgs> ResponseRead = delegate { };
    }
}