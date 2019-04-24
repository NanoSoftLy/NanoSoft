using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NanoSoft.Wpf.Mvvm
{
    public abstract class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<string>> _addedErrors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        private void RaiseErrorChanged(string args)
        {
            ErrorsChanged(this, new DataErrorsChangedEventArgs(args));
        }

        public void ClearErrors()
        {
            StopEvaluateErrors();
            foreach (var error in _errors)
            {
                error.Value.Clear();
                OnPropertyChanged(error.Key);
                RaiseErrorChanged(error.Key);
            }

            foreach (var error in _addedErrors)
            {
                error.Value.Clear();
                OnPropertyChanged(error.Key);
                RaiseErrorChanged(error.Key);
            }

            _errors.Clear();
            _addedErrors.Clear();
            StartEvaluateErrors();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null)
                return null;

            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];

            return _addedErrors.ContainsKey(propertyName) ? _addedErrors[propertyName] : null;
        }

        public bool HasErrors => _errors.Count > 0 || _addedErrors.Count > 0;
        public bool HasFieldErrors => _errors.Count > 0;

        protected override void SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            base.SetProperty(ref member, value, propertyName);
            ValidateProperty(propertyName, value);
        }

        protected void StartEvaluateErrors() => _evaluateErrors = true;
        protected void StopEvaluateErrors() => _evaluateErrors = false;
        private bool _evaluateErrors = true;

        private void ValidateProperty<T>(string propertyName, T value)
        {
            if (!_evaluateErrors)
                return;

            var results = new List<ValidationResult>();

            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            Validator.TryValidateProperty(value, context, results);

            if (results.Any() && !_errors.ContainsKey(propertyName))
            {
                _errors.Add(propertyName, results.Select(e => e.ErrorMessage).ToList());
            }
            else if (!results.Any())
            {
                _errors.Remove(propertyName);
                _addedErrors.Remove(propertyName);
            }

            RaiseErrorChanged(propertyName);
        }

        public void AddErrors(Dictionary<string, List<string>> errors)
        {
            _addedErrors.Clear();
            foreach (var error in errors)
            {
                _addedErrors.Add(error.Key, error.Value);
                RaiseErrorChanged(error.Key);
            }
        }

        protected bool CanSubmit() => !HasFieldErrors && !IsLoading;
    }
}