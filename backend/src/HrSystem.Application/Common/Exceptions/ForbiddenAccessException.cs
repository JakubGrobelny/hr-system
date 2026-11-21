namespace HrSystem.Application.Common.Exceptions;

/// <summary>
/// Thrown when an authenticated user tries to access data they are not allowed to see,
/// e.g. an employee requesting another employee's salary.
/// </summary>
public sealed class ForbiddenAccessException(string message = "You do not have permission to perform this operation.")
    : Exception(message);
