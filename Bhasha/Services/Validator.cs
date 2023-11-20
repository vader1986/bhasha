using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;

namespace Bhasha.Services;

public class Validator : IValidator
{
    private readonly ITranslationRepository _translations;

    public Validator(ITranslationRepository translations)
    {
        _translations = translations;
    }

    public async Task<Validation> Validate(ValidationInput input)
    {
        var languages = input.Key;
        var translation = input.Translation;
        var solution = await _translations.Get(translation.Id);

        if (solution == null)
            throw new ArgumentOutOfRangeException($"translation {translation.Id} not found");

        if (translation.Language != languages.Target)
            return new Validation(ValidationResult.Wrong, "Translation was submit in the wrong language!");

        if (translation.Text != solution.Text)
            return new Validation(ValidationResult.Wrong);

        return new Validation(ValidationResult.Correct);
    }
}

