using RetroEngine.Core.Signing;
using RetroEngine.UnitTest.TestData.Components;
using System.Collections.Specialized;

namespace RetroEngine.UnitTest.Core.Signing
{
    /// <summary>
    /// Defines unit test cases for Negotiation class.
    /// </summary>
    public class NegotiationTests
    {
        [Fact]
        public void Negotiation_CreateNew_HasNoClauses()
        {
            // Arrange
            var negotiation = new Negotiation();

            // Act
            var result = negotiation.GetClauses();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Negotiation_WhenIncludingClauses_GetClausesGetsThem()
        {
            // Arrange
            var negotiation = new Negotiation();

            // Act
            negotiation.Include<TagComponent>();
            negotiation.Include<FlagsComponent>();
            negotiation.Include<CountComponent>();
            var result = negotiation.GetClauses().ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(typeof(TagComponent), result[0]);
            Assert.Equal(typeof(FlagsComponent), result[1]);
            Assert.Equal(typeof(CountComponent), result[2]);
        }

        [Fact]
        public void Negotiation_SigningContractWithMoreClauses_ReturnsSignatureWithSameLentghWithSignedBits()
        {
            // Arrange
            var negotiation = new Negotiation()
                .Include<TagComponent>()
                .Include<FlagsComponent>()
                .Include<CountComponent>();
            var offer = new Contract()
                .Extend<TagComponent>()
                .Extend<DateTime>()
                .Extend<FlagsComponent>()
                .Extend<CountComponent>()
                .Extend<BitVector32>();

            // Act
            var result = negotiation.Sign(offer);

            // Assert
            Assert.Equal(offer.GetClauses().Count(), result.Length);
            Assert.True(result[0]);
            Assert.False(result[1]);
            Assert.True(result[2]);
            Assert.True(result[3]);
            Assert.False(result[4]);
        }

        [Fact]
        public void Negotiation_SigningContractWithLessClauses_ReturnsSignatureWithSameLentghWithSignedBits()
        {
            // Arrange
            var negotiation = new Negotiation()
                .Include<TagComponent>()
                .Include<DateTime>()
                .Include<FlagsComponent>()
                .Include<CountComponent>()
                .Include<BitVector32>();
            var offer = new Contract()
                .Extend<TagComponent>()
                .Extend<FlagsComponent>()
                .Extend<CountComponent>();

            // Act
            var result = negotiation.Sign(offer);

            // Assert
            Assert.Equal(offer.GetClauses().Count(), result.Length);
            Assert.True(result[0]);
            Assert.True(result[1]);
            Assert.True(result[2]);
        }
    }
}
