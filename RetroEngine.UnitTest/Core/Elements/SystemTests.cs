using RetroEngine.Core.Exceptions;
using RetroEngine.UnitTest.TestData.Systems;

namespace RetroEngine.UnitTest.Core.Elements
{
    /// <summary>
    /// Defines unit test cases for BaseSystem class.
    /// </summary>
    public class SystemTests
    {
        [Fact]
        public void System_AddingZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var system = new FlagSystem();

            // Act
            void actionZero() => system.AddEntity(0);
            void actionNegative() => system.AddEntity(-1);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(197)]
        public void System_AddingEntity_AppearsInTheEnum(int entityId)
        {
            // Arrange
            var system = new FlagSystem();

            // Act
            system.AddEntity(entityId);
            var result = system.GetEntities().ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(entityId, result[0]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(197)]
        public void System_AddingRepeatedEntity_DoesNotAccumulateInEnum(int entityId)
        {
            // Arrange
            var system = new FlagSystem();

            // Act
            system.AddEntity(entityId);
            system.AddEntity(entityId);
            system.AddEntity(entityId);
            system.AddEntity(entityId);
            var result = system.GetEntities().ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(entityId, result[0]);
        }

        [Fact]
        public void System_RemovingZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var system = new FlagSystem();

            // Act
            void actionZero() => system.RemoveEntity(0);
            void actionNegative() => system.RemoveEntity(-1);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void System_RemovingEntity_DisappearsInTheEnum()
        {
            // Arrange
            var system = new FlagSystem();
            system.AddEntity(10);
            system.AddEntity(20);
            system.AddEntity(30);

            // Act
            system.RemoveEntity(20);
            var result = system.GetEntities().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(10, result[0]);
            Assert.Equal(30, result[1]);
        }

        [Fact]
        public void System_RemovingRepeatedEntity_DoesNotDeleteExtra()
        {
            // Arrange
            var system = new FlagSystem();
            system.AddEntity(10);
            system.AddEntity(20);
            system.AddEntity(30);

            // Act
            system.RemoveEntity(20);
            system.RemoveEntity(20);
            system.RemoveEntity(20);
            system.RemoveEntity(20);
            var result = system.GetEntities().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(10, result[0]);
            Assert.Equal(30, result[1]);
        }
    }
}
