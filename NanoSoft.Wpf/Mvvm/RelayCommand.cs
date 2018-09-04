using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NanoSoft.Wpf.Mvvm
{
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public RelayCommand(Func<Task> executeAsync)
        {
            _executeAsync = executeAsync;
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public async void Execute(object parameter)
        {
            if (_executeAsync != null)
            {
                await _executeAsync();
                return;
            }

            _execute();
        }

        public Task ExecuteAsync() => _executeAsync();
    }

    public class RelayCommand<T> : ICommand
    {
        #region Fields

        private readonly Func<T, Task> _executeAsync;
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Func<T, Task> executeAsync, Predicate<T> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        ///<summary>
        ///Defines the method that determines whether the command can execute in its current state.
        ///</summary>
        ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        ///<returns>
        ///true if this command can be executed; otherwise, false.
        ///</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        ///<summary>
        ///Occurs when changes occur that affect whether or not the command should execute.
        ///</summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        ///<summary>
        ///Defines the method to be called when the command is invoked.
        ///</summary>
        ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public async void Execute(object parameter)
        {
            if (_executeAsync != null)
            {
                await _executeAsync((T)parameter);
                return;
            }

            _execute((T)parameter);
        }

        public Task ExecuteAsync(object parameter) => _executeAsync((T)parameter);
        #endregion
    }

}