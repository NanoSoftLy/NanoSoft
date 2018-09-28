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

        public Guid Id { get; set; }

        private RelayCommand _newCommand;
        public virtual RelayCommand NewCommand
        {
            get => _newCommand ?? new RelayCommand(NewAsync);
            protected set => _newCommand = value;
        }


        private RelayCommand _saveCommand;
        public virtual RelayCommand SaveCommand
        {
            get => _saveCommand ?? new RelayCommand(SaveAsync, CanSubmit);
            protected set => _saveCommand = value;
        }

        private RelayCommand _deleteCommand;
        public virtual RelayCommand DeleteCommand
        {
            get => _deleteCommand ?? new RelayCommand(DeleteAsync);
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

        private Task NewAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        private void Clear()
        {
            StopEvaluateErrors();
            Id = default(Guid);
            LoginName = null;
            Password = null;
            ConfirmPassword = null;
            ClearErrors();
            StartEvaluateErrors();
        }

        private async Task SaveAsync()
        {
            try
            {
                LoadingStarted();

                if (Id == default(Guid))
                    await CreateAsync();
                else
                    await EditAsync();
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

        private async Task CreateAsync()
        {
            var identityUser = NewIdentityUser();

            using (var unitOfWork = _identityService.Initialize())
            {
                await unitOfWork.IdentityUsers.AddAsync(identityUser);

                if (!await unitOfWork.TryCompleteAsync())
                {
                    Failed(this, unitOfWork.ValidationState.Message);
                    return;
                }

                Id = identityUser.Id;
                Succeed(this, System.EventArgs.Empty);
            }
        }

        private async Task EditAsync()
        {
            using (var unitOfWork = _identityService.Initialize())
            {
                var identityUser = await unitOfWork.IdentityUsers.FindAsync(Id);

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

                Succeed(this, System.EventArgs.Empty);
            }
        }

        private async Task DeleteAsync()
        {
            using (var unitOfWork = _identityService.Initialize())
            {
                var identityUser = await unitOfWork.IdentityUsers.FindAsync(Id);

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

                Succeed(this, System.EventArgs.Empty);
            }
        }

        protected abstract TIdentityUser NewIdentityUser();
        protected abstract void ModifyIdentityUser(TIdentityUser identityUser);

        public event EventHandler<string> Failed = delegate { };
        public event EventHandler Succeed = delegate { };
    }
}
