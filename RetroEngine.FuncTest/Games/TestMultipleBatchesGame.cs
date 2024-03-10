using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroEngine.Core;
using RetroEngine.Ecs.Components;
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
        private SpriteBatch _batching;
        private SpriteBatch _batching2;
        private World _world;
        private Texture _texture;
        private Texture _texture2;
        private float _lastUpdate;
        private float _rotation;

        public const float SPEED = 0f;// MathHelper.Pi;

        float _cameraSpeed = 15f;

        Vector3 position = Vector3.UnitZ * 10f;
        Vector3 front = -Vector3.UnitZ;
        Vector3 up = Vector3.UnitY;

        public TestMultipleBatchesGame()
            : base("Test multiple batching", 800, 600)
        {
        }

        protected override void Initialize()
        {
            _world = new WorldBuilder().Build();
        }

        protected override void LoadContent()
        {
            _texture = TextureFactory.CreateRectangle(1, 1, Color4.Blue);
            _texture2 = TextureFactory.CreateRectangle(1, 1, Color4.Red);
            _batching = new SpriteBatch(GraphicSettings, _texture);
            _batching2 = new SpriteBatch(GraphicSettings, _texture2);

            Random rand = new Random();
            for (int i = 1; i <= 10; i++)
            {
                var entity = _world.CreateEntity()
                    .Attach(new Transform()
                    {
                        Position = new Vector2((float)rand.NextDouble() * 10f - 5f, (float)rand.NextDouble() * 10f - 5f)
                    })
                    .Attach(new SpriteRenderer()
                    {
                        //Color = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1f),
                        LayerDepth = i % 2 * 10
                    });
            }
        }

        protected override void Update(GameTime gameTime)
        {
            _rotation += SPEED * gameTime.DeltaTime;
            foreach (var id in _world.GetAllEntityIDs())
            {
                ref var transform = ref _world.GetComponent<Transform>(id);
                transform.Rotation = _rotation;
            }

            var input = KeyboardState;
            var speed = _cameraSpeed * gameTime.DeltaTime;

            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                position += up * speed; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= up * speed; //Down
            }
        }

        protected override void Render(GameTime gameTime)
        {
            ClearScreen(Color4.CornflowerBlue);

            Matrix4 model = Matrix4.Identity;
            //Matrix4 model = Matrix4.CreateScale(100f); // pixels per unit
            //Matrix4 model = Matrix4.CreateTranslation(Vector3.UnitZ * -1f);
            //model.Transpose();

            //Matrix4 view = Matrix4.Identity;
            //Matrix4 view = Matrix4.LookAt(new Vector3(100f, 0f, -1f), new Vector3(100f, 0f, 0f), new Vector3(0f, 1f, 0f)); //CreateTranslation(Vector3.UnitZ * -10f); // camera position
            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            //view.Transpose();

            //Matrix4 projection = Matrix4.Identity;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3, GraphicSettings.AspectRatio, 0.3f, 100f);
            //Matrix4 projection = Matrix4.CreateOrthographic(GraphicSettings.Width, GraphicSettings.Height, 0.3f, 100f); // camera view
            //projection.Transpose();

            //_batching.Transformation = projection * view * model;

            _batching.Begin(model * view * projection);
            for (int i = 1; i <= 10; i += 2)
            {
                ref var transform = ref _world.GetComponent<Transform>(i);
                ref var renderer = ref _world.GetComponent<SpriteRenderer>(i);

                _batching.Draw(
                    transform.Position,
                    Vector2.Zero,
                    new Vector2(_texture.Width, _texture.Height),
                    renderer.Color,
                    transform.Rotation,
                    transform.Scale,
                    (renderer.Flip & Flip.X) == Flip.X,
                    (renderer.Flip & Flip.Y) == Flip.Y,
                    renderer.LayerDepth
                );
            }
            _batching.End();
            _batching2.Begin(model * view * projection);
            for (int i = 2; i <= 10; i += 2)
            {
                ref var transform = ref _world.GetComponent<Transform>(i);
                ref var renderer = ref _world.GetComponent<SpriteRenderer>(i);

                _batching2.Draw(
                    transform.Position,
                    Vector2.Zero,
                    new Vector2(_texture2.Width, _texture2.Height),
                    renderer.Color,
                    transform.Rotation,
                    transform.Scale,
                    (renderer.Flip & Flip.X) == Flip.X,
                    (renderer.Flip & Flip.Y) == Flip.Y,
                    renderer.LayerDepth
                );
            }
            _batching2.End();

            _lastUpdate += gameTime.DeltaTime;
            if (_lastUpdate >= 1f)
            {
                Title = $"{(int)(1f / gameTime.DeltaTime)} FPS";
                _lastUpdate = 0f;
            }
        }
    }
}
