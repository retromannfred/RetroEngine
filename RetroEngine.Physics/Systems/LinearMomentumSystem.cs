using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines a system that updates linear velocities following the law of conservation of linear momentum.
    /// </summary>
    public class LinearMomentumSystem()
        : UpdateSystem(Contract
            .Include<Transform>()
            .Include<Collider2D>()
            .Include<LinearPhysics2D>()
            .Include<RigidBody>())
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
                        ref var linearA = ref world.GetComponent<LinearPhysics2D>(entityA);
                        ref var bodyA = ref world.GetComponent<RigidBody>(entityA);

                        ref var linearB = ref world.GetComponent<LinearPhysics2D>(entityB);
                        ref var bodyB = ref world.GetComponent<RigidBody>(entityB);

                        transformA.Translate(new Vector3(direction) * -depth / 2f);
                        transformB.Translate(new Vector3(direction) * depth / 2f);

                        var sharedRestitution = Math.Min(colliderA.Restitution, colliderB.Restitution);
                        var massSum = bodyA.Mass + bodyB.Mass;
                        var velDiff = Vector2.Dot(linearA.Velocity - linearB.Velocity, direction) * direction;

                        linearA.Velocity -= ((1 + sharedRestitution) * bodyB.Mass) / massSum * velDiff;
                        linearB.Velocity += ((1 + sharedRestitution) * bodyA.Mass) / massSum * velDiff;
                    }
                }
            }
        }
    }
}
