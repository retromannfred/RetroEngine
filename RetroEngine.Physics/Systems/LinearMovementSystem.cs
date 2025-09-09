using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines a system that updates the transform component of an entity applying its linear kinetic physics.
    /// </summary>
    public class LinearMovementSystem() : UpdateSystem(Contract
            .Include<Transform>()
            .Include<LinearPhysics2D>())
    {
        /// <inheritdoc/>
        public override void Process(World world, GameTime time)
        {
            foreach (var entity in GetEntities())
            {
                ref var transform = ref world.GetComponent<Transform>(entity);
                ref var linear = ref world.GetComponent<LinearPhysics2D>(entity);

                linear.Velocity *= 1f - linear.Drag * time.Delta;
                transform.Translate(new Vector3(linear.Velocity.X * time.Delta, linear.Velocity.Y * time.Delta, 0));
            }
        }
    }
}
