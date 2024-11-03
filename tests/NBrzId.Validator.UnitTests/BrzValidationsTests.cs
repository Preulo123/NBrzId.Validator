using Xunit;

namespace NBrzId.Validator.Tests
{
    public sealed class BrzValidationsTests
    {
        [Theory]
        [InlineData("00000000000000", false)]
        [InlineData("61226356000051", false)]
        [InlineData("40297511000113", false)]
        [InlineData("66747877000156", false)]
        [InlineData("I6788314000130", false)]
        [InlineData("16788314000130", true)]
        [InlineData("55789558100062", true)]
        public void ShouldValidateCnpj(string cnpjCandidate, bool expected)
        {
            var actual = BrzValidations.CheckCnpj(cnpjCandidate);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("00000000000", false)]
        [InlineData("33333333333", false)]
        [InlineData("43G99704742", false)]
        [InlineData("18738704178", false)]
        [InlineData("78274468926", false)]
        [InlineData("27593051633", true)]
        public void ShouldValidateCpf(string cpfCandidate, bool expected)
        {
            var actual = BrzValidations.CheckCpf(cpfCandidate);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("00000000000", null, false)]
        [InlineData("00000000000", "",   false)]
        [InlineData("00000000000", "RS", false)]
        [InlineData("4O466131224", null, false)]
        [InlineData("4O466131224", "",   false)]
        [InlineData("4O466131224", "AC", false)]
        [InlineData("11328885496", null, true)]
        [InlineData("32728128731", "",   true)]
        [InlineData("40466131224", "RR", true)]
        public void ShouldValidateCpfWithStateCode(string cpfCandidate, string stateCode, bool expected)
        {
            var actual = BrzValidations.CheckCpf(cpfCandidate, stateCode);

            Assert.Equal(expected, actual);
        }
    }
}