using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Graphics
{
    /// <summary>
    /// Defines constant buffer data information about vertices.
    /// </summary>
    internal static class FixedBufferData
    {
#pragma warning disable IDE0300 // Simplificar la inicialización de la recopilación
        public static float[] PositionAndTextureCoords = {
#pragma warning restore IDE0300 // Simplificar la inicialización de la recopilación
            //  [---Positions---] [Texcoords]
                -0.5f,  0.5f, 0f, 0f, 1f,
                 0.5f,  0.5f, 0f, 1f, 1f,
                 0.5f, -0.5f, 0f, 1f, 0f,
                -0.5f, -0.5f, 0f, 0f, 0f,
        };

#pragma warning disable IDE0300 // Simplificar la inicialización de la recopilación
        public static uint[] ElementBufferIndices = {
#pragma warning restore IDE0300 // Simplificar la inicialización de la recopilación
            0,1,2, // Triangle one
            2,3,0, // Triangle two
        };
    }
}
