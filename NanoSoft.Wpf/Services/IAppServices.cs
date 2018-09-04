﻿using System.Threading.Tasks;

namespace NanoSoft.Wpf.Services
{
    public interface IAppServices
    {
        void Alert(string message);
        Task<bool> ConfirmAsync(string message);
        void Logout();
    }

    public interface IAppServices<out TApp> : IAppServices
    {
        TApp Initialize();
    }
}
