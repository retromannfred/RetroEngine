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
        private readonly RetroEngineWindow _gameWindow;

        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title { get => _gameWindow.Title; set => _gameWindow.Title = value; }

        /// <summary>
        /// Gets the graphic settings of this
        /// </summary>
        public GraphicSettings GraphicSettings { get; private set; }

        /// <summary>
        /// Gets the keyboard state of the game.
        /// </summary>
        public KeyboardState KeyboardState { get => _gameWindow.KeyboardState; }

        /// <summary>
        /// Gets the mouse state of the game.
        /// </summary>
        public MouseState MouseState { get => _gameWindow.MouseState; }

        /// <summary>
        /// Gets the game controllers state of the game.
        /// </summary>
        public IReadOnlyList<JoystickState> JoystickStates { get => _gameWindow.JoystickStates; }

        /// <summary>
        /// Gets or sets maximum of updates the CPU will be doing if there is a restricted update.
        /// </summary>
        public float TargetUPS { get => _gameWindow.TargetUPS; set => _gameWindow.TargetUPS = value; }

        /// <summary>
        /// Gets or sets maximum of frames the GPU will be rendering if there is a restricted render.
        /// </summary>
        public float TargetFPS { get => _gameWindow.TargetFPS; set => _gameWindow.TargetFPS = value; }

        /// <summary>
        /// Creates a new game window.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="initialWindowWidth"></param>
        /// <param name="initialWindowHeight"></param>
        public Game(string title, int initialWindowWidth, int initialWindowHeight)
        {
            GraphicSettings = new GraphicSettings(initialWindowWidth, initialWindowHeight);

            var nativeWindowSettings = NativeWindowSettings.Default;
            nativeWindowSettings.ClientSize = new Vector2i(GraphicSettings.Width, GraphicSettings.Height);

            _gameWindow = new(GameWindowSettings.Default, NativeWindowSettings.Default)
            {
                Title = title
            };

            GameTime gameTime = new();

            GL.Enable(EnableCap.DepthTest);
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
        /// Finalizes the window process.
        /// </summary>
        public void Close()
        {
            _gameWindow.Close();
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