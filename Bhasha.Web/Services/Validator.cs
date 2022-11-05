using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class Validator : IValidator
{
    private readonly IRepository<Translation> _translations;

    public Validator(IRepository<Translation> translations)
    {
        _translations = translations;
    }

    public async Task<ValidationResult> Validate(ValidationInput input)
    {
        var languages = input.Languages;
        var translation = input.Translation;
        var solution = await _translations.Get(translation.Id);

        if (solution == null)
            throw new ArgumentOutOfRangeException($"translation {translation.Id} not found");

        if (translation.Language != languages.Target)
            return new ValidationResult(ValidationResultType.Wrong, "Translation was submit in the wrong language!");

        if (translation.Native != solution.Native)
            return new ValidationResult(ValidationResultType.Wrong);

        return new ValidationResult(ValidationResultType.Correct);
    }
}

