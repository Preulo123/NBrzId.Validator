using NBrzId.Common;

namespace NBrzId.Validator
{
    /// <summary>
    /// Provides methods for validating Brazilian identifiers
    /// </summary>
    /// <remarks>
    /// This interface defines the standard validation methods for Brazilian identifiers, supporting flexible validation options.
    /// It includes general identifier validation, as well as specific methods for validating CPF and CNPJ numbers, with optional
    /// controls for formatting and padding.
    /// </remarks>
    public interface IBrzValidator
    {
        /// <summary>
        /// Validates an identifier
        /// </summary>
        /// <param name="identifier">An object that implements the IBrzIdentifier interface</param>
        /// <param name="value">The text value of the identifier that will be validated</param>
        /// <param name="auxiliaryValue">Any additional contextual data that might be used to support validation</param>
        /// <param name="removeFormatters">When <c>true</c>, the identifier's formatting characters are removed before executing the validation</param>
        /// <param name="pad">When <c>true</c>, the identifier is left padded with its padding character to make its length equal to the valid length for this identifier type</param>
        /// <returns><c>true</c> if value is a valid <paramref name="identifier" />; otherwise, <c>false</c></returns>
        bool Validate(IBrzIdentifier identifier, string value, string auxiliaryValue = null, bool removeFormatters = true, bool pad = false);
        /// <summary>
        /// Validates a CNPJ identifier (brazilian federal tax identifier for legal entities)
        /// </summary>
        /// <param name="cnpj">The CNPJ to validate</param>
        /// <param name="removeFormatters">When <c>true</c>, the CNPJ's formatting characters are removed before executing the validation</param>
        /// <param name="pad">When <c>true</c>, the CNPJ is left padded with its padding character to make its length equal to the valid length for a CNPJ</param>
        /// <returns><c>true</c> if cnpj is a valid CNPJ identifier; otherwise, <c>false</c></returns>
        bool ValidateCnpj(string cnpj, bool removeFormatters = true, bool pad = false);
        /// <summary>
        /// Validates a CPF identifier (brazilian federal tax identifier for private individuals)
        /// </summary>
        /// <param name="cpf">The CPF to validate</param>
        /// <param name="stateAbbreviation">The two-letter abbreviation for the CPF's issuance state (optional)</param>
        /// <param name="removeFormatters">When <c>true</c>, the CPF's formatting characters are removed before executing the validation</param>
        /// <param name="pad">When <c>true</c>, the CPF is left padded with its padding character to make its length equal to the valid length for a CPF</param>
        /// <returns><c>true</c> if cpf is a valid CPF identifier; otherwise, <c>false</c></returns>
        bool ValidateCpf(string cpf, string stateAbbreviation = null, bool removeFormatters = true, bool pad = false);
    }
}
