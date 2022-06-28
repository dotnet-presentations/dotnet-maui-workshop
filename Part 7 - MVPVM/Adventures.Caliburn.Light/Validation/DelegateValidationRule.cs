using System;

namespace Caliburn.Light
{
    internal sealed class DelegateValidationRule : ValidationRule
    {
        private readonly Func<object, bool> _validateProperty;

        public DelegateValidationRule(string propertyName, Func<object, bool> validateProperty, string errorMessage)
            : base(propertyName, errorMessage)
        {
            _validateProperty = validateProperty;
        }

        public override bool Apply(object obj)
        {
            return _validateProperty(obj);
        }
    }
}
