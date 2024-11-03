using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NBrzId.Validator.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class BrzIdentifierMockValidator : IBrzIdentifierValidator<BrzMockIdentifier>
    {
        private const string NullStringPlaceholder = "0e1cbf9693c0495eb37e8309c46bd604991fa1c77d4f4e14ad857af74848e6cad73ca05e8a9f4326bc33130dd31c8bc9";

        private readonly IDictionary<string, int> _applyValidationCalls;

        private readonly BrzMockIdentifier _identifier;
        private readonly bool _applyValidationReturn;

        public BrzMockIdentifier Identifier => throw new NotImplementedException();

        public BrzIdentifierMockValidator(BrzMockIdentifier identifier, bool applyValidationReturn)
        {
            _applyValidationCalls = new Dictionary<string, int>();

            _identifier            = identifier;
            _applyValidationReturn = applyValidationReturn;
        }

        internal void VerifyApplyValidationCalls(string value, string auxiliaryValue, bool ignoreFormat, bool pad, int times)
        {
            var key = GetApplyValidationCallId(value, auxiliaryValue, ignoreFormat, pad);

            if (!_applyValidationCalls.ContainsKey(key) || _applyValidationCalls[key] != times)
            {
                var calls = !_applyValidationCalls.ContainsKey(key) ? 0 : _applyValidationCalls[key];

                throw new Exception($"Expected invocation on ApplyValidation {times} times, but it was {calls} times");
            }
        }

        private static string GetApplyValidationCallId(string value, string auxiliaryValue, bool ignoreFormat, bool pad)
        {
            return string.Concat(value ?? NullStringPlaceholder, "|", auxiliaryValue ?? NullStringPlaceholder, "|", ignoreFormat, "|", pad);
        }

        public bool ApplyValidation(string value, string auxiliaryValue = null!, bool removeFormatters = true, bool pad = false)
        {
            var key = GetApplyValidationCallId(value, auxiliaryValue, removeFormatters, pad);

            if (!_applyValidationCalls.ContainsKey(key))
            {
                _applyValidationCalls.Add(key, 0);
            }

            _applyValidationCalls[key]++;

            return _applyValidationReturn;
        }
    }
}
