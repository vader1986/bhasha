namespace Bhasha.Domain;

public record Validation(
	ValidationResult Result,
	string? Reason = default);