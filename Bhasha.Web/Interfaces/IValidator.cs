using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface IValidator
{
	Task<Validation> Validate(ValidationInput input);
}