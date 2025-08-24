using OpenTK.Mathematics;
using RetroEngine.Buddies.Helpers;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Core.Elements;
using RetroEngine.Physics;
using RetroEngine.Physics.Components;


namespace RetroEngine.Buddies.System
{
    /// <summary>
    /// Creates a render system that helps you to visualize collisions masks and their interactions.
    /// </summary>
    /// <param name="graphicSettings">Graphic settings of the game.</param>
    public class BuddyCollider2DSystem(GraphicSettings graphicSettings)
        : RenderSystem(Contract
            .Include<Transform>()
            .Include<Collider2D>())
    {
        private readonly GraphicSettings _graphicSettings = graphicSettings;
        private readonly RectangleColliderRenderer _rectangleRenderer = new();

        /// <inheritdoc/>
        public override void Process(World world, GameTime time)
        {
            foreach (var clipSpace in _graphicSettings.ClipSpaces)
            {
                foreach (var entityA in GetEntities())
                {
                    bool collided = false;

                    ref var transformA = ref world.GetComponent<Transform>(entityA);
                    ref var colliderA = ref world.GetComponent<Collider2D>(entityA);

                    foreach (var entityB in GetEntities())
                    {
                        if (entityA == entityB)
                            continue;

                        ref var transformB = ref world.GetComponent<Transform>(entityB);
                        ref var colliderB = ref world.GetComponent<Collider2D>(entityB);

                        if (Collider2D.Intersects(
                            transformA, colliderA,
                            transformB, colliderB,
                            out _, out _))
                        {
                            _rectangleRenderer.Draw(transformA, colliderA, clipSpace.View, clipSpace.Projection, (Vector4)Color4.Red);
                            break;
                        }
                    }

                    if (collided == false)
                        _rectangleRenderer.Draw(transformA, colliderA, clipSpace.View, clipSpace.Projection, new Vector4(0f, 1f, 0f, 1f));
                }
            }
        }
    }
}
