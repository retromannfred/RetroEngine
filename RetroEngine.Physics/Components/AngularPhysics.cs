using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines an angular movement.
    /// </summary>
    public struct AngularPhysics()
    {
        private float _angularDrag = .05f;

        /// <summary>
        /// Gets or sets the angular velocity of the entity.
        /// </summary>
        public float AngularVelocity { get; set; } = 0;

        /// <summary>
        /// Gets or sets the angular drag of the entity.
        /// </summary>
        /// <remarks>Set value will be clamped between 0 and 1.</remarks>
        public float AngularDrag { readonly get => _angularDrag; set => _angularDrag = Math.Clamp(value, 0, 1); }
    }
}
