using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private GameLoopMode _gameLoopMode = GameLoopMode.FreeUpdateFixedRender;

        private const double FIXED_UPDATE_RATE = 1.0 / 60.0; // 60 UPS
        private const double FIXED_RENDER_RATE = 1.0 / 60.0; // 60 FPS

        private double updateAccumulator = 0.0;
        private double timeSinceLastRender = 0.0;

        // Contadores para medir UPS/FPS
        private int updatesThisSecond = 0;
        private int framesThisSecond = 0;
        private double counterTimer = 0.0;
        private int UPS = 0;
        private int FPS = 0;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            switch (_gameLoopMode)
            {
                case GameLoopMode.FixedUpdateFixedRender:
                    FixedUpdate(e);
                    FixedRender(e);
                    break;
                case GameLoopMode.FixedUpdateFreeRender:
                    FixedUpdate(e);
                    FreeRender(e);
                    break;
                case GameLoopMode.FreeUpdateFixedRender:
                    FreeUpdate(e);
                    FixedRender(e);
                    break;
                case GameLoopMode.FreeUpdateFreeRender:
                    FreeUpdate(e);
                    FreeRender(e);
                    break;
                default:
                    break;
            }

            // Contador de UPS/FPS
            counterTimer += e.Time;
            if (counterTimer >= 1.0)
            {
                UPS = updatesThisSecond;
                FPS = framesThisSecond;
                updatesThisSecond = 0;
                framesThisSecond = 0;
                counterTimer = 0.0;
                Console.Write($"\rUPS: {UPS}, FPS: {FPS}");
            }
        }

        private void FixedUpdate(FrameEventArgs e)
        {
            updateAccumulator += e.Time;

            while (updateAccumulator >= FIXED_UPDATE_RATE)
            {
                RetroUpdateFrame?.Invoke(new FrameEventArgs(FIXED_UPDATE_RATE));
                updateAccumulator -= FIXED_UPDATE_RATE;
                updatesThisSecond++;
            }
        }

        private void FreeUpdate(FrameEventArgs e)
        {
            RetroUpdateFrame?.Invoke(new FrameEventArgs(FIXED_UPDATE_RATE));
            updatesThisSecond++;
        }

        private void FixedRender(FrameEventArgs e)
        {
            timeSinceLastRender += e.Time;
            if (timeSinceLastRender >= FIXED_RENDER_RATE)
            {
                RetroRenderFrame?.Invoke(new FrameEventArgs(timeSinceLastRender));
                timeSinceLastRender = 0.0;
                framesThisSecond++;
            }
        }

        private void FreeRender(FrameEventArgs e)
        {
            RetroRenderFrame?.Invoke(new FrameEventArgs(timeSinceLastRender));
            framesThisSecond++;
        }

        /// <summary>
        /// Occurs when it is time to update a frame.
        /// </summary>
        public event Action<FrameEventArgs>? RetroUpdateFrame;

        /// <summary>
        /// Occurs when it is time to render a frame.
        /// </summary>
        public event Action<FrameEventArgs>? RetroRenderFrame;
    }
}
