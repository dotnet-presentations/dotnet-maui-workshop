using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// Builds a delegate <see cref="System.Windows.Input.ICommand"/> in a strongly typed fashion.
    /// </summary>
    public static class DelegateCommandBuilder
    {
        /// <summary>
        /// Builds a command with no parameters.
        /// </summary>
        /// <returns>The command builder.</returns>
        public static DCB_nP NoParameter() => new DCB_nP();

        /// <summary>
        /// Builds a command with a parameter.
        /// </summary>
        /// <typeparam name="TParameter">The type of the command parameter.</typeparam>
        /// <param name="coerceParameter">The function to coerce the provided value to <typeparamref name="TParameter"/>.</param>
        /// <returns>The command builder.</returns>
        public static DCB_wP<TParameter> WithParameter<TParameter>(Func<object, TParameter> coerceParameter = null) => new DCB_wP<TParameter>(coerceParameter);
    }

    /// <summary>
    /// Builds a <see cref="DelegateCommand"/> or <see cref="AsyncDelegateCommand"/> in a strongly typed fashion.
    /// </summary>
    public sealed class DCB_nP
    {
        /// <summary>
        /// Sets the command execute function.
        /// </summary>
        /// <param name="execute">The execute function.</param>
        /// <returns>The command builder.</returns>
        public DCB_nPs OnExecute(Action execute) => new DCB_nPs(execute);

        /// <summary>
        /// Sets the command execute function.
        /// </summary>
        /// <param name="execute">The execute function.</param>
        /// <returns>The command builder.</returns>
        public DCB_nPa OnExecute(Func<Task> execute) => new DCB_nPa(execute);
    }

    /// <summary>
    /// Builds a <see cref="DelegateCommand"/> in a strongly typed fashion.
    /// </summary>
    public sealed class DCB_nPs
    {
        private readonly Action _execute;
        private Func<bool> _canExecute;
        private INotifyPropertyChanged _target;
        private string[] _propertyNames;

        internal DCB_nPs(Action execute)
        {
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
        }

        /// <summary>
        /// Sets the command canExecute function.
        /// </summary>
        /// <param name="canExecute">The canExecute function.</param>
        /// <returns>The command builder.</returns>
        public DCB_nPs OnCanExecute(Func<bool> canExecute)
        {
            if (canExecute is null)
                throw new ArgumentNullException(nameof(canExecute));

            _canExecute = canExecute;
            return this;
        }

        /// <summary>
        /// Sets the properties to listen for change notifications.
        /// </summary>
        /// <param name="target">The object to observe.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <returns>The command builder.</returns>
        public DCB_nPs Observe(INotifyPropertyChanged target, params string[] propertyNames)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (propertyNames is null || propertyNames.Length == 0)
                throw new ArgumentNullException(nameof(propertyNames));

            _target = target;
            _propertyNames = propertyNames;
            return this;
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <returns>The newly build command.</returns>
        public DelegateCommand Build()
        {
            return (_target is null) ?
                new DelegateCommand(_execute, _canExecute) :
                new DelegateCommand(_execute, _canExecute, _target, _propertyNames);
        }
    }

    /// <summary>
    /// Builds an <see cref="AsyncDelegateCommand"/> in a strongly typed fashion.
    /// </summary>
    public sealed class DCB_nPa
    {
        private readonly Func<Task> _execute;
        private Func<bool> _canExecute;
        private INotifyPropertyChanged _target;
        private string[] _propertyNames;

        internal DCB_nPa(Func<Task> execute)
        {
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
        }

        /// <summary>
        /// Sets the command canExecute function.
        /// </summary>
        /// <param name="canExecute">The canExecute function.</param>
        /// <returns>The command builder.</returns>
        public DCB_nPa OnCanExecute(Func<bool> canExecute)
        {
            if (canExecute is null)
                throw new ArgumentNullException(nameof(canExecute));

            _canExecute = canExecute;
            return this;
        }

        /// <summary>
        /// Sets the properties to listen for change notifications.
        /// </summary>
        /// <param name="target">The object to observe.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <returns>The command builder.</returns>
        public DCB_nPa Observe(INotifyPropertyChanged target, params string[] propertyNames)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (propertyNames is null || propertyNames.Length == 0)
                throw new ArgumentNullException(nameof(propertyNames));

            _target = target;
            _propertyNames = propertyNames;
            return this;
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <returns>The newly build command.</returns>
        public AsyncDelegateCommand Build()
        {
            return (_target is null) ?
                new AsyncDelegateCommand(_execute, _canExecute) :
                new AsyncDelegateCommand(_execute, _canExecute, _target, _propertyNames);
        }
    }

    /// <summary>
    /// Builds a <see cref="DelegateCommand&lt;TParameter&gt;"/> or <see cref="AsyncDelegateCommand&lt;TParameter&gt;"/> in a strongly typed fashion.
    /// </summary>
    /// <typeparam name="TParameter">The type of the command parameter.</typeparam>
    public sealed class DCB_wP<TParameter>
    {
        private readonly Func<object, TParameter> _coerceParameter;

        internal DCB_wP(Func<object, TParameter> coerceParameter)
        {
            _coerceParameter = coerceParameter ?? CoerceParameter<TParameter>.Default;
        }

        /// <summary>
        /// Sets the command execute function.
        /// </summary>
        /// <param name="execute">The execute function.</param>
        /// <returns>The command builder.</returns>
        public DCB_wPs<TParameter> OnExecute(Action<TParameter> execute) => new DCB_wPs<TParameter>(_coerceParameter, execute);

        /// <summary>
        /// Sets the command execute function.
        /// </summary>
        /// <param name="execute">The execute function.</param>
        /// <returns>The command builder.</returns>
        public DCB_wPa<TParameter> OnExecute(Func<TParameter, Task> execute) => new DCB_wPa<TParameter>(_coerceParameter, execute);
    }

    /// <summary>
    /// Builds a <see cref="DelegateCommand&lt;TParameter&gt;"/> in a strongly typed fashion.
    /// </summary>
    /// <typeparam name="TParameter">The type of the command parameter.</typeparam>
    public sealed class DCB_wPs<TParameter>
    {
        private readonly Func<object, TParameter> _coerceParameter;
        private readonly Action<TParameter> _execute;
        private Func<TParameter, bool> _canExecute;
        private INotifyPropertyChanged _target;
        private string[] _propertyNames;

        internal DCB_wPs(Func<object, TParameter> coerceParameter, Action<TParameter> execute)
        {
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));

            _coerceParameter = coerceParameter;
            _execute = execute;
        }

        /// <summary>
        /// Sets the command canExecute function.
        /// </summary>
        /// <param name="canExecute">The canExecute function.</param>
        /// <returns>The command builder.</returns>
        public DCB_wPs<TParameter> OnCanExecute(Func<TParameter, bool> canExecute)
        {
            if (canExecute is null)
                throw new ArgumentNullException(nameof(canExecute));

            _canExecute = canExecute;
            return this;
        }

        /// <summary>
        /// Sets the properties to listen for change notifications.
        /// </summary>
        /// <param name="target">The object to observe.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <returns>The command builder.</returns>
        public DCB_wPs<TParameter> Observe(INotifyPropertyChanged target, params string[] propertyNames)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (propertyNames is null || propertyNames.Length == 0)
                throw new ArgumentNullException(nameof(propertyNames));

            _target = target;
            _propertyNames = propertyNames;
            return this;
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <returns>The newly build command.</returns>
        public DelegateCommand<TParameter> Build()
        {
            return (_target is null) ?
                new DelegateCommand<TParameter>(_coerceParameter, _execute, _canExecute) :
                new DelegateCommand<TParameter>(_coerceParameter, _execute, _canExecute, _target, _propertyNames);
        }
    }

    /// <summary>
    /// Builds an <see cref="AsyncDelegateCommand&lt;TParameter&gt;"/> in a strongly typed fashion.
    /// </summary>
    /// <typeparam name="TParameter">The type of the command parameter.</typeparam>
    public sealed class DCB_wPa<TParameter>
    {
        private readonly Func<object, TParameter> _coerceParameter;
        private readonly Func<TParameter, Task> _execute;
        private Func<TParameter, bool> _canExecute;
        private INotifyPropertyChanged _target;
        private string[] _propertyNames;

        internal DCB_wPa(Func<object, TParameter> coerceParameter, Func<TParameter, Task> execute)
        {
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));

            _coerceParameter = coerceParameter;
            _execute = execute;
        }

        /// <summary>
        /// Sets the command canExecute function.
        /// </summary>
        /// <param name="canExecute">The canExecute function.</param>
        /// <returns>The command builder.</returns>
        public DCB_wPa<TParameter> OnCanExecute(Func<TParameter, bool> canExecute)
        {
            if (canExecute is null)
                throw new ArgumentNullException(nameof(canExecute));

            _canExecute = canExecute;
            return this;
        }

        /// <summary>
        /// Sets the properties to listen for change notifications.
        /// </summary>
        /// <param name="target">The object to observe.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <returns>The command builder.</returns>
        public DCB_wPa<TParameter> Observe(INotifyPropertyChanged target, params string[] propertyNames)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (propertyNames is null || propertyNames.Length == 0)
                throw new ArgumentNullException(nameof(propertyNames));

            _target = target;
            _propertyNames = propertyNames;
            return this;
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <returns>The newly build command.</returns>
        public AsyncDelegateCommand<TParameter> Build()
        {
            return (_target is null) ?
                new AsyncDelegateCommand<TParameter>(_coerceParameter, _execute, _canExecute) :
                new AsyncDelegateCommand<TParameter>(_coerceParameter, _execute, _canExecute, _target, _propertyNames);
        }
    }
}
