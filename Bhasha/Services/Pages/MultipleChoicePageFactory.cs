using Bhasha.Domain;
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

    public async Task<DisplayedPage<MultipleChoice>> CreateAsync(Page page, ProfileKey languages)
    {
        var expression = await _expressions.Get(page.ExpressionId);

        if (expression == null)
            throw new InvalidOperationException(
                $"Expression for {page.ExpressionId} not found");

        var expressions = await _expressions.Find(expression.Level, MaxNumberOfChoices - 1).ToListAsync();

        var translations = await Task.WhenAll(expressions
            .Where(x => x.Id != expression.Id)
            .Append(expression)
            .Select(async x => await _translations.Find(x.Id, languages.Target)));

        var choices = translations
            .Where(x => x != null)
            .Select(x => x! with { ExpressionId = default }) // hide expression id to avoid cheating
            .OrderBy(_ => Guid.NewGuid()) // randomize list
            .ToArray();

        var word = await _translations.Find(page.ExpressionId, languages.Native);
        if (word == null)
            throw new InvalidOperationException(
                $"Translation for {page.ExpressionId} to {languages.Native} not found");

        return new DisplayedPage<MultipleChoice>(
            page.PageType,
            word,
            Lead: default,
            new MultipleChoice(choices));
    }
}