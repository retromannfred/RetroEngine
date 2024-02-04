using OpenTK.Mathematics;
using RetroEngine.Components;
using RetroEngine.Core;
using RetroEngine.Core.Batching;
using RetroEngine.ECS.Elements;
using RetroEngine.ECS.Managers;
using System.Transactions;

namespace RetroEngine.Systems
{
    public class SpriteRendererSystem : RenderSystem
    {
        private Dictionary<int, SpriteBatch> _batches;

        public SpriteRendererSystem()
            : base(Aspect.All<Transform>().All<SpriteRenderer>())
        {
            _batches = new Dictionary<int, SpriteBatch>();
        }

        public override void Render(GameTime gameTime)
        {
            var entityCount = ActiveEntities.Count;

            //var positions = new Vector3[entityCount * 4];
            //var textureCoords = new Vector3[entityCount * 4];
            //var colors = new Vector3[entityCount * 4];
            //var indices = new Vector3[entityCount * 6];

            foreach (var entity in ActiveEntities)
            {
                ref var transform = ref World.GetComponent<Transform>(entity);
                ref var spriteRenderer = ref World.GetComponent<SpriteRenderer>(entity);

                if( _batches.Keys.Contains(spriteRenderer.SpriteId) == false)
                {
                    _batches.Add(spriteRenderer.SpriteId, new SpriteBatch(new Texture(spriteRenderer.SpriteId)));
                }

                //_batches[spriteRenderer.SpriteId].Draw
            }
        }
    }
}