using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Batching;
using RetroEngine.ECS;
using RetroEngine.FuncTest.ECS;

namespace RetroEngine.FuncTest.Games
{
    internal class TestSpriteBatchGame : Game
    {
        private Texture _texture;
        private SpriteBatch _spriteBatch;

        private float _lastUpdate = 0f;

        public TestSpriteBatchGame() : base("Test game", 800, 600)
        {
        }

        protected override void Initialize()
        {

        }

        protected override void LoadContent()
        {
            _texture = TextureFactory.CreateFromFile("Sprites/person.png");
            _spriteBatch = new SpriteBatch(_texture);
        }

        protected override void Update(GameTime gameTime)
        {
            
        }

        protected override void Render(GameTime gameTime)
        {
            ClearScreen(Color4.CornflowerBlue);

            _spriteBatch.Begin(Matrix4.CreateTranslation(Vector3.UnitZ * -10) * Matrix4.CreateOrthographic(this.Width, this.Height, 0.3f, 1000f));

            _spriteBatch.Draw(new Vector2(3f, 3f), Vector2.Zero, new Vector2(_texture.Width, _texture.Height), Color4.White, 0.1f, Vector2.One * 10, 0f);
            //_spriteBatch.Draw(Vector3.One * -10, new Vector2(_texture.Width / 2, _texture.Height / 2), new Vector2(_texture.Width / 2, _texture.Height / 2), ((Vector4)Color4.White), 0f, Vector2.One);

            _spriteBatch.End();
        }
    }
}
