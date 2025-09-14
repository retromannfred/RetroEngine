using RetroEngine.Core;


namespace RetroEngine.Graphics
{
    /// <summary>
    /// Defines extension method for the WorldBuilder class.
    /// </summary>
    public static class WorldBuilderExtensions
    {
        /// <summary>
        /// Adds SpriteSystem and CameraSystem to the world builder.
        /// </summary>
        /// <param name="worldBuilder">World builder to add the components.</param>
        /// <param name="graphicSettings">Graphic settings of the game.</param>
        /// <returns>Same world builder.</returns>
        public static WorldBuilder RegisterGraphicsEngine(this WorldBuilder worldBuilder, GraphicSettings graphicSettings)
        {
            return worldBuilder
                .RegisterSystem(new SpriteSystem(graphicSettings))
                .RegisterSystem(new CameraSystem(graphicSettings));
        }
    }
}
