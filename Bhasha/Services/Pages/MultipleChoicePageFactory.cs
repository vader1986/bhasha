using Bhasha.Domain.Interfaces;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using Bhasha.Shared.Domain.Pages;

namespace Bhasha.Services.Pages;

public class MultipleChoicePageFactory : IMultipleChoicePageFactory
{
    private const int MaxNumberOfChoices = 3;

    private readonly IExpressionRepository _expressions;
    private readonly ITranslationRepository _translations;

    public MultipleChoicePageFactory(IExpressionRepository expressions, ITranslationRepository translations)
	{
        _expressions = expressions;
        _translations = translations;
    }

    public async Task<DisplayedPage<MultipleChoice>> CreateAsync(Expression page, ProfileKey languages)
    {
        var expression = await _expressions.Get(page.Id);

        if (expression == null)
            throw new InvalidOperationException($"Expression for {page.Id} not found");

        var expressions = await _expressions
            .Find(expression.Level, MaxNumberOfChoices - 1).ToListAsync();

        var translations = await Task.WhenAll(expressions
            .Where(x => x.Id != expression.Id)
            .Append(expression)
            .Select(async x => await _translations.Find(x.Id, languages.Target)));

        var choices = translations
            .Where(x => x != null)
            .Select(x => x! with { Expression = Expression.Create(x.Expression.Level) }) // hide expression id to avoid cheating
            .OrderBy(_ => Guid.NewGuid()) // randomize list
            .ToArray();

        var word = await _translations.Find(page.Id, languages.Native);
        if (word == null)
            throw new InvalidOperationException(
                $"Translation for {page.Id} to {languages.Native} not found");

        return new DisplayedPage<MultipleChoice>(
            PageType.MultipleChoice,
            word,
            Lead: default,
            new MultipleChoice(choices));
    }
}