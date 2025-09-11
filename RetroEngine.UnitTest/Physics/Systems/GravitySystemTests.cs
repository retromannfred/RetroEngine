using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Physics;

namespace RetroEngine.UnitTest.Physics.Systems
{
    /// <summary>
    /// Defines unit test cases for GravitySystem class.
    /// </summary>
    public class GravitySystemTests
    {
        [Fact]
        public void GravitySystem_ProcessSystem_JustModifiesVelocityOfEntitiesWithGravity()
        {
            // Arrange
            var system = new GravitySystem(Vector2.UnitY * 8);
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var affected = world.CreateEntity()
                .Attach(new Transform())
                .Attach(new LinearPhysics2D())
                .Attach(new RigidBody());
            var unaffected = world.CreateEntity()
                .Attach(new Transform())
                .Attach(new LinearPhysics2D())
                .Attach(new RigidBody() { GravityScale = 0f });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            Assert.Equal(Vector2.Zero, unaffected.GetComponent<LinearPhysics2D>().Velocity);
            Assert.Equal(Vector2.UnitY * 16, affected.GetComponent<LinearPhysics2D>().Velocity);
        }

        [Fact]
        public void GravitySystem_ProcessSystem_ModifiesVelocityByGravityScale()
        {
            // Arrange
            var system = new GravitySystem(Vector2.UnitY * 12);
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entity = world.CreateEntity()
                .Attach(new Transform())
                .Attach(new LinearPhysics2D())
                .Attach(new RigidBody() { GravityScale = .2f});

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            Assert.Equal(Vector2.UnitY * 4.8f, entity.GetComponent<LinearPhysics2D>().Velocity);
        }
    }
}
