using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NanoSoft.Wpf.EventArgs;
using NanoSoft.Wpf.Resources;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;

namespace NanoSoft.Wpf.Services
{
    public abstract class AppServices<TApp, TIdentityResult, TUser> : IAppServices<TApp, TIdentityResult, TUser>
    {
        private readonly Window _window;

        protected AppServices(Window window)
        {
            _window = window;
        }

        public abstract Task<TApp> InitializeAsync();
        public abstract Task<TUser> GetUserAsync(TIdentityResult identityResult);

        public virtual void OnMainThread(Action action)
            => Application.Current.Dispatcher.Invoke(action);

        public virtual void Alert(string message)
        {
            if (!(_window is MetroWindow metroWindow))
            {
                MessageBox.Show(message);
                return;
            }

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = SharedPhrases.Continue,
                DialogMessageFontSize = 15,
                DialogTitleFontSize = 20,
            };

            Application.Current.Dispatcher.Invoke(() => metroWindow.ShowMessageAsync(SharedPhrases.Alert, message?.Replace(".", ".\n"),
                MessageDialogStyle.Affirmative, mySettings));
        }

        public virtual async Task<bool> ConfirmAsync(string message)
        {
            if (!(_window is MetroWindow metroWindow))
                return false;

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = SharedPhrases.Continue,
                DialogMessageFontSize = 15,
                DialogTitleFontSize = 20,
                NegativeButtonText = SharedPhrases.Cancel,
            };

            var result = await metroWindow.ShowMessageAsync(SharedPhrases.Alert, message,
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            return result != MessageDialogResult.Negative;
        }

        public abstract void Logout();

        protected virtual StoredProperties<TAppUser, TSettings, TCompanyInfo> GetStoredProperties<TAppUser, TSettings, TCompanyInfo>
            (IDictionary dictionary)
            where TAppUser : class
            where TSettings : class
            where TCompanyInfo : class
        {
            var token = dictionary["token"] as string;
            var storedUser = dictionary["user"] as TAppUser;
            var storedSettings = dictionary["settings"] as TSettings;
            var storedCompanyInfo = dictionary["companyInfo"] as TCompanyInfo;
            var expire = dictionary["expire"] as DateTime?;

            if (token == null
                || storedUser == null
                || storedSettings == null
                || storedCompanyInfo == null
                || (expire == null
                || expire < DateTime.Now))
                return null;

            return new StoredProperties<TAppUser, TSettings, TCompanyInfo>()
            {
                CompanyInfo = storedCompanyInfo,
                Settings = storedSettings,
                User = storedUser,
                Token = token
            };
        }


        protected virtual void StoreProperties<TAppUser, TSettings, TCompanyInfo>(StoredProperties<TAppUser, TSettings, TCompanyInfo> properties, IDictionary dictionary)
        {
            dictionary["token"] = properties.Token;
            dictionary["user"] = properties.User;
            dictionary["settings"] = properties.Settings;
            dictionary["companyInfo"] = properties.CompanyInfo;
            dictionary["expire"] = GetNewExpireDate();
        }

        protected virtual DateTime GetNewExpireDate() => DateTime.Now.AddMinutes(5);

        public virtual void HandleException(Exception e)
        {
            Alert(e.Message);
            Console.WriteLine(e);
            OnExceptionThrown(e);
        }

        protected void OnExceptionThrown(Exception e) => ExceptionThrown(this, new ExceptionEventArgs(e));

        public event EventHandler<ExceptionEventArgs> ExceptionThrown = delegate { };
    }
}