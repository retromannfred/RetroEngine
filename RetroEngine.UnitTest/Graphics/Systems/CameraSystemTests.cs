using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.UnitTest.Graphics.Systems
{
    /// <summary>
    /// Defines unit test cases for CameraSystem class.
    /// </summary>
    public class CameraSystemTests
    {
        [Fact]
        public void CameraSystem_ProcessSystem_UpdatesCameraVectors()
        {
            // Arrange
            var world = new WorldBuilder()
                .RegisterSystem(new CameraSystem(new GraphicSettings(800, 600)))
                .Build();
            var entity = world.CreateEntity()
                .Attach(new Transform() { Rotation = Vector3.UnitX * MathHelper.PiOver6 })
                .Attach(new Camera());

            // Act
            ref var camera = ref entity.GetComponent<Camera>();
            ref var transform = ref entity.GetComponent<Transform>();
            world.Update(new GameTime());
            world.Render(new GameTime());
            transform = camera.MoveForward(transform, 1f);

            // Assert
            Assert.Equal(.5f, MathHelper.Round(entity.GetComponent<Transform>().Position.Y, 2));
        }

        [Fact]
        public void CameraSystem_ProcessSystem_FillsClipSpaces()
        {
            // Arrange
            var numberOfCameras = 10;
            var graphicSpaces = new GraphicSettings(800, 600);
            var world = new WorldBuilder()
                .RegisterSystem(new CameraSystem(graphicSpaces))
                .Build();
            for (int i = 0; i < numberOfCameras; i++)
            {
                world.CreateEntity()
                    .Attach(new Transform())
                    .Attach(new Camera());
            }

            // Act
            world.Update(new GameTime());
            world.Render(new GameTime());

            // Assert
            Assert.Equal(numberOfCameras, graphicSpaces.ClipSpaces.Count);
        }
    }
}
