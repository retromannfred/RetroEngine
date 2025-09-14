using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Physics;

namespace RetroEngine.UnitTest.Physics.Extensions
{
    /// <summary>
    /// Defines unit test cases for WorldBuilderExtensions class.
    /// </summary>
    public class WorldBuilderExtensionsTests
    {
        [Fact]
        public void WorldBuilder_RegisterKineticPhysics_AddsLinearTranslationAndCollisionResolutionSystems()
        {
            // Arrange
            var worldBuilder = new WorldBuilder();

            // Act
            var world = worldBuilder
                .RegisterKineticPhysicsEngine()
                .Build();

            // Act
            var entity = world.CreateEntity();

            Assert.Null(Record.Exception(() => entity.Attach(new Transform())));
            Assert.Null(Record.Exception(() => entity.Attach(new Collider2D())));
            Assert.Null(Record.Exception(() => entity.Attach(new LinearPhysics2D())));
        }

        [Fact]
        public void WorldBuilder_RegisterDynamicPhysics_AddsLinearTranslationAndCollisionResolutionSystems()
        {
            // Arrange
            var worldBuilder = new WorldBuilder();

            // Act
            var world = worldBuilder
                .RegisterDynamicPhysicsEngine(Vector2.UnitY * -9.8f)
                .Build();

            // Act
            var entity = world.CreateEntity();

            Assert.Null(Record.Exception(() => entity.Attach(new Transform())));
            Assert.Null(Record.Exception(() => entity.Attach(new Collider2D())));
            Assert.Null(Record.Exception(() => entity.Attach(new LinearPhysics2D())));
            Assert.Null(Record.Exception(() => entity.Attach(new RigidBody())));
        }
    }
}
