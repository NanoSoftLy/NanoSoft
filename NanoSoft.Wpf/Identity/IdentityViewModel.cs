using NanoSoft.Attributes;
using NanoSoft.Identity;
using NanoSoft.Resources;
using NanoSoft.Wpf.Mvvm;
using NanoSoft.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoSoft.Wpf.Identity
{
    public abstract class IdentityBaseViewModel<TApp, TIdentityServiceProvider, TIdentityService, TIdentityResult> : NanoSoftBindableBase<TApp>
        where TIdentityService : IIdentityService<TIdentityResult>
        where TIdentityServiceProvider : IIdentityServiceProvider<TIdentityService>
    {
        private readonly TIdentityServiceProvider _identityServiceProvider;
        public IdentityBaseViewModel(IAppServices<TApp> appService, TIdentityServiceProvider identityServiceProvider)
            : base(appService)
        {
            _identityServiceProvider = identityServiceProvider;
        }

        private RelayCommand _newCommand;
        public virtual RelayCommand NewCommand
        {
            get => _newCommand ?? new RelayCommand(NewAsync);
            protected set => _newCommand = value;
        }

        private RelayCommand<Guid> _editCommand;
        public virtual RelayCommand<Guid> EditCommand
        {
            get => _editCommand ?? new RelayCommand<Guid>(EditAsync, id => CanSubmit());
            protected set => _editCommand = value;
        }

        private RelayCommand<Guid> _deleteCommand;
        public virtual RelayCommand<Guid> DeleteCommand
        {
            get => _deleteCommand ?? new RelayCommand<Guid>(DeleteAsync);
            protected set => _deleteCommand = value;
        }

        private string _loginName;
        [IsRequired, SharedTitle(nameof(SharedTitles.UserName))]
        public string LoginName
        {
            get => _loginName;
            set => SetProperty(ref _loginName, value);
        }

        private string _password;
        [IsRequired, SharedTitle(nameof(SharedTitles.Password))]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        [IsRequired, SharedTitle(nameof(SharedTitles.ConfirmPassword))]
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public List<KeyValuePair> IdentityUsers { get; set; } = new List<KeyValuePair>();

        public virtual async Task LoadAsync()
        {
            try
            {
                LoadingStarted();

                Response<List<KeyValuePair>> response;
                using (var service = _identityServiceProvider.Initialize())
                {
                    response = await service.GetAllAsync();
                }

                if (!response.IsValid)
                {
                    Failed(this, response.Message);
                    return;
                }

                IdentityUsers = response.Model;
            }
            catch (Exception e)
            {
                Services.HandleException(e);
            }
            finally
            {
                LoadingEnded();
            }
        }

        public virtual async Task LoadAsync(Guid id)
        {
            try
            {
                LoadingStarted();

                Response<string> response;
                using (var service = _identityServiceProvider.Initialize())
                {
                    response = await service.GetIdentityNameByIdAsync(id);
                }

                if (!response.IsValid)
                {
                    Failed(this, response.Message);
                    return;
                }

                LoginName = response.Model;
            }
            catch (Exception e)
            {
                Services.HandleException(e);
            }
            finally
            {
                LoadingEnded();
            }
        }

        private Task NewAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        private void Clear()
        {
            LoginName = null;
            Password = null;
            ConfirmPassword = null;
            ClearErrors();
        }

        public virtual async Task CreateAsync(Guid id)
        {
            try
            {
                LoadingStarted();

                Response<string> response;
                using (var services = _identityServiceProvider.Initialize())
                {
                    response = await services.CreateIdentityAsync(id, LoginName);
                }

                if (!response.IsValid)
                {
                    Failed(this, response.Message);
                    return;
                }

                Created(this, response.Model);
            }
            catch (Exception e)
            {
                Services.HandleException(e);
            }
            finally
            {
                LoadingEnded();
            }
        }

        private async Task EditAsync(Guid id)
        {
            if (!IsValid())
                return;

            try
            {
                LoadingStarted();

                Response response;
                using (var services = _identityServiceProvider.Initialize())
                {
                    response = await services.TryUpdateIdentityNameAsync(id, LoginName, Password);
                }

                if (!response.IsValid)
                {
                    Failed(this, response.Message);
                    return;
                }

                Modified(this, System.EventArgs.Empty);
            }
            catch (Exception e)
            {
                Services.HandleException(e);
            }
            finally
            {
                LoadingEnded();
            }
        }

        private async Task DeleteAsync(Guid id)
        {

            try
            {
                LoadingStarted();

                Response response;
                using (var services = _identityServiceProvider.Initialize())
                {
                    response = await services.DeleteIdentityAsync(id);
                }

                if (!response.IsValid)
                {
                    Failed(this, response.Message);
                    return;
                }

                Deleted(this, System.EventArgs.Empty);
                Clear();
            }
            catch (Exception e)
            {
                Services.HandleException(e);
            }
            finally
            {
                LoadingEnded();
            }
        }

        protected virtual bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Failed(this, string.Format(SharedMessages.IsRequired, SharedTitles.Password));
                return false;
            }

            if (Password != ConfirmPassword)
            {
                Failed(this, SharedMessages.PasswordNotMatch);
                return false;
            }

            return true;
        }

        public event EventHandler<string> Failed = delegate { };
        public event EventHandler<string> Created = delegate { };
        public event EventHandler Modified = delegate { };
        public event EventHandler Deleted = delegate { };
    }
}
