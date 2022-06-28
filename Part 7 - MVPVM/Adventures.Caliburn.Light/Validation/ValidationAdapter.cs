using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Caliburn.Light
{
    /// <summary>
    /// A validation helper for implementing <see cref="INotifyDataErrorInfo"/>.
    /// </summary>
    public sealed class ValidationAdapter
    {
        private readonly Action<string> _onErrorsChanged;
        private readonly Dictionary<string, IReadOnlyCollection<string>> _errors = new Dictionary<string, IReadOnlyCollection<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAdapter"/> class.
        /// </summary>
        /// <param name="onErrorsChanged">Called when a property was validated.</param>
        public ValidationAdapter(Action<string> onErrorsChanged = null)
        {
            _onErrorsChanged = onErrorsChanged;
        }

        /// <summary>
        /// Gets or sets the used validator.
        /// </summary>
        public IValidator Validator { get; set; }

        /// <summary>
        /// Validates property <paramref name="propertyName"/> of the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True, if validation succeeded.</returns>
        public bool ValidateProperty(object instance, string propertyName)
        {
            var validator = Validator ?? NullValidator.Instance;
            var errors = validator.ValidateProperty(instance, propertyName);

            if (errors.Count == 0)
                _errors.Remove(propertyName);
            else
                _errors[propertyName] = errors;

            OnErrorsChanged(propertyName);
            return errors.Count == 0;
        }

        /// <summary>
        /// Validates all properties of the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>True, if validation succeeded.</returns>
        public bool Validate(object instance)
        {
            var validator = Validator ?? NullValidator.Instance;
            var errors = validator.Validate(instance);

            _errors.Clear();
            foreach (var error in errors)
            {
                _errors.Add(error.Key, error.Value);
            }

            OnErrorsChanged(string.Empty);
            return _errors.Count == 0;
        }

        /// <summary>
        /// Gets all validation errors of the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>List of validation errors.</returns>
        public IReadOnlyCollection<string> GetPropertyErrors(string propertyName)
        {
            if (_errors.TryGetValue(propertyName, out var errors))
                return errors;
            return Array.Empty<string>();
        }

        /// <summary>
        /// Determines whether the specified property has validation errors.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True, if the property has validation errors.</returns>
        public bool HasPropertyErrors(string propertyName)
        {
            return _errors.ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets a value indicating whether any property has validation errors.
        /// </summary>
        public bool HasErrors => _errors.Count > 0;

        private void OnErrorsChanged(string propertyName)
        {
            _onErrorsChanged?.Invoke(propertyName);
        }
    }
}
