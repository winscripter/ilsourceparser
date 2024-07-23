namespace ILSourceParser.Common;

/// <summary>
/// Represents an exception thrown when the security action is undefined.
/// </summary>
public class UnknownSecurityActionException : Exception
{
    /// <summary>
    /// The security action as a string which is not known.
    /// </summary>
    public string? Action { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownSecurityActionException"/> class.
    /// </summary>
    public UnknownSecurityActionException()
    {
        Action = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownSecurityActionException"/> class.
    /// </summary>
    /// <param name="message">Exception message &amp; the security action that wasn't defined.</param>
    public UnknownSecurityActionException(string? message) : base(message)
    {
        Action = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownSecurityActionException"/> class.
    /// </summary>
    /// <param name="message">Exception message &amp; the security action that wasn't defined.</param>
    /// <param name="innerException">Inner exception.</param>
    public UnknownSecurityActionException(string? message, Exception? innerException) : base(message, innerException)
    {
        Action = message;
    }
}
