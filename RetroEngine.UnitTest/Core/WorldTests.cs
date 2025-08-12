using RetroEngine.Core;
using RetroEngine.Core.Elements;
using RetroEngine.Core.Exceptions;
using RetroEngine.UnitTest.TestData.Components;
using RetroEngine.UnitTest.TestData.Systems;
using System.Collections.Specialized;
using System.Reflection;

namespace RetroEngine.UnitTest.Core
{
    /// <summary>
    /// Tests functionality of classes World and WorldBuilder.
    /// </summary>
    public class WorldTests
    {
        [Fact]
        public void World_AccessConstructor_CannotBeDone()
        {
            // Arrange
            ConstructorInfo[] ctors;

            // Act
            Type type = typeof(World);
            ctors = type.GetConstructors();

            // Assert
            Assert.Empty(ctors);
        }

        [Fact]
        public void World_CreateEmptyWorld_CanBeDone()
        {
            // Arrange
            var builder = new WorldBuilder();

            // Act
            var world = builder.Build();

            // Assert
            Assert.NotNull(world);
        }

        [Fact]
        public void World_CreateEntities_EnumeratesIdsFromOneToN()
        {
            // Arrange
            var world = new WorldBuilder().Build();

            // Act
            var one = world.CreateEntity();
            var two = world.CreateEntity();
            var three = world.CreateEntity();
            var four = world.CreateEntity();

            // Assert
            Assert.Equal(1, one.Id);
            Assert.Equal(2, two.Id);
            Assert.Equal(3, three.Id);
            Assert.Equal(4, four.Id);
        }

        [Fact]
        public void World_DestroyAndCreateEntities_RecyclesIds()
        {
            // Arrange
            var world = new WorldBuilder().Build();

            // Act
            var one = world.CreateEntity();
            var two = world.CreateEntity();
            var three = world.CreateEntity();
            world.DestroyEntity(two);
            var four = world.CreateEntity();

            // Assert
            Assert.Equal(1, one.Id);
            Assert.Equal(2, two.Id);
            Assert.Equal(3, three.Id);
            Assert.Equal(2, four.Id);
        }

        [Fact]
        public void World_RegisterSystems_PermitsAttachComponentsOfTheirNegotiations()
        {
            // Arrange
            var world = new WorldBuilder()
                .RegisterSystem(new FlagSystem())
                .RegisterSystem(new CountSystem())
                .Build();

            // Act
            var entity = world.CreateEntity()
                .Attach(new TagComponent("test"))
                .Attach(new FlagsComponent() { FlagB = true })
                .Attach(new CountComponent() { Count = 567});

            // Assert
            Assert.Equal("test", world.GetComponent<TagComponent>(entity).Tag);
            Assert.True(world.GetComponent<FlagsComponent>(entity).FlagB);
            Assert.Equal(567, world.GetComponent<CountComponent>(entity).Count);
        }

        [Fact]
        public void World_RegisterSystems_DoesNotPermitAttachComponentsOutsideNegotiations()
        {
            // Arrange
            var world = new WorldBuilder()
                .RegisterSystem(new FlagSystem())
                .RegisterSystem(new CountSystem())
                .Build();
            var entity = world.CreateEntity();

            // Act
            void actionDateTime() => entity.Attach(new DateTime());
            void actionBitVector() => entity.Attach(new BitVector32());

            // Assert
            Assert.Throws<RegisterException>(actionDateTime);
            Assert.Throws<RegisterException>(actionBitVector);
        }
    }
}