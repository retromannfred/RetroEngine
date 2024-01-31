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

        public TestSpriteBatchGame() : base("Test game", 800, 600)
        {

        }

        protected override void Initialize()
        {
            
        }

        protected override void LoadContent()
        {
            _texture = new Texture("Sprites/person.png");
            _spriteBatch = new SpriteBatch(_texture);
        }

        protected override void Update(GameTime gameTime)
        {
            
        }

        protected override void Render(GameTime gameTime)
        {
            ClearScreen(Color4.CornflowerBlue);

            _spriteBatch.Begin(Matrix4.CreateOrthographic(this.Width, this.Height, 0.01f, 3000f));
            _spriteBatch.Draw(new Vector3(3f, 3f, -1f), Vector2.Zero, new Vector2(_texture.Width, _texture.Height), ((Vector4)Color4.White), 0.1f, Vector2.One);
            //_spriteBatch.Draw(Vector3.One * -10, new Vector2(_texture.Width / 2, _texture.Height / 2), new Vector2(_texture.Width / 2, _texture.Height / 2), ((Vector4)Color4.White), 0f, Vector2.One);
            _spriteBatch.End();
        }
    }
}
