namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record Validation(
	ValidationResult Result,
	string? Reason = default);