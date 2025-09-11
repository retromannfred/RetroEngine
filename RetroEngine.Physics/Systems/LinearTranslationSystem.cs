using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines a system that updates the transform component of an entity applying its linear kinetic physics.
    /// </summary>
    public class LinearTranslationSystem() : UpdateSystem(Contract
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

                if (linear.Drag > 0)
                    linear.Velocity *= MathF.Pow(1 - linear.Drag, time.Delta);

                var translateX = linear.Velocity.X * time.Delta;
                var translateY = linear.Velocity.Y * time.Delta;

                transform.Translate(
                    new Vector3(
                        (linear.FreezedMovement & FreezedMovement.Horizontal) == FreezedMovement.Horizontal ? 0 : translateX,
                        (linear.FreezedMovement & FreezedMovement.Vertical) == FreezedMovement.Vertical ? 0 : translateY,
                        0
                    )
                );
            }
        }
    }
}
