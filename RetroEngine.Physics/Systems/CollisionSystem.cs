using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Core.Elements;
using RetroEngine.Physics.Components;
using RetroEngine.Physics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RetroEngine.Physics.Systems
{
    public class CollisionSystem()
        : UpdateSystem(Contract
            .Include<Transform>()
            .Include<RigidBody2D>()
            .Include<Collider2D>())
    {
        public override void Process(World world, GameTime time)
        {
            foreach (var entityA in GetEntities())
            {
                ref var transformA = ref world.GetComponent<Transform>(entityA);
                ref var bodyA = ref world.GetComponent<RigidBody2D>(entityA);
                ref var colliderA = ref world.GetComponent<Collider2D>(entityA);

                foreach (var entityB in GetEntities())
                {
                    if (entityA >= entityB)
                        continue;

                    ref var transformB = ref world.GetComponent<Transform>(entityB);
                    ref var bodyB = ref world.GetComponent<RigidBody2D>(entityB);
                    ref var colliderB = ref world.GetComponent<Collider2D>(entityB);

                    if (CollisionMath.Intersects(
                        transformA, colliderA,
                        transformB, colliderB,
                        out Vector2 direction, out float depth))
                    {
                        transformA.Translate(new Vector3(direction) * -depth / 2f);
                        transformB.Translate(new Vector3(direction) * depth / 2f);

                        var velA = bodyA.LinearVelocity;
                        var velB = bodyB.LinearVelocity;

                        var sharedRestitution = Math.Min(colliderA.Restitution, colliderB.Restitution);
                        var massSum = bodyA.Mass + bodyB.Mass;
                        var velDiff = Vector2.Dot(velA - velB, direction) * direction;

                        bodyA.LinearVelocity = velA - ((1 + sharedRestitution) * bodyB.Mass) / massSum * velDiff;
                        bodyB.LinearVelocity = velB + ((1 + sharedRestitution) * bodyA.Mass) / massSum * velDiff;
                    }
                }
            }
        }
    }
}
