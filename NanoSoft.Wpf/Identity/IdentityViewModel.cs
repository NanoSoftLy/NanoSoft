using NanoSoft.Attributes;
using NanoSoft.Identity;
using NanoSoft.Repository;
using NanoSoft.Resources;
using NanoSoft.Wpf.Mvvm;
using NanoSoft.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoSoft.Wpf.Identity
{
    public abstract class IdentityViewModel<TApp, TIdentityRepository, TIdentityUser> : NanoSoftBindableBase<TApp>
        where TIdentityUser : BaseIdentityUser
        where TIdentityRepository : IRepository<TIdentityUser>
    {
        private readonly IIdentityServices<TIdentityRepository, TIdentityUser> _identityService;

        public IdentityViewModel(IAppServices<TApp> appService, IIdentityServices<TIdentityRepository, TIdentityUser> identityService)
            : base(appService)
        {
            _identityService = identityService;
        }

        private RelayCommand _newCommand;
        public virtual RelayCommand NewCommand
        {
            get => _newCommand ?? new RelayCommand(NewAsync);
            protected set => _newCommand = value;
        }


        private RelayCommand<Guid> _createCommand;
        public virtual RelayCommand<Guid> CreateCommand
        {
            get => _createCommand ?? new RelayCommand<Guid>(CreateAsync, id => CanSubmit());
            protected set => _createCommand = value;
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

        public List<TIdentityUser> IdentityUsers { get; set; } = new List<TIdentityUser>();

        public virtual async Task LoadAsync()
        {
            try
            {
                LoadingStarted();

                using (var unitOfWork = _identityService.Initialize())
                {
                    IdentityUsers = await unitOfWork.IdentityUsers.GetAllAsync();
                }
            }
            catch (Exception e)
            {
                Services.HandleException(e);
            }
            finally
            {
                LoadingEnded();
                StartEvaluateErrors();
            }
        }

        public virtual async Task LoadAsync(Guid id)
        {
            try
            {
                LoadingStarted();

                TIdentityUser identityUser;
                using (var unitOfWork = _identityService.Initialize())
                {
                    identityUser = await unitOfWork.IdentityUsers.FindAsync(id);
                }

                if (identityUser == null)
                    return;

                LoginName = identityUser.Name;
            }
            catch (Exception e)
            {
                Services.HandleException(e);
            }
            finally
            {
                LoadingEnded();
                StartEvaluateErrors();
            }
        }

        private Task NewAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        private void Clear()
        {
            StopEvaluateErrors();
            LoginName = null;
            Password = null;
            ConfirmPassword = null;
            ClearErrors();
            StartEvaluateErrors();
        }

        private async Task CreateAsync(Guid id)
        {
            try
            {
                LoadingStarted();

                if (!IsValid())
                    return;

                var identityUser = NewIdentityUser(id);

                using (var unitOfWork = _identityService.Initialize())
                {
                    await unitOfWork.IdentityUsers.AddAsync(identityUser);

                    if (await unitOfWork.TryCompleteAsync())
                    {
                        Created(this, identityUser);
                        return;
                    }
                }

                using (var unitOfWork = _identityService.Initialize())
                {
                    identityUser.Name = Guid.NewGuid().ToString();

                    await unitOfWork.IdentityUsers.AddAsync(identityUser);

                    if (!await unitOfWork.TryCompleteAsync())
                    {
                        Failed(this, unitOfWork.ValidationState.Message);
                        return;
                    }

                    Created(this, identityUser);
                }
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
                using (var unitOfWork = _identityService.Initialize())
                {
                    var identityUser = await unitOfWork.IdentityUsers.FindAsync(id);

                    if (identityUser == null)
                    {
                        Failed(this, SharedMessages.ResponseState_NotFound);
                        return;
                    }

                    ModifyIdentityUser(identityUser);

                    if (!await unitOfWork.TryCompleteAsync())
                    {
                        Failed(this, unitOfWork.ValidationState.Message);
                        return;
                    }

                    Modified(this, identityUser);
                }
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
            using (var unitOfWork = _identityService.Initialize())
            {
                var identityUser = await unitOfWork.IdentityUsers.FindAsync(id);

                if (identityUser == null)
                {
                    Failed(this, SharedMessages.ResponseState_NotFound);
                    return;
                }

                await unitOfWork.IdentityUsers.RemoveAsync(identityUser);

                if (!await unitOfWork.TryCompleteAsync())
                {
                    Failed(this, unitOfWork.ValidationState.Message);
                    return;
                }

                Deleted(this, System.EventArgs.Empty);
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

        protected abstract TIdentityUser NewIdentityUser(Guid id);
        protected abstract void ModifyIdentityUser(TIdentityUser identityUser);

        public event EventHandler<string> Failed = delegate { };
        public event EventHandler<TIdentityUser> Created = delegate { };
        public event EventHandler<TIdentityUser> Modified = delegate { };
        public event EventHandler Deleted = delegate { };
    }
}
