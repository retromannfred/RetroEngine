using OpenTK.Mathematics;
using RetroEngine.Components;
using RetroEngine.Core;
using RetroEngine.Core.Batching;
using RetroEngine.ECS;
using RetroEngine.FuncTest.ECS;
using RetroEngine.Systems;

namespace RetroEngine.FuncTest.Games
{
    internal class TestSpriteRendererGame : Game
    {
        private World _world;
        private Texture _texture;

        private float _rotation;

        private const int NUMBER_OF_ITEMS = 100000;
        private const float SPEED = MathHelper.Pi;

        private float _lastUpdate = 0f;

        public TestSpriteRendererGame() : base("Test sprite renderer", 800, 600)
        {
            
        }

        protected override void Initialize()
        {
            _world = new WorldBuilder()
                .AddSystem(new SpriteRendererSystem(GraphicSettings))
                .Build();
        }

        protected override void LoadContent()
        {
            int width = 32;
            int height = 32;
            var data = new byte[width * height * 4];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 255;
            }

            _texture = new Texture(width, height, data);

            Random rand = new Random();
            for (int i = 1; i <= NUMBER_OF_ITEMS; i++)
            {
                _world.CreateEntity()
                    .Attach(new Transform()
                    {
                        Position = new Vector2(rand.Next(-GraphicSettings.Width / 2, GraphicSettings.Width / 2), rand.Next(-GraphicSettings.Height / 2, GraphicSettings.Height / 2))
                    })
                    .Attach(new SpriteRenderer()
                    {
                        SpriteId = _texture.Id,
                        Width = _texture.Width,
                        Height = _texture.Height,
                        Color = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1f)
                    });
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //_rotation += SPEED * gameTime.DeltaTime;
            _world.Update(gameTime);
        }

        protected override void Render(GameTime gameTime)
        {
            ClearScreen(Color4.CornflowerBlue);

            //_spriteBatch.Begin(Matrix4.CreateTranslation(Vector3.UnitZ * -10) * Matrix4.CreateOrthographic(this.Width, this.Height, 0.3f, 1000f));

            //for (int i = 0; i < NUMBER_OF_ITEMS; i++)
            //{
            //    _spriteBatch.Draw(
            //        _positions[i],
            //        Vector2.Zero,
            //        new Vector2(_texture.Width, _texture.Height),
            //        _colors[i],
            //        _rotation,
            //        Vector2.One * 1.0f,
            //        0f
            //        );
            //}
            //_spriteBatch.End();

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
