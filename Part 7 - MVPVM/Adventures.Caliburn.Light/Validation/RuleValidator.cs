using System;
using System.Collections.Generic;

namespace Caliburn.Light
{
    /// <summary>
    /// Rule based validator.
    /// </summary>
    public sealed class RuleValidator : IValidator
    {
        private readonly Dictionary<string, List<ValidationRule>> _rules = new Dictionary<string, List<ValidationRule>>();

        /// <summary>
        /// Adds a rule to the validator.
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        public void AddRule(ValidationRule rule)
        {
            if (rule is null)
                throw new ArgumentNullException(nameof(rule));

            if (!_rules.TryGetValue(rule.PropertyName, out var current))
            {
                current = new List<ValidationRule>();
                _rules.Add(rule.PropertyName, current);
            }

            current.Add(rule);
        }

        /// <summary>
        /// Removes all rules for a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>true if rules where removed; otherwise, false.</returns>
        public bool RemoveRules(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return _rules.Remove(propertyName);
        }

        /// <summary>
        /// Determines whether this instance can validate the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True, if this instance can validate the property.</returns>
        public bool CanValidateProperty(string propertyName)
        {
            return _rules.ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets the name of all properties that can be validated by this instance.
        /// </summary>
        public IReadOnlyCollection<string> ValidatableProperties
        {
            get { return _rules.Keys; }
        }

        /// <summary>
        /// Applies the rules contained in this instance to <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the rules to.</param>
        /// <param name="propertyName">Name of the property we want to apply rules for.</param>
        /// <returns>A collection of errors.</returns>
        public IReadOnlyCollection<string> ValidateProperty(object obj, string propertyName)
        {
            if (!_rules.TryGetValue(propertyName, out var propertyRules))
                return new List<string>();

            return ValidateCore(propertyRules, obj);
        }

        /// <summary>
        /// Applies the rules contained in this instance to <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the rules to.</param>
        /// <returns>A collection of errors.</returns>
        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Validate(object obj)
        {
            var errors = new Dictionary<string, IReadOnlyCollection<string>>();

            foreach (var propertyRules in _rules)
            {
                var propertyErrors = ValidateCore(propertyRules.Value, obj);

                if (propertyErrors.Count > 0)
                {
                    errors[propertyRules.Key] = propertyErrors;
                }
            }

            return errors;
        }

        private static IReadOnlyCollection<string> ValidateCore(List<ValidationRule> rules, object obj)
        {
            var errors = new List<string>();

            foreach (var rule in rules)
            {
                var valid = rule.Apply(obj);

                if (!valid)
                {
                    errors.Add(rule.ErrorMessage);
                }
            }

            return errors;
        }
    }
}
