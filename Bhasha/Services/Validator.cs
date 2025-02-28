using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Services;

public sealed class Validator(ITranslationRepository translations) : IValidator
{
    public async Task<Validation> Validate(ValidationInput input)
    {
        var languages = input.Key;
        
        var solution = await translations
            .Find(input.ExpressionId, languages.Target);

        if (solution == null)
            throw new ArgumentOutOfRangeException($"translation {languages.Target} for expression {input.ExpressionId} not found");

        if (input.UserInput != solution.Text)
            return new Validation(ValidationResult.Wrong);

        return new Validation(ValidationResult.Correct);
    }
}

