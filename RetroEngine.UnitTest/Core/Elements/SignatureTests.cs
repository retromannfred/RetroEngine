using RetroEngine.Core.Elements;

namespace RetroEngine.UnitTest.Core.Elements
{
    /// <summary>
    /// Defines unit test cases for Signature struct.
    /// </summary>
    public class SignatureTests
    {
        [Fact]
        public void Signature_GettingValueLessThanZero_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var signature = new Signature(32);

            // Act
            void action() { var data = signature[-1]; }

            // Assert
            Assert.Throws<IndexOutOfRangeException>(action);
        }

        [Fact]
        public void Signature_GettingValueGreaterOrEqualThanSize_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var size = 32;
            var signature = new Signature(size);

            // Act
            void actionEqual() { var data = signature[size]; }
            void actionGreater() { var data = signature[2 * size]; }

            // Assert
            Assert.Throws<IndexOutOfRangeException>(actionEqual);
            Assert.Throws<IndexOutOfRangeException>(actionGreater);
        }

        [Fact]
        public void Signature_SettingValueLessThanZero_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var signature = new Signature(32);

            // Act
            void action() { signature[-1] = true; }

            // Assert
            Assert.Throws<IndexOutOfRangeException>(action);
        }

        [Fact]
        public void Signature_SettingValueGreaterOrEqualThanSize_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var size = 32;
            var signature = new Signature(size);

            // Act
            void actionEqual() { signature[size] = true; }
            void actionGreater() { signature[2 * size] = true; }

            // Assert
            Assert.Throws<IndexOutOfRangeException>(actionEqual);
            Assert.Throws<IndexOutOfRangeException>(actionGreater);
        }

        [Fact]
        public void Signature_CreatingNew_IsAllSettedZeros()
        {
            // Arrange
            var signature = new Signature();

            // Act & Assert
            for (int i = 0; i < signature.Length; i++)
            {
                Assert.False(signature[i]);
            }
        }

        [Fact]
        public void Signature_SettingBit_GetsItProperlyAfter()
        {
            // Arrange
            var signature = new Signature(32);

            // Act
            signature[10] = true;

            // Assert
            Assert.True(signature[10]);
        }

        [Fact]
        public void Signature_IfDoesNotMatchesOtherSignaturesSize_ThrowsArgument()
        {
            // Arrange
            var signature = new Signature(32);
            var other = new Signature(10);

            // Act
            void action() { var result = signature.IsSignedFor(other); }

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Signature_IfDoesNotMatchesOtherSignature_IsSignedForReturnsFalse()
        {
            // Arrange
            var signature = new Signature(32);
            var other = new Signature(32);

            signature[10] = true;
            signature[9] = true;
            signature[31] = true;
            other[10] = true;
            other[9] = true;
            other[30] = true;

            // Act
            var result = signature.IsSignedFor(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Signature_IfDoesMatchesOtherSignature_IsSignedForReturnsTrue()
        {
            // Arrange
            var signature = new Signature(32);
            var other = new Signature(32);

            signature[10] = true;
            signature[9] = true;
            signature[31] = true;
            other[10] = true;
            other[9] = true;
            other[31] = true;

            // Act
            var result = signature.IsSignedFor(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Signature_IfDoesMatchesOtherSignatureAndHaveAditional_IsSignedForReturnsTrue()
        {
            // Arrange
            var signature = new Signature(32);
            var other = new Signature(32);

            signature[10] = true;
            signature[9] = true;
            signature[31] = true;
            signature[6] = true;
            signature[7] = true;
            signature[24] = true;
            other[10] = true;
            other[9] = true;
            other[31] = true;

            // Act
            var result = signature.IsSignedFor(other);

            // Assert
            Assert.True(result);
        }
    }
}
