using NBrzId.Common;

using NBrzId.Validator.Implementation;

namespace NBrzId.Validator
{
    /// <summary>
    /// Provides static methods for validating Brazilian identifiers
    /// </summary>
    public static class BrzValidations
    {
        private static IBrzIdentifierValidator<Cnpj> _cnpjValidator;
        private static IBrzIdentifierValidator<Cpf>  _cpfValidator;
        
        /// <summary>
        /// Validates a CNPJ identifier (Brazilian federal tax identifier for legal entities)
        /// </summary>
        /// <param name="cnpj">The CNPJ to validate</param>
        /// <param name="removeFormatters">When <c>true</c>, the CNPJ's formatting characters are removed before executing the validation</param>
        /// <param name="pad">When <c>true</c>, the CNPJ is left padded with its padding character to make its length equal to the valid length for a CNPJ</param>
        /// <returns><c>true</c> if cnpj is a valid CNPJ identifier; otherwise, <c>false</c></returns>
        public static bool CheckCnpj(string cnpj, bool removeFormatters = true, bool pad = false)
        {
            if (_cnpjValidator == null)
            {
                _cnpjValidator = new CnpjValidatorImpl(BrzIdentifier.Cnpj);
            }

            return _cnpjValidator.ApplyValidation(cnpj, removeFormatters: removeFormatters, pad: pad);
        }

        /// <summary>
        /// Validates a CPF identifier (Brazilian federal tax identifier for private individuals)
        /// </summary>
        /// <param name="cpf">The CPF to validate</param>
        /// <param name="stateAbbreviation">The two-letter abbreviation for the CPF's issuance state (optional)</param>
        /// <param name="removeFormatters">When <c>true</c>, the CPF's formatting characters are removed before executing the validation</param>
        /// <param name="pad">When <c>true</c>, the CPF is left padded with its padding character to make its length equal to the valid length for a CPF</param>
        /// <returns><c>true</c> if cpf is a valid CPF identifier; otherwise, <c>false</c></returns>
        public static bool CheckCpf(string cpf, string stateAbbreviation = null, bool removeFormatters = true, bool pad = false)
        {
            if (_cpfValidator == null)
            {
                _cpfValidator = new CpfValidatorImpl(BrzIdentifier.Cpf);
            }
            
            return _cpfValidator.ApplyValidation(cpf, stateAbbreviation, removeFormatters, pad);
        }
    }
}
