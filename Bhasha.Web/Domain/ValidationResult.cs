namespace Bhasha.Web.Domain;

public record ValidationResult(
	ValidationResultType Result,
	string? Reason = default);