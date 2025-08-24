using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Physics.Enums
{
    /// <summary>
    /// Defines types of physic bodies as how they interact in the world.
    /// </summary>
    public enum BodyType
    {
        /// <summary>
        /// A body that doesn't move and doesn't respond to interactions.
        /// </summary>
        Static,

        /// <summary>
        /// A massless body that moves and interacts with other bodies without using forces or mass.
        /// </summary>
        Kinetic,

        /// <summary>
        /// A body that that moves and interacts with all bodies and all forces at full simulation.
        /// </summary>
        Dynamic
    }
}
