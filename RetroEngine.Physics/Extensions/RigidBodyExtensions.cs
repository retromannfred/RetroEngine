using OpenTK.Mathematics;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines functions for RigidBody component.
    /// </summary>
    public static class RigidBodyExtensions
    {
        /// <summary>
        /// Applies a force to this body, modifying its linear physics 2D component.
        /// </summary>
        /// <param name="body">This component.</param>
        /// <param name="linear">Linear physics 2D component of the same entity.</param>
        /// <param name="force">Force vector to apply.</param>
        public static void ApplyForce(this RigidBody body, ref LinearPhysics2D linear, Vector2 force)
        {
            linear.Velocity += force / body.Mass;
        }
    }
}
