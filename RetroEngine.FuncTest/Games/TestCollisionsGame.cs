using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroEngine.Buddies.System;
using RetroEngine.Core;
using RetroEngine.Graphics;
using RetroEngine.Physics;

namespace RetroEngine.FuncTest.Games
{
    /// <summary>
    /// Game to test how collision system works.
    /// </summary>
    internal class TestCollisionsGame : Game
    {
        private const float PLAYER_WALK_SPEED = 3f;
        private const float PLAYER_RUN_SPEED = 10f;
        private const float CAMERA_SENSITIVITY = .6f;

        private readonly World _world;

        private int _cameraId = 0;
        private int _playerId = 0;

        public TestCollisionsGame()
            : base("Test collisions", 800, 600)
        {
            Console.WriteLine();
            Console.WriteLine("Move the cirecle with WASD keys.");
            Console.WriteLine("You will see colliders as green lines.");
            Console.WriteLine("When two object are colliding, collider lines will turn red.");
            Console.WriteLine();

            _world = new WorldBuilder()
                .RegisterSystem(new BuddyCollider2DSystem(GraphicSettings))
                .RegisterSystem(new SpriteSystem(GraphicSettings))
                .RegisterSystem(new CameraSystem(GraphicSettings))
                .Build();
        }

        protected override void LoadContent()
        {
            var texture = TextureFactory.CreateCircle(100, Color4.White);
            var rand = new Random();

            _cameraId = _world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = Vector3.UnitZ * 20f
                })
                .Attach(new Camera()
                {
                    Projection = ProjectionType.Perspective
                })
            .Id;

            _playerId = _world.CreateEntity()
                .Attach(new Transform() { Scale = Vector3.One * 5f})
                .Attach(new SpriteRenderer(texture) { Color = Color4.Blue })
                .Attach(new Collider2D() { Shape = Shape2D.Circle })
                .Id;

            for (int i = 0; i < 10; i++)
            {
                _world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = new Vector3(rand.Next(-8, 8), rand.Next(-8, 8), 0f)
                })
                .Attach(new SpriteRenderer(texture)
                {
                    Color = Color4.White
                })
                .Attach(new Collider2D() { Shape = Shape2D.Circle });
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
            ref var playerTransform = ref _world.GetComponent<Transform>(_playerId);
            playerTransform.Rotate(Vector3.UnitZ * PLAYER_RUN_SPEED * MouseState!.ScrollDelta.Y * time.Delta);

            ref var cameraTransform = ref _world.GetComponent<Transform>(_cameraId);
            ref var camera = ref _world.GetComponent<Camera>(_cameraId);

            if (MouseState!.IsButtonDown(MouseButton.Left))
            {
                cameraTransform = camera.LookUp(cameraTransform, (MouseState!.Delta.Y) * CAMERA_SENSITIVITY * time.Delta);
                cameraTransform = camera.LookRight(cameraTransform, (MouseState!.Delta.X) * CAMERA_SENSITIVITY * time.Delta);
            }

            ClearScreen(Color4.CornflowerBlue);
            _world.Render(time);
        }
    }
}
