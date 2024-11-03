using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using NBrzId.Common;

using NBrzId.Validator.Implementation;
using NBrzId.Validator.Tests.Mocks;

namespace NBrzId.Validator.Tests
{
    public sealed class BrzValidatorTests
    {
        [Fact]
        public void ShouldThrowArgumentNullExceptionWheValidatorsUnspecified()
        {
            Assert.Throws<ArgumentNullException>(() => new BrzValidator(null!));
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWheValidatorsIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new BrzValidator(Enumerable.Empty<IBrzIdentifierValidator<IBrzIdentifier>>()));
        }

        [Fact]
        public void ShouldInstantiateWhenDependenciesSpecified()
        {
            var target = new BrzValidator(
                new List<IBrzIdentifierValidator<IBrzIdentifier>>{
                    new BrzIdentifierMockValidator(BrzMockIdentifier.Instance, true)
                }
                );

            Assert.NotNull(target);
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWhenValidatorNotFoundForIdentifierType()
        {
            var target = new BrzValidator(
                new List<IBrzIdentifierValidator<IBrzIdentifier>>{
                    new BrzIdentifierMockValidator(BrzMockIdentifier.Instance, true)
                }
                );

            var exceptionThrown1 = Assert.Throws<ArgumentException>(() => target.ValidateCnpj(string.Empty));
            var exceptionThrown2 = Assert.Throws<ArgumentException>(() => target.ValidateCpf(string.Empty));

            Assert.Equal("Validator not found for type NBrzId.Common.Cnpj", exceptionThrown1.Message);
            Assert.Equal("Validator not found for type NBrzId.Common.Cpf",  exceptionThrown2.Message);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShouldReturnValidatorApplyValidation(bool applyValidationReturn)
        {
            var brzMockValidator = new BrzIdentifierMockValidator(BrzMockIdentifier.Instance, applyValidationReturn);

            var brzMockValidators = new List<IBrzIdentifierValidator<IBrzIdentifier>>()
            {
                brzMockValidator
            };

            var target = new BrzValidator(brzMockValidators);

            var actual1 = target.Validate(BrzMockIdentifier.Instance, string.Empty);
            var actual2 = target.Validate(BrzMockIdentifier.Instance, string.Empty, "aux");
            var actual3 = target.Validate(BrzMockIdentifier.Instance, string.Empty, "aux", false);
            var actual4 = target.Validate(BrzMockIdentifier.Instance, string.Empty, "aux", false, true);

            Assert.Equal(applyValidationReturn, actual1);
            Assert.Equal(applyValidationReturn, actual2);
            Assert.Equal(applyValidationReturn, actual3);
            Assert.Equal(applyValidationReturn, actual4);

            brzMockValidator.VerifyApplyValidationCalls(string.Empty, null!, true,  false, 1);
            brzMockValidator.VerifyApplyValidationCalls(string.Empty, "aux", true,  false, 1);
            brzMockValidator.VerifyApplyValidationCalls(string.Empty, "aux", false, false, 1);
            brzMockValidator.VerifyApplyValidationCalls(string.Empty, "aux", false, true,  1);
        }
    }
}
