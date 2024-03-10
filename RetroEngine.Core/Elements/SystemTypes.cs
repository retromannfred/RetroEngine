using RetroEngine.Core.Mapping;
using RetroEngine.Graphics;

namespace RetroEngine.Core.Elements
{
    /// <summary>
    /// Defines a system called on game updating.
    /// </summary>
    public interface IUpdateSystem
    {
        /// <summary>
        /// Updates entities filtered in this system.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        void Update(GameTime gameTime);
    }

    /// <summary>
    /// Implements a system called on game updating.
    /// </summary>
    public abstract class UpdateSystem : System, IUpdateSystem
    {
        /// <summary>
        /// Creates a new update system.
        /// </summary>
        /// <param name="builder">AspectBuilder for component restriction.</param>
        protected UpdateSystem(AspectBuilder builder) : base(builder) { }

        /// <summary>
        /// Updates entities filtered in this system.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        public abstract void Update(GameTime gameTime);
    }

    /// <summary>
    /// Defines a system called on game rendering.
    /// </summary>
    public interface IRenderSystem
    {
        /// <summary>
        /// Renders entities filtered in this system.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        void Render(GameTime gameTime);
    }

    /// <summary>
    /// Implements a system called on game rendering.
    /// </summary>
    public abstract class RenderSystem : System, IRenderSystem
    {
        /// <summary>
        /// Creates a new render system.
        /// </summary>
        /// <param name="builder">AspectBuilder for component restriction.</param>
        protected RenderSystem(AspectBuilder builder) : base(builder) { }

        /// <summary>
        /// Renders entities filtered in this system.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        public abstract void Render(GameTime gameTime);
    }
}
