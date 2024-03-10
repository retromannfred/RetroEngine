using OpenTK.Mathematics;
using RetroEngine.Graphics;
using RetroEngine.Graphics.Batching;
using RetroEngine.Core;
using RetroEngine.FuncTest.ECS;

namespace RetroEngine.FuncTest.Games
{
    internal class TestSpritePerformanceGame : Game
    {
        private Texture _texture;
        private SpriteBatch _spriteBatch;

        private List<Vector2> _positions;
        private List<Color4> _colors;
        private float _rotation;

        private const int NUMBER_OF_ITEMS = 10000;
        private const float SPEED = MathHelper.Pi;

        private float _lastUpdate = 0f;

        public TestSpritePerformanceGame() : base("Test game", 800, 600)
        {
            
        }

        protected override void Initialize()
        {
            _positions = new List<Vector2>();
            _colors = new List<Color4>();
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
            _spriteBatch = new SpriteBatch(GraphicSettings, _texture);

            Random rand = new Random();
            for (int i = 1; i <= NUMBER_OF_ITEMS; i++)
            {
                _positions.Add(new Vector2(rand.Next(-GraphicSettings.Width / 2, GraphicSettings.Width / 2), rand.Next(-GraphicSettings.Height / 2, GraphicSettings.Height / 2)));
                _colors.Add(new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1f));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //_rotation += SPEED * gameTime.DeltaTime;
        }

        protected override void Render(GameTime gameTime)
        {
            ClearScreen(Color4.CornflowerBlue);

            //_spriteBatch.Begin(Matrix4.CreateTranslation(Vector3.UnitZ * -10) * Matrix4.CreateOrthographic(GraphicSettings.Width, GraphicSettings.Height, 0.3f, 1000f));

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

            _lastUpdate += gameTime.DeltaTime;
            if (_lastUpdate >= 1f)
            {
                Title = $"{(int)(1f / gameTime.DeltaTime)} FPS";
                _lastUpdate = 0f;
            }
        }
    }
}
