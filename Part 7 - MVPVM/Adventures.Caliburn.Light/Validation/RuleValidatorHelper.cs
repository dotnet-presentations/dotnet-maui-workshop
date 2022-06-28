using System;
using System.Text.RegularExpressions;

namespace Caliburn.Light
{
    /// <summary>
    /// Extensions for <see cref="RuleValidator"/>.
    /// </summary>
    public static class RuleValidatorHelper
    {
        /// <summary>
        /// Adds a delegate validation rule.
        /// </summary>
        /// <typeparam name="T">The type of the object the rule applies to.</typeparam>
        /// <param name="ruleValidator">The rule validator.</param>
        /// <param name="propertyName">The name of the property this instance applies to.</param>
        /// <param name="validateProperty">The validation delegate.</param>
        /// <param name="errorMessage">The error message if the rules fails.</param>
        public static void AddDelegateRule<T>(this RuleValidator ruleValidator, string propertyName, Func<T, bool> validateProperty, string errorMessage)
        {
            if (ruleValidator is null)
                throw new ArgumentNullException(nameof(ruleValidator));
            if (validateProperty is null)
                throw new ArgumentNullException(nameof(validateProperty));

            var rule = new DelegateValidationRule(propertyName, obj => validateProperty((T)obj), errorMessage);
            ruleValidator.AddRule(rule);
        }

        /// <summary>
        /// Adds a range validation rule.
        /// </summary>
        /// <typeparam name="T">The type of the object the rule applies to.</typeparam>
        /// <typeparam name="TProperty">The type of the property the rule applies to.</typeparam>
        /// <param name="ruleValidator">The rule validator.</param>
        /// <param name="propertyName">The name of the property this instance applies to.</param>
        /// <param name="getPropertyValue">Gets the value of the property.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="errorMessage">The error message.</param>
        public static void AddRangeRule<T, TProperty>(this RuleValidator ruleValidator, string propertyName, Func<T, TProperty> getPropertyValue, TProperty minimum, TProperty maximum, string errorMessage)
            where TProperty : IComparable<TProperty>
        {
            if (ruleValidator is null)
                throw new ArgumentNullException(nameof(ruleValidator));
            if (getPropertyValue is null)
                throw new ArgumentNullException(nameof(getPropertyValue));
            if (minimum.CompareTo(maximum) > 0)
                throw new ArgumentOutOfRangeException(nameof(maximum));

            bool validateProperty(object obj)
            {
                var value = getPropertyValue((T)obj);
                return value.CompareTo(minimum) >= 0 && value.CompareTo(maximum) <= 0;
            }

            var rule = new DelegateValidationRule(propertyName, validateProperty, errorMessage);
            ruleValidator.AddRule(rule);
        }

        /// <summary>
        /// Adds a <see cref="Regex"/> validation rule.
        /// </summary>
        /// <typeparam name="T">The type of the object the rule applies to.</typeparam>
        /// <param name="ruleValidator">The rule validator.</param>
        /// <param name="propertyName">The name of the property this instance applies to.</param>
        /// <param name="getPropertyValue">Gets the value of the property.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="errorMessage">The error message.</param>
        public static void AddRegexRule<T>(this RuleValidator ruleValidator, string propertyName, Func<T, string> getPropertyValue, string pattern, string errorMessage)
        {
            if (ruleValidator is null)
                throw new ArgumentNullException(nameof(ruleValidator));
            if (getPropertyValue is null)
                throw new ArgumentNullException(nameof(getPropertyValue));
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentNullException(nameof(pattern));

            var regex = new Regex(pattern);
            var rule = new DelegateValidationRule(propertyName, obj => regex.IsMatch(getPropertyValue((T)obj)), errorMessage);
            ruleValidator.AddRule(rule);
        }

        /// <summary>
        /// Adds a <see cref="string.Length"/> validation rule.
        /// </summary>
        /// <typeparam name="T">The type of the object the rule applies to.</typeparam>
        /// <param name="ruleValidator">The rule validator.</param>
        /// <param name="propertyName">The name of the property this instance applies to.</param>
        /// <param name="getPropertyValue">Gets the value of the property.</param>
        /// <param name="minimumLength">The minimum length.</param>
        /// <param name="maximumLength">The maximum length.</param>
        /// <param name="errorMessage">The error message.</param>
        public static void AddStringLengthRule<T>(this RuleValidator ruleValidator, string propertyName, Func<T, string> getPropertyValue, int minimumLength, int maximumLength, string errorMessage)
        {
            if (ruleValidator is null)
                throw new ArgumentNullException(nameof(ruleValidator));
            if (getPropertyValue is null)
                throw new ArgumentNullException(nameof(getPropertyValue));
            if (minimumLength < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            if (maximumLength < 0 || minimumLength > maximumLength)
                throw new ArgumentOutOfRangeException(nameof(maximumLength));

            bool validateProperty(object obj)
            {
                var value = getPropertyValue((T)obj);
                var length = string.IsNullOrEmpty(value) ? 0 : GetTrimmedLength(value);
                return length >= minimumLength && length <= maximumLength;
            }

            var rule = new DelegateValidationRule(propertyName, validateProperty, errorMessage);
            ruleValidator.AddRule(rule);
        }

        private static int GetTrimmedLength(string value)
        {
            //end will point to the first non-trimmed character on the right
            //start will point to the first non-trimmed character on the Left
            var end = value.Length - 1;
            var start = 0;

            for (; start < value.Length; start++)
            {
                if (!char.IsWhiteSpace(value[start])) break;
            }

            for (; end >= start; end--)
            {
                if (!char.IsWhiteSpace(value[end])) break;
            }

            return end - start + 1;
        }
    }
}
