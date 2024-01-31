using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines functionallity for a game window and keyboard control.
    /// </summary>
    public abstract class Game
    {
        private GameWindowSettings _gameWindowSettings = GameWindowSettings.Default;
        private NativeWindowSettings _nativeWindowSettings = NativeWindowSettings.Default;

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the aspect ratio of the window (width / height).
        /// </summary>
        public float AspectRatio
        {
            get { return (float)Width / Height; }
        }

        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the keyboard state of the game.
        /// </summary>
        public KeyboardState? KeyboardState { get; private set; }

        /// <summary>
        /// Creates a new game window.
        /// </summary>
        /// <param name="title">Title of the window.</param>
        /// <param name="initialWindowWidth">Initial width of the window.</param>
        /// <param name="initialWindowHeight">Initial height of the window.</param>
        public Game(string title, int initialWindowWidth, int initialWindowHeight)
        {
            _nativeWindowSettings.ClientSize = new Vector2i(initialWindowWidth, initialWindowHeight);
            _nativeWindowSettings.Title = title;

            Title = title;
            Width = initialWindowWidth;
            Height = initialWindowHeight;
        }

        /// <summary>
        /// Runs the game logic.
        /// </summary>
        public void Run()
        {
            this.Initialize();

            using var gameWindow = new GameWindow(_gameWindowSettings, _nativeWindowSettings);
            GameTime gameTime = new();
            KeyboardState = gameWindow.KeyboardState;

            gameWindow.Load += LoadContent;
            gameWindow.UpdateFrame += (FrameEventArgs eventArgs) =>
            {
                gameWindow.Title = Title;
                gameTime.ElapsedGameTime = TimeSpan.FromSeconds(eventArgs.Time);
                gameTime.TotalGameTime += TimeSpan.FromSeconds(eventArgs.Time);
                Update(gameTime);
            };
            gameWindow.RenderFrame += (FrameEventArgs eventArgs) =>
            {
                Render(gameTime);
                gameWindow.SwapBuffers();
            };
            gameWindow.Resize += (ResizeEventArgs eventArgs) =>
            {
                GL.Viewport(0, 0, eventArgs.Width, eventArgs.Height);

                Width = eventArgs.Width;
                Height = eventArgs.Height;
            };
            gameWindow.Run();
        }

        /// <summary>
        /// Initializes the parameters for the game before the window is created.
        /// </summary>
        protected abstract void Initialize();
        
        /// <summary>
        /// Loads the content needed for the game loop.
        /// </summary>
        protected abstract void LoadContent();

        /// <summary>
        /// Updates the logic of the game.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        protected abstract void Update(GameTime gameTime);

        /// <summary>
        /// Renders graphics into the window.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        protected abstract void Render(GameTime gameTime);


        /// <summary>
        /// Clears the screen with a background color.
        /// </summary>
        /// <param name="color">Background color.</param>
        protected void ClearScreen(Color4 color)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(color);
        }
    }
}