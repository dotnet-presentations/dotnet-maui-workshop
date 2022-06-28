using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// A base implementation that requires activation and may prevent closing.
    /// </summary>
    public class Screen : ViewAware, IActivatable, ICloseGuard
    {
        private bool _isActive;
        private bool _isInitialized;

        /// <summary>
        /// Indicates whether or not this instance is currently active.
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            private set { SetProperty(ref _isActive, value); }
        }

        /// <summary>
        /// Indicates whether or not this instance is currently initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            private set { SetProperty(ref _isInitialized, value); }
        }

        /// <summary>
        /// Raised after activation occurs.
        /// </summary>
        public event EventHandler<ActivationEventArgs> Activated;

        /// <summary>
        /// Raised before deactivation.
        /// </summary>
        public event EventHandler<DeactivationEventArgs> Deactivating;

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        public event EventHandler<DeactivationEventArgs> Deactivated;

        private void OnActivated(bool wasInitialized)
        {
            Activated?.Invoke(this, new ActivationEventArgs(wasInitialized));
        }

        private void OnDeactivating(bool wasClosed)
        {
            Deactivating?.Invoke(this, new DeactivationEventArgs(wasClosed));
        }

        private void OnDeactivated(bool wasClosed)
        {
            Deactivated?.Invoke(this, new DeactivationEventArgs(wasClosed));
        }

        async Task IActivatable.ActivateAsync()
        {
            if (IsActive) return;

            var initialized = false;
            if (!IsInitialized)
            {
                IsInitialized = initialized = true;
                await OnInitializeAsync();
            }

            IsActive = true;
            Trace.TraceInformation("Activating {0}.", this);
            await OnActivateAsync();

            OnActivated(initialized);
        }

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected virtual Task OnInitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when activating.
        /// </summary>
        protected virtual Task OnActivateAsync()
        {
            return Task.CompletedTask;
        }

        async Task IActivatable.DeactivateAsync(bool close)
        {
            if (IsActive || (IsInitialized && close))
            {
                OnDeactivating(close);

                IsActive = false;
                Trace.TraceInformation("Deactivating {0} (close={1}).", this, close);
                await OnDeactivateAsync(close);

                OnDeactivated(close);
            }
        }

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name = "close">Indicates whether this instance will be closed.</param>
        protected virtual Task OnDeactivateAsync(bool close)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <returns>A task containing the result of the close check.</returns>
        public virtual Task<bool> CanCloseAsync()
        {
            return TaskHelper.TrueTask;
        }

        /// <summary>
        /// Tries to close this instance by asking its Parent to initiate shutdown or by asking its corresponding view to close.
        /// </summary>
        public virtual async Task TryCloseAsync()
        {
            if (this is IChild child && child.Parent is IConductor conductor)
            {
                await conductor.DeactivateItemAsync(this, true);
                return;
            }

            foreach (var entry in Views)
            {
                if (await ViewHelper.TryCloseAsync(entry.Value.Target))
                    return;
            }

            Trace.TraceInformation("TryClose {0} requires an IChild.Parent of IConductor or a top-level view.", this);
        }
    }
}
