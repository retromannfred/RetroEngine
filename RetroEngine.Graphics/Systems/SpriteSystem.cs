using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Core.Elements;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Components;

namespace RetroEngine.Graphics.Systems
{
    /// <summary>
    /// Renders sprites into the game screen.
    /// </summary>
    /// <param name="graphicSettings">Graphic settings of the game.</param>
    public class SpriteSystem(GraphicSettings graphicSettings)
        : RenderSystem(Contract
            .Include<Transform>()
            .Include<SpriteRenderer>())
    {
        private readonly GraphicSettings _graphicSettings = graphicSettings;
        private readonly Dictionary<string, SpriteBatch> _batches = [];

        public override void Process(World world, GameTime time)
        {
            foreach (var clipSpace in _graphicSettings.ClipSpaces)
            {
                SpriteBatch? lastBatch = null;

                foreach (var entity in GetEntities())
                {
                    ref var transform = ref world.GetComponent<Transform>(entity);
                    ref var renderer = ref world.GetComponent<SpriteRenderer>(entity);

                    var batchKey = SpriteBatch.GetBatchKey(renderer.Texture);

                    if (lastBatch == null || lastBatch.BatchKey != batchKey)
                    {
                        if (_batches.TryGetValue(batchKey, out lastBatch) == false)
                        {
                            lastBatch = new SpriteBatch(renderer.Texture);
                            _batches.Add(batchKey, lastBatch);
                        }
                    }

                    lastBatch.UpdateSpriteData(transform, renderer);
                }

                foreach (var batch in _batches.Values)
                {
                    batch.Begin(clipSpace.View, clipSpace.Projection);
                    batch.DrawBatch();
                    batch.End();
                }
            }
        }
    }
}