using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines a system that updates linear velocity of an entity following a wold gravity.
    /// </summary>
    /// <param name="gravity">Gravity of the world.</param>
    public class GravitySystem(Vector2 gravity) : UpdateSystem(Contract
            .Include<Transform>()
            .Include<LinearPhysics2D>()
            .Include<RigidBody>())
    {
        private Vector2 _gravity = gravity;

        /// <summary>
        /// Creates a new physics system with no gravity.
        /// </summary>
        public GravitySystem() : this(Vector2.Zero) { }

        /// <inheritdoc/>
        public override void Process(World world, GameTime time)
        {
            foreach (var entity in GetEntities())
            {
                ref var transform = ref world.GetComponent<Transform>(entity);
                ref var linear = ref world.GetComponent<LinearPhysics2D>(entity);
                ref var body = ref world.GetComponent<RigidBody>(entity);

                linear.Velocity += _gravity * time.Delta * body.GravityScale;
                transform.Translate(new Vector3(linear.Velocity * time.Delta));
            }
        }
    }
}
