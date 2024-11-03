using System;
using System.Collections.Generic;
using System.Linq;

using NBrzId.Common;
using NBrzId.Common.Constants;

namespace NBrzId.Validator.Implementation
{
    sealed class CpfValidatorImpl : BasicIdentifierValidator<Cpf>, IBrzIdentifierValidator<Cpf>
    {
		private static readonly IDictionary<char, string[]> _taxRegionsByDigit = new Dictionary<char, string[]>
		{
			{ '0', new string[] { BrzState.RioGrandeDoSulAbbreviation } },
			
			{ '1', new string[] { BrzState.DistritoFederalAbbreviation,
				                  BrzState.GoiasAbbreviation,
				                  BrzState.MatoGrossoDoSulAbbreviation,
				                  BrzState.MatoGrossoAbbreviation,
				                  BrzState.TocantinsAbbreviation } },
			
			{ '2', new string[] { BrzState.AcreAbbreviation,
				                  BrzState.AmazonasAbbreviation,
				                  BrzState.AmapaAbbreviation,
				                  BrzState.ParaAbbreviation,
				                  BrzState.RondoniaAbbreviation,
				                  BrzState.RoraimaAbbreviation } },

			{ '3', new string[] { BrzState.CearaAbbreviation,
				                  BrzState.MaranhaoAbbreviation,
				                  BrzState.PiauiAbbreviation } },

			{ '4', new string[] { BrzState.AlagoasAbbreviation,
				                  BrzState.ParaibaAbbreviation,
				                  BrzState.PernambucoAbbreviation,
				                  BrzState.RioGrandeDoNorteAbbreviation } },

			{ '5', new string[] { BrzState.BahiaAbbreviation,
				                  BrzState.SergipeAbbreviation } },

			{ '6', new string[] { BrzState.MinasGeraisAbbreviation } },

			{ '7', new string[] { BrzState.EspiritoSantoAbbreviation,
				                  BrzState.RioDeJaneiroAbbreviation } },

			{ '8', new string[] { BrzState.SaoPauloAbbreviation } },

			{ '9', new string[] { BrzState.ParanaAbbreviation,
				                  BrzState.SantaCatarinaAbbreviation } }
		};

        private static readonly int[] multiplier1 = new int[9]  { 10, 9,  8, 7, 6, 5, 4, 3, 2 };
		private static readonly int[] multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        public CpfValidatorImpl(Cpf cpf)
        {
            Identifier = cpf;
        }

        public override bool ApplySpecificValidation(string value, string auxiliaryValue = null)
        {
			if (!string.IsNullOrEmpty(auxiliaryValue) && !BrzState.Abbreviations.Contains(auxiliaryValue.ToUpper()))
			{
				throw new ArgumentException("If provided, auxiliary value must match one of the Brazilian states' acronyms", nameof(auxiliaryValue));
			}

			return ApplyBasicValidations(value) && ApplyStateValidation(value, auxiliaryValue) && ApplyMod11Validation(value);
        }

		private static bool ApplyStateValidation(string cpf, string stateCode)
		{
			if (string.IsNullOrEmpty(stateCode))
			{
				return true;
			}

			return _taxRegionsByDigit[cpf[8]].Contains(stateCode.ToUpper());
		}

        private static bool ApplyMod11Validation(string cpf)
		{
			string digits;
			
			int sum;
			int remainder;
			
			string tempCpf = cpf.Trim().Substring(0,9);
			
			sum = 0;

			for (int i = 0; i < 9; i++)
				sum += ParseChar(tempCpf[i]) * multiplier1[i];

			remainder = sum % 11;
			
			if (remainder < 2)
				remainder = 0;
			else
				remainder = 11 - remainder;
			
			digits = remainder.ToString();
			
			tempCpf += digits;
			
			sum = 0;
			
			for (int i = 0; i < 10; i++)
				sum += ParseChar(tempCpf[i]) * multiplier2[i];
			
			remainder = sum % 11;
			
			if (remainder < 2)
				remainder = 0;
			else
				remainder = 11 - remainder;

			digits += remainder.ToString();

			return cpf.EndsWith(digits);
		}
    }
}
