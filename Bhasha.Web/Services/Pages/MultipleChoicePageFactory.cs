using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services.Pages
{
	public class MultipleChoicePageFactory : IAsyncFactory<Page, Profile, DisplayedPage<MultipleChoice>>
	{
        private readonly ITranslationProvider _translationProvider;
        private readonly IRepository<Expression> _expressions;

        public MultipleChoicePageFactory(ITranslationProvider translationProvider, IRepository<Expression> expressions)
		{
            _translationProvider = translationProvider;
            _expressions = expressions;
        }

        public async Task<DisplayedPage<MultipleChoice>> CreateAsync(Translation word)
        {
            var expression = await _expressions.Get(word.ExpressionId);

            if (expression == null)
                throw new InvalidOperationException($"Expression for {word} not found");

            var expressions = await _expressions.Find(
                x => x.Level == expression.Level && x.Id != expression.Id, 3);

            var expressionIds = expressions.Select(x => x.Id).ToArray();
            var words = await _translationProvider.FindAll(word.Language, expressionIds);

            var choices = words
                .Values
                .Append(word)
                .Select(x => x with { ExpressionId = Guid.Empty }) // hide expression id to avoid cheating
                .ToArray();

            return new DisplayedPage<MultipleChoice>(word.Native, word.Spoken, default, default, new MultipleChoice(choices));
        }

        public async Task<DisplayedPage<MultipleChoice>> CreateAsync(Page page, Profile profile)
        {
            var expression = await _expressions.Get(page.ExpressionId);

            if (expression == null)
                throw new InvalidOperationException($"Expression for {page.ExpressionId} not found");

            var expressions = await _expressions.Find(
                x => x.Level == expression.Level && x.Id != expression.Id, 3);

            var expressionIds = expressions.Select(x => x.Id).ToArray();
            var wrongChoices = await _translationProvider.FindAll(profile.Target, expressionIds);

            var correctChoice = await _translationProvider.Find(page.ExpressionId, profile.Target);
            if (correctChoice == null)
                throw new InvalidOperationException($"Translation for {page.ExpressionId} to {profile.Target} not found");

            var choices = wrongChoices
                .Values
                .Append(correctChoice)
                .Select(x => x with { ExpressionId = Guid.Empty }) // hide expression id to avoid cheating
                .ToArray();

            var word = await _translationProvider.Find(page.ExpressionId, profile.Native);
            if (word == null)
                throw new InvalidOperationException($"Translation for {page.ExpressionId} to {profile.Native} not found");

            return new DisplayedPage<MultipleChoice>(word.Native, word.Spoken, default, page.Leads.Any(), new MultipleChoice(choices));
        }
    }
}