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
        public T Identifier { get; set; }

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

        public abstract bool ApplySpecificValidation(string value, string auxiliaryValue = null);

        protected virtual bool ApplyBasicValidations(string value)
        {
            return !AllCharsAreEqual(value) && AllCharsAreValid(value);
        }

        protected static bool AllCharsAreEqual(string value)
        {
            return value.Distinct().Count() == 1;
        }

        protected virtual bool AllCharsAreValid(string value)
        {
            return value.All(c => char.IsDigit(c));
        }

        protected virtual int ParseChar(char value)
        {
            return value - '0';
        }
    }
}
