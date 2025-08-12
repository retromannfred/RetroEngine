using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Managers;
using RetroEngine.Core.Signing;

namespace RetroEngine.UnitTest.Core.Managers
{
    /// <summary>
    /// Defines unit test cases for ComponentManager class.
    /// </summary>
    public class EntityManagerTests
    {
        [Fact]
        public void EntityManager_CreatingEntities_GeneratesIdsInOrder()
        {
            // Arrange
            var manager = new EntityManager(32);

            // Act
            var one = manager.Create();
            var two = manager.Create();
            var three = manager.Create();
            var four = manager.Create();

            // Assert
            Assert.Equal(1, one);
            Assert.Equal(2, two);
            Assert.Equal(3, three);
            Assert.Equal(4, four);
        }

        [Fact]
        public void EntityManager_DeletingAndCreatingEntities_RecyclesIds()
        {
            // Arrange
            var manager = new EntityManager(32);

            // Act
            for (int i = 0; i < 4; i++)
                manager.Create();

            manager.Destroy(2);
            var recycled = manager.Create();

            // Assert
            Assert.Equal(2, recycled);
        }

        [Fact]
        public void EntityManager_GettingSignatureFromZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new EntityManager(32);
            manager.Create();

            // Act
            void actionZero() => manager.GetSignature(0);
            void actionNegative() => manager.GetSignature(-1);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void EntityManager_GettingSignatureFromNonExistingEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new EntityManager(32);
            manager.Create();

            // Act
            void action() => manager.GetSignature(10);

            // Assert
            Assert.Throws<EntityException>(action);
        }

        [Fact]
        public void EntityManager_GettingSignatureFromRecentCreatedEntity_HaveSizeSpecifiedOnManager()
        {
            // Arrange
            var maxComponents = 32;
            var manager = new EntityManager(maxComponents);

            // Act
            var entity = manager.Create();
            var signature = manager.GetSignature(entity);

            // Assert
            Assert.Equal(maxComponents, signature.Length);
        }

        [Fact]
        public void EntityManager_GettingSignatureFromRecentCreatedEntity_HaveAllSignatureFlagsOff()
        {
            // Arrange
            var maxComponents = 32;
            var manager = new EntityManager(maxComponents);

            // Act
            var entity = manager.Create();
            var signature = manager.GetSignature(entity);

            // Assert
            for (int i = 0; i < signature.Length; i++)
            {
                Assert.False(signature[i]);
            }
        }

        [Fact]
        public void EntityManager_SettingSignatureFromZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new EntityManager(32);
            manager.Create();

            // Act
            void actionZero() => manager.SetSignature(0, default);
            void actionNegative() => manager.SetSignature(-1, default);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void EntityManager_SettingSignatureFromNonExistingEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new EntityManager(32);
            manager.Create();

            // Act
            void action() => manager.SetSignature(10, default);

            // Assert
            Assert.Throws<EntityException>(action);
        }

        [Fact]
        public void EntityManager_SettingSignatureWithDifferentLengthThanManager_ThrowsArgumentException()
        {
            // Arrange
            var maxComponents = 32;
            var manager = new EntityManager(maxComponents);
            var entity = manager.Create();
            var signature = new Signature(10);

            // Act
            void action() => manager.SetSignature(entity, signature);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void EntityManager_SettingSignatureOfAnEntity_GetItProperlyAfter()
        {
            // Arrange
            var maxComponents = 32;
            var manager = new EntityManager(maxComponents);
            var entity = manager.Create();
            var signature = new Signature(maxComponents);

            signature[3] = true;
            signature[4] = true;
            signature[17] = true;
            signature[19] = true;
            signature[28] = true;

            // Act
            manager.SetSignature(entity, signature);
            var result = manager.GetSignature(entity);

            // Assert
            for (int i = 0; i < signature.Length; i++)
            {
                Assert.Equal(signature[i], result[i]);
            }
        }

        [Fact]
        public void EntityManager_DestroyingZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new EntityManager(32);
            manager.Create();

            // Act
            void actionZero() => manager.Destroy(0);
            void actionNegative() => manager.Destroy(-1);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void EntityManager_DestroyingNonExistingEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new EntityManager(32);
            manager.Create();

            // Act
            void action() => manager.Destroy(10);

            // Assert
            Assert.Throws<EntityException>(action);
        }

        [Fact]
        public void EntityManager_DestroyingAndRecreatingEntity_ResetsItsSignature()
        {
            // Arrange
            var maxComponents = 32;
            var manager = new EntityManager(maxComponents);
            var entity = manager.Create();
            var signature = new Signature(maxComponents);

            signature[3] = true;
            signature[4] = true;
            signature[17] = true;
            signature[19] = true;
            signature[28] = true;

            // Act
            manager.SetSignature(entity, signature);
            manager.Destroy(entity);
            var newEntity = manager.Create();
            var newSignature = manager.GetSignature(newEntity);

            // Assert
            Assert.Equal(entity, newEntity);
            for (int i = 0; i < maxComponents; i++)
            {
                Assert.False(newSignature[i]);
            }
        }
    }
}
