namespace RetroEngine.Graphics.Batching
{
    /// <summary>
    /// Defines constant buffer data information about vertices.
    /// </summary>
    internal static class FixedSpriteBatchBufferData
    {
        public static float[] PositionAndTextureCoords = {
            //  [---Positions---] [Texcoords]
                -0.5f,  0.5f, 0f, 0f, 1f,
                 0.5f,  0.5f, 0f, 1f, 1f,
                 0.5f, -0.5f, 0f, 1f, 0f,
                -0.5f, -0.5f, 0f, 0f, 0f,
        };

        public static uint[] ElementBufferIndices = {
            0,1,2, // Triangle one
            2,3,0, // Triangle two
        };
    }
}
