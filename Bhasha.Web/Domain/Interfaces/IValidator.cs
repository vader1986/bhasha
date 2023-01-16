namespace Bhasha.Web.Domain.Interfaces;

public interface IValidator
{
	Task<Validation> Validate(ValidationInput input);
}