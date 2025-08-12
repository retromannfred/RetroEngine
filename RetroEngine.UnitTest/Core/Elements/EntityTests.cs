using Moq;
using RetroEngine.Core;
using RetroEngine.Core.Elements;
using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Managers;
using RetroEngine.UnitTest.TestData.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var manager = new ComponentManager();

            // Act
            void actionZero()
            {
                var entity = new Entity(0, manager);
            }
            void actionNegative()
            {
                var entity = new Entity(-1, manager);
            }

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void Entity_AttachComponent_CallsManagerAddComponent()
        {
            // Arrange
            var entityId = 10;
            var tag = new TagComponent("test");
            var manager = new ComponentManager();
            manager.Register<TagComponent>();
            var entity = new Entity(entityId, manager);

            // Act
            entity.Attach(tag);

            // Assert
            Assert.Equal("test", manager.GetComponent<TagComponent>(10).Tag);
        }

        [Fact]
        public void Entity_DeattachComponent_CallsManagerRemoveComponent()
        {
            // Arrange
            var entityId = 10;
            var tag = new TagComponent("test");
            var flags = new FlagsComponent();
            var manager = new ComponentManager();
            manager.Register<TagComponent>();
            manager.Register<FlagsComponent>();
            var entity = new Entity(entityId, manager);

            // Act
            entity.Attach(tag);
            entity.Attach(flags);
            entity.Deattach<FlagsComponent>();

            // Assert
            Assert.Equal("test", manager.GetComponent<TagComponent>(entityId).Tag);
            Assert.Throws<ComponentException>(() => manager.GetComponent<FlagsComponent>(entityId));
        }
    }
}
