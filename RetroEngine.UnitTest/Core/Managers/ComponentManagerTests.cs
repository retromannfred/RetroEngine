using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Managers;
using RetroEngine.UnitTest.TestData.Components;

namespace RetroEngine.UnitTest.Core.Managers
{
    /// <summary>
    /// Defines unit test cases for ComponentManager class.
    /// </summary>
    public class ComponentManagerTests
    {
        [Fact]
        public void ComponentManager_AddingNonRegisteredComponent_ThrowsRegisterException()
        {
            // Arrange
            var manager = new ComponentManager();

            // Act
            void action() => manager.AddComponent<DateTime>(10, default);

            // Assert
            Assert.Throws<RegisterException>(action);
        }

        [Fact]
        public void ComponentManager_AddingRegisteredComponent_StoresTheComponent()
        {
            // Arrange
            var testGuid = Guid.NewGuid().ToString();
            var manager = new ComponentManager();
            manager.Register<TagComponent>();

            // Act
            manager.AddComponent(10, new TagComponent(testGuid));
            var result = manager.GetComponent<TagComponent>(10);

            // Assert
            Assert.Equal(testGuid, result.Tag);
        }

        [Fact]
        public void ComponentManager_AddingComponentForZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();

            // Act
            void actionZero() => manager.AddComponent(0, new TagComponent());
            void actionNegative() => manager.AddComponent(-1, new TagComponent());

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void ComponentManager_GettingNonRegisteredComponent_ThrowsRegisterException()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();

            // Act
            void action() => manager.GetComponent<FlagsComponent>(10);

            // Assert
            Assert.Throws<RegisterException>(action);
        }

        [Fact]
        public void ComponentManager_GettingComponentFromZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();

            // Act
            void actionZero() => manager.GetComponent<TagComponent>(0);
            void actionNegative() => manager.GetComponent<TagComponent>(-1);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void ComponentManager_GettingComponentFromAndEntityThatDoesNotHaveIt_ThrowsComponentException()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();

            // Act
            void action() => manager.GetComponent<TagComponent>(10);

            // Assert
            Assert.Throws<ComponentException>(action);
        }

        [Fact]
        public void ComponentManager_RemovingComponentFromZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();

            // Act
            void actionZero() => manager.RemoveComponent<TagComponent>(0);
            void actionNegative() => manager.RemoveComponent<TagComponent>(-1);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void ComponentManager_RemovingComponentFromAndEntityThatDoesNotHaveIt_ThrowsComponentException()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();

            // Act
            void action() => manager.RemoveComponent<TagComponent>(10);

            // Assert
            Assert.Throws<ComponentException>(action);
        }

        [Fact]
        public void ComponentManager_RemovingNonRegisteredComponent_DoesNothing()
        {
            // Arrange
            var manager = new ComponentManager();

            // Act
            manager.RemoveComponent<TagComponent>(10);
            void action() => manager.GetComponent<TagComponent>(10);

            // Assert
            Assert.Throws<RegisterException>(action);
        }

        [Fact]
        public void ComponentManager_RemovingAllFromZeroOrNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();
            manager.Register<FlagsComponent>();
            manager.Register<CountComponent>();

            // Act
            void actionZero() => manager.RemoveAllComponents(0);
            void actionNegative() => manager.RemoveAllComponents(-1);

            // Assert
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void ComponentManager_RemovingAllFromNonExistingEntity_DoestNothing()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();
            manager.Register<FlagsComponent>();
            manager.Register<CountComponent>();

            // Act
            manager.RemoveAllComponents(10);
        }

        [Fact]
        public void ComponentManager_RemovingAllFromNonExistingEntity_CannotRetrieveThemLater()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();
            manager.Register<FlagsComponent>();
            manager.Register<CountComponent>();

            manager.AddComponent(10, new TagComponent());
            manager.AddComponent(10, new FlagsComponent());

            // Act
            manager.RemoveAllComponents(10);

            // Assert
            Assert.Throws<ComponentException>(() => { manager.GetComponent<TagComponent>(10); });
            Assert.Throws<ComponentException>(() => { manager.GetComponent<FlagsComponent>(10); });
        }

        [Fact]
        public void ComponentManager_AfterEditingComponentGotFromManagerAsRef_GetComponentGetsItModified()
        {
            // Arrange
            var manager = new ComponentManager();
            manager.Register<TagComponent>();
            manager.AddComponent(1, new TagComponent("A"));
            ref var firstGet = ref manager.GetComponent<TagComponent>(1);

            // Act
            firstGet.Tag = "modified";

            // Assert
            Assert.Equal("modified", manager.GetComponent<TagComponent>(1).Tag);
        }
    }
}
