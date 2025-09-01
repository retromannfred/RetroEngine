using RetroEngine.Core;
using RetroEngine.UnitTest.TestData.Components;

namespace RetroEngine.UnitTest.Core.Elements
{
    /// <summary>
    /// Defines unit test cases for Contract struct.
    /// </summary>
    public class ContractTests
    {
        [Fact]
        public void Contract_CreateNew_DoesNotContainAnyClause()
        {
            // Arrange
            var contract = new Contract();

            // Act
            var clauses = contract.GetClauses();

            // Assert
            Assert.Empty(clauses);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(Environment))]
        [InlineData(typeof(ContractTests))]
        [InlineData(typeof(string))]
        public void Contract_ExtendClassClauses_ThrowsArgumentException(Type type)
        {
            // Arrange
            var contract = new Contract();

            // Act
            void action() => contract.Extend(type);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(float))]
        public void Contract_ExtendPrimitiveClauses_ThrowsArgumentException(Type type)
        {
            // Arrange
            var contract = new Contract();

            // Act
            void action() => contract.Extend(type);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Contract_WhenExtendClauses_GetClausesGetsThatClauses()
        {
            // Arrange
            var contract = new Contract();

            // Act
            contract.Extend<TagComponent>();
            contract.Extend<FlagsComponent>();
            contract.Extend<CountComponent>();

            var clauses = contract.GetClauses().ToList();

            // Assert
            Assert.Equal(3, clauses.Count);
            Assert.Equal(typeof(TagComponent), clauses[0]);
            Assert.Equal(typeof(FlagsComponent), clauses[1]);
            Assert.Equal(typeof(CountComponent), clauses[2]);
        }

        [Fact]
        public void Contract_WhenExtendClausesAsGeneric_GetClausesGetsThatClauses()
        {
            // Arrange
            var contract = new Contract();

            // Act
            contract.Extend<TagComponent>();
            contract.Extend<FlagsComponent>();
            contract.Extend<CountComponent>();

            var clauses = contract.GetClauses().ToList();

            // Assert
            Assert.Equal(3, clauses.Count);
            Assert.Equal(typeof(TagComponent), clauses[0]);
            Assert.Equal(typeof(FlagsComponent), clauses[1]);
            Assert.Equal(typeof(CountComponent), clauses[2]);
        }

        [Fact]
        public void Contract_ExtendRepeatedClauses_KeepsJustDistinctClauses()
        {
            // Arrange
            var contract = new Contract();

            // Act
            contract.Extend<TagComponent>();
            contract.Extend<TagComponent>();
            contract.Extend<FlagsComponent>();
            contract.Extend<FlagsComponent>();
            contract.Extend<FlagsComponent>();
            contract.Extend<CountComponent>();
            contract.Extend<CountComponent>();
            contract.Extend<CountComponent>();
            contract.Extend<CountComponent>();

            var clauses = contract.GetClauses().ToList();

            // Assert
            Assert.Equal(3, clauses.Count);
            Assert.Equal(typeof(TagComponent), clauses[0]);
            Assert.Equal(typeof(FlagsComponent), clauses[1]);
            Assert.Equal(typeof(CountComponent), clauses[2]);
        }

        [Fact]
        public void Contract_WhenExtendListOfClauses_GetClausesGetsThatClauses()
        {
            // Arrange
            var contract = new Contract();

            // Act
            contract.Extend([
                typeof(TagComponent),
                typeof(FlagsComponent),
                typeof(CountComponent),
            ]);

            var clauses = contract.GetClauses().ToList();

            // Assert
            Assert.Equal(3, clauses.Count);
            Assert.Equal(typeof(TagComponent), clauses[0]);
            Assert.Equal(typeof(FlagsComponent), clauses[1]);
            Assert.Equal(typeof(CountComponent), clauses[2]);
        }

        [Fact]
        public void Contract_ExtendOtherContract_MergesClauses()
        {
            // Arrange
            var contract = new Contract();
            var other = new Contract();

            // Act
            contract.Extend<TagComponent>();
            other.Extend<FlagsComponent>();
            other.Extend<CountComponent>();
            contract.Extend(other);

            var clauses = contract.GetClauses().ToList();

            // Assert
            Assert.Equal(3, clauses.Count);
            Assert.Equal(typeof(TagComponent), clauses[0]);
            Assert.Equal(typeof(FlagsComponent), clauses[1]);
            Assert.Equal(typeof(CountComponent), clauses[2]);
        }
    }
}
