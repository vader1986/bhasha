using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface IValidator
{
	Task<ValidationResult> Validate(ValidationInput input);
}