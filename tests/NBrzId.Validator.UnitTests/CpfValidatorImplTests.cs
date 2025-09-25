using System;

using Xunit;

using NBrzId.Common;

using NBrzId.Validator.Implementation;

namespace NBrzId.Validator.Tests
{
    public sealed class CpfValidatorImplTests
    {
        [Fact]
        public void ShouldReturnCnpjInstanceAsIdentifier()
        {
            var target = new CpfValidatorImpl(BrzIdentifier.Cpf);

            Assert.Same(BrzIdentifier.Cpf, target.Identifier);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldReturnFalseWhenCpfIsNullOrEmpty(string cpfCandidate)
        {
            var target = new CpfValidatorImpl(BrzIdentifier.Cpf);

            var actual = target.ApplyValidation(cpfCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("00000000000")]
        [InlineData("11111111111")]
        [InlineData("22222222222")]
        [InlineData("33333333333")]
        [InlineData("44444444444")]
        [InlineData("55555555555")]
        [InlineData("66666666666")]
        [InlineData("77777777777")]
        [InlineData("88888888888")]
        [InlineData("99999999999")]
        public void ShouldReturnFalseWhenAllDigitsAreEqual(string cpfCandidate)
        {
            var target = new CpfValidatorImpl(BrzIdentifier.Cpf);

            var actual = target.ApplyValidation(cpfCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("¼0000000000")]
        [InlineData("111½1111111")]
        [InlineData("2¾222222222")]
        [InlineData("3333333333¥")]
        [InlineData("444444444˅4")]
        [InlineData("555Ѽ5555555")]
        [InlineData("66666Ç66666")]
        [InlineData("777 7777777")]
        [InlineData("88888 88888")]
        [InlineData("9999999999​")]
        public void ShouldReturnFalseWhenAnyOfCharsAreNotDigits(string cpfCandidate)
        {
            var target = new CpfValidatorImpl(BrzIdentifier.Cpf);

            var actual = target.ApplyValidation(cpfCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("33 33")]
        [InlineData("1")]
        [InlineData("384")]
        public void ShouldReturnFalseWhenMod11ValidationThrowsArgumentOutOfRangeException(string cpfCandidate)
        {
            var target = new CpfValidatorImpl(BrzIdentifier.Cpf);

            var actual = target.ApplyValidation(cpfCandidate);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("17482587705")]
        [InlineData("00438880110")]
        [InlineData("37607035427")]
        [InlineData("77777386239")]
        [InlineData("28753660846")]
        [InlineData("38830552550")]
        [InlineData("48628623060")]
        [InlineData("66881678673")]
        [InlineData("28072824589")]
        [InlineData("1023106299")]
        public void ShouldReturnTrueWhenMod11ValidationReturnsTrue(string cpfCandidate)
        {
            var target = new CpfValidatorImpl(BrzIdentifier.Cpf);

            var actual = target.ApplyValidation(cpfCandidate, pad: true);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("00036059064", "DF")]
        [InlineData("86043310180", "AC")]
        [InlineData("84023836290", "CE")]
        [InlineData("48579800374", "AL")]
        [InlineData("45377639499", "BA")]
        [InlineData("82865948510", "MG")]
        [InlineData("05909610635", "ES")]
        [InlineData("18145388791", "SP")]
        [InlineData("88084100807", "PR")]
        [InlineData("87025987956", "RS")]
        public void ShouldReturnFalseWhenTaxDigitDoesNotMatchState(string cpfCandidate, string stateCode)
        {
            var target = new CpfValidatorImpl(BrzIdentifier.Cpf);

            var actual = target.ApplyValidation(cpfCandidate, stateCode);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("21768146063", "fd")]
        [InlineData("86424438122", "null")]
        [InlineData("52425206205", "invalid")]
        [InlineData("89490403369", "nonexistent")]
        [InlineData("54456313463", "UT")]
        [InlineData("54468250550", "LA")]
        [InlineData("03966902621", "tocantins")]
        [InlineData("93469285730", "ñÑ")]
        [InlineData("98622648885", "  ")]
        [InlineData("08594921926", "..")]
        public void ShouldArgumentExceptionWhenStateCodeDoesNotExists(string cpfCandidate, string stateCode)
        {
            var target = new CpfValidatorImpl(BrzIdentifier.Cpf);

            Assert.Throws<ArgumentException>(() => target.ApplyValidation(cpfCandidate, stateCode));
        }
    }
}
