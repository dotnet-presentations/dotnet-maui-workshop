using System;

namespace Caliburn.Light
{
    /// <summary>
    /// Provides a way to create a custom rule in order to check the validity of an object.
    /// </summary>
    public abstract class ValidationRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRule"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property this instance applies to.</param>
        /// <param name="errorMessage">The error message if the rules fails.</param>
        protected ValidationRule(string propertyName, string errorMessage)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));

            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the name of the property this instance applies to.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the error message if the rules fails.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Applies the rule to the specified object.
        /// </summary>
        /// <param name="obj">The object to apply the rule to.</param>
        /// <returns>
        /// <c>true</c> if the object satisfies the rule, otherwise <c>false</c>.
        /// </returns>
        public abstract bool Apply(object obj);
    }
}
