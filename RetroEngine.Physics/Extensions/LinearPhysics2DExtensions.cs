namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines functions for LinearPhysics2D component.
    /// </summary>
    public static class LinearPhysics2DExtensions
    {
        /// <summary>
        /// Freezes the horizontal movement of the component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        public static void FreezeHorizontally(ref this LinearPhysics2D component)
        {
            component.FreezedMovement |= FreezedMovement.Horizontal;
        }

        /// <summary>
        /// Freezes the vertical movement of the component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        public static void FreezeVertically(ref this LinearPhysics2D component)
        {
            component.FreezedMovement |= FreezedMovement.Vertical;
        }

        /// <summary>
        /// Unfreezes the horizontal movement of the component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        public static void UnfreezeHorizontally(ref this LinearPhysics2D component)
        {
            component.FreezedMovement &= ~FreezedMovement.Horizontal;
        }

        /// <summary>
        /// Unfreezes the vertical movement of the component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        public static void UnfreezeVertically(ref this LinearPhysics2D component)
        {
            component.FreezedMovement &= ~FreezedMovement.Vertical;
        }

        /// <summary>
        /// Toggles the freeze on the horizontal axis of the component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        public static void ToggleHorizontalFreeze(ref this LinearPhysics2D component)
        {
            component.FreezedMovement ^= FreezedMovement.Horizontal;
        }

        /// <summary>
        /// Toggles the freeze on the vertical axis of the component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        public static void ToggleVertialFreeze(ref this LinearPhysics2D component)
        {
            component.FreezedMovement ^= FreezedMovement.Vertical;
        }
    }
}
