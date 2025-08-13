using Moq;
using RetroEngine.Core;
using RetroEngine.Core.Elements;
using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Managers;
using RetroEngine.UnitTest.TestData.Systems;

namespace RetroEngine.UnitTest.Core.Managers
{
    /// <summary>
    /// Defines unit test cases for SystemManager class.
    /// </summary>
    public class SystemManagerTests
    {
        [Fact]
        public void SystemManager_AddingNeitherUpdateNorRender_ThrowsRegisterException()
        {
            // Arrange
            var manager = new SystemManager();

            // Act
            void action() => manager.AddSystem(new ExtendedBaseSystem());


            // Assert
            Assert.Throws<RegisterException>(action);
        }

        [Fact]
        public void SystemManager_AddingEitherUpdateOrRender_CanBeEnumerated()
        {
            // Arrange
            var manager = new SystemManager();

            // Act
            manager.AddSystem(new FlagSystem());
            manager.AddSystem(new CountSystem());
            var result = manager.GetAllSystems().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(typeof(FlagSystem), result[1].GetType());
            Assert.Equal(typeof(CountSystem), result[0].GetType());
        }

        [Fact]
        public void SystemManager_RepeatAddingEitherUpdateOrRender_IsRedundant()
        {
            // Arrange
            var manager = new SystemManager();

            // Act
            manager.AddSystem(new FlagSystem());
            manager.AddSystem(new CountSystem());
            manager.AddSystem(new FlagSystem());
            manager.AddSystem(new CountSystem());
            manager.AddSystem(new FlagSystem());
            manager.AddSystem(new CountSystem());
            var result = manager.GetAllSystems().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(typeof(FlagSystem), result[1].GetType());
            Assert.Equal(typeof(CountSystem), result[0].GetType());
        }

        [Fact]
        public void SystemManager_AddingSystemWithoutSignature_SetsItsSignatureAsDefault()
        {
            // Arrange
            var manager = new SystemManager();

            // Act
            manager.AddSystem(new FlagSystem());
            var result = manager.GetSignature<FlagSystem>();

            // Assert
            Assert.Equal(0, result.Length);
        }

        [Fact]
        public void SystemManager_SetSystemSignature_GetsItsSignatureModified()
        {
            // Arrange
            var manager = new SystemManager();
            manager.AddSystem(new FlagSystem());
            var signature = new Signature(10);
            signature[7] = true;

            // Act
            manager.SetSignature<FlagSystem>(signature);
            var result = manager.GetSignature<FlagSystem>();

            // Assert
            Assert.True(result[7]);
        }

        [Fact]
        public void SystemManager_AddingSystemWithItsSignature_GetsItsSignatureModified()
        {
            // Arrange
            var manager = new SystemManager();
            var signature = new Signature(10);
            signature[6] = true;

            // Act
            manager.AddSystem(new FlagSystem(), signature);
            var result = manager.GetSignature<FlagSystem>();

            // Assert
            Assert.True(result[6]);
        }

        [Fact]
        public void SystemManager_NotifyDestroyedZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new SystemManager();

            // Act
            void actionZero() => manager.NotifyDestroyedEntity(0);
            void actionNegative() => manager.NotifyDestroyedEntity(-1);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void SystemManager_NotifyDestroyedEntity_RemovesEntityFromBeingProcessed()
        {
            // Arrange
            var manager = new SystemManager();
            var flagSystem = new FlagSystem();
            flagSystem.AddEntity(10);
            var countSystem = new CountSystem();
            countSystem.AddEntity(10);

            manager.AddSystem(flagSystem);
            manager.AddSystem(countSystem);

            // Act
            manager.NotifyDestroyedEntity(10);

            // Assert
            Assert.Empty(flagSystem.GetEntities());
            Assert.Empty(countSystem.GetEntities());
        }

        [Fact]
        public void SystemManager_RepeteNotifyDestroyedEntity_IsRedundant()
        {
            // Arrange
            var manager = new SystemManager();
            var flagSystem = new FlagSystem();
            flagSystem.AddEntity(10);
            var countSystem = new CountSystem();
            countSystem.AddEntity(10);

            manager.AddSystem(flagSystem);
            manager.AddSystem(countSystem);

            // Act
            manager.NotifyDestroyedEntity(10);
            manager.NotifyDestroyedEntity(10);
            manager.NotifyDestroyedEntity(10);
            manager.NotifyDestroyedEntity(10);

            // Assert
            Assert.Empty(flagSystem.GetEntities());
            Assert.Empty(countSystem.GetEntities());
        }

        [Fact]
        public void SystemManager_NotifyChangedZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new SystemManager();

            // Act
            void actionZero() => manager.NotifyChangedEntitySignature(0, default);
            void actionNegative() => manager.NotifyChangedEntitySignature(0, default);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void SystemManager_NotifyChangedEntity_AddsEntityFromProperSystems()
        {
            // Arrange
            var manager = new SystemManager();

            var flagSignature = new Signature(3);
            flagSignature[0] = true;
            flagSignature[1] = true;
            var flagSystem = new FlagSystem();
            manager.AddSystem(flagSystem, flagSignature);

            var countSignature = new Signature(3);
            countSignature[0] = true;
            countSignature[2] = true;
            var countSystem = new CountSystem();
            manager.AddSystem(countSystem, countSignature);

            // Act
            manager.NotifyChangedEntitySignature(10, countSignature);

            // Assert
            Assert.Empty(flagSystem.GetEntities());
            Assert.Single(countSystem.GetEntities());
        }

        [Fact]
        public void SystemManager_NotifyChangedEntity_RemovesEntityFromProperSystems()
        {
            // Arrange
            var manager = new SystemManager();

            var flagSignature = new Signature(3);
            flagSignature[0] = true;
            flagSignature[1] = true;
            var flagSystem = new FlagSystem();
            manager.AddSystem(flagSystem, flagSignature);

            var countSignature = new Signature(3);
            countSignature[0] = true;
            countSignature[2] = true;
            var countSystem = new CountSystem();
            manager.AddSystem(countSystem, countSignature);

            flagSystem.AddEntity(10);
            countSystem.AddEntity(10);

            // Act
            manager.NotifyChangedEntitySignature(10, countSignature);

            // Assert
            Assert.Empty(flagSystem.GetEntities());
            Assert.Single(countSystem.GetEntities());
        }

        [Fact]
        public void SystemManager_PerformUpdates_CallsUpdateMethodOfUpdateSystem()
        {
            // Arrange
            var flagSystemMock = new Mock<FlagSystem>();
            var countSystemMock = new Mock<CountSystem>();
            var manager = new SystemManager();
            manager.AddSystem(flagSystemMock.Object);
            manager.AddSystem(countSystemMock.Object);
            var world = new WorldBuilder().Build();
            var time = new GameTime(TimeSpan.FromSeconds(1.8), TimeSpan.FromSeconds(1.8));

            // Act
            manager.PerformUpdate(world, time);

            // Assert
            flagSystemMock.Verify(s => s.Process(world, time), Times.Never());
            countSystemMock.Verify(s => s.Process(world, time), Times.Once());
        }

        [Fact]
        public void SystemManager_PerformRenders_CallsRenderMethodOfRenderSystem()
        {
            // Arrange
            var flagSystemMock = new Mock<FlagSystem>();
            var countSystemMock = new Mock<CountSystem>();
            var manager = new SystemManager();
            manager.AddSystem(flagSystemMock.Object);
            manager.AddSystem(countSystemMock.Object);
            var world = new WorldBuilder().Build();
            var time = new GameTime(TimeSpan.FromSeconds(1.8), TimeSpan.FromSeconds(1.8));

            // Act
            manager.PerformRender(world, time);

            // Assert
            flagSystemMock.Verify(s => s.Process(world, time), Times.Once());
            countSystemMock.Verify(s => s.Process(world, time), Times.Never());
        }
    }
}
