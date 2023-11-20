namespace Bhasha.Shared.Domain;

public record Validation(
	ValidationResult Result,
	string? Reason = default);