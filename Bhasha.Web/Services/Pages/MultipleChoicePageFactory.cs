using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services.Pages
{
	public class MultipleChoicePageFactory : IAsyncFactory<Page, Profile, DisplayedPage<MultipleChoice>>
	{
        private const int ExpressionSamples = 10;
        private const int MaxNumberOfChoices = 3;

        private readonly IRepository<Expression> _expressions;
        private readonly IRepository<Translation> _translations;

        public MultipleChoicePageFactory(IRepository<Expression> expressions, IRepository<Translation> translations)
		{
            _expressions = expressions;
            _translations = translations;
        }

        public async Task<DisplayedPage<MultipleChoice>> CreateAsync(Page page, Profile profile)
        {
            var expression = await _expressions.Get(page.ExpressionId);

            if (expression == null)
                throw new InvalidOperationException(
                    $"Expression for {page.ExpressionId} not found");

            var expressions = await _expressions.Find(
                x => x.Level == expression.Level && x.Id != expression.Id,
                ExpressionSamples);

            var translations = await Task.WhenAll(expressions
                .Append(expression)
                .Select(async x => await _translations.Find(
                    t => t.ExpressionId == x.Id && t.Language == profile.Target)));

            var choicesMap = translations
                .SelectMany(x => x)
                .Where(x => x.Language == profile.Target)
                .Take(MaxNumberOfChoices)
                .ToDictionary(x => x.ExpressionId, x => x);

            var choices = choicesMap
                .Values
                .Select(x => x with { ExpressionId = Guid.Empty }) // hide expression id to avoid cheating
                .ToArray();


            var currentTranslations = await _translations.Find(
                x => x.ExpressionId == page.ExpressionId && x.Language == profile.Native);

            var word = currentTranslations.FirstOrDefault();
            if (word == null)
                throw new InvalidOperationException(
                    $"Translation for {page.ExpressionId} to {profile.Native} not found");

            return new DisplayedPage<MultipleChoice>(
                page.PageType,
                word,
                default,
                page.Leads.Any(),
                new MultipleChoice(choices));
        }
    }
}