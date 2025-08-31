using RetroEngine.Core;
using RetroEngine.Core.Exceptions;
using RetroEngine.UnitTest.TestData.Components;
using RetroEngine.UnitTest.TestData.Systems;

namespace RetroEngine.UnitTest.Core.Elements
{
    /// <summary>
    /// Defines unit test cases for Entity class.
    /// </summary>
    public class EntityTests
    {
        [Fact]
        public void Entity_CreateZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var world = new WorldBuilder().Build();

            // Act
            void actionZero()
            {
                var entity = new Entity(0, world);
            }
            void actionNegative()
            {
                var entity = new Entity(-1, world);
            }

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void Entity_AttachComponent_CallsWorldAddComponent()
        {
            // Arrange
            var tag = new TagComponent("test");
            var world = new WorldBuilder().RegisterSystem(new FlagSystem()).Build();
            var entity = world.CreateEntity();

            // Act
            entity.Attach(tag);

            // Assert
            Assert.Equal("test", world.GetComponent<TagComponent>(entity.Id).Tag);
        }

        [Fact]
        public void Entity_DeattachComponent_CallsWorldRemoveComponent()
        {
            // Arrange
            var tag = new TagComponent("test");
            var flags = new FlagsComponent();
            var world = new WorldBuilder().RegisterSystem(new FlagSystem()).Build();
            var entity = world.CreateEntity();

            // Act
            entity.Attach(tag);
            entity.Attach(flags);
            entity.Deattach<FlagsComponent>();

            // Assert
            Assert.Equal("test", world.GetComponent<TagComponent>(entity.Id).Tag);
            Assert.Throws<ComponentException>(() => world.GetComponent<FlagsComponent>(entity.Id));
        }
    }
}
