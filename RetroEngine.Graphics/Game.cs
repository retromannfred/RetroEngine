using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RetroEngine.Graphics.Settings;

namespace RetroEngine.Graphics
{
    /// <summary>
    /// Defines functionallity for a game window and keyboard control.
    /// </summary>
    public abstract class Game
    {
        /// <summary>
        /// Gets the graphic settings of this
        /// </summary>
        public GraphicSettings GraphicSettings { get; private set; }

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
            Title = title;
            GraphicSettings = new GraphicSettings(initialWindowWidth, initialWindowHeight);
        }

        /// <summary>
        /// Runs the game logic.
        /// </summary>
        public void Run()
        {
            this.Initialize();

            var nativeWindowSettings = NativeWindowSettings.Default;
            nativeWindowSettings.ClientSize = new Vector2i(GraphicSettings.Width, GraphicSettings.Height);

            using var gameWindow = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);
            GameTime gameTime = new();
            KeyboardState = gameWindow.KeyboardState;

            GL.Enable(EnableCap.DepthTest);

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
                GraphicSettings.Width = eventArgs.Width;
                GraphicSettings.Height = eventArgs.Height;
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