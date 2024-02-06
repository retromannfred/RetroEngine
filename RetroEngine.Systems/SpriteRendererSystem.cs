using OpenTK.Mathematics;
using RetroEngine.Components;
using RetroEngine.Core;
using RetroEngine.Core.Batching;
using RetroEngine.Core.Settings;
using RetroEngine.ECS.Elements;
using RetroEngine.ECS.Managers;
using System.Transactions;

namespace RetroEngine.Systems
{
    public class SpriteRendererSystem : RenderSystem
    {
        private GraphicSettings _graphicSettings;
        private Dictionary<int, SpriteBatch> _batches;

        public SpriteRendererSystem(GraphicSettings graphicSettings)
            : base(Aspect.All<Transform>().All<SpriteRenderer>())
        {
            _graphicSettings = graphicSettings;
            _batches = new Dictionary<int, SpriteBatch>();
        }

        public override void Render(GameTime gameTime)
        {
            var entityCount = ActiveEntities.Count;

            foreach (var entity in ActiveEntities)
            {
                ref var transform = ref World.GetComponent<Transform>(entity);
                ref var spriteRenderer = ref World.GetComponent<SpriteRenderer>(entity);

                if( _batches.Keys.Contains(spriteRenderer.SpriteId) == false)
                {
                    _batches.Add(spriteRenderer.SpriteId, new SpriteBatch(new Texture(spriteRenderer.SpriteId)));
                }

                _batches[spriteRenderer.SpriteId].Draw(
                    transform.Position,
                    Vector2.Zero,
                    new Vector2(spriteRenderer.Width, spriteRenderer.Height),
                    spriteRenderer.Color,
                    transform.Rotation,
                    transform.Scale,
                    spriteRenderer.LayerDepth
                );
            }

            foreach (var batch in _batches.Values)
            {
                batch.Begin(Matrix4.CreateTranslation(Vector3.UnitZ * -10) * Matrix4.CreateOrthographic(_graphicSettings.Width, _graphicSettings.Height, 0.3f, 1000f));
                batch.End();
            }
        }
    }
}