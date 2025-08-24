namespace RetroEngine.Core.Exceptions
{
    // <summary>
    /// Defines an exception involving an entity.
    /// </summary>
    /// <param name="message">Message describing the error.</param>
    public class EntityException(string message) : Exception(message) { }
}
