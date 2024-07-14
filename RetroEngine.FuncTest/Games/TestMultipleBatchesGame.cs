using OpenTK.Mathematics;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroEngine.Core;
using RetroEngine.Core.Elements;
using RetroEngine.Ecs.Components;
using RetroEngine.Ecs.Systems;
using RetroEngine.Graphics;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.FuncTest.Games
{
    internal class TestMultipleBatchesGame : Game
    {
        private World _world;
        private Texture _texture;
        private float _lastUpdate;

        float _cameraSpeed = 15f;
        private Entity? _camera;

        public TestMultipleBatchesGame()
            : base("Test multiple batching", 800, 600)
        {
            _world = new WorldBuilder().Build();
        }

        protected override void Initialize()
        {
            _world = new WorldBuilder()
                .AddSystem(new SpriteSystem(GraphicSettings))
                .AddSystem(new CameraSystem(GraphicSettings))
                .Build();
        }

        protected override void LoadContent()
        {
            _texture = TextureFactory.CreateRectangle(1, 1, Color4.White);

            Random rand = new Random();
            for (int i = 1; i <= 10; i++)
            {
                var entity = _world.CreateEntity()
                    .Attach(new Transform()
                    {
                        Position = new Vector3((float)rand.NextDouble() * 10f - 5f, (float)rand.NextDouble() * 10f - 5f, i % 2 * -10)
                    })
                    .Attach(new SpriteRenderer(_texture)
                    {
                        Color = i % 2 == 0 ? Color4.Blue : Color4.Red
                    });
            }

            _camera = _world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = Vector3.UnitZ * 10f,
                    Rotation = Vector3.UnitY * -MathHelper.Pi
                })
                .Attach(new Camera()
                {
                    Projection = Projections.Perspective
                });
        }

        protected override void Update(GameTime gameTime)
        {
            if (KeyboardState == null)
                return;

            var input = KeyboardState;
            var movement = _cameraSpeed * gameTime.DeltaTime;

            ref var trans = ref _camera.Get<Transform>();
            ref var cam = ref _camera.Get<Camera>();

            if (input.IsKeyDown(Keys.W))
                cam.MoveForward(ref trans, movement);

            if (input.IsKeyDown(Keys.S))
                cam.MoveBackwards(ref trans, movement);

            if (input.IsKeyDown(Keys.A))
                cam.MoveLeft(ref trans, movement);

            if (input.IsKeyDown(Keys.D))
                cam.MoveRight(ref trans, movement);

            if (input.IsKeyDown(Keys.Space))
                cam.MoveUp(ref trans, movement);

            if (input.IsKeyDown(Keys.LeftShift))
                cam.MoveDown(ref trans, movement);

            if (input.IsKeyDown(Keys.Right))
                cam.LookRight(ref trans, movement / 15);

            if (input.IsKeyDown(Keys.Left))
                cam.LookLeft(ref trans, movement / 15);

            if (input.IsKeyDown(Keys.Up))
                cam.LookUp(ref trans, movement / 15);

            if (input.IsKeyDown(Keys.Down))
                cam.LookDown(ref trans, movement / 15);
        }

        protected override void Render(GameTime gameTime)
        {
            ClearScreen(Color4.CornflowerBlue);

            _world.Render(gameTime);

            _lastUpdate += gameTime.DeltaTime;
            if (_lastUpdate >= 1f)
            {
                Title = $"{(int)(1f / gameTime.DeltaTime)} FPS";
                _lastUpdate = 0f;
            }
        }
    }
}
