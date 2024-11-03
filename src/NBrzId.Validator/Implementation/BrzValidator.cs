using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NBrzId.Common;

namespace NBrzId.Validator.Implementation
{
    internal sealed class BrzValidator : IBrzValidator
    {
        private readonly IDictionary<string, bool> _validatorSearchCache;

        private readonly IEnumerable<IBrzIdentifierValidator<IBrzIdentifier>> _validators;

        public BrzValidator(IEnumerable<IBrzIdentifierValidator<IBrzIdentifier>> validators)
        {
            if (validators == null)
            {
                throw new ArgumentNullException(nameof(validators));
            }

            if (validators.Count() == 0)
            {
                throw new ArgumentException(nameof(validators), "No implementation of IBrzIdentifierValidator was found");
            }

            _validators = validators;

            _validatorSearchCache = new Dictionary<string, bool>();
        }

        public bool Validate(IBrzIdentifier identifier, string value, string auxiliaryValue = null, bool removeFormatters = true, bool pad = false)
            => GetValidator(identifier.GetType()).ApplyValidation(value, auxiliaryValue, removeFormatters, pad);

        public bool ValidateCnpj(string cnpj, bool removeFormatters = true, bool pad = false)
            => Validate(BrzIdentifier.Cnpj, cnpj, removeFormatters: removeFormatters, pad: pad);

        public bool ValidateCpf(string cpf, string stateAbbreviation = null, bool removeFormatters = true, bool pad = false)
            => Validate(BrzIdentifier.Cpf, cpf, stateAbbreviation, removeFormatters, pad);

        private IBrzIdentifierValidator<IBrzIdentifier> GetValidator(Type identifierType)
            => _validators.FirstOrDefault(GetValidatorPredicate(identifierType)) ?? throw new ArgumentException($"Validator not found for type {identifierType.FullName}");

        private Func<IBrzIdentifierValidator<IBrzIdentifier>, bool> GetValidatorPredicate(Type identifierType)
            =>  (v) =>
            {
                var typeKey = string.Concat(v.GetType().AssemblyQualifiedName, "|", identifierType.AssemblyQualifiedName);

                if (_validatorSearchCache.ContainsKey(typeKey))
                {
                    return _validatorSearchCache[typeKey];
                }

                var validatorType = v.GetType();

                var genericArgs = validatorType.GetTypeInfo().ImplementedInterfaces.First(t => t.GetGenericTypeDefinition() == typeof(IBrzIdentifierValidator<>)).GetGenericArguments();

                var isRequestedValidator = genericArgs.Contains(identifierType);

                _validatorSearchCache.Add(typeKey, isRequestedValidator);

                return isRequestedValidator;
            };
    }
}