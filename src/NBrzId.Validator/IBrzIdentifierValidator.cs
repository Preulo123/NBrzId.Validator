using NBrzId.Common;

namespace NBrzId.Validator
{
    /// <summary>
    /// Defines the structure for a validator that checks the validity of a Brazilian identifier.
    /// </summary>
    /// <typeparam name="T">The type of identifier to be validated, which implements <see cref="NBrzId.Common.IBrzIdentifier"/>.</typeparam>
    public interface IBrzIdentifierValidator<out T> where T : IBrzIdentifier
    {
        /// <summary>
        /// Validates an identifier value
        /// </summary>
        /// <param name="value">The identifier value to validate</param>
        /// <param name="auxiliaryValue">Any additional contextual data that might be used to support validation</param>
        /// <param name="removeFormatters">When <c>true</c>, the identifier's formatting characters are removed before executing the validation</param>
        /// <param name="pad">When <c>true</c>, the identifier is left padded with its padding character to make its length equal to the valid length for this identifier type</param>
        /// <returns><c>true</c>c> if the identifier is valid; otherwise, <c>false</c></returns>
        bool ApplyValidation(string value, string auxiliaryValue = null, bool removeFormatters = true, bool pad = false);
    }
}
