namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines a rigid body phisics behaviour.
    /// </summary>
    public struct RigidBody()
    {
        /// <summary>
        /// Gets or sets the mass of the body.
        /// </summary>
        public float Mass { get; set; } = 1;

        /// <summary>
        /// Gets or sets the coeficient indicating how gravity affects the body.
        /// </summary>
        public float GravityScale { get; set; } = 1;
    }
}
