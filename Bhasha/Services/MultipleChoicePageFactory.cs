using Bhasha.Domain.Interfaces;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using Bhasha.Shared.Domain.Pages;

namespace Bhasha.Services;

public class MultipleChoicePageFactory : IMultipleChoicePageFactory
{
    private const int MaxNumberOfChoices = 4;

    private readonly ITranslationRepository _translations;

    public MultipleChoicePageFactory(ITranslationRepository translations)
	{
        _translations = translations;
    }

    public async Task<DisplayedPage<MultipleChoice>> Create(Chapter chapter, Expression expression, ProfileKey languages)
    {
        var translations = new List<Translation>();
        
        foreach (var page in chapter.Pages)
        {
            var translation = await _translations.Find(page.Id, languages.Target);

            if (translation is null)
            {
                throw new InvalidOperationException($"No translation for expression {page.Id} to {languages.Target}");
            }
            
            translations.Add(translation);
        }

        var choices = translations
            .Where(x => x.Expression.Id != expression.Id)
            .OrderBy(_ => Guid.NewGuid())
            .Take(Math.Min(MaxNumberOfChoices - 1, translations.Count))
            .Append(translations
                .First(x => x.Expression.Id == expression.Id))
            .Select(x => x with
            {
                // hide expression id to avoid cheating
                Expression = Expression.Create(x.Expression.Level) 
            })
            .ToArray();

        Random.Shared.Shuffle(choices);
        
        var word = await _translations
            .Find(expression.Id, languages.Native);
        
        if (word is null)
            throw new InvalidOperationException($"No translation for expression {expression.Id} to {languages.Native}");

        return new DisplayedPage<MultipleChoice>(
            PageType.MultipleChoice,
            word,
            Lead: default,
            new MultipleChoice(choices));
    }
}