using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroEngine.Buddies.System;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Components;
using RetroEngine.Graphics.Enums;
using RetroEngine.Graphics.Systems;
using RetroEngine.Physics.Components;
using RetroEngine.Physics.Enums;

namespace RetroEngine.FuncTest.Games
{
    internal class TestCollisionsGame : Game
    {
        private const float PLAYER_WALK_SPEED = 3f;
        private const float PLAYER_RUN_SPEED = 10f;

        private readonly World _world;

        private int _cameraId = 0;
        private int _playerId = 0;

        public TestCollisionsGame()
            : base("Test collisions", 800, 600)
        {
            Console.WriteLine();
            Console.WriteLine("Move the rotation square with WASD keys.");
            Console.WriteLine("When two object are colliding, they will turn red.");
            Console.WriteLine();

            _world = new WorldBuilder()
                .RegisterSystem(new BuddyCollider2DSystem(GraphicSettings))
                .RegisterSystem(new SpriteSystem(GraphicSettings))
                .RegisterSystem(new CameraSystem(GraphicSettings))
                .Build();
        }

        protected override void LoadContent()
        {
            var texture = TextureFactory.CreateRectangle(1, 1, Color4.Black);
            var rand = new Random();

            _cameraId = _world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = Vector3.UnitZ * 20f,
                    Rotation = Vector3.UnitY * MathHelper.Pi
                })
                .Attach(new Camera()
                {
                })
            .Id;

            _playerId = _world.CreateEntity()
                .Attach(new Transform())
                .Attach(new SpriteRenderer(texture))
                .Attach(new Collider2D(Shapes2D.Rectangle))
                .Id;

            for (int i = 0; i < 10; i++)
            {
                _world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = new Vector3(rand.Next(-9, 9), rand.Next(-9, 9), 0f)
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.White
                })
                .Attach(new Collider2D(Shapes2D.Rectangle));
            }
        }

        protected override void Update(GameTime time)
        {
            ref var transform = ref _world.GetComponent<Transform>(_playerId);

            if (KeyboardState!.IsKeyDown(Keys.W))
                transform.Translate(Vector3.UnitY * PLAYER_RUN_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.A))
                transform.Translate(Vector3.UnitX * -PLAYER_RUN_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.S))
                transform.Translate(Vector3.UnitY * -PLAYER_RUN_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.D))
                transform.Translate(Vector3.UnitX * PLAYER_RUN_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.Up))
                transform.Translate(Vector3.UnitY * PLAYER_WALK_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.Left))
                transform.Translate(Vector3.UnitX * -PLAYER_WALK_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.Down))
                transform.Translate(Vector3.UnitY * -PLAYER_WALK_SPEED * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.Right))
                transform.Translate(Vector3.UnitX * PLAYER_WALK_SPEED * time.Delta);

            _world.Update(time);
        }

        protected override void Render(GameTime time)
        {
            ref var transform = ref _world.GetComponent<Transform>(_playerId);
            transform.Rotate(Vector3.UnitZ * PLAYER_RUN_SPEED * MouseState.ScrollDelta.Y * time.Delta);

            ClearScreen(Color4.CornflowerBlue);
            _world.Render(time);
        }
    }
}
