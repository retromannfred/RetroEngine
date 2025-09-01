using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroEngine.Buddies.System;
using RetroEngine.Core;
using RetroEngine.Graphics;
using RetroEngine.Physics;

namespace RetroEngine.FuncTest.Games
{
    /// <summary>
    /// Game to test how the physics system works.
    /// </summary>
    internal class TestPhysicsGame : Game
    {
        private const float PLAYER_ACCELERATION = 6f;

        private readonly World _world;
        private int _playerOneId = 0;
        private int _playerTwoId = 0;

        public TestPhysicsGame()
            : base("Test collisions", 800, 600)
        {
            Console.WriteLine();
            Console.WriteLine("You should be seeing two circles, one green, one yellow.");
            Console.WriteLine("Use WASD keys to apply force to green circle.");
            Console.WriteLine("Use up-left-down-right arrow keys to apply force to yellow circle.");
            Console.WriteLine();

            _world = new WorldBuilder()
                .RegisterSystem(new BuddyCollider2DSystem(GraphicSettings))
                .RegisterSystem(new SpriteSystem(GraphicSettings))
                .RegisterSystem(new CameraSystem(GraphicSettings))
                .RegisterSystem(new PhysicsSystem(Vector2.UnitY * -9.8f))
                .RegisterSystem(new CollisionSystem())
                .Build();
        }

        protected override void LoadContent()
        {
            var texture = TextureFactory.CreateCircle(100, Color4.White);

            _world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = Vector3.UnitZ * 15f
                })
                .Attach(new Camera() { Projection = ProjectionType.Orthographic });

            _playerOneId = _world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitX * -3 })
                .Attach(new SpriteRenderer(texture) { Color = Color4.Green })
                .Attach(new RigidBody2D() { Mass = 1f, GravityScale = 0f })
                .Attach(new Collider2D(Shape2D.Circle) { Restitution = 1f })
                .Id;

            _playerTwoId = _world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitX * 3 })
                .Attach(new SpriteRenderer(texture) { Color = Color4.Yellow })
                .Attach(new RigidBody2D() { Mass = 1f, GravityScale = 0f })
                .Attach(new Collider2D(Shape2D.Circle) { Restitution = 1f })
                .Id;
        }

        protected override void Update(GameTime time)
        {
            ref var transformA = ref _world.GetComponent<Transform>(_playerOneId);
            ref var bodyA = ref _world.GetComponent<RigidBody2D>(_playerOneId);

            if (KeyboardState!.IsKeyDown(Keys.W))
                bodyA.ApplyForce(Vector2.UnitY * PLAYER_ACCELERATION * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.A))
                bodyA.ApplyForce(-Vector2.UnitX * PLAYER_ACCELERATION * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.S))
                bodyA.ApplyForce(-Vector2.UnitY * PLAYER_ACCELERATION * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.D))
                bodyA.ApplyForce(Vector2.UnitX * PLAYER_ACCELERATION * time.Delta);

            ref var transformB = ref _world.GetComponent<Transform>(_playerTwoId);
            ref var bodyB = ref _world.GetComponent<RigidBody2D>(_playerTwoId);

            if (KeyboardState!.IsKeyDown(Keys.Up))
                bodyB.ApplyForce(Vector2.UnitY * PLAYER_ACCELERATION * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.Left))
                bodyB.ApplyForce(-Vector2.UnitX * PLAYER_ACCELERATION * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.Down))
                bodyB.ApplyForce(-Vector2.UnitY * PLAYER_ACCELERATION * time.Delta);

            if (KeyboardState!.IsKeyDown(Keys.Right))
                bodyB.ApplyForce(Vector2.UnitX * PLAYER_ACCELERATION * time.Delta);

            _world.Update(time);
        }

        protected override void Render(GameTime time)
        {
            ClearScreen(Color4.CornflowerBlue);
            _world.Render(time);
        }
    }
}
