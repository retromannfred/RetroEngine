using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Physics;

namespace RetroEngine.UnitTest.Physics.Systems
{
    /// <summary>
    /// Defines unit test cases for LinearTranslationSystem class.
    /// </summary>
    public class LinearTranslationSystemTests
    {
        [Fact]
        public void LinearMovementSystem_ProcessWithVelocity_ChangesPositionOfTransform()
        {
            // Arrange
            var system = new LinearTranslationSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entity = world.CreateEntity()
                .Attach(new Transform())
                .Attach(new LinearPhysics2D()
                {
                    Velocity = new Vector2(2.4f, -3.2f)
                });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            Assert.Equal(new Vector3(4.8f, -6.4f, 0), entity.GetComponent<Transform>().Position);
        }

        [Fact]
        public void LinearMovementSystem_ProcessWithDrag_ChangesLinearVelocity()
        {
            // Arrange
            var system = new LinearTranslationSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entity = world.CreateEntity()
                .Attach(new Transform())
                .Attach(new LinearPhysics2D()
                {
                    Velocity = new Vector2(19.2f, -25.6f),
                    Drag = .5f
                });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3)));

            // Assert
            var result = entity.GetComponent<LinearPhysics2D>().Velocity;
            Assert.Equal(new Vector2(2.4f, -3.2f), new Vector2((float)MathHelper.Round(result.X, 2), (float)MathHelper.Round(result.Y, 2)));
        }

        [Theory]
        [InlineData(FreezedMovement.None)]
        [InlineData(FreezedMovement.Horizontal)]
        [InlineData(FreezedMovement.Vertical)]
        [InlineData(FreezedMovement.Both)]
        public void LinearMovementSystem_ProcessWithFreezing_FreezesMovement(FreezedMovement freeze)
        {
            // Arrange
            var system = new LinearTranslationSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entity = world.CreateEntity()
                .Attach(new Transform())
                .Attach(new LinearPhysics2D()
                {
                    Velocity = new Vector2(2.4f, -3.2f),
                    FreezedMovement = freeze
                });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            Assert.Equal(
                new Vector3(
                    (freeze & FreezedMovement.Horizontal) == FreezedMovement.Horizontal ? 0 : 4.8f,
                    (freeze & FreezedMovement.Vertical) == FreezedMovement.Vertical ? 0 : -6.4f,
                    0)
                , entity.GetComponent<Transform>().Position);
        }
    }
}
