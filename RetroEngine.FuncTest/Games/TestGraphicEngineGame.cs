using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Components;
using RetroEngine.Graphics.Enums;
using RetroEngine.Graphics.Systems;
using System;

namespace RetroEngine.FuncTest.Games
{
    /// <summary>
    /// Game to test how the graphics engine works with the ECS engine.
    /// </summary>
    internal class TestGraphicEngineGame : Game
    {
        private const float CAMERA_SPEED = 8.0f;
        private const float CAMERA_SENSITIVITY = .5f;

        private int _cameraId = 0;
        private readonly World _world;

        public TestGraphicEngineGame()
            : base($"CAMERA: speed={CAMERA_SPEED}; sensitivity={CAMERA_SENSITIVITY}", 800, 600)
        {
            Console.WriteLine();
            Console.WriteLine("Su should be seeing 3 planes on {X=0,Y=0,Z=0} and 10 cubes in an arbitrary area.");
            Console.WriteLine("Use WASD keys to translate the camera, and left mouse button to point it.");
            Console.WriteLine();

            _world = new WorldBuilder()
                .RegisterSystem(new SpriteSystem(GraphicSettings))
                .RegisterSystem(new CameraSystem(GraphicSettings))
            .Build();
        }

        protected override void LoadContent()
        {
            _cameraId = _world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = Vector3.UnitZ * 10f,
                    Rotation = Vector3.UnitY * MathHelper.Pi
                })
                .Attach(new Camera()
                {
                    Projection = Projection.Perspective
                })
            .Id;

            var texture = TextureFactory.CreateRectangle(1, 1, Color4.White);
            var rand = new Random();

            CreateAxis(texture);
            for (int i = 0; i < 10; i++)
            {
                CreateCube(texture, new Vector3(rand.Next(-10, 10), rand.Next(-10, 10), rand.Next(-30, 0)));
            }
        }

        protected override void Update(GameTime time)
        {
            ref var transform = ref _world.GetComponent<Transform>(_cameraId);
            ref var camera = ref _world.GetComponent<Camera>(_cameraId);

            if (KeyboardState!.IsKeyDown(Keys.W))
                transform = camera.MoveForward(transform, CAMERA_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.A))
                transform = camera.MoveLeft(transform, CAMERA_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.S))
                transform = camera.MoveBackwards(transform, CAMERA_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.D))
                transform = camera.MoveRight(transform, CAMERA_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.Space))
                transform = camera.MoveUp(transform, CAMERA_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.LeftShift))
                transform = camera.MoveDown(transform, CAMERA_SPEED * time.Delta);

            _world.Update(time);
        }

        protected override void Render(GameTime time)
        {
            ClearScreen(Color4.CornflowerBlue);

            ref var transform = ref _world.GetComponent<Transform>(_cameraId);
            ref var camera = ref _world.GetComponent<Camera>(_cameraId);

            if (MouseState!.IsButtonDown(MouseButton.Left))
            {
                transform = camera.LookUp(transform, (MouseState!.Delta.Y) * CAMERA_SENSITIVITY * time.Delta);
                transform = camera.LookRight(transform, (MouseState!.Delta.X) * CAMERA_SENSITIVITY * time.Delta);
            }

            _world.Render(time);
        }

        private void CreateAxis(Texture2D texture)
        {
            _world.CreateEntity() // Y-Plane
                .Attach(new Transform()
                {
                    Rotation = Vector3.UnitX * MathHelper.PiOver2
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.Yellow,
                });

            _world.CreateEntity() // Z-Plane
                .Attach(new Transform())
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.Magenta,
                });

            _world.CreateEntity() // X-Plane
                .Attach(new Transform()
                {
                    Rotation = Vector3.UnitY * MathHelper.PiOver2
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.Cyan,
                });
        }

        private void CreateCube(Texture2D texture, Vector3 position)
        {
            _world.CreateEntity() // TOP
                .Attach(new Transform()
                {
                    Position = position + new Vector3(0, .5f, 0),
                    Rotation = Vector3.UnitX * MathHelper.PiOver2
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.Yellow,
                });

            _world.CreateEntity() // BOTTOM
                .Attach(new Transform()
                {
                    Position = position + new Vector3(0, -.5f, 0),
                    Rotation = Vector3.UnitX * MathHelper.PiOver2
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.White,
                });

            _world.CreateEntity() // FRONT
                .Attach(new Transform()
                {
                    Position = position + new Vector3(0, 0, .5f),
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.Red,
                });

            _world.CreateEntity() // BACK
                .Attach(new Transform()
                {
                    Position = position + new Vector3(0, 0, -.5f),
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.Orange,
                });

            _world.CreateEntity() // RIGHT
                .Attach(new Transform()
                {
                    Position = position + new Vector3(.5f, 0, 0),
                    Rotation = Vector3.UnitY * MathHelper.PiOver2
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.Green,
                });

            _world.CreateEntity() // LEFT
                .Attach(new Transform()
                {
                    Position = position + new Vector3(-.5f, 0, 0),
                    Rotation = Vector3.UnitY * MathHelper.PiOver2
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.Blue,
                });
        }
    }
}
