using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Caliburn.Light
{
    /// <summary>
    /// Defines a command that can be data-bound.
    /// </summary>
    public abstract class BindableCommand : ICommand, INotifyPropertyChanged
    {
        private bool _isExecutableNeedsInvalidation = true;
        private bool _isExecutable;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        protected void OnCanExecuteChanged()
        {
            _isExecutableNeedsInvalidation = true;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(IsExecutable));
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The implementation of <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        protected virtual bool CanExecuteCore(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            var result = CanExecuteCore(parameter);

            _isExecutable = result;
            _isExecutableNeedsInvalidation = false;

            return result;
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        public bool IsExecutable
        {
            get { return (_isExecutableNeedsInvalidation) ? CanExecute(null) : _isExecutable; }
        }

        /// <summary>
        /// Called when the command is invoked.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// Defines the method to be called when using x:Bind event binding.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="eventArgs">Event data for the event.</param>
        public void OnEvent(object sender, object eventArgs)
        {
            var parameter = DetermineParameter(sender, eventArgs);
            Execute(parameter);
        }

        private static object DetermineParameter(object sender, object eventArgs)
        {
            var resolvedParameter = ViewHelper.GetCommandParameter(sender);

            if (resolvedParameter is ISpecialValue specialValue)
            {
                var context = new CommandExecutionContext
                {
                    Source = sender,
                    EventArgs = eventArgs,
                };
                resolvedParameter = specialValue.Resolve(context);
            }

            return resolvedParameter ?? eventArgs;
        }

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/> so every command invoker can re-query to check if the command can execute.
        /// </summary>
        /// <remarks>Note that this will trigger the execution of <see cref="CanExecute"/> once for each invoker.</remarks>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }
    }
}
