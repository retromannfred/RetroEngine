using RetroEngine.Core;
using RetroEngine.ECS.Managers;

namespace RetroEngine.ECS.Elements
{
    /// <summary>
    /// Defines a system called on game updating.
    /// </summary>
    public interface IUpdateSystem
    {
        void Update(GameTime gameTime);
    }

    /// <summary>
    /// Implements a system called on game updating.
    /// </summary>
    public abstract class UpdateSystem : System, IUpdateSystem
    {
        protected UpdateSystem(AspectBuilder builder) : base(builder) { }

        public abstract void Update(GameTime gameTime);
    }

    /// <summary>
    /// Defines a system called on game rendering.
    /// </summary>
    public interface IRenderSystem
    {
        void Render(GameTime gameTime);
    }

    /// <summary>
    /// Implements a system called on game rendering.
    /// </summary>
    public abstract class RenderSystem : System, IRenderSystem
    {
        protected RenderSystem(AspectBuilder builder) : base(builder) { }

        public abstract void Render(GameTime gameTime);
    }
}
