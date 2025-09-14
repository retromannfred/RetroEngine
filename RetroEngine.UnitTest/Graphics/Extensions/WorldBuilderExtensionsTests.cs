using RetroEngine.Core;
using RetroEngine.Graphics;

namespace RetroEngine.UnitTest.Graphics.Extensions
{
    /// <summary>
    /// Defines unit test cases for WorldBuilderExtensions class.
    /// </summary>
    public class WorldBuilderExtensionsTests
    {
        [Fact]
        public void WorldBuilderExtensions_RegisterGraphicsEngine_AddsSpriteAndCameraSystems()
        {
            // Arrange
            var worldBuilder = new WorldBuilder();

            // Act
            var world = worldBuilder
                .RegisterGraphicsEngine(new GraphicSettings(800, 600))
                .Build();

            // Act
            var entity = world.CreateEntity();

            Assert.Null(Record.Exception(() => entity.Attach(new Transform())));
            Assert.Null(Record.Exception(() => entity.Attach(new SpriteRenderer())));
            Assert.Null(Record.Exception(() => entity.Attach(new Camera())));
        }
    }
}
