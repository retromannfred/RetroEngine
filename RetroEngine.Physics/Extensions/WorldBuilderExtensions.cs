using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines extension method for the WorldBuilder class.
    /// </summary>
    public static class WorldBuilderExtensions
    {
        /// <summary>
        /// Adds CollisionResolutionSystem and LinearTranslationSystem to the world builder.
        /// </summary>
        /// <param name="worldBuilder">World builder to add systems.</param>
        /// <returns>Same world builder.</returns>
        public static WorldBuilder RegisterKineticPhysicsEngine(this WorldBuilder worldBuilder)
        {
            return worldBuilder
                .RegisterSystem(new CollisionResolutionSystem())
                .RegisterSystem(new LinearTranslationSystem());
        }

        /// <summary>
        /// Adds CollisionResolution, LinearTranslation, LinearMomentum and Gravity systems to the world builder.
        /// </summary>
        /// <param name="worldBuilder">World builder to add systems.</param>
        /// <param name="gravity">Gravity of the world.</param>
        /// <returns>Same world builder.</returns>
        public static WorldBuilder RegisterDynamicPhysicsEngine(this WorldBuilder worldBuilder, Vector2 gravity)
        {
            return worldBuilder
                .RegisterSystem(new CollisionResolutionSystem())
                .RegisterSystem(new LinearTranslationSystem())
                .RegisterSystem(new LinearMomentumSystem())
                .RegisterSystem(new GravitySystem());
        }
    }
}
