using System.Linq;

using NBrzId.Common;

namespace NBrzId.Validator.Implementation
{
    /// <summary>
    /// Provides a base implementation for validators of Brazilian identifiers, offering methods for common validation operations,
    /// including formatter removal, padding, general usual checks and specific validations tailored to each identifier type.
    /// </summary>
    /// <typeparam name="T">The type of identifier being validated, which implements <see cref="NBrzId.Common.IBrzIdentifier"/>.</typeparam>
    public abstract class BasicIdentifierValidator<T> : IBrzIdentifierValidator<T> where T : IBrzIdentifier
    {
        /// <summary>
        /// An instance of <see cref="NBrzId.Common.IBrzIdentifier"/>.
        /// </summary>
        public T Identifier { get; set; }
        /// <summary>
        /// Validates an identifier of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The identifier's value.</param>
        /// <param name="auxiliaryValue">Contextual data that might be necessary to perform the validation.</param>
        /// <param name="removeFormatters"><c>true</c> to remove the formatting characters before executing the validation; otherwise, <c>false</c>.</param>
        /// <param name="pad"><c>true</c> left pads the identifier with its padding character to make its length equal to the expected length.</param>
        /// <returns></returns>
        public virtual bool ApplyValidation(string value, string auxiliaryValue = null, bool removeFormatters = true, bool pad = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string adjustedValue = value;

            if (removeFormatters)
            {
                var formatterIndices = Identifier.Mask.Select((c, i) => {
                    if (c == 'N' || c == 'X' || c == 'A')
                    {
                        return -1;
                    }

                    return i;
                }).Where(i => i != -1);

                adjustedValue = new string(adjustedValue.Where((c, i) => !formatterIndices.Contains(i) || !Identifier.FormattingCharacters.Contains(c)).ToArray());
            }

            if (pad)
            {
                adjustedValue = adjustedValue.PadLeft(Identifier.Length, Identifier.PaddingCharacter);
            }

            if (adjustedValue.Length != Identifier.Length)
            {
                return false;
            }

            return ApplySpecificValidation(adjustedValue, auxiliaryValue);
        }
        /// <summary>
        /// Validates the identifier with its specific rules.
        /// </summary>
        /// <param name="value">The identifier's value.</param>
        /// <param name="auxiliaryValue">Contextual data that might be necessary to perform the validation.</param>
        /// <returns><c>true</c> if <paramref name="value"/> meets the criteria for a valid identifier; otherwise, <c>false</c>.</returns>
        public abstract bool ApplySpecificValidation(string value, string auxiliaryValue = null);
        /// <summary>
        /// Performs basic validations for an identifier before any specific algorithm (eg. mod 11) is used.
        /// </summary>
        /// <param name="value">The identifier's value.</param>
        /// <returns><c>true</c> if not all characters are the same and are decimal digits; otherwise, <c>false</c>.</returns>
        protected virtual bool ApplyBasicValidations(string value)
        {
            return !AllCharsAreEqual(value) && AllCharsAreValid(value);
        }
        /// <summary>
        /// Checks if all data symbols of the identifier are equal.
        /// </summary>
        /// <param name="value">The identifier's value.</param>
        /// <returns><c>true</c> if all characters are the same; otherwise, <c>false</c>.</returns>
        protected static bool AllCharsAreEqual(string value)
        {
            return value.Distinct().Count() == 1;
        }
        /// <summary>
        /// Checks if all characters of the identifier are valid.
        /// </summary>
        /// <param name="value">The identifier's value.</param>
        /// <returns><c>true</c> if all characters are decimal digits; otherwise, <c>false</c>.</returns>
        protected virtual bool AllCharsAreValid(string value)
        {
            return value.All(c => char.IsDigit(c));
        }
        /// <summary>
        /// Converts a character to its integer representation for calculation purposes.
        /// </summary>
        /// <param name="value">The character to convert.</param>
        /// <returns>An integer that represent the value of <paramref name="value"/>.</returns>
        protected virtual int ParseChar(char value)
        {
            return value - '0';
        }
    }
}
