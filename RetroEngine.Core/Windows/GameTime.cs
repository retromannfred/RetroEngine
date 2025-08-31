namespace RetroEngine.Core
{
    /// <summary>
    /// Defines time elapsed in a game.
    /// </summary>
    public struct GameTime
    {
        /// <summary>
        /// Gets total elapsed time in the game.
        /// </summary>
        public TimeSpan TotalGameTime { get; internal set; } = TimeSpan.Zero;

        /// <summary>
        /// Gets the time elapsed since the last update call.
        /// </summary>
        public TimeSpan ElapsedGameTime { get; internal set; } = TimeSpan.Zero;

        /// <summary>
        /// Gets se total seconds elapsed since the last update call.
        /// </summary>
        public readonly float Delta => (float) ElapsedGameTime.TotalSeconds;

        /// <summary>
        /// Creates an empty game time element.
        /// </summary>
        public GameTime() { }

        /// <summary>
        /// Creates a new game time element.
        /// </summary>
        /// <param name="totalGameTime"></param>
        /// <param name="elapsedGameTime"></param>
        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
        }
    }
}
