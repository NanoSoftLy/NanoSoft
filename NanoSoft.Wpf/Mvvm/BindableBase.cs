using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NanoSoft.Wpf.Mvvm
{
    public abstract class BindableBase : INotifyPropertyChanged
    {

        protected virtual void SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(member, value)) return;
            member = value;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // Notify Only , not overridable so it doesn't trigger validation or any other function
        protected void NotifyChange<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(member, value)) return;
            member = value;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private bool _isloading;
        // ReSharper disable once MemberCanBeProtected.Global
        public bool IsLoading
        {
            get => _isloading;
            private set => NotifyChange(ref _isloading, value);
        }

        protected void LoadingStarted() => IsLoading = true;
        protected void LoadingEnded() => IsLoading = false;
    }
}