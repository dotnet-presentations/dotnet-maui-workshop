using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caliburn.Light
{
    /// <summary>
    /// An asynchronous delegate command that can be data-bound.
    /// </summary>
    public sealed class AsyncDelegateCommand : AsyncCommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly string[] _propertyNames;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Holds the weak event registration alive")]
        private readonly IDisposable _propertyChangedRegistration;

        /// <summary>
        /// Initializes a new instance of <see cref="AsyncDelegateCommand"/>.
        /// </summary>
        /// <param name="execute">The execute function.</param>
        /// <param name="canExecute">The canExecute function.</param>
        public AsyncDelegateCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AsyncDelegateCommand"/>.
        /// </summary>
        /// <param name="execute">The execute function.</param>
        /// <param name="canExecute">The canExecute function.</param>
        /// <param name="target">The object to observe for change notifications.</param>
        /// <param name="propertyNames">The property names.</param>
        public AsyncDelegateCommand(Func<Task> execute, Func<bool> canExecute, INotifyPropertyChanged target, params string[] propertyNames)
        {
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));
            if (canExecute is null)
                throw new ArgumentNullException(nameof(canExecute));
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (propertyNames is null || propertyNames.Length == 0)
                throw new ArgumentNullException(nameof(propertyNames));

            _execute = execute;
            _canExecute = canExecute;
            _propertyNames = propertyNames;
            _propertyChangedRegistration = target.RegisterPropertyChangedWeak(this, (t, _, e) => t.OnPropertyChanged(e));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || Array.IndexOf(_propertyNames, e.PropertyName) >= 0)
                OnCanExecuteChanged();
        }

        /// <summary>
        /// The implementation of <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        protected override bool CanExecuteCore(object parameter)
        {
            if (IsExecuting) return false;
            if (_canExecute is null) return true;
            return _canExecute();
        }

        /// <summary>
        /// The implementation of <see cref="ICommand.Execute(object)"/>.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task ExecuteAsync(object parameter)
        {
            return _execute();
        }
    }

    /// <summary>
    /// An asynchronous delegate command that can be data-bound.
    /// </summary>
    /// <typeparam name="TParameter">The type of the command parameter.</typeparam>
    public sealed class AsyncDelegateCommand<TParameter> : AsyncCommand
    {
        private readonly Func<object, TParameter> _coerceParameter;
        private readonly Func<TParameter, Task> _execute;
        private readonly Func<TParameter, bool> _canExecute;
        private readonly string[] _propertyNames;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Holds the weak event registration alive")]
        private readonly IDisposable _propertyChangedRegistration;

        /// <summary>
        /// Initializes a new instance of <see cref="AsyncDelegateCommand&lt;TParameter&gt;"/>.
        /// </summary>
        /// <param name="coerceParameter">The function to coerce the provided value to <typeparamref name="TParameter"/>.</param>
        /// <param name="execute">The execute function.</param>
        /// <param name="canExecute">The canExecute function.</param>
        public AsyncDelegateCommand(Func<object, TParameter> coerceParameter, Func<TParameter, Task> execute, Func<TParameter, bool> canExecute = null)
        {
            if (coerceParameter is null)
                throw new ArgumentNullException(nameof(coerceParameter));
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));

            _coerceParameter = coerceParameter;
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AsyncDelegateCommand&lt;TParameter&gt;"/>.
        /// </summary>
        /// <param name="coerceParameter">The function to coerce the provided value to <typeparamref name="TParameter"/>.</param>
        /// <param name="execute">The execute function.</param>
        /// <param name="canExecute">The canExecute function.</param>
        /// <param name="target">The object to observe for change notifications.</param>
        /// <param name="propertyNames">The property names.</param>
        public AsyncDelegateCommand(Func<object, TParameter> coerceParameter, Func<TParameter, Task> execute, Func<TParameter, bool> canExecute,
            INotifyPropertyChanged target, params string[] propertyNames)
        {
            if (coerceParameter is null)
                throw new ArgumentNullException(nameof(coerceParameter));
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));
            if (canExecute is null)
                throw new ArgumentNullException(nameof(canExecute));
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (propertyNames is null || propertyNames.Length == 0)
                throw new ArgumentNullException(nameof(propertyNames));

            _coerceParameter = coerceParameter;
            _execute = execute;
            _canExecute = canExecute;
            _propertyNames = propertyNames;
            _propertyChangedRegistration = target.RegisterPropertyChangedWeak(this, (t, _, e) => t.OnPropertyChanged(e));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || Array.IndexOf(_propertyNames, e.PropertyName) >= 0)
                OnCanExecuteChanged();
        }

        /// <summary>
        /// The implementation of <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        protected override bool CanExecuteCore(object parameter)
        {
            if (IsExecuting) return false;
            if (_canExecute is null) return true;
            var value = _coerceParameter(parameter);
            return _canExecute(value);
        }

        /// <summary>
        /// The implementation of <see cref="ICommand.Execute(object)"/>.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task ExecuteAsync(object parameter)
        {
            var value = _coerceParameter(parameter);
            return _execute(value);
        }
    }
}
