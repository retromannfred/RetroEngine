namespace RetroEngine.Core.Exceptions
{
    /// <summary>
    /// Defines an exception involving a component.
    /// </summary>
    /// <param name="message">Message describing the error.</param>
    public class ComponentException(string message) : Exception(message) { }
}
