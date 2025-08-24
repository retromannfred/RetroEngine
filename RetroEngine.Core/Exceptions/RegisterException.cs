namespace RetroEngine.Core.Exceptions
{
    // <summary>
    /// Defines an exception involving a register method.
    /// </summary>
    /// <param name="message">Message describing the error.</param>
    public class RegisterException(string message) : Exception(message) { }
}
