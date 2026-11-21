namespace HrSystem.Domain.Common;

/// <summary>
/// Thrown when an operation would violate a business rule,
/// e.g. editing a timesheet that has already been submitted.
/// </summary>
public class DomainException(string message) : Exception(message);
