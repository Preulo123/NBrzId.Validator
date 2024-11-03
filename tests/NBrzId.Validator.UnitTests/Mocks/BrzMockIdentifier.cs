using System.Diagnostics.CodeAnalysis;

using NBrzId.Common;

namespace NBrzId.Validator.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class BrzMockIdentifier : IBrzIdentifier
    {
        public static BrzMockIdentifier Instance = new BrzMockIdentifier();

        public string Mask                 => "NN.NNNNNNN.NNNN.NN";

        public char PaddingCharacter       => '0';

        public int Length                  => 15;

        public char[] FormattingCharacters => new char[] { '.' };

        private BrzMockIdentifier()
        { }
    }
}