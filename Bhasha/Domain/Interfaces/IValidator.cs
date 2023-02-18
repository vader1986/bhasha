namespace Bhasha.Domain.Interfaces;

public interface IValidator
{
	Task<Validation> Validate(ValidationInput input);
}