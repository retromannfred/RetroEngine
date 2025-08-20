using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using RetroEngine.Buddies.Helpers;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Core.Elements;
using RetroEngine.Physics;
using RetroEngine.Physics.Components;
using RetroEngine.Physics.Enums;


namespace RetroEngine.Buddies.System
{
    public class BuddyCollider2DSystem : RenderSystem
    {
        private GraphicSettings _graphicSettings;

        private RectangleCollisionRenderer _rectangleRenderer;

        public BuddyCollider2DSystem(GraphicSettings graphicSettings)
            : base(Contract
            .Include<Transform>()
            .Include<Collider2D>())
        {
            _graphicSettings = graphicSettings;
            _rectangleRenderer = new RectangleCollisionRenderer();
        }

        public override void Process(World world, GameTime time)
        {
            foreach (var clipSpace in _graphicSettings.ClipSpaces)
            {
                foreach (var entityA in GetEntities())
                {
                    ref var transformA = ref world.GetComponent<Transform>(entityA);
                    ref var colliderA = ref world.GetComponent<Collider2D>(entityA);

                    foreach (var entityB in GetEntities())
                    {
                        if (entityA == entityB)
                            continue;

                        ref var transformB = ref world.GetComponent<Transform>(entityB);
                        ref var colliderB = ref world.GetComponent<Collider2D>(entityB);

                        if (Intersects(
                            transformA, colliderA,
                            transformB, colliderB,
                            out Vector2 direction, out float depth))
                        {
                            _rectangleRenderer.Draw(transformA, colliderA, clipSpace.View, clipSpace.Projection, (Vector4)Color4.Red);
                            break;
                        }
                    }
                }
            }
        }

        private bool Intersects(
            Transform transformA, Collider2D colliderA,
            Transform transformB, Collider2D colliderB,
            out Vector2 direction, out float depth)
        {
            direction = Vector2.Zero;
            depth = 0;

            if (colliderA.Shape == Shapes2D.Circle && colliderB.Shape == Shapes2D.Circle)
            {
                return CollisionMath.IntersectCircles(
                    transformA.Position.Xy + colliderA.Offset, colliderA.Radius,
                    transformB.Position.Xy + colliderB.Offset, colliderB.Radius,
                    out direction, out depth);
            }
            else if (colliderA.Shape == Shapes2D.Rectangle && colliderB.Shape == Shapes2D.Rectangle)
            {
                var verticesA = CollisionMath.GetRectangleVertices(transformA, colliderA);
                var verticesB = CollisionMath.GetRectangleVertices(transformB, colliderB);

                return CollisionMath.IntersectPolygons(verticesA, verticesB, out direction, out depth);
            }
            
            return false;
        }
    }
}
