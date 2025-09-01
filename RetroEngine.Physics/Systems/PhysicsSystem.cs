using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines a system that updates the transform component of an entity that applies physics with a body.
    /// </summary>
    /// <param name="gravity">Gravity of the world.</param>
    public class PhysicsSystem(Vector2 gravity) : UpdateSystem(Contract
            .Include<Transform>()
            .Include<RigidBody2D>())
    {
        private Vector2 _gravity = gravity;

        /// <summary>
        /// Creates a new physics system with no gravity.
        /// </summary>
        public PhysicsSystem() : this(Vector2.Zero) { }

        /// <inheritdoc/>
        public override void Process(World world, GameTime time)
        {
            foreach (var entity in GetEntities())
            {
                ref var transform = ref world.GetComponent<Transform>(entity);
                ref var body = ref world.GetComponent<RigidBody2D>(entity);

                body.LinearVelocity *= 1f - body.LinearDrag * time.Delta;
                body.LinearVelocity += _gravity * time.Delta * body.GravityScale;
                transform.Translate(new Vector3(body.LinearVelocity.X * time.Delta, body.LinearVelocity.Y * time.Delta, 0));
            }
        }
    }
}
