using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core
{
    public enum GameLoopMode
    {
        /// <summary>
        /// Game will call its update and render process at a fixed rate.
        /// </summary>
        FixedUpdateFixedRender,

        /// <summary>
        /// Game will call its update process at a fixed rate and will let GPU render as much as it can.
        /// </summary>
        FixedUpdateFreeRender,

        /// <summary>
        /// Game will call its render process at a fixed rate and will let CPU update logic as much as it can.
        /// </summary>
        FreeUpdateFixedRender,

        /// <summary>
        /// Game will let CPU and GPU manage update and render process as much as they can.
        /// </summary>
        FreeUpdateFreeRender,
    }
}
