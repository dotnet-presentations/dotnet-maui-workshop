using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Caliburn.Light
{
    internal sealed class NullValidator : IValidator
    {
        public static readonly NullValidator Instance = new NullValidator();

        private static readonly ReadOnlyCollection<string> _emtpyList = new ReadOnlyCollection<string>(new List<string>());

        private static readonly ReadOnlyDictionary<string, IReadOnlyCollection<string>> _emptyDictionary =
            new ReadOnlyDictionary<string, IReadOnlyCollection<string>>(new Dictionary<string, IReadOnlyCollection<string>>());

        private NullValidator()
        {
        }

        public IReadOnlyCollection<string> ValidateProperty(object obj, string propertyName)
        {
            return _emtpyList;
        }

        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Validate(object obj)
        {
            return _emptyDictionary;
        }
    }
}
