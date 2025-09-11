using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Enumerates all ways a linear movement can be freezed.
    /// </summary>
    public enum FreezedMovement
    {
        /// <summary>
        /// No movement is freezed in any axis.
        /// </summary>
        None = 0,

        /// <summary>
        /// Movement is freezed on the horizontal axis.
        /// </summary>
        Horizontal = 1,

        /// <summary>
        /// Movement is freezed on the vertical axis.
        /// </summary>
        Vertical = 2,

        /// <summary>
        /// Movement is freezed on all axis.
        /// </summary>
        Both = 3
    }
}
