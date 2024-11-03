using System;

using Xunit;

using NBrzId.Common;

using NBrzId.Validator.Implementation;

namespace NBrzId.Validator.Tests
{
    public sealed class CnpjValidatorImplTests
    {
        [Fact]
        public void ShouldReturnCnpjInstanceAsIdentifier()
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            Assert.Same(BrzIdentifier.Cnpj, target.Identifier);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldReturnFalseWhenCnpjIsNullOrEmpty(string cnpjCandidate)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            var actual = target.ApplyValidation(cnpjCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("34812338749")]
        public void ShouldReturnFalseWhenCnpjHasInvalidLengthAndPadIsFalse(string cnpjCandidate)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            var actual = target.ApplyValidation(cnpjCandidate, pad: false);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("00000000000000")]
        [InlineData("11111111111111")]
        [InlineData("22222222222222")]
        [InlineData("33333333333333")]
        [InlineData("44444444444444")]
        [InlineData("55555555555555")]
        [InlineData("66666666666666")]
        [InlineData("77777777777777")]
        [InlineData("88888888888888")]
        [InlineData("99999999999999")]
        public void ShouldReturnFalseWhenAllDigitsAreEqual(string cnpjCandidate)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            var actual = target.ApplyValidation(cnpjCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("00000000000000")]
        [InlineData("111111111111i1")]
        [InlineData("22222222222Z22")]
        [InlineData("3333333333e333")]
        [InlineData("444444444A4444")]
        [InlineData("55555555s55555")]
        [InlineData("6666666G666666")]
        [InlineData("777777t7777777")]
        [InlineData("88888%88888888")]
        [InlineData("n9999999999999")]
        public void ShouldReturnFalseWhenAnyOfCharsAreNotDigits(string cnpjCandidate)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            var actual = target.ApplyValidation(cnpjCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("08216422000020")]
        [InlineData("21132466000030")]
        [InlineData("46013442000092")]
        [InlineData("67666203000001")]
        [InlineData("86488363000052")]
        public void ShouldReturnFalseWhenBranchNumberIsInvalid(string cnpjCandidate)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            var actual = target.ApplyValidation(cnpjCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("62483248000108")]
        [InlineData("03762152000110")]
        [InlineData("22805485000123")]
        [InlineData("68888333000132")]
        [InlineData("01065288000145")]
        [InlineData("65453720000158")]
        [InlineData("26624425000165")]
        [InlineData("63167461000173")]
        [InlineData("52041587000180")]
        [InlineData("45232885000192")]
        public void ShouldReturnTrueWhenMod11ValidationReturnsTrue(string cnpjCandidate)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            var actual = target.ApplyValidation(cnpjCandidate);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("62483248000180")]
        [InlineData("03762152000120")]
        [InlineData("22805485000132")]
        [InlineData("68888333000143")]
        [InlineData("01065288000199")]
        [InlineData("65453720000150")]
        [InlineData("26624425000155")]
        [InlineData("63167461000126")]
        [InlineData("52041587000108")]
        [InlineData("45232885000130")]
        public void ShouldReturnFalseWhenMod11ValidationReturnsFalse(string cnpjCandidate)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            var actual = target.ApplyValidation(cnpjCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("state")]
        [InlineData("sp")]
        [InlineData("RJ")]
        [InlineData(" ")]
        public void ShouldThrowArgumentExceptionWhenAuxiliaryValueIsSpecified(string auxiliaryValue)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            Assert.Throws<ArgumentException>(() => target.ApplyValidation("62483248000108", auxiliaryValue));
        }

        [Theory]
        [InlineData("62483248000108", null, true)]
        [InlineData("62483248000108", "", true)]
        [InlineData("62483248900908", null, false)]
        [InlineData("62483248900908", "", false)]
        public void ShouldReturnWhetherCnpjIsValidWhenAuxiliaryValueIsNullOrEmpty(string cnpjCandidate, string auxiliaryValue, bool expected)
        {
            var target = new CnpjValidatorImpl(BrzIdentifier.Cnpj);

            var actual = target.ApplyValidation(cnpjCandidate, auxiliaryValue);

            Assert.Equal(expected, actual);
        }
    }
}