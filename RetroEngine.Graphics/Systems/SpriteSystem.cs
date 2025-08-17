using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Core.Elements;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Components;

namespace RetroEngine.Ecs.Systems
{
    /// <summary>
    /// Renders sprites into the game screen.
    /// </summary>
    public class SpriteSystem : RenderSystem
    {
        private GraphicSettings _graphicSettings;
        private Dictionary<string, SpriteBatch> _batches;

        /// <summary>
        /// Creates a new sprite renderer system.
        /// </summary>
        /// <param name="graphicSettings">Graphic settings of the game.</param>
        public SpriteSystem(GraphicSettings graphicSettings)
            : base(Contract.Include<Transform>().Include<SpriteRenderer>())
        {
            _graphicSettings = graphicSettings;
            _batches = [];
        }

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
                        if (_batches.ContainsKey(batchKey))
                        {
                            lastBatch = _batches[batchKey];
                        }
                        else
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