using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caliburn.Light
{
    /// <summary>
    /// An asynchronous command that can be data-bound.
    /// </summary>
    public abstract class AsyncCommand : BindableCommand
    {
        private bool _isExecuting;

        /// <summary>
        /// The implementation of <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        protected override bool CanExecuteCore(object parameter)
        {
            return !IsExecuting;
        }

        /// <summary>
        /// Called when the command is invoked.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public sealed override async void Execute(object parameter)
        {
            SetIsExecuting(true);
            try
            {
                var task = ExecuteAsync(parameter);
                if (!task.IsCompleted)
                    Executing?.Invoke(null, new TaskEventArgs(task));
                await task;
            }
            finally
            {
                SetIsExecuting(false);
            }
        }

        /// <summary>
        /// Occurs when <see cref="ExecuteAsync(object)"/> is invoked and the operation has not completed synchronously.
        /// </summary>
        public static event EventHandler<TaskEventArgs> Executing;

        private void SetIsExecuting(bool value)
        {
            _isExecuting = value;
            OnCanExecuteChanged();
            OnPropertyChanged(nameof(IsExecuting));
        }

        /// <summary>
        /// The implementation of <see cref="ICommand.Execute(object)"/>.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected abstract Task ExecuteAsync(object parameter);

        /// <summary>
        /// Determines whether the command is currently executing.
        /// </summary>
        public bool IsExecuting
        {
            get { return _isExecuting; }
        }
    }
}
