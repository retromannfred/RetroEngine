using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Ecs.Systems;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Components;
using RetroEngine.Graphics.Systems;
using System;

namespace RetroEngine.FuncTest.Games
{
    internal class TestGraphicsGame : Game
    {
        int _cameraId;

        World _world;

        public TestGraphicsGame()
            : base("Test graphics", 800, 600)
        {
            _world = new WorldBuilder()
                .RegisterSystem(new SpriteSystem(GraphicSettings))
                .RegisterSystem(new CameraSystem(GraphicSettings))
            .Build();
        }

        protected override void LoadContent()
        {
            var rand = new Random();
            var texSquare = TextureFactory.CreateRectangle(10, 10, Color4.White);

            var camera = _world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitZ * -3})
                .Attach(new Camera());
            _cameraId = camera.Id;

            for (int i = 0; i < 10; i++)
            {
                _world.CreateEntity()
                    .Attach(new Transform()
                    {
                        Position = new Vector3(rand.Next(-100, 100), rand.Next(-100, 100), rand.Next(-100, 100))
                    })
                    .Attach(new SpriteRenderer(texSquare)
                    {
                        Color = Color4.Blue,
                    });
            }
        }

        protected override void Render(GameTime time)
        {
            _world.Render(time);
        }

        protected override void Update(GameTime time)
        {
            _world.Update(time);
        }
    }
}
