namespace Bhasha.Domain;

[GenerateSerializer]
public record Validation(
	ValidationResult Result,
	string? Reason = default);