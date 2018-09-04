using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NanoSoft.Wpf.Resources;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;

namespace NanoSoft.Wpf.Services
{
    public abstract class AppServices<TApp> : IAppServices<TApp>
    {
        private readonly Window _window;

        protected AppServices(Window window)
        {
            _window = window;
        }

        public abstract TApp Initialize();

        public virtual void Alert(string message)
        {
            if (!(_window is MetroWindow metroWindow))
            {
                MessageBox.Show(message);
                return;
            }

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = Phrases.Continue,
                DialogMessageFontSize = 15,
                DialogTitleFontSize = 20,
            };

            Application.Current.Dispatcher.Invoke(() => metroWindow.ShowMessageAsync(Phrases.Warning, message?.Replace(".", ".\n"),
                MessageDialogStyle.Affirmative, mySettings));
        }

        public virtual async Task<bool> ConfirmAsync(string message)
        {
            if (!(_window is MetroWindow metroWindow))
                return false;

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = Phrases.Continue,
                DialogMessageFontSize = 15,
                DialogTitleFontSize = 20,
                NegativeButtonText = Phrases.Cancel,
            };

            var result = await metroWindow.ShowMessageAsync(Phrases.Warning, message,
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            return result != MessageDialogResult.Negative;
        }

        public abstract void Logout();

        protected virtual StoredProperties<TUser, TSettings, TCompanyInfo> GetStoredProperties<TUser, TSettings, TCompanyInfo>
            (IDictionary dictionary)
            where TUser : class
            where TSettings : class
            where TCompanyInfo : class
        {
            var storedUser = dictionary["user"] as TUser;
            var storedSettings = dictionary["settings"] as TSettings;
            var storedCompanyInfo = dictionary["info"] as TCompanyInfo;
            var expire = dictionary["expire"] as DateTime?;

            if (storedUser == null
                || storedSettings == null
                || storedCompanyInfo == null
                || (expire == null
                || expire >= DateTime.UtcNow))
                return null;

            return new StoredProperties<TUser, TSettings, TCompanyInfo>()
            {
                CompanyInfo = storedCompanyInfo,
                Settings = storedSettings,
                User = storedUser
            };
        }


        protected virtual void StoreProperties<TUser, TSettings, TCompanyInfo>(StoredProperties<TUser, TSettings, TCompanyInfo> properties, IDictionary dictionary)
        {
            dictionary["user"] = properties.User;
            dictionary["settings"] = properties.Settings;
            dictionary["companyInfo"] = properties.CompanyInfo;
            dictionary["expire"] = DateTime.UtcNow.AddMinutes(5);
        }
    }
}