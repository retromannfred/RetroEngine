using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines functionallity for a game window and keyboard control.
    /// </summary>
    /// <param name="title">Title of the window.</param>
    /// <param name="initialWindowWidth">Initial width of the window.</param>
    /// <param name="initialWindowHeight">Initial height of the window.</param>
    public abstract class Game
    {
        private RetroEngineWindow _gameWindow;
        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the graphic settings of this
        /// </summary>
        public GraphicSettings GraphicSettings { get; private set; }

        /// <summary>
        /// Gets the keyboard state of the game.
        /// </summary>
        public KeyboardState? KeyboardState { get; private set; }

        /// <summary>
        /// Gets the mouse state of the game.
        /// </summary>
        public MouseState? MouseState { get; private set; }

        public Game(string title, int initialWindowWidth, int initialWindowHeight)
        {
            Title = title;
            GraphicSettings = new GraphicSettings(initialWindowWidth, initialWindowHeight);

            var nativeWindowSettings = NativeWindowSettings.Default;
            nativeWindowSettings.ClientSize = new Vector2i(GraphicSettings.Width, GraphicSettings.Height);

            _gameWindow = new (GameWindowSettings.Default, NativeWindowSettings.Default);
            _gameWindow.Title = Title;
            GameTime gameTime = new();

            KeyboardState = _gameWindow.KeyboardState;
            MouseState = _gameWindow.MouseState;

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _gameWindow.Load += LoadContent;
            _gameWindow.RetroUpdateFrame += eventArgs =>
            {
                gameTime.ElapsedGameTime = TimeSpan.FromSeconds(eventArgs.Time);
                gameTime.TotalGameTime += TimeSpan.FromSeconds(eventArgs.Time);
                Update(gameTime);
            };
            _gameWindow.RetroRenderFrame += eventArgs =>
            {
                Render(gameTime);
                _gameWindow.SwapBuffers();
            };
            _gameWindow.Resize += eventArgs =>
            {
                GL.Viewport(0, 0, eventArgs.Width, eventArgs.Height);
                GraphicSettings.Width = eventArgs.Width;
                GraphicSettings.Height = eventArgs.Height;
            };
        }

        /// <summary>
        /// Runs the game logic.
        /// </summary>
        public void Run()
        {
            _gameWindow.Run();
        }
        
        /// <summary>
        /// Loads the content needed for the game loop.
        /// </summary>
        protected abstract void LoadContent();

        /// <summary>
        /// Updates the logic of the game.
        /// </summary>
        /// <param name="time">Elapsed time of the game.</param>
        protected abstract void Update(GameTime time);

        /// <summary>
        /// Renders graphics into the window.
        /// </summary>
        /// <param name="time">Elapsed time of the game.</param>
        protected abstract void Render(GameTime time);


        /// <summary>
        /// Clears the screen with a background color.
        /// </summary>
        /// <param name="color">Screen background color.</param>
        protected static void ClearScreen(Color4 color)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(color);
        }
    }
}