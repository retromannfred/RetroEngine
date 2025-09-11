using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Updates entities with bodies that collides with another body.
    /// </summary>
    public class CollisionResolutionSystem()
        : UpdateSystem(Contract
            .Include<Transform>()
            .Include<Collider2D>())
    {
        /// <inheritdoc/>
        public override void Process(World world, GameTime time)
        {
            foreach (var entityA in GetEntities())
            {
                ref var transformA = ref world.GetComponent<Transform>(entityA);
                ref var colliderA = ref world.GetComponent<Collider2D>(entityA);

                foreach (var entityB in GetEntities())
                {
                    if (entityA >= entityB)
                        continue;

                    ref var transformB = ref world.GetComponent<Transform>(entityB);
                    ref var colliderB = ref world.GetComponent<Collider2D>(entityB);

                    if (Collider2D.Intersects(
                        transformA, colliderA,
                        transformB, colliderB,
                        out Vector2 direction, out float depth))
                    {
                        transformA.Translate(new Vector3(direction) * -depth / 2f);
                        transformB.Translate(new Vector3(direction) * depth / 2f);
                    }
                }
            }
        }
    }
}
