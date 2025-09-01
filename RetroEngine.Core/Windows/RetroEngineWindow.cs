using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RetroEngine.UnitTest")]
namespace RetroEngine.Core
{
    /// <summary>
    /// Defines a wrapper to OpenTK GameWindow to have custom OnUpdateFrame() and OnRenderFrame() custom handle and behaviour.
    /// </summary>
    /// <param name="gameWindowSettings">The <see cref="GameWindow"/> related settings.</param>
    /// <param name="nativeWindowSettings">The <see cref="NativeWindow"/> related settings.</param>
    internal class RetroEngineWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : GameWindow(gameWindowSettings, nativeWindowSettings)
    {
        private readonly GameLoopMode _gameLoopMode = GameLoopMode.RestrictedUpdate;

        private float _updateRate = 1f / 60f;
        private float _renderRate = 1f / 60f;

        private double updateAccumulator = 0.0;
        private double timeSinceLastRender = 0.0;

        /// <summary>
        /// Gets or sets maximum of updates the CPU will be doing if there is a restricted update.
        /// </summary>
        public float TargetUPS { get => 1f / _updateRate; set => _updateRate = 1f / value; }

        /// <summary>
        /// Gets or sets maximum of frames the GPU will be rendering if there is a restricted render.
        /// </summary>
        public float TargetFPS { get => 1f / _renderRate; set => _renderRate = 1f / value; }

        /// <summary>
        /// Occurs when it is time to update a frame.
        /// </summary>
        public event Action<FrameEventArgs>? RetroUpdateFrame;

        /// <summary>
        /// Occurs when it is time to render a frame.
        /// </summary>
        public event Action<FrameEventArgs>? RetroRenderFrame;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            switch (_gameLoopMode)
            {
                case GameLoopMode.AllRestricted:
                    RestrictedUpdate(e);
                    FixedRender(e);
                    break;
                case GameLoopMode.RestrictedUpdate:
                    RestrictedUpdate(e);
                    FreeRender(e);
                    break;
                case GameLoopMode.RestrictedRender:
                    FreeUpdate(e);
                    FixedRender(e);
                    break;
                case GameLoopMode.AllFree:
                    FreeUpdate(e);
                    FreeRender(e);
                    break;
                default:
                    break;
            }
        }

        private void RestrictedUpdate(FrameEventArgs e)
        {
            updateAccumulator += e.Time;

            while (updateAccumulator >= _updateRate)
            {
                RetroUpdateFrame?.Invoke(new FrameEventArgs(_updateRate));
                updateAccumulator -= _updateRate;
            }
        }

        private void FreeUpdate(FrameEventArgs e)
        {
            RetroUpdateFrame?.Invoke(new FrameEventArgs(e.Time));
        }

        private void FixedRender(FrameEventArgs e)
        {
            timeSinceLastRender += e.Time;
            if (timeSinceLastRender >= _renderRate)
            {
                RetroRenderFrame?.Invoke(new FrameEventArgs(timeSinceLastRender));
                timeSinceLastRender = 0.0;
            }
        }

        private void FreeRender(FrameEventArgs e)
        {
            RetroRenderFrame?.Invoke(new FrameEventArgs(e.Time));
        }
    }
}
