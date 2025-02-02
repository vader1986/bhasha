using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Domain.Pages;
using Chapter = Bhasha.Domain.Chapter;
using Expression = Bhasha.Domain.Expression;

namespace Bhasha.Services;

public class MultipleChoicePageFactory(ITranslationRepository translations) : IMultipleChoicePageFactory
{
    private const int MaxNumberOfChoices = 4;

    public async Task<DisplayedPage<MultipleChoice>> Create(Chapter chapter, Expression expression, ProfileKey languages)
    {
        var translations1 = new List<Translation>();
        
        foreach (var page in chapter.Pages)
        {
            var translation = await translations.Find(page.Id, languages.Target);

            if (translation is null)
            {
                throw new InvalidOperationException($"No translation for expression {page.Id} to {languages.Target}");
            }
            
            translations1.Add(translation);
        }

        var choices = translations1
            .Where(x => x.Expression.Id != expression.Id)
            .OrderBy(_ => Guid.NewGuid())
            .Take(Math.Min(MaxNumberOfChoices - 1, translations1.Count))
            .Append(translations1
                .First(x => x.Expression.Id == expression.Id))
            .Select(x => x with
            {
                // hide expression id to avoid cheating
                Expression = Expression.Create(x.Expression.Level) 
            })
            .ToArray();

        Random.Shared.Shuffle(choices);
        
        var word = await translations
            .Find(expression.Id, languages.Native);
        
        if (word is null)
            throw new InvalidOperationException($"No translation for expression {expression.Id} to {languages.Native}");

        return new DisplayedPage<MultipleChoice>(
            PageType.MultipleChoice,
            word,
            Lead: null,
            new MultipleChoice(choices, expression.ResourceId));
    }
}