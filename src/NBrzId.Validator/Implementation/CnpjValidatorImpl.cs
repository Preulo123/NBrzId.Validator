using System;

using NBrzId.Common;

namespace NBrzId.Validator.Implementation
{
    sealed class CnpjValidatorImpl : BasicIdentifierValidator<Cnpj>, IBrzIdentifierValidator<Cnpj>
    {
        private const string InvalidBranchNumber = "0000";

		private static readonly int[] multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
		private static readonly int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

		public CnpjValidatorImpl(Cnpj cnpj)
		{
			Identifier = cnpj;
		}

        public override bool ApplySpecificValidation(string value, string auxiliaryValue = null)
        {
			if (!string.IsNullOrEmpty(auxiliaryValue))
			{
				throw new ArgumentException("Auxiliary value must not be provided when validating a CNPJ", nameof(auxiliaryValue));
			}

			return ApplyBasicValidations(value) && BranchNumberIsValid(value) && ApplyMod11Validation(value);
        }

		private static bool BranchNumberIsValid(string cnpj)
		{
			return cnpj.Substring(8, 4) != InvalidBranchNumber;
		}

		private static bool ApplyMod11Validation(string cnpj)
		{
			int sum;
			int remainder;

			string digits;

			string tempCnpj = cnpj.Trim().Substring(0, 12);

			sum = 0;

			for (int i = 0; i < 12; i++)
				sum += ParseChar(tempCnpj[i]) * multiplier1[i];

			remainder = sum % 11;

			if (remainder < 2)
				remainder = 0;
			else
				remainder = 11 - remainder;

			digits = remainder.ToString();

			if (digits[0] != cnpj[12])
			{
				return false;
			}

			tempCnpj += digits;

			sum = 0;

			for (int i = 0; i < 13; i++)
				sum += ParseChar(tempCnpj[i]) * multiplier2[i];

			remainder = sum % 11;

			if (remainder < 2)
				remainder = 0;
			else
				remainder = 11 - remainder;

			digits += remainder.ToString();

			return cnpj.EndsWith(digits);
		}
    }
}
